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
			e1.Parameters["X"] = x;
			e1.Parameters["Y"] = y;
			return (double)e1.Evaluate();
		}
		private double calculateY(double x, double y)
		{
			e2.Parameters["X"] = x;
			e2.Parameters["Y"] = y;
			return (double)e2.Evaluate();
		}*/

		
		private double calculateX(double x, double y)
		{
			return 5 * Math.Log10(y) + 2 - y / 2;
        }
		private double calculateY(double x, double y)
		{
			return (5 / 6) * x + (10 / 3) * Math.Log10(x) + 8 / 3;
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

			}
			else if(methodType==2)
			{

			}
			txt.Text += (string.Format("Розвязок: x={0}\ty={1}\nКількість ітерацій: {2}", Math.Round(xCur, getRound(precision)), Math.Round(yCur, getRound(precision)), count));
		}

		//Graphic
		private void button1_Click(object sender, RoutedEventArgs e)
		{

		}

		private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			methodType = Convert.ToUInt32(comboBox.SelectedIndex);
		}
	}
}
