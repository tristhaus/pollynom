using System;
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
        private readonly Color[] graphColors = { Colors.Black, Colors.Blue, Colors.Green, Colors.Pink, Colors.Brown };
        private readonly SolidColorBrush errorSolidBrush = new SolidColorBrush(Colors.Red);
        private readonly SolidColorBrush goodDotActiveSolidBrush = new SolidColorBrush(Colors.LightBlue);
        private readonly SolidColorBrush goodDotAsleepSolidBrush = new SolidColorBrush(Colors.DarkBlue);

        private bool disposedValue = false;
        private CoordinateHelper coordinateHelper;
        private PollyController controller;
        private SemaphoreSlim controllerMutex;

        private Label label0;
        private Button button0;
        private TextBox textBox0;
        private DockPanel controls0;
        private UniformGrid controlsGrid;

        private Canvas canvas;
        private Grid gridForCanvas;
        private DockPanel dpForCanvas;

        public MainWindow()
        {
            this.controller = new PollyController();
            this.controllerMutex = new SemaphoreSlim(1, 1);
            this.InitializeComponent();

            this.ConstructLayout();
            this.SetEnabledOnMenuItems(true);
            this.RedrawAll();

            this.button0.Click += this.Button0_Click;
            this.SizeChanged += this.HandleSizeChanged;
            this.dpForCanvas.SizeChanged += this.HandleSizeChanged;
            this.textBox0.PreviewKeyDown += this.TextBox0_KeyDown;
            this.textBox0.TextChanged += this.TextBox0_TextChanged;
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
            this.label0 = new Label()
            {
                Content = "y =",
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Left,
            };
            DockPanel.SetDock(this.label0, Dock.Left);

            this.button0 = new Button()
            {
                Content = "Calc",
                Height = 20,
            };
            DockPanel.SetDock(this.button0, Dock.Right);

            this.textBox0 = new TextBox()
            {
                Text = "(x)*(x-1)*(x+1)",
                Height = 20,
                TextWrapping = TextWrapping.NoWrap,
                VerticalAlignment = VerticalAlignment.Center
            };

            this.controls0 = new DockPanel()
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

            this.controls0.Children.Add(this.label0);
            this.controls0.Children.Add(this.button0);
            this.controls0.Children.Add(this.textBox0);

            this.controlsGrid = new UniformGrid()
            {
                Margin = new Thickness()
                {
                    Bottom = 6,
                    Right = 6,
                    Top = 6,
                    Left = 6,
                },
            };
            this.controlsGrid.Children.Add(this.controls0);
            DockPanel.SetDock(this.controlsGrid, Dock.Bottom);

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

            this.mainDockPanel.Children.Add(this.controlsGrid);
            this.mainDockPanel.Children.Add(this.dpForCanvas);

            this.UpdateLayout();
            this.AdjustCanvasSize();
        }

        private void HandleSizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.RedrawAll();
        }

        private async void TextBox0_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                await this.controllerMutex.WaitAsync();
                string input = this.textBox0.Text;
                bool parseable = await Task.Run(() =>
                {
                    return string.IsNullOrWhiteSpace(input) || this.controller.TestExpression(input);
                });
                this.textBox0.Foreground = parseable ? SystemColors.WindowTextBrush : this.errorSolidBrush;
            }
            finally
            {
                this.controllerMutex.Release();
            }
        }

        private async void TextBox0_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                await this.HandleUpdate();
            }
        }

        private async void Button0_Click(object sender, RoutedEventArgs e)
        {
            await this.HandleUpdate();
        }

        private async Task HandleUpdate()
        {
            try
            {
                await this.controllerMutex.WaitAsync();
                this.SetEnabledOnMenuItems(false);
                string input = this.textBox0.Text;
                bool parseable = await Task.Run(() =>
                {
                    return string.IsNullOrWhiteSpace(input) || this.controller.TestExpression(input);
                });

                if (parseable)
                {
                    if ((parseable && this.controller.ExpressionCount < 5) || string.IsNullOrWhiteSpace(input))
                    {
                        await Task.Run(() => this.controller.UpdateExpression(input));
                    }

                    this.RedrawAll();
                }
            }
            finally
            {
                this.SetEnabledOnMenuItems(true);
                this.textBox0.Focus();
                Keyboard.Focus(this.textBox0);
                this.controllerMutex.Release();
            }
        }

        private void SetEnabledOnMenuItems(bool newValue)
        {
            this.button0.IsEnabled = newValue;
            this.textBox0.IsEnabled = newValue;
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
            for (int i = 0; i < this.controller.ExpressionCount; i++)
            {
                Brush brush = new SolidColorBrush(this.graphColors[i]);

                var lists = this.controller.GetListsOfLogicalPointsByIndex(i);
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
