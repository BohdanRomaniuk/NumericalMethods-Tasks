using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

namespace _6__Integrals_Computation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        NCalc.Expression func;
        NCalc.Expression perv;
        double a;
        double b;
        double precision;
        public MainWindow()
        {
            InitializeComponent();
            func = new NCalc.Expression(textBox.Text);
            perv = new NCalc.Expression(textBox1.Text);
            a = Convert.ToDouble(textBox2.Text);
            b = Convert.ToDouble(textBox2_Copy.Text);
            precision = Convert.ToDouble(textBox2_Copy1.Text);
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
        private double calculateFunction(double x)
        {
            func.Parameters["X"] = x;
            return (double)func.Evaluate();
        }
        private double calculatePervisna(double x)
        {
            //return 1 / Math.Sqrt(5) * (Math.Log(x + Math.Sqrt(Math.Pow(x, 2) - 0.08)));
            perv.Parameters["X"] = x;
            return (double)perv.Evaluate();
        }
        /*
        private int fac(int a)
        {
            int result = 1;
            for(int i=1; i<=a; ++i)
            {
                result*= i;
            }
            return result;
        }
        private double[] multiplyPolynoms(double[] first, double[] second)
        {
            double[] result = new double[first.Length + second.Length - 1];
            for (int i = 0; i < first.Length; ++i)
            {
                for (int j = 0; j < second.Length; ++j)
                {
                    result[i + j] += first[i] * second[j];
                }
            }
            return result;
        }
        private double[] sumPolynoms(double[] first, double[] second)
        {
            double[] result = new double[(first.Length > second.Length) ? first.Length : second.Length];
            for (int i = 0; i < first.Length; ++i)
            {
                result[i] = first[i];
            }
            for (int i = 0; i < second.Length; ++i)
            {
                result[i] = second[i];
            }
            return result;
        }
        private double differential(double[] coefficients, int numCoefficients, double x)
        {
            double ret = 0;
            for (int i = 0; i < numCoefficients; ++i)
                ret += x * i * Math.Pow(coefficients[i], i - 1);
            return ret;
        }
        private double calculateCoeficient(double xk,int n,double a,double b)
        {
            double result = (Math.Pow(fac(n),4)*Math.Pow(b-a,2*n-1))/ (Math.Pow(fac(2*n),2)*(xk-a)*(b-xk));
            return result;
        }
        private double calculatCoeficient(double xk, int n, double a, double b)
        {
            return 1;
        }
        */
        private void button_Click(object sender, RoutedEventArgs e)
        {
            func = new NCalc.Expression(textBox.Text);
            perv = new NCalc.Expression(textBox1.Text);
            a = Convert.ToDouble(textBox2.Text);
            b = Convert.ToDouble(textBox2_Copy.Text);
            precision = Convert.ToDouble(textBox2_Copy1.Text);
            int methodType = Convert.ToInt32(comboBox.SelectedIndex);
            uint iterations = 0;
            double result = 0;
            int n = (int)((b - a)/precision);
            if(methodType==0)//Прямокутників
            {
                double sum = 0;
                for(int i=1; i<=2*n-1; i+=2)
                {
                    sum += calculateFunction(a + (i * precision) / 2);
                    ++iterations;
                }
                result = precision * sum;
            }
            else if(methodType==1)//Трапецій
            {
                double sum = calculateFunction(a)+calculateFunction(b);
                iterations = 2;
                for(int i=1; i<=n-1; ++i)
                {
                    sum += 2 * calculateFunction(a + i * precision);
                    ++iterations;
                }
                result = (precision / 2) * sum;
            }
            else if(methodType==2)//Парабол
            {
                double sum = calculateFunction(a) + calculateFunction(b);
                iterations = 2;
                int duplicator = 4;
                for(int i=1; i<=2*n-1; ++i)
                {
                    sum += duplicator * calculateFunction(a + i * (precision/2));
                    duplicator = (duplicator == 4) ? 2 : 4;
                    ++iterations;
                }
                result = (precision / 6) * sum;
            }
            else if(methodType==3)//Гауса чотириточкова
            {
                double[] t = new double[4];
                t[0] = -0.8611363;
                t[1] = -0.339981;
                t[2] = 0.339981;
                t[3] = 0.861136;
                double[] c = new double[4];
                c[0] = 0.3478548;
                c[1] = 0.6521452;
                c[2] = 0.6521452;
                c[3] = 0.3478548;
                double sum = 0;
                for(int i=0; i<4; ++i)
                {
                    double xi = (b + a) / 2 + (t[i] * (b - a)) / 2;
                    sum += c[i] * calculateFunction(xi);
                    ++iterations;
                }
                result = sum * (b - a) / 2;
            }
            else if(methodType==4)//Гауса пятиточкова
            {
                double[] t = new double[5];
                t[0] = -0.9061798;
                t[1] = -0.5384693;
                t[2] = 0;
                t[3] = 0.5384693;
                t[4] = 0.9061798;
                double[] c = new double[5];
                c[0] = 0.2369269;
                c[1] = 0.4786287;
                c[2] = 0.5688889;
                c[3] = 0.4786287;
                c[4] = 0.2369269;
                double sum = 0;
                for (int i = 0; i < 5; ++i)
                {
                    double xi = (b + a) / 2 + (t[i] * (b - a)) / 2;
                    sum += c[i] * calculateFunction(xi);
                    ++iterations;
                }
                result = sum * (b - a) / 2;
            }
            textBox3.Text = Math.Round(result,getRound(precision)).ToString();
            textBox4.Text = (calculatePervisna(b) - calculatePervisna(a)).ToString();
            label8.Content = iterations;
        }
    }
}
