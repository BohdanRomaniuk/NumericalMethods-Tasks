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
using NCalc;

namespace _3__Simple__Iteration_and_Newton_SNLE
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private uint methodType;
		private NCalc.Expression e1;
		private NCalc.Expression e2;
		public MainWindow()
		{
			InitializeComponent();
			methodType = 0;
			e1 = new NCalc.Expression(textBox1.Text);
			e2 = new NCalc.Expression(textBox2.Text);
			Style noSpaceStyle = new Style(typeof(Paragraph));
			noSpaceStyle.Setters.Add(new Setter(Paragraph.MarginProperty, new Thickness(0)));
			richTextBox.Resources.Add(typeof(Paragraph), noSpaceStyle);
		}
		
		private int getRound(double precision)
		{
			int roundCount = 0;
			do
			{
				precision *= 10;
				++roundCount;
			} while (precision != 1);
			return roundCount;
		}
		/*
		private double calculateX(double x, double y)
		{
			//return Math.Sin(0.5 * (x - y)) / 2;
			return 5 * Math.Log10(y) + 2 - y / 2;
		}
		private double calculateY(double x, double y)
		{
			//return Math.Cos(0.5 * (x + y)) / 2;
			return 5 * (x / 6) + 10 * (Math.Log10(x) / 3) + 8 / 3;
		}
		*/

		private double calculateX(double x, double y)
		{
			e1.Parameters["X"] = x;
			e1.Parameters["Y"] = y;
			return (double)e1.Evaluate();
		}

		private double calculateY(double x, double y)
		{
			e2.Parameters["X"] = x;
			e2.Parameters["Y"] = y;
			return (double)e2.Evaluate();
		}

		private double calculateF1(double x, double y)
		{
			//return 2 * x - Math.Sin(0.5 * (x - y));
			return 5 * x - 6 * y + 20 * Math.Log10(x) + 16;
		}

		private double calculateF2(double x, double y)
		{
			//return 2 * y - Math.Cos(0.5 * (x + y));
			return 2 * x + y - 10 * Math.Log10(y) - 4;
		}

		private double[,] calculateJacobiMatrix(double x, double y)
		{
			double[,] result = new double[2, 2];
			/*
			result[0, 0] = 2 - 0.5 * Math.Cos(0.5 * (x - y));
			result[0, 1] = 0.5 * Math.Cos(0.5 * (x - y));
			result[1, 0] = 0.5 * Math.Sin(0.5 * (x + y));
			result[1, 1] = 2 + 0.5 * Math.Sin(0.5 * (x + y));
			*/
			result[0, 0] = 5 + 20 / (x * Math.Log(10));
			result[0, 1] = -6;
			result[1, 0] = 2;
			result[1, 1] = 1 - 10 / (y * Math.Log(10));
			return result;
		}
		private double[,] calculateInvertMatrix(double[,] matrix, double precision)
		{
			int num_rows = matrix.GetUpperBound(0) + 1;
			double[,] augmented = new double[num_rows, 2 * num_rows];
			for (int row = 0; row < num_rows; row++)
			{
				for (int col = 0; col < num_rows; col++)
					augmented[row, col] = matrix[row, col];
				augmented[row, row + num_rows] = 1;
			}
			int num_cols = 2 * num_rows;
			for (int row = 0; row < num_rows; row++)
			{
				if (Math.Abs(augmented[row, row]) < precision)
				{
					for (int r2 = row + 1; r2 < num_rows; r2++)
					{
						if (Math.Abs(augmented[r2, row]) > precision)
						{
							for (int c = 0; c < num_cols; c++)
							{
								double tmp = augmented[row, c];
								augmented[row, c] = augmented[r2, c];
								augmented[r2, c] = tmp;
							}
							break;
						}
					}
				}
				
				if (Math.Abs(augmented[row, row]) > precision)
				{
					for (int col = 0; col < num_cols; col++)
						if (col != row)
							augmented[row, col] /= augmented[row, row];
					augmented[row, row] = 1;
					for (int row2 = 0; row2 < num_rows; row2++)
					{
						if (row2 != row)
						{
							double factor = augmented[row2, row] / augmented[row, row];
							for (int col = 0; col < num_cols; col++)
								augmented[row2, col] -= factor * augmented[row, col];
						}
					}
				}
			}

			// Якщо немає розвязку повернути null
			if (augmented[num_rows - 1, num_rows - 1] == 0)
			{
				return null;
			}
			double[,] inverse = new double[num_rows, num_rows];
			for (int row = 0; row < num_rows; row++)
			{
				for (int col = 0; col < num_rows; col++)
				{
					inverse[row, col] = augmented[row, col + num_rows];
				}
			}
			return inverse;
		}
		private static double[] multiplyMatrixAndVec(double[,] matrix, double[] vec)
		{
			double[] multi = new double[vec.Length];
			for (uint i = 0; i < matrix.GetLength(0); ++i)
			{
				for (uint j = 0; j < matrix.GetLength(1); ++j)
				{
					multi[i] += matrix[i, j] * vec[j];
				}
			}
			return multi;
		}

		//Calculations
		private void button_Click(object sender, RoutedEventArgs e)
		{
			TextRange txt = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd);
			txt.Text = "";
			double precision = Convert.ToDouble(textBox.Text);
			e1 = new NCalc.Expression(textBox1.Text);
			e2 = new NCalc.Expression(textBox2.Text);
			double xPrev = Convert.ToDouble(textBox3.Text);
			double yPrev = Convert.ToDouble(textBox4.Text);
			double xCur = 0, yCur = 0;
			uint count = 0;
			if (methodType==0)
			{
				double xOld = 0, yOld = 0;
				do
				{
					xCur = calculateX(xPrev, yPrev);
					yCur = calculateY(xPrev, yPrev);
					txt.Text += (string.Format("Наближення №{1}: x{1}=g1(x{0},y{0}) y{1}=g2(x{0},y{0})\nx{1}={2}\ty{1}={3}\n", count++, count, xCur, yCur));
					xOld = xPrev;
					yOld = yPrev;
					xPrev = xCur;
					yPrev = yCur;
				} while (Math.Abs(xCur - xOld) >= precision && Math.Abs(yCur - yOld) >= precision);
			}
			else if(methodType==1)
			{
				double xOld = 0, yOld = 0;
				do
				{
					double[,] matrix = new double[2, 2];
					double[,] invertMatrix = new double[2, 2];
					matrix = calculateJacobiMatrix(xPrev, yPrev);
					invertMatrix = calculateInvertMatrix(matrix, precision);
					double[] func = new double[2];
					func[0] = calculateF1(xPrev, yPrev);
					func[1] = calculateF2(xPrev, yPrev);
					double[] result = new double[2];
					result = multiplyMatrixAndVec(invertMatrix, func);
					xCur = xPrev - result[0];
					yCur = yPrev - result[1];
					count++;
					txt.Text += "Матриця Якобі №"+count+":\n[" + matrix[0, 0] + " " + matrix[0, 1] + "]\n";
					txt.Text += "[" + matrix[1, 0] + " " + matrix[1, 1] + "]\n\n";
					txt.Text += "Обернена матриця Якобі №" + count + ":\n[" + invertMatrix[0, 0] + " " + invertMatrix[0, 1] + "]\n";
					txt.Text += "[" + invertMatrix[1, 0] + " " + invertMatrix[1, 1] + "]\n\n";
					txt.Text += (string.Format("Наближення №{1}: \nF1(x{0},y{0})={4}\nF2(x{0},y{0})={5}\nx{1}={2}\ty{1}={3}\n\n", count-1 , count, xCur, yCur, func[0] , func[1]));
					xOld = xPrev;
					yOld = yPrev;
					xPrev = xCur;
					yPrev = yCur;
				} while (Math.Abs(xCur - xOld) >= precision && Math.Abs(yCur - yOld) >= precision);
			}
			else if(methodType==2)
			{
				double xOld = 0, yOld = 0;
				double[,] matrix = new double[2, 2];
				double[,] invertMatrix = new double[2, 2];
				matrix = calculateJacobiMatrix(xPrev, yPrev);
				invertMatrix = calculateInvertMatrix(matrix, precision);
				txt.Text += "Матриця Якобі:\n[" + matrix[0, 0] + " " + matrix[0, 1] + "]\n";
				txt.Text += "[" + matrix[1, 0] + " " + matrix[1, 1] + "]\n\n";
				txt.Text += "Обернена матриця Якобі:\n[" + invertMatrix[0, 0] + " " + invertMatrix[0, 1] + "]\n";
				txt.Text += "[" + invertMatrix[1, 0] + " " + invertMatrix[1, 1] + "]\n\n";
				do
				{
					double[] func = new double[2];
					func[0] = calculateF1(xPrev, yPrev);
					func[1] = calculateF2(xPrev, yPrev);
					double[] result = new double[2];
					result = multiplyMatrixAndVec(invertMatrix, func);
					xCur = xPrev - result[0];
					yCur = yPrev - result[1];
					txt.Text += (string.Format("Наближення №{1}: \nF1(x{0},y{0})={4}\nF2(x{0},y{0})={5}\nx{1}={2}\ty{1}={3}\n\n", count++, count, xCur, yCur, func[0], func[1]));
					xOld = xPrev;
					yOld = yPrev;
					xPrev = xCur;
					yPrev = yCur;
				} while (Math.Abs(xCur - xOld) >= precision && Math.Abs(yCur - yOld) >= precision);
			}
			txt.Text += (string.Format("Розвязок: x={0}\ty={1}\nКількість ітерацій: {2}", Math.Round(xCur, getRound(precision)), Math.Round(yCur, getRound(precision)), count));
		}

		//Graphic
		private void button1_Click(object sender, RoutedEventArgs e)
		{
			double ymax = canGraph.Height - 20;
			double xmax = canGraph.Width - 20;
			double xCoord = Convert.ToDouble(textBox3.Text);
			double yCoord = Convert.ToDouble(textBox4.Text);

			PointCollection points1 = new PointCollection();
			
			points1.Add(new Point(20, ymax - yCoord*20));
			points1.Add(new Point(xmax, ymax - yCoord*20));

			Polyline polyline1 = new Polyline();
            polyline1.StrokeThickness = 1;
			polyline1.Stroke = Brushes.Red;
			polyline1.Points = points1;

			PointCollection points2 = new PointCollection();

			points2.Add(new Point(20+xCoord * 20, 20));
			points2.Add(new Point(20+xCoord * 20, ymax));

			Polyline polyline2 = new Polyline();
			polyline2.StrokeThickness = 1;
			polyline2.Stroke = Brushes.Red;
			polyline2.Points = points2;

			canGraph.Children.Add(polyline1);
			canGraph.Children.Add(polyline2);
		}

		private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			methodType = Convert.ToUInt32(comboBox.SelectedIndex);
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
