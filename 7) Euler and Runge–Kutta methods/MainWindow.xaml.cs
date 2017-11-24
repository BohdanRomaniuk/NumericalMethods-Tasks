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

namespace _7__Euler_and_Runge_Kutta_methods
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        NCalc.Expression funct;
        NCalc.Expression exact;
        public MainWindow()
        {
            InitializeComponent();
            funct = new NCalc.Expression(textBox.Text);
            exact = new NCalc.Expression(textBox1.Text);
        }
        private double calculateFunction(double x, double y)
        {
            funct.Parameters["X"] = x;
            funct.Parameters["Y"] = y;
            return (double)funct.Evaluate();
        }
        private double calculateExact(double x)
        {
            exact.Parameters["X"] = x;
            return (double)exact.Evaluate();
        }
        private void button_Click(object sender, RoutedEventArgs e)
        {
            TextRange txt = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd);
            txt.Text = "";
            funct = new NCalc.Expression(textBox.Text);
            exact = new NCalc.Expression(textBox1.Text);
            double a = Convert.ToDouble(textBox3.Text);
            double b = Convert.ToDouble(textBox4.Text);
            double h = Convert.ToDouble(textBox5.Text);
            uint methodType = Convert.ToUInt32(comboBox.SelectedIndex);
            double yprev = Convert.ToDouble(textBox2.Text);
            double ynext = 0;
            if (methodType==0)
            {
                txt.Text += "Метод Ейлера:\n";
                txt.Text += String.Format("{0}\t{1}\t\t\t{2}\n", "xi", "yi", "yi(точне)");
                double x = a;
                uint quantity = 0;
                do
                {
                    ynext = yprev + h * calculateFunction(x, yprev);
                    x += h;
                    txt.Text += String.Format("{0}\t{1}\t{2}\n", x, ynext, calculateExact(x));
                    yprev = ynext;
                    ++quantity;
                } while (x <= b);
                txt.Text += String.Format("Кількість ітерацій: {0}\n", quantity);
                txt.Text += String.Format("Точність(|y(b)-y*(b)|): {0}", Math.Abs(ynext - calculateExact(x)));
            }
            else if(methodType==1)
            {
                txt.Text += "Метод Рунге-Кутта 4-го порядку:\n";
                txt.Text += String.Format("{0}\t{1}\t\t\t{2}\n", "xi", "yi", "yi(точне)");
                double x = a;
                uint quantity = 0;
                double k1=0, k2=0, k3=0, k4=0;
                do
                {
                    /*
                    k1 = h * calculateFunction(x, yprev);
                    k2 = h * calculateFunction(x + h / 2, yprev + k1 / 2);
                    k3 = h * calculateFunction(x + h / 2, yprev + k2 / 2);
                    k4 = h * calculateFunction(x + h, yprev + k3);
                    ynext = yprev + (k1+2*k2+2*k3+k4)/6;
                    */
                    k1 = h * calculateFunction(x, yprev);
                    k2 = h * calculateFunction(x + h / 4, yprev + k1 / 4);
                    k3 = h * calculateFunction(x + h / 2, yprev + k2 / 2);
                    k4 = h * calculateFunction(x + h, yprev + k1 - 2 * k2 + 2 * k3);
                    ynext = yprev + (k1 + 4 * k3 + k4) / 6;
                    x += h;
                    txt.Text += String.Format("{0}\t{1}\t{2}\n", x, ynext, calculateExact(x));
                    yprev = ynext;
                    ++quantity;
                } while (x <= b);
                txt.Text += String.Format("Кількість ітерацій: {0}\n", quantity);
                txt.Text += String.Format("Точність(|y(b)-y*(b)|): {0}", Math.Abs(ynext - calculateExact(x)));
            }
        }
    }
}
