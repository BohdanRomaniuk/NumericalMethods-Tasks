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
			double y = 0;
			for(int i=0; i<quantity; ++i)
			{
				y += polynomial[i] * Math.Pow(x, i);
			}
			yRes.Text = y.ToString();
		}

		private void drawGraphic_Click(object sender, RoutedEventArgs e)
		{

		}
	}
}
