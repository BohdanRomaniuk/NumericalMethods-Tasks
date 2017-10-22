using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _4__Lagrange_Polynomial
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private double[] vectorX;
		private double[] vectorY;
		private double[] polynomial;
		private int quantity;
		public MainWindow()
		{
			InitializeComponent();
		}
		private double[] multiplyPolynoms(double[] first, double[] second)
		{
			double[] result = new double[first.Length + second.Length-1];
			for(int i=0; i<first.Length; ++i)
			{
				for(int j=0; j<second.Length; ++j)
				{
					result[i + j] += first[i] * second[j];
				}
			}
			return result;
		}

		private void calculatePolynom_Click(object sender, RoutedEventArgs e)
		{
			lagrangePolynom.Text = "";
			string[] inputXLineElems = xVector.Text.Split(' ');
			string[] inputYLineElems = yVector.Text.Split(' ');
			quantity = inputXLineElems.Length;
			vectorX = new double[quantity];
			vectorY = new double[quantity];
			for (int i = 0; i < quantity; ++i)
			{
				vectorX[i] = Convert.ToDouble(inputXLineElems[i]);
				vectorY[i] = Convert.ToDouble(inputYLineElems[i]);
			}

			
			double[] numerator = new double[1];
			polynomial = new double[quantity];
			double denominator;
			for (int i = 0; i < quantity; ++i)
			{
				numerator = new double[1] { vectorY[i] };
				denominator = 1;
				for (int j = 0; j < quantity; ++j)
				{
					if (i != j)
					{
						double[] newNum = new double[2] { -vectorX[j], 1 };
						numerator = multiplyPolynoms(numerator, newNum);
						denominator *= vectorX[i] - vectorX[j];
					}
				}
				for(int j=0; j<numerator.Length; ++j)
				{
					numerator[j] /= denominator;
					polynomial[j] += numerator[j];
				}
			}
			lagrangePolynom.Text += String.Format("L{0}(x)=", quantity-1);
			for (int i=quantity-1; i>=0; --i)
			{
				if(polynomial[i]!=0)
				lagrangePolynom.Text += String.Format("{0}{1}{2}",(polynomial[i]>=0 && i!=quantity-1)?"+":"", Math.Round(polynomial[i],2),(i!=0)?"x^"+i:"");
			}

		}

		private void calCulateYFromX_Click(object sender, RoutedEventArgs e)
		{
			double x = Convert.ToDouble(xCur.Text);
			yRes.Text = calculateValueFromX(x).ToString();
		}

		private double calculateValueFromX(double x)
		{
			double y = 0;
			for (int i = 0; i < quantity; ++i)
			{
				y += polynomial[i] * Math.Pow(x, i);
			}
			return y;
		}

		private void drawGraphic_Click(object sender, RoutedEventArgs e)
		{
			double ymax = canGraph.Height - 20;
			double xmax = canGraph.Width - 20;
			PointCollection points = new PointCollection();

			for(uint x=20; x<xmax; ++x)
			{
				double y = calculateValueFromX((double)x/20-1);
				points.Add(new Point(x, ymax - y * 20));
			}

			Polyline polyline = new Polyline();
			polyline.StrokeThickness = 1;
			polyline.Stroke = Brushes.Blue;
			polyline.Points = points;
			canGraph.Children.Add(polyline);
			MessageBox.Show("Графік побудовано", "Важлива інформація");
		}

		private void LoadGraphic(object sender, RoutedEventArgs e)
		{
			const double margin = 20;
			double xmin = margin;
			double xmax = canGraph.Width - margin;
			double ymin = margin;
			double ymax = canGraph.Height - margin;
			const double step = 20;

			// Make the X axis.
			GeometryGroup xaxis_geom = new GeometryGroup();
			xaxis_geom.Children.Add(new LineGeometry(
				new Point(0, ymax), new Point(canGraph.Width, ymax)));
			for (double x = xmin + step; x <= canGraph.Width - step; x += step)
			{
				xaxis_geom.Children.Add(new LineGeometry(new Point(x, ymax - margin / 4), new Point(x, ymax + margin / 4)));
				DrawText(canGraph, (x / 20 - 1).ToString(), new Point(x, ymax + margin / 4), 12, HorizontalAlignment.Center, VerticalAlignment.Top);
			}

			Path xaxis_path = new Path();
			xaxis_path.StrokeThickness = 1;
			xaxis_path.Stroke = Brushes.Black;
			xaxis_path.Data = xaxis_geom;

			canGraph.Children.Add(xaxis_path);

			// Make the Y ayis.
			GeometryGroup yaxis_geom = new GeometryGroup();
			yaxis_geom.Children.Add(new LineGeometry(
				new Point(xmin, 0), new Point(xmin, canGraph.Height)));
			int count = 13;
			for (double y = ymin + step; y <= canGraph.Height - step; y += step)
			{
				yaxis_geom.Children.Add(new LineGeometry(new Point(xmin - margin / 4, y), new Point(xmin + margin / 4, y)));
				DrawText(canGraph, (count--).ToString(), new Point(xmin - margin, y - 35), 12, HorizontalAlignment.Center, VerticalAlignment.Top);
			}

			Path yaxis_path = new Path();
			yaxis_path.StrokeThickness = 1;
			yaxis_path.Stroke = Brushes.Black;
			yaxis_path.Data = yaxis_geom;

			canGraph.Children.Add(yaxis_path);
		}
		private void DrawText(Canvas can, string text, Point location,
			double font_size,
			HorizontalAlignment halign, VerticalAlignment valign)
		{
			// Make the label.
			Label label = new Label();
			label.Content = text;
			label.FontSize = font_size;
			can.Children.Add(label);

			// Position the label.
			label.Measure(new Size(double.MaxValue, double.MaxValue));

			double x = location.X;
			if (halign == HorizontalAlignment.Center)
				x -= label.DesiredSize.Width / 2;
			else if (halign == HorizontalAlignment.Right)
				x -= label.DesiredSize.Width;
			Canvas.SetLeft(label, x);

			double y = location.Y;
			if (valign == VerticalAlignment.Center)
				y -= label.DesiredSize.Height / 2;
			else if (valign == VerticalAlignment.Bottom)
				y -= label.DesiredSize.Height;
			Canvas.SetTop(label, y);
		}
	}
}
