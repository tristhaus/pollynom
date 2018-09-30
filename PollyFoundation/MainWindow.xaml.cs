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
            this.coordinateHelper = new CoordinateHelper(this.canvas.ActualWidth, this.canvas.ActualHeight, CanvasMargin, 20);

            this.canvas.Children.Clear();

            this.DrawCoordinateSystem();
        }

        private void DrawCoordinateSystem()
        {
            // Make the X axis.
            GeometryGroup xaxis_geom = new GeometryGroup();
            xaxis_geom.Children.Add(new LineGeometry(this.coordinateHelper.Convert(10.5, 0), this.coordinateHelper.Convert(-10.5, 0)));
            for (int i = 1; i <= 10; i++)
            {
                xaxis_geom.Children.Add(new LineGeometry(this.coordinateHelper.Convert(+i, -0.3), this.coordinateHelper.Convert(+i, +0.3)));
                xaxis_geom.Children.Add(new LineGeometry(this.coordinateHelper.Convert(-i, -0.3), this.coordinateHelper.Convert(-i, +0.3)));
            }

            xaxis_geom.Children.Add(new LineGeometry(this.coordinateHelper.Convert(10.5, 0.0), this.coordinateHelper.Convert(10.2, -0.3)));
            xaxis_geom.Children.Add(new LineGeometry(this.coordinateHelper.Convert(10.5, 0.0), this.coordinateHelper.Convert(10.2, +0.3)));

            Path xaxis_path = new Path();
            xaxis_path.StrokeThickness = 1;
            xaxis_path.Stroke = Brushes.Black;
            xaxis_path.Data = xaxis_geom;

            this.canvas.Children.Add(xaxis_path);

            // Make the Y axis.
            GeometryGroup yaxis_geom = new GeometryGroup();
            yaxis_geom.Children.Add(new LineGeometry(this.coordinateHelper.Convert(0, 10.5), this.coordinateHelper.Convert(0, -10.5)));
            for (int i = 1; i <= 10; i++)
            {
                yaxis_geom.Children.Add(new LineGeometry(this.coordinateHelper.Convert(-0.3, +i), this.coordinateHelper.Convert(+0.3, +i)));
                yaxis_geom.Children.Add(new LineGeometry(this.coordinateHelper.Convert(-0.3, -i), this.coordinateHelper.Convert(+0.3, -i)));
            }

            yaxis_geom.Children.Add(new LineGeometry(this.coordinateHelper.Convert(0.0, 10.5), this.coordinateHelper.Convert(-0.3, 10.2)));
            yaxis_geom.Children.Add(new LineGeometry(this.coordinateHelper.Convert(0.0, 10.5), this.coordinateHelper.Convert(+0.3, 10.2)));

            Path yaxis_path = new Path();
            yaxis_path.StrokeThickness = 1;
            yaxis_path.Stroke = Brushes.Black;
            yaxis_path.Data = yaxis_geom;

            this.canvas.Children.Add(yaxis_path);
        }
    }
}
