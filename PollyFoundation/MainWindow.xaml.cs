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
    public sealed partial class MainWindow : Window
    {
        private const double CanvasMargin = 10;
        private double xmax;
        private double xmin;
        private double ymin;
        private double ymax;
        private double squareLength;
        private double scale;

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

            this.RefreshCanvasInfo(csi);

            this.DrawCoordinateSystem(csi);

            /*
            // Make some data sets.
            Brush[] brushes = { Brushes.Red, Brushes.Green, Brushes.Blue };
            Random rand = new Random();
            for (int data_set = 0; data_set < 3; data_set++)
            {
                int last_y = rand.Next((int)this.ymin, (int)this.ymax);

                PointCollection points = new PointCollection();
                for (double x = this.xmin; x <= this.xmax; x += step)
                {
                    last_y = rand.Next(last_y - 10, last_y + 10);
                    if (last_y < this.ymin) last_y = (int)this.ymin;
                    if (last_y > this.ymax) last_y = (int)this.ymax;
                    points.Add(new Point(x, last_y));
                }

                Polyline polyline = new Polyline();
                polyline.StrokeThickness = 1;
                polyline.Stroke = brushes[data_set];
                polyline.Points = points;

                this.canvas.Children.Add(polyline);
            }
            */
        }

        private void RefreshCanvasInfo(CoordinateSystemInfo csi)
        {
            this.squareLength = Math.Min(this.canvas.Width, this.canvas.Height) - 2 * CanvasMargin;
            this.scale = this.squareLength / (csi.EndX - csi.StartX);

            this.xmin = 0.5 * (this.canvas.Width - this.squareLength) + CanvasMargin;
            this.xmax = this.xmin + this.squareLength;
            this.ymin = 0.5 * (this.canvas.Height - this.squareLength) + CanvasMargin;
            this.ymax = this.ymin + this.squareLength;
        }

        private void DrawCoordinateSystem(CoordinateSystemInfo csi)
        {
            // Make the X axis.
            GeometryGroup xaxis_geom = new GeometryGroup();
            xaxis_geom.Children.Add(new LineGeometry(
                new Point(this.xmin, 0.5 * (this.ymin + this.ymax)), new Point(this.xmax, 0.5 * (this.ymin + this.ymax))));
            for (double x = csi.StartX + csi.TickInterval;
                x <= csi.EndX; x += csi.TickInterval)
            {
                xaxis_geom.Children.Add(new LineGeometry(
                    new Point(x * this.scale + this.xmin, 0.5 * (this.ymin + this.ymax) - CanvasMargin / 2),
                    new Point(x * this.scale + this.xmin, 0.5 * (this.ymin + this.ymax) + CanvasMargin / 2)));
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
