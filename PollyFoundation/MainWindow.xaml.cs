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
using Persistence.Models;

namespace PollyFoundation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public sealed partial class MainWindow : Window, IDisposable
    {
        /// <summary>
        /// Margin to be used in the canvas.
        /// </summary>
        private const double CanvasMargin = 10;

        /// <summary>
        /// Prefix to be used in the title text of the main window.
        /// </summary>
        private const string TitlePrefix = "PollyNom - Score: ";

        /// <summary>
        /// Label text to be used on the button(s).
        /// </summary>
        private const string ButtonLabelText = "Calc";

        /// <summary>
        /// Colors to be used for the graphs.
        /// </summary>
        private readonly Color[] graphColors = { Colors.Black, Colors.Blue, Colors.DarkViolet, Colors.Brown, Colors.PowderBlue, };

        /// <summary>
        /// Brush to be used for error state of a text box.
        /// </summary>
        private readonly SolidColorBrush errorSolidBrush = new SolidColorBrush(Colors.Red);

        /// <summary>
        /// Brush to be used to indicate that the content of a textbox has been graphed.
        /// </summary>
        private readonly SolidColorBrush hasBeenGraphedSolidBrush = new SolidColorBrush(Colors.PowderBlue);

        /// <summary>
        /// Brush to be used for painting good dots that HAVE been hit.
        /// </summary>
        private readonly SolidColorBrush goodDotActiveSolidBrush = new SolidColorBrush(Colors.LightBlue);

        /// <summary>
        /// Brush to be used for painting good dots that have NOT been hit.
        /// </summary>
        private readonly SolidColorBrush goodDotAsleepSolidBrush = new SolidColorBrush(Colors.DarkBlue);

        /// <summary>
        /// Brush to be used for painting bad dots that HAVE been hit.
        /// </summary>
        private readonly SolidColorBrush badDotActiveSolidBrush = new SolidColorBrush(Colors.OrangeRed);

        /// <summary>
        /// Brush to be used for painting bad dots that have NOT been hit.
        /// </summary>
        private readonly SolidColorBrush badDotAsleepSolidBrush = new SolidColorBrush(Colors.LightPink);

        /// <summary>
        /// The total number of expressions supported by the <see cref="PollyController"/> instance.
        /// </summary>
        private readonly int numberOfExpressions;

        /// <summary>
        /// The controller instance.
        /// </summary>
        private readonly PollyController controller;

        /// <summary>
        /// The mutex implementation serializing access to the <see cref="this.controller"/></see>.
        /// </summary>
        private readonly SemaphoreSlim controllerMutex;

        /// <summary>
        /// Helper value needed for the <see cref="IDisposable"/> implementation.
        /// </summary>
        private bool disposedValue = false;

        /// <summary>
        /// The instance helping with coordinate transformation.
        /// </summary>
        private CoordinateHelper coordinateHelper;

        /// <summary>
        /// The main menu (strip) at the top of the window.
        /// </summary>
        private Menu mainMenu;

        /// <summary>
        /// The first-level file menu enty.
        /// </summary>
        private MenuItem fileMenuItem;

        /// <summary>
        /// The new game menu item, found under <see cref="this.fileMenuItem"/>.
        /// </summary>
        private MenuItem newGameMenuItem;

        /// <summary>
        /// The open existing game menu item, found under <see cref="this.fileMenuItem"/>.
        /// </summary>
        private MenuItem openGameMenuItem;

        /// <summary>
        /// The save current game menu item, found under <see cref="this.fileMenuItem"/>.
        /// </summary>
        private MenuItem saveGameMenuItem;

        /// <summary>
        /// The canvas on which output is drawn.
        /// </summary>
        private Canvas canvas;

        /// <summary>
        /// The grid holding the canvas such that resizing is easier.
        /// </summary>
        private Grid gridForCanvas;

        /// <summary>
        /// The container holding the output grid.
        /// </summary>
        private DockPanel dpForCanvas;

        /// <summary>
        /// The grid splitter to resize input area.
        /// </summary>
        private GridSplitter gridSplitter;

        /// <summary>
        /// The collection of labels indicating the color of the graphs in the UI.
        /// </summary>
        private Label[] colorLabels;

        /// <summary>
        /// The collection of 'yn =' labels to be used in the UI.
        /// </summary>
        private Label[] yLabels;

        /// <summary>
        /// The collection of text boxes to be used in the UI.
        /// </summary>
        private TextBox[] textBoxes;

        /// <summary>
        /// The collection of buttons to be used in the UI.
        /// </summary>
        private Button[] buttons;

        /// <summary>
        /// The collection of containers holding <see cref="this.labels"/>, <see cref="this.textBoxes"/>, <see cref="this.buttons"/>.
        /// </summary>
        private DockPanel[] controlContainers;

        /// <summary>
        /// The grid holding the controlContainers, encapsulating for <see cref="ScrollViewer.Content"/>.
        /// </summary>
        private UniformGrid controlsGrid;

        /// <summary>
        /// The scroll viewer holding the input controls.
        /// </summary>
        private ScrollViewer scrollViewer;

        /// <summary>
        /// The grid definining the main input and output area.
        /// </summary>
        private Grid inputOutputGrid;

        /// <summary>
        /// Tooltip displayed when <see cref="saveGameMenuItem"/> is disabled.
        /// </summary>
        private ToolTip savingNotPossibleToolTip;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            this.controller = new PollyController();
            this.controllerMutex = new SemaphoreSlim(1, 1);
            this.numberOfExpressions = this.controller.MaxExpressionCount;
            this.InitializeComponent();

            this.ConstructLayout();
            this.SetEnabledOnMenuItems(true);
            this.RedrawAll();

            this.SetTextBoxContentsAreNotInController();

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

            this.newGameMenuItem.Click += this.NewGameMenuItem_Click;
            this.openGameMenuItem.Click += this.OpenGameMenuItem_Click;
            this.saveGameMenuItem.Click += this.SaveGameMenuItem_Click;
        }

        /// <inheritdoc />
        public void Dispose()
        {
            this.Dispose(true);
        }

        /// <summary>
        /// Helper method to implement a MS disposing pattern.
        /// </summary>
        /// <param name="disposing">Flag indicating whether the held resources are really to be disposed.</param>
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

        /// <summary>
        /// Constructs the UI.
        /// </summary>
        private void ConstructLayout()
        {
            this.yLabels = new Label[this.numberOfExpressions];
            this.colorLabels = new Label[this.numberOfExpressions];
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
            };
            DockPanel.SetDock(this.scrollViewer, Dock.Bottom);

            this.controlsGrid = new UniformGrid()
            {
                Rows = this.numberOfExpressions,
                Columns = 1,
            };

            for (int controlIndex = 0; controlIndex < this.numberOfExpressions; ++controlIndex)
            {
                this.colorLabels[controlIndex] = new Label()
                {
                    Content = "⬤",
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Foreground = new SolidColorBrush(this.graphColors[controlIndex]),
                    Margin = new Thickness(1, 0, 1, 0), // LTRB
                };

                this.yLabels[controlIndex] = new Label()
                {
                    Content = $"y{controlIndex + 1} =",
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Margin = new Thickness(1, 0, 1, 0), // LTRB
                };

                this.buttons[controlIndex] = new Button()
                {
                    Content = ButtonLabelText,
                    Height = 20,
                    Margin = new Thickness(1, 0, 1, 0), // LTRB
                };
                DockPanel.SetDock(this.buttons[controlIndex], Dock.Right);

                this.textBoxes[controlIndex] = new TextBox()
                {
                    Height = 20,
                    TextWrapping = TextWrapping.NoWrap,
                    VerticalAlignment = VerticalAlignment.Center,
                    Margin = new Thickness(1, 0, 1, 0), // LTRB
                };

                this.controlContainers[controlIndex] = new DockPanel()
                {
                    Margin = new Thickness(2, 0, 2, 0), // LTRB
                    LastChildFill = true,
                };

                this.controlContainers[controlIndex].Children.Add(this.colorLabels[controlIndex]);
                this.controlContainers[controlIndex].Children.Add(this.yLabels[controlIndex]);
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

            this.gridSplitter = new GridSplitter()
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
            };

            this.inputOutputGrid = new Grid();

            this.inputOutputGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
            this.inputOutputGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(5, GridUnitType.Pixel) });
            this.inputOutputGrid.RowDefinitions.Add(new RowDefinition()
            {
                Height = new GridLength(60, GridUnitType.Pixel),
                MinHeight = 50,
                MaxHeight = 150,
            });

            Grid.SetRow(this.dpForCanvas, 0);
            Grid.SetRow(this.gridSplitter, 1);
            Grid.SetRow(this.scrollViewer, 2);

            this.inputOutputGrid.Children.Add(this.dpForCanvas);
            this.inputOutputGrid.Children.Add(this.gridSplitter);
            this.inputOutputGrid.Children.Add(this.scrollViewer);

            this.mainMenu = new Menu();
            DockPanel.SetDock(this.mainMenu, Dock.Top);
            this.fileMenuItem = new MenuItem() { Header = "_File", };
            this.newGameMenuItem = new MenuItem() { Header = "_New Game", };
            this.openGameMenuItem = new MenuItem() { Header = "_Open Game", };
            this.saveGameMenuItem = new MenuItem() { Header = "_Save Game", };
            ToolTipService.SetShowOnDisabled(this.saveGameMenuItem, true);

            this.mainMenu.Items.Add(this.fileMenuItem);
            this.fileMenuItem.Items.Add(this.newGameMenuItem);
            this.fileMenuItem.Items.Add(this.openGameMenuItem);
            this.fileMenuItem.Items.Add(this.saveGameMenuItem);

            this.basePanel.Children.Add(this.mainMenu);
            this.basePanel.Children.Add(this.inputOutputGrid);

            this.savingNotPossibleToolTip = new ToolTip();
            this.savingNotPossibleToolTip.Content = "Saving possible only after graphing expressions";

            this.UpdateLayout();
            this.AdjustCanvasSize();
        }

        /// <summary>
        /// Handle a SizeChangedEvent by resizing and redrawing the canvas.
        /// </summary>
        /// <param name="sender">The sending object</param>
        /// <param name="e">The EventArgs.</param>
        private void HandleSizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.RedrawAll();
        }

        /// <summary>
        /// Handle a TextChangedEvent by checking the appropriate textbox for parseability and updating the UI.
        /// </summary>
        /// <param name="sender">The sending object</param>
        /// <param name="e">The EventArgs.</param>
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

                this.SetTextBoxContentsAreNotInController();
                theTextBox.ClearValue(TextBox.BackgroundProperty);

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

        /// <summary>
        /// Handle a TextChangedEvent by checking for the Enter key to set input.
        /// </summary>
        /// <param name="sender">The sending object</param>
        /// <param name="e">The EventArgs.</param>
        private async void TextBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                await this.HandleUpdateAsync();
            }
        }

        /// <summary>
        /// Handle a ButtonClickEvent to set input.
        /// </summary>
        /// <param name="sender">The sending object</param>
        /// <param name="e">The EventArgs.</param>
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            await this.HandleUpdateAsync();
        }

        /// <summary>
        /// Performs the update of the controller input and eventually the UI.
        /// </summary>
        /// <returns>An awaitable task.</returns>
        private async Task HandleUpdateAsync()
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
                        var localIndex = expressionIndex;
                        string input = this.textBoxes[expressionIndex].Text;
                        tasks[expressionIndex] = new Task(() => this.controller.SetExpressionAtIndex(localIndex, input));
                        tasks[expressionIndex].Start();
                    }

                    await Task.WhenAll(tasks);
                    await Task.Run(() => this.controller.UpdateData());

                    this.SetTextBoxContentsAreInController();
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

        /// <summary>
        /// Enable or disables all relevant UI items.
        /// </summary>
        /// <param name="newValue">The new value to be applied to the <c>Enabled</c> property.</param>
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

        /// <summary>
        /// Called to indicate that not all textbox contents have been put into the controller.
        /// </summary>
        private void SetTextBoxContentsAreNotInController()
        {
            this.saveGameMenuItem.IsEnabled = false;
            this.saveGameMenuItem.ToolTip = this.savingNotPossibleToolTip;
        }

        /// <summary>
        /// Called to indicate that all textbox contents have been put into the controller.
        /// </summary>
        private void SetTextBoxContentsAreInController()
        {
            this.saveGameMenuItem.IsEnabled = true;
            this.saveGameMenuItem.ClearValue(MenuItem.ToolTipProperty);
            foreach (var textBox in this.textBoxes)
            {
                if (!string.IsNullOrWhiteSpace(textBox.Text))
                {
                    textBox.Background = this.hasBeenGraphedSolidBrush;
                }
            }
        }

        /// <summary>
        /// Redraws/Updates all of the output area.
        /// </summary>
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

        /// <summary>
        /// Change the canvas size such that it is a fitting square.
        /// </summary>
        private void AdjustCanvasSize()
        {
            this.gridForCanvas.Height = this.dpForCanvas.ActualHeight < this.dpForCanvas.ActualWidth ? this.dpForCanvas.ActualHeight : this.dpForCanvas.ActualWidth;
            this.gridForCanvas.Width = this.dpForCanvas.ActualHeight < this.dpForCanvas.ActualWidth ? this.dpForCanvas.ActualHeight : this.dpForCanvas.ActualWidth;
            this.coordinateHelper = new CoordinateHelper(this.canvas.ActualWidth, this.canvas.ActualHeight, CanvasMargin, 21.2);
        }

        /// <summary>
        /// Draw a coordinate system on the canvas.
        /// </summary>
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
            var calculatedFontSize = Math.Max(0.1D, this.coordinateHelper.ConvertXLength(0.8));

            // X labels
            TextBlock xMinusTextBlock = new TextBlock()
            {
                Text = "-10",
                FontSize = calculatedFontSize,
                Foreground = Brushes.Black,
            };
            Point xMinusPoint = this.coordinateHelper.ConvertCoordinates(-10.5, -0.5);
            Canvas.SetLeft(xMinusTextBlock, xMinusPoint.X);
            Canvas.SetTop(xMinusTextBlock, xMinusPoint.Y);
            this.canvas.Children.Add(xMinusTextBlock);

            TextBlock xPlusTextBlock = new TextBlock()
            {
                Text = "10",
                FontSize = calculatedFontSize,
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
                FontSize = calculatedFontSize,
                Foreground = Brushes.Black,
            };
            Point yMinusPoint = this.coordinateHelper.ConvertCoordinates(0.5, -9.5);
            Canvas.SetLeft(yMinusTextBlock, yMinusPoint.X);
            Canvas.SetTop(yMinusTextBlock, yMinusPoint.Y);
            this.canvas.Children.Add(yMinusTextBlock);

            TextBlock yPlusTextBlock = new TextBlock()
            {
                Text = "10",
                FontSize = calculatedFontSize,
                Foreground = Brushes.Black,
            };
            Point yPlusPoint = this.coordinateHelper.ConvertCoordinates(0.5, 10.5);
            Canvas.SetLeft(yPlusTextBlock, yPlusPoint.X);
            Canvas.SetTop(yPlusTextBlock, yPlusPoint.Y);
            this.canvas.Children.Add(yPlusTextBlock);
        }

        /// <summary>
        /// Draw the graphs on the canvas.
        /// </summary>
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

        /// <summary>
        /// Draw the dots on the canvas.
        /// </summary>
        private void DrawDots()
        {
            GeometryGroup goodDotsHitGeometryGroup = new GeometryGroup();
            GeometryGroup goodDotMissedGeometryGroup = new GeometryGroup();
            GeometryGroup badDotsHitGeometryGroup = new GeometryGroup();
            GeometryGroup badDotMissedGeometryGroup = new GeometryGroup();

            foreach (var dot in this.controller.GetDrawDots())
            {
                double radius = this.coordinateHelper.ConvertXLength(dot.Radius);
                EllipseGeometry ellipseGeometry = new EllipseGeometry(this.coordinateHelper.ConvertCoordinates(dot.Position.Item1, dot.Position.Item2), radius, radius);
                if (dot.IsHit)
                {
                    if (dot.Kind == DotKind.Good)
                    {
                        goodDotsHitGeometryGroup.Children.Add(ellipseGeometry);
                    }
                    else
                    {
                        badDotsHitGeometryGroup.Children.Add(ellipseGeometry);
                    }
                }
                else
                {
                    if (dot.Kind == DotKind.Good)
                    {
                        goodDotMissedGeometryGroup.Children.Add(ellipseGeometry);
                    }
                    else
                    {
                        badDotMissedGeometryGroup.Children.Add(ellipseGeometry);
                    }
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

            Path badDotsHitPath = new Path();
            badDotsHitPath.StrokeThickness = 0.0;
            badDotsHitPath.Fill = this.badDotActiveSolidBrush;
            badDotsHitPath.Data = badDotsHitGeometryGroup;
            this.canvas.Children.Add(badDotsHitPath);

            Path badDotMissedPath = new Path();
            badDotMissedPath.StrokeThickness = 0.0;
            badDotMissedPath.Fill = this.badDotAsleepSolidBrush;
            badDotMissedPath.Data = badDotMissedGeometryGroup;
            this.canvas.Children.Add(badDotMissedPath);
        }

        /// <summary>
        /// Change the window title to reflect the current score.
        /// </summary>
        private void UpdateWindowTitle()
        {
            string score = this.controller.Score >= 0 ? this.controller.Score.ToString() : "-∞";
            this.Title = TitlePrefix + score;
        }

        /// <summary>
        /// Load the expression strings from the controller into the textboxes.
        /// </summary>
        private void LoadExpressionStrings()
        {
            for (int i = 0; i < this.textBoxes.Length && i < this.controller.MaxExpressionCount; i++)
            {
                this.textBoxes[i].Text = this.controller.ExpressionStrings[i];
            }
        }

        /// <summary>
        /// Create a new random game.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The event args.</param>
        private void NewGameMenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.controller.NewRandomGame();
            this.LoadExpressionStrings();
            this.controller.UpdateData();
            this.SetTextBoxContentsAreInController();

            this.RedrawAll();
        }

        /// <summary>
        /// Load a game from disk.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The even args.</param>
        private async void OpenGameMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();

            dialog.DefaultExt = ".json";
            dialog.Filter = "Game JSON Files (*.json)|*.json;";

            bool? result = dialog.ShowDialog();

            if (result == true)
            {
                string path = dialog.FileName;
                this.controller.LoadGame(path);
                this.LoadExpressionStrings();
                await this.HandleUpdateAsync();
                this.SetTextBoxContentsAreInController();

                this.RedrawAll();
            }
        }

        /// <summary>
        /// Save a game to disk.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The even args.</param>
        private void SaveGameMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog dialog = new Microsoft.Win32.SaveFileDialog();

            dialog.DefaultExt = ".json";
            dialog.Filter = "Game JSON Files (*.json)|*.json;";

            bool? result = dialog.ShowDialog();

            if (result == true)
            {
                string path = dialog.FileName;
                this.controller.SaveGame(path);
                this.RedrawAll();
            }
        }
    }
}
