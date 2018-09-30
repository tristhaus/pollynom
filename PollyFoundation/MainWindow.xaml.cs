using System.Windows;
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

        public MainWindow()
        {
            this.controller = new PollyController();
            this.InitializeComponent();
        }

        private void Button0_Click(object sender, RoutedEventArgs e)
        {
            this.controller.UpdateExpression(this.textBox0.Text);

            CoordinateSystemInfo csi = this.controller.CoordinateSystemInfo;
            this.coordinateHelper = new CoordinateHelper(this.canvas.ActualWidth, this.canvas.ActualHeight, CanvasMargin, 21.2);

            this.canvas.Children.Clear();

            this.DrawCoordinateSystem();

            this.DrawGraphs();
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
