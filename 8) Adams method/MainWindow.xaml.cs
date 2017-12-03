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

namespace _8__Adams_method
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
            double yprev = Convert.ToDouble(textBox2.Text);
            double ynext = 0;
            txt.Text += "Методи Адамса:\n";
            txt.Text += String.Format("{0}\t{1}\t\t\t{2}\t\t\t{3}\n", "xi", "yi*", "yi", "yi(точне)");
            double x = a;
            uint quantity = 0;
            double k1 = 0, k2 = 0, k3 = 0, k4 = 0;
            int steps = Convert.ToInt32(textBox6.Text);
            double yadamsnext = 0, yadamsprev = 0;

            while (x <= b)
            {
                k1 = h * calculateFunction(x, yprev);
                k2 = h * calculateFunction(x + h / 3, yprev + k1 / 3);
                k3 = h * calculateFunction(x + 2 * h / 3, yprev - k1 / 3 + k2);
                k4 = h * calculateFunction(x + h, yprev + k1 - k2 + k3);
                ynext = yprev + (k1 + 3 * k2 + 3 * k3 + k4) / 8;
                
                yadamsprev = ynext;
                
                for (int i = 0; i < steps; ++i)
                {
                    yadamsnext = yprev + (h / 2) * (calculateFunction(x + h, yadamsprev) + calculateFunction(x, yprev));
                    yadamsprev = yadamsnext;
                    
                    ++quantity;
                }

                x += h;
                txt.Text += String.Format("{0}\t{1}\t{2}\t{3}\n", x, ynext, yadamsnext, calculateExact(x));
                yprev = ynext;
                //yprev = yadamsnext;
                ++quantity;
            }
            txt.Text += String.Format("Кількість ітерацій: {0}\n", quantity);
            txt.Text += String.Format("Точність(|y(b)-y*(b)|): {0}", Math.Abs(yadamsnext - calculateExact(x)));
        }
    }
}
