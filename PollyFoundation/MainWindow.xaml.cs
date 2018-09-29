using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

using Backend.Controller;

namespace PollyFoundation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const double margin = 10;
        private const double step = 10;
        private double xmax;
        private double xmin;
        private double ymin;
        private double ymax;
        private double squareLength;
        private double scale;

        private PollyController controller;

        public MainWindow()
        {
            controller = new PollyController();
            InitializeComponent();
        }

        private void button0_Click(object sender, RoutedEventArgs e)
        {
            controller.UpdateExpression(textBox0.Text);

            CoordinateSystemInfo csi = controller.CoordinateSystemInfo;

            RefreshCanvasInfo(csi);

            DrawCoordinateSystem(csi);

            // Make some data sets.
            Brush[] brushes = { Brushes.Red, Brushes.Green, Brushes.Blue };
            Random rand = new Random();
            for (int data_set = 0; data_set < 3; data_set++)
            {
                int last_y = rand.Next((int)ymin, (int)ymax);

                PointCollection points = new PointCollection();
                for (double x = xmin; x <= xmax; x += step)
                {
                    last_y = rand.Next(last_y - 10, last_y + 10);
                    if (last_y < ymin) last_y = (int)ymin;
                    if (last_y > ymax) last_y = (int)ymax;
                    points.Add(new Point(x, last_y));
                }

                Polyline polyline = new Polyline();
                polyline.StrokeThickness = 1;
                polyline.Stroke = brushes[data_set];
                polyline.Points = points;

                this.canvas.Children.Add(polyline);
            }
        }

        private void RefreshCanvasInfo(CoordinateSystemInfo csi)
        {
            squareLength = Math.Min(this.canvas.Width, this.canvas.Height) - 2 * margin;
            scale = squareLength / (csi.EndX - csi.StartX);

            this.xmin = 0.5 * (this.canvas.Width - squareLength) + margin;
            this.xmax = xmin + squareLength;
            this.ymin = 0.5 * (this.canvas.Height - squareLength) + margin;
            this.ymax = ymin + squareLength;
        }

        private void DrawCoordinateSystem(CoordinateSystemInfo csi)
        {
            // Make the X axis.
            GeometryGroup xaxis_geom = new GeometryGroup();
            xaxis_geom.Children.Add(new LineGeometry(
                new Point(xmin, 0.5 * (ymin + ymax)), new Point(xmax, 0.5 * (ymin + ymax))));
            for (double x = csi.StartX + csi.TickInterval;
                x <= csi.EndX; x += csi.TickInterval)
            {
                xaxis_geom.Children.Add(new LineGeometry(
                    new Point(x * scale + xmin, 0.5 * (ymin + ymax) - margin / 2),
                    new Point(x * scale + xmin, 0.5 * (ymin + ymax) + margin / 2)));
            }

            Path xaxis_path = new Path();
            xaxis_path.StrokeThickness = 1;
            xaxis_path.Stroke = Brushes.Black;
            xaxis_path.Data = xaxis_geom;

            this.canvas.Children.Add(xaxis_path);

            /*
            // Make the Y ayis.
            GeometryGroup yaxis_geom = new GeometryGroup();
            yaxis_geom.Children.Add(new LineGeometry(
                new Point(xmin, 0), new Point(xmin, canvas.Height)));
            for (double y = step; y <= canvas.Height - step; y += step)
            {
                yaxis_geom.Children.Add(new LineGeometry(
                    new Point(xmin - margin / 2, y),
                    new Point(xmin + margin / 2, y)));
            }

            Path yaxis_path = new Path();
            yaxis_path.StrokeThickness = 1;
            yaxis_path.Stroke = Brushes.Black;
            yaxis_path.Data = yaxis_geom;

            this.canvas.Children.Add(yaxis_path);
            //*/   
        }
    }
}
