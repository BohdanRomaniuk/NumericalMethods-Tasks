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

            }
            else if(methodType==4)//Гауса пятиточкова
            {

            }
            textBox3.Text = Math.Round(result,getRound(precision)).ToString();
            textBox4.Text = (calculatePervisna(b) - calculatePervisna(a)).ToString();
            label8.Content = iterations;
        }
    }
}
