using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Shapes;
using Backend.Controller;

namespace PollyFoundation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        private const double CanvasMargin = 10;
        private readonly Color[] graphColors = { Colors.Black, Colors.Blue, Colors.Green, Colors.Pink, Colors.Brown };

        private CoordinateHelper coordinateHelper;
        private PollyController controller;

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
            this.InitializeComponent();

            this.ConstructLayout();
            this.RedrawAll();

            this.button0.Click += this.Button0_Click;
            this.SizeChanged += this.HandleSizeChanged;
            this.dpForCanvas.SizeChanged += this.HandleSizeChanged;
            this.textBox0.KeyDown += this.TextBox0_KeyDown;
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

        private void TextBox0_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                this.UpdateExpression();
            }
        }

        private void Button0_Click(object sender, RoutedEventArgs e)
        {
            this.UpdateExpression();
        }

        private void UpdateExpression()
        {
            this.controller.UpdateExpression(this.textBox0.Text);
            this.RedrawAll();
        }

        private void RedrawAll()
        {
            this.UpdateLayout();
            this.AdjustCanvasSize();

            this.canvas.Children.Clear();

            this.DrawCoordinateSystem();
            this.DrawGraphs();
        }

        private void AdjustCanvasSize()
        {
            this.gridForCanvas.Height = this.dpForCanvas.ActualHeight < this.dpForCanvas.ActualWidth ? this.dpForCanvas.ActualHeight : this.dpForCanvas.ActualWidth;
            this.gridForCanvas.Width = this.dpForCanvas.ActualHeight < this.dpForCanvas.ActualWidth ? this.dpForCanvas.ActualHeight : this.dpForCanvas.ActualWidth;
            this.coordinateHelper = new CoordinateHelper(this.canvas.ActualWidth, this.canvas.ActualHeight, CanvasMargin, 21.2);
        }

        private void DrawCoordinateSystem()
        {
            // Make the X axis.
            GeometryGroup xAxisGeometryGroup = new GeometryGroup();
            xAxisGeometryGroup.Children.Add(new LineGeometry(this.coordinateHelper.Convert(10.5, 0), this.coordinateHelper.Convert(-10.5, 0)));
            for (int i = 1; i <= 10; i++)
            {
                xAxisGeometryGroup.Children.Add(new LineGeometry(this.coordinateHelper.Convert(+i, -0.3), this.coordinateHelper.Convert(+i, +0.3)));
                xAxisGeometryGroup.Children.Add(new LineGeometry(this.coordinateHelper.Convert(-i, -0.3), this.coordinateHelper.Convert(-i, +0.3)));
            }

            xAxisGeometryGroup.Children.Add(new LineGeometry(this.coordinateHelper.Convert(10.5, 0.0), this.coordinateHelper.Convert(10.2, -0.3)));
            xAxisGeometryGroup.Children.Add(new LineGeometry(this.coordinateHelper.Convert(10.5, 0.0), this.coordinateHelper.Convert(10.2, +0.3)));

            Path xAxisPath = new Path();
            xAxisPath.StrokeThickness = 1;
            xAxisPath.Stroke = Brushes.Black;
            xAxisPath.Data = xAxisGeometryGroup;

            this.canvas.Children.Add(xAxisPath);

            // Make the Y axis.
            GeometryGroup yAxisGeometryGroup = new GeometryGroup();
            yAxisGeometryGroup.Children.Add(new LineGeometry(this.coordinateHelper.Convert(0, 10.5), this.coordinateHelper.Convert(0, -10.5)));
            for (int i = 1; i <= 10; i++)
            {
                yAxisGeometryGroup.Children.Add(new LineGeometry(this.coordinateHelper.Convert(-0.3, +i), this.coordinateHelper.Convert(+0.3, +i)));
                yAxisGeometryGroup.Children.Add(new LineGeometry(this.coordinateHelper.Convert(-0.3, -i), this.coordinateHelper.Convert(+0.3, -i)));
            }

            yAxisGeometryGroup.Children.Add(new LineGeometry(this.coordinateHelper.Convert(0.0, 10.5), this.coordinateHelper.Convert(-0.3, 10.2)));
            yAxisGeometryGroup.Children.Add(new LineGeometry(this.coordinateHelper.Convert(0.0, 10.5), this.coordinateHelper.Convert(+0.3, 10.2)));

            Path yAxisPath = new Path();
            yAxisPath.StrokeThickness = 1.5;
            yAxisPath.Stroke = Brushes.Black;
            yAxisPath.Data = yAxisGeometryGroup;

            this.canvas.Children.Add(yAxisPath);
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
                        polyLine.Points.Add(this.coordinateHelper.Convert(point.X, point.Y));
                    }

                    polyLine.Stroke = brush;
                    polyLine.StrokeThickness = 1;
                    this.canvas.Children.Add(polyLine);
                }
            }
        }
    }
}
