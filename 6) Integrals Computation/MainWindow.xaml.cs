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
            double h=0, h2=0, h4=0;
            if(methodType==0)//Прямокутників
            {
                double sum = 0;
                for(int i=1; i<=2*n-1; i+=2)
                {
                    sum += calculateFunction(a + (i * precision) / 2);
                    ++iterations;
                }
                result = precision * sum;

                //Порядок збіжності
                h = result;
                sum = 0;
                for (int i = 1; i <= n - 1; i += 2)
                {
                    sum += calculateFunction(a + (i * precision) / 2);
                }
                h2 = precision * sum;

                sum = 0;
                for (int i = 1; i <= n/2 - 1; i += 2)
                {
                    sum += calculateFunction(a + (i * precision) / 2);
                }
                h4 = precision * sum;
            }
            else if(methodType==1)//Трапецій
            {
                double sum = calculateFunction(a)+calculateFunction(b);
                iterations = 1;
                for(int i=1; i<=n-1; ++i)
                {
                    sum += 2 * calculateFunction(a + i * precision);
                    ++iterations;
                }
                result = (precision / 2) * sum;

                //Порядок збіжності
                h = result;
                sum = calculateFunction(a) + calculateFunction(b);
                for (int i = 1; i <= n/2 - 1; ++i)
                {
                    sum += 2 * calculateFunction(a + i * precision);
                }
                h2 = (precision / 2) * sum;
                sum = calculateFunction(a) + calculateFunction(b);
                for (int i = 1; i <= n / 4 - 1; ++i)
                {
                    sum += 2 * calculateFunction(a + i * precision);
                }
                h4 = (precision / 2) * sum;
            }
            else if(methodType==2)//Парабол
            {
                double sum = calculateFunction(a) + calculateFunction(b);
                iterations = 1;
                int duplicator = 4;
                for(int i=1; i<=2*n-1; ++i)
                {
                    sum += duplicator * calculateFunction(a + i * (precision/2));
                    duplicator = (duplicator == 4) ? 2 : 4;
                    ++iterations;
                }
                result = (precision / 6) * sum;

                //Порядок збіжності
                h = result;
                sum = calculateFunction(a) + calculateFunction(b);
                for (int i = 1; i <= n - 1; ++i)
                {
                    sum += duplicator * calculateFunction(a + i * (precision / 2));
                    duplicator = (duplicator == 4) ? 2 : 4;
                }
                h2 = (precision / 6) * sum;
                sum = calculateFunction(a) + calculateFunction(b);
                for (int i = 1; i <= n/2 - 1; ++i)
                {
                    sum += duplicator * calculateFunction(a + i * (precision / 2));
                    duplicator = (duplicator == 4) ? 2 : 4;
                }
                h4 = (precision / 6) * sum;
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
                
                //Порядок збіжності
                h = result;
                sum = 0;
                double tb = a + ((b - a) / 2);
                for (int i = 0; i < 4; ++i)
                {
                    double xi = (tb + a) / 2 + (t[i] * (tb - a)) / 2;
                    sum += c[i] * calculateFunction(xi);
                }
                h2 = sum * (tb - a) / 2;
                sum = 0;
                tb = a + ((tb - a) / 2);
                for (int i = 0; i < 4; ++i)
                {
                    double xi = (tb + a) / 2 + (t[i] * (tb - a)) / 2;
                    sum += c[i] * calculateFunction(xi);
                }
                h4 = sum * (tb - a) / 2;
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

                //Порядок збіжності
                h = result;
                sum = 0;
                double tb = a + ((b - a) / 2);
                for (int i = 0; i < 4; ++i)
                {
                    double xi = (tb + a) / 2 + (t[i] * (tb - a)) / 2;
                    sum += c[i] * calculateFunction(xi);
                }
                h2 = sum * (tb - a) / 2;
                sum = 0;
                tb = a + ((tb - a) / 2);
                for (int i = 0; i < 4; ++i)
                {
                    double xi = (tb + a) / 2 + (t[i] * (tb - a)) / 2;
                    sum += c[i] * calculateFunction(xi);
                }
                h4 = sum * (tb - a) / 2;
            }
            textBox3.Text = Math.Round(result,getRound(precision)).ToString();
            textBox4.Text = (calculatePervisna(b) - calculatePervisna(a)).ToString();
            label8.Content = iterations;
            label10.Content = Math.Log((h - h2) / (h2 - h4)) / Math.Log(2);
        }
    }
}
