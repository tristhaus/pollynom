using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using Backend.Controller;

namespace PollyFoundation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public sealed partial class MainWindow : Window, IDisposable
    {
        private const double CanvasMargin = 10;
        private const string TitlePrefix = "PollyNom - Score: ";
        private const string ButtonLabelText = "Calc";

        private readonly Color[] graphColors = { Colors.Black, Colors.Blue, Colors.Green, Colors.Pink, Colors.Brown };
        private readonly SolidColorBrush errorSolidBrush = new SolidColorBrush(Colors.Red);
        private readonly SolidColorBrush goodDotActiveSolidBrush = new SolidColorBrush(Colors.LightBlue);
        private readonly SolidColorBrush goodDotAsleepSolidBrush = new SolidColorBrush(Colors.DarkBlue);

        private readonly int numberOfExpressions;

        private bool disposedValue = false;
        private CoordinateHelper coordinateHelper;
        private PollyController controller;
        private SemaphoreSlim controllerMutex;

        private Label[] labels;
        private Button[] buttons;
        private TextBox[] textBoxes;
        private DockPanel[] controlContainers;
        private ScrollViewer scrollViewer;
        private UniformGrid controlsGrid;

        private Canvas canvas;
        private Grid gridForCanvas;
        private DockPanel dpForCanvas;

        public MainWindow()
        {
            this.controller = new PollyController();
            this.controllerMutex = new SemaphoreSlim(1, 1);
            this.numberOfExpressions = this.controller.MaxExpressionCount;
            this.InitializeComponent();

            this.ConstructLayout();
            this.SetEnabledOnMenuItems(true);
            this.RedrawAll();

            this.textBoxes[0].Text = "(x)*(x-1)*(x+1)";

            foreach (var button in this.buttons)
            {
                button.Click += this.Button_Click;
            }

            foreach (var textBox in this.textBoxes)
            {
                textBox.PreviewKeyDown += this.TextBox_KeyDown;
                textBox.TextChanged += this.TextBox_TextChanged;
            }

            this.SizeChanged += this.HandleSizeChanged;
            this.dpForCanvas.SizeChanged += this.HandleSizeChanged;
        }

        public void Dispose()
        {
            this.Dispose(true);
        }

        private void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                    ((IDisposable)this.controllerMutex).Dispose();
                }

                this.disposedValue = true;
            }
        }

        private void ConstructLayout()
        {
            this.labels = new Label[this.numberOfExpressions];
            this.buttons = new Button[this.numberOfExpressions];
            this.textBoxes = new TextBox[this.numberOfExpressions];
            this.controlContainers = new DockPanel[this.numberOfExpressions];

            this.scrollViewer = new ScrollViewer()
            {
                Margin = new Thickness()
                {
                    Bottom = 6,
                    Right = 6,
                    Top = 6,
                    Left = 6,
                },
                Height = 60,
            };
            DockPanel.SetDock(this.scrollViewer, Dock.Bottom);

            this.controlsGrid = new UniformGrid()
            {
                Rows = this.numberOfExpressions,
                Columns = 1,
            };

            for (int controlIndex = 0; controlIndex < this.numberOfExpressions; ++controlIndex)
            {
                this.labels[controlIndex] = new Label()
                {
                    Content = $"y{controlIndex + 1} =",
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Left,
                };

                this.buttons[controlIndex] = new Button()
                {
                    Content = ButtonLabelText,
                    Height = 20,
                };
                DockPanel.SetDock(this.buttons[controlIndex], Dock.Right);

                this.textBoxes[controlIndex] = new TextBox()
                {
                    Height = 20,
                    TextWrapping = TextWrapping.NoWrap,
                    VerticalAlignment = VerticalAlignment.Center
                };

                this.controlContainers[controlIndex] = new DockPanel()
                {
                    Margin = new Thickness()
                    {
                        Bottom = 0,
                        Right = 0,
                        Top = 0,
                        Left = 0,
                    },
                    LastChildFill = true,
                };

                this.controlContainers[controlIndex].Children.Add(this.labels[controlIndex]);
                this.controlContainers[controlIndex].Children.Add(this.buttons[controlIndex]);
                this.controlContainers[controlIndex].Children.Add(this.textBoxes[controlIndex]);

                this.controlsGrid.Children.Add(this.controlContainers[controlIndex]);
            }

            this.scrollViewer.Content = this.controlsGrid;

            this.canvas = new Canvas()
            {
                Margin = new Thickness()
                {
                    Bottom = 0,
                    Right = 0,
                    Top = 0,
                    Left = 0,
                },
                ClipToBounds = true,
            };

            this.gridForCanvas = new Grid()
            {
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
            };
            this.gridForCanvas.Children.Add(this.canvas);

            this.dpForCanvas = new DockPanel()
            {
            };
            this.dpForCanvas.Children.Add(this.gridForCanvas);

            this.mainDockPanel.Children.Add(this.scrollViewer);
            this.mainDockPanel.Children.Add(this.dpForCanvas);

            this.UpdateLayout();
            this.AdjustCanvasSize();
        }

        private void HandleSizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.RedrawAll();
        }

        private async void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                await this.controllerMutex.WaitAsync();

                var theTextBox = sender as TextBox;
                if (theTextBox == null)
                {
                    return;
                }

                string input = theTextBox.Text;
                bool parseable = await Task.Run(() =>
                {
                    return string.IsNullOrWhiteSpace(input) || this.controller.TestExpression(input);
                });
                theTextBox.Foreground = parseable ? SystemColors.WindowTextBrush : this.errorSolidBrush;
            }
            finally
            {
                this.controllerMutex.Release();
            }
        }

        private async void TextBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                await this.HandleUpdate();
            }
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            await this.HandleUpdate();
        }

        private async Task HandleUpdate()
        {
            try
            {
                await this.controllerMutex.WaitAsync();
                this.SetEnabledOnMenuItems(false);

                bool[] parseable = new bool[this.numberOfExpressions];

                for (int expressionIndex = 0; expressionIndex < this.numberOfExpressions; ++expressionIndex)
                {
                    string input = this.textBoxes[expressionIndex].Text;
                    parseable[expressionIndex] = await Task.Run(() =>
                    {
                        return string.IsNullOrWhiteSpace(input) || this.controller.TestExpression(input);
                    });
                }

                if (parseable.All(x => x == true))
                {
                    Task[] tasks = new Task[this.numberOfExpressions];
                    for (int expressionIndex = 0; expressionIndex < this.numberOfExpressions; expressionIndex++)
                    {
                        string input = this.textBoxes[expressionIndex].Text;
                        if (string.IsNullOrWhiteSpace(input))
                        {
                            tasks[expressionIndex] = Task.CompletedTask;
                        }
                        else
                        {
                            var localIndex = expressionIndex;
                            tasks[expressionIndex] = new Task(() => this.controller.SetExpressionAtIndex(localIndex, input));
                            tasks[expressionIndex].Start();
                        }
                    }

                    await Task.WhenAll(tasks);
                    await Task.Run(() => this.controller.UpdateData());

                    this.RedrawAll();
                }
            }
            finally
            {
                this.SetEnabledOnMenuItems(true);
                this.textBoxes[0].Focus();
                Keyboard.Focus(this.textBoxes[0]);
                this.controllerMutex.Release();
            }
        }

        private void SetEnabledOnMenuItems(bool newValue)
        {
            foreach (var button in this.buttons)
            {
                button.IsEnabled = newValue;
            }

            foreach (var textBox in this.textBoxes)
            {
                textBox.IsEnabled = newValue;
            }
        }

        private void RedrawAll()
        {
            this.UpdateLayout();
            this.AdjustCanvasSize();

            this.canvas.Children.Clear();

            this.DrawCoordinateSystem();
            this.DrawGraphs();
            this.DrawDots();
            this.UpdateWindowTitle();
        }

        private void AdjustCanvasSize()
        {
            this.gridForCanvas.Height = this.dpForCanvas.ActualHeight < this.dpForCanvas.ActualWidth ? this.dpForCanvas.ActualHeight : this.dpForCanvas.ActualWidth;
            this.gridForCanvas.Width = this.dpForCanvas.ActualHeight < this.dpForCanvas.ActualWidth ? this.dpForCanvas.ActualHeight : this.dpForCanvas.ActualWidth;
            this.coordinateHelper = new CoordinateHelper(this.canvas.ActualWidth, this.canvas.ActualHeight, CanvasMargin, 21.2);
        }

        private void DrawCoordinateSystem()
        {
            // X axis and ticks
            GeometryGroup xAxisGeometryGroup = new GeometryGroup();
            xAxisGeometryGroup.Children.Add(new LineGeometry(this.coordinateHelper.ConvertCoordinates(10.5, 0), this.coordinateHelper.ConvertCoordinates(-10.5, 0)));
            for (int i = 1; i <= 10; i++)
            {
                double length = i % 5 == 0 ? 0.4 : 0.3;
                xAxisGeometryGroup.Children.Add(new LineGeometry(this.coordinateHelper.ConvertCoordinates(+i, -length), this.coordinateHelper.ConvertCoordinates(+i, +length)));
                xAxisGeometryGroup.Children.Add(new LineGeometry(this.coordinateHelper.ConvertCoordinates(-i, -length), this.coordinateHelper.ConvertCoordinates(-i, +length)));
            }

            xAxisGeometryGroup.Children.Add(new LineGeometry(this.coordinateHelper.ConvertCoordinates(10.5, 0.0), this.coordinateHelper.ConvertCoordinates(10.2, -0.3)));
            xAxisGeometryGroup.Children.Add(new LineGeometry(this.coordinateHelper.ConvertCoordinates(10.5, 0.0), this.coordinateHelper.ConvertCoordinates(10.2, +0.3)));
            Path xAxisPath = new Path();
            xAxisPath.StrokeThickness = 1;
            xAxisPath.Stroke = Brushes.Black;
            xAxisPath.Data = xAxisGeometryGroup;
            this.canvas.Children.Add(xAxisPath);

            // X labels
            TextBlock xMinusTextBlock = new TextBlock()
            {
                Text = "-10",
                FontSize = this.coordinateHelper.ConvertXLength(0.8),
                Foreground = Brushes.Black,
            };
            Point xMinusPoint = this.coordinateHelper.ConvertCoordinates(-10.5, -0.5);
            Canvas.SetLeft(xMinusTextBlock, xMinusPoint.X);
            Canvas.SetTop(xMinusTextBlock, xMinusPoint.Y);
            this.canvas.Children.Add(xMinusTextBlock);

            TextBlock xPlusTextBlock = new TextBlock()
            {
                Text = "10",
                FontSize = this.coordinateHelper.ConvertXLength(0.8),
                Foreground = Brushes.Black,
            };
            Point xPlusPoint = this.coordinateHelper.ConvertCoordinates(9.5, -0.5);
            Canvas.SetLeft(xPlusTextBlock, xPlusPoint.X);
            Canvas.SetTop(xPlusTextBlock, xPlusPoint.Y);
            this.canvas.Children.Add(xPlusTextBlock);

            // Y axis and ticks
            GeometryGroup yAxisGeometryGroup = new GeometryGroup();
            yAxisGeometryGroup.Children.Add(new LineGeometry(this.coordinateHelper.ConvertCoordinates(0, 10.5), this.coordinateHelper.ConvertCoordinates(0, -10.5)));
            for (int i = 1; i <= 10; i++)
            {
                double length = i % 5 == 0 ? 0.4 : 0.3;
                yAxisGeometryGroup.Children.Add(new LineGeometry(this.coordinateHelper.ConvertCoordinates(-length, +i), this.coordinateHelper.ConvertCoordinates(+length, +i)));
                yAxisGeometryGroup.Children.Add(new LineGeometry(this.coordinateHelper.ConvertCoordinates(-length, -i), this.coordinateHelper.ConvertCoordinates(+length, -i)));
            }

            yAxisGeometryGroup.Children.Add(new LineGeometry(this.coordinateHelper.ConvertCoordinates(0.0, 10.5), this.coordinateHelper.ConvertCoordinates(-0.3, 10.2)));
            yAxisGeometryGroup.Children.Add(new LineGeometry(this.coordinateHelper.ConvertCoordinates(0.0, 10.5), this.coordinateHelper.ConvertCoordinates(+0.3, 10.2)));
            Path yAxisPath = new Path();
            yAxisPath.StrokeThickness = 1.5;
            yAxisPath.Stroke = Brushes.Black;
            yAxisPath.Data = yAxisGeometryGroup;

            this.canvas.Children.Add(yAxisPath);

            // Y labels
            TextBlock yMinusTextBlock = new TextBlock()
            {
                Text = "-10",
                FontSize = this.coordinateHelper.ConvertXLength(0.8),
                Foreground = Brushes.Black,
            };
            Point yMinusPoint = this.coordinateHelper.ConvertCoordinates(0.5, -9.5);
            Canvas.SetLeft(yMinusTextBlock, yMinusPoint.X);
            Canvas.SetTop(yMinusTextBlock, yMinusPoint.Y);
            this.canvas.Children.Add(yMinusTextBlock);

            TextBlock yPlusTextBlock = new TextBlock()
            {
                Text = "10",
                FontSize = this.coordinateHelper.ConvertXLength(0.8),
                Foreground = Brushes.Black,
            };
            Point yPlusPoint = this.coordinateHelper.ConvertCoordinates(0.5, 10.5);
            Canvas.SetLeft(yPlusTextBlock, yPlusPoint.X);
            Canvas.SetTop(yPlusTextBlock, yPlusPoint.Y);
            this.canvas.Children.Add(yPlusTextBlock);
        }

        private void DrawGraphs()
        {
            for (int expressionIndex = 0; expressionIndex < this.controller.MaxExpressionCount; expressionIndex++)
            {
                Brush brush = new SolidColorBrush(this.graphColors[expressionIndex]);

                var lists = this.controller.GetListsOfLogicalPointsByIndex(expressionIndex);
                foreach (var list in lists)
                {
                    Polyline polyLine = new Polyline();
                    foreach (var point in list.Points)
                    {
                        polyLine.Points.Add(this.coordinateHelper.ConvertCoordinates(point.X, point.Y));
                    }

                    polyLine.Stroke = brush;
                    polyLine.StrokeThickness = 1;
                    this.canvas.Children.Add(polyLine);
                }
            }
        }

        private void DrawDots()
        {
            GeometryGroup goodDotsHitGeometryGroup = new GeometryGroup();
            GeometryGroup goodDotMissedGeometryGroup = new GeometryGroup();

            foreach (var goodDot in this.controller.GetDrawDots())
            {
                double radius = this.coordinateHelper.ConvertXLength(goodDot.Radius);
                EllipseGeometry ellipseGeometry = new EllipseGeometry(this.coordinateHelper.ConvertCoordinates(goodDot.Position.Item1, goodDot.Position.Item2), radius, radius);
                if (goodDot.IsHit)
                {
                    goodDotsHitGeometryGroup.Children.Add(ellipseGeometry);
                }
                else
                {
                    goodDotMissedGeometryGroup.Children.Add(ellipseGeometry);
                }
            }

            Path goodDotsHitPath = new Path();
            goodDotsHitPath.StrokeThickness = 0.0;
            goodDotsHitPath.Fill = this.goodDotActiveSolidBrush;
            goodDotsHitPath.Data = goodDotsHitGeometryGroup;
            this.canvas.Children.Add(goodDotsHitPath);

            Path goodDotMissedPath = new Path();
            goodDotMissedPath.StrokeThickness = 0.0;
            goodDotMissedPath.Fill = this.goodDotAsleepSolidBrush;
            goodDotMissedPath.Data = goodDotMissedGeometryGroup;
            this.canvas.Children.Add(goodDotMissedPath);
        }

        private void UpdateWindowTitle()
        {
            this.Title = TitlePrefix + this.controller.Score.ToString();
        }
    }
}
