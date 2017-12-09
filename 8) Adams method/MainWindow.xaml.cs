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

        private void explicitMethod(uint rank)
        {
            TextRange txt = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd);
            txt.Text = "";
            funct = new NCalc.Expression(textBox.Text);
            exact = new NCalc.Expression(textBox1.Text);
            double a = Convert.ToDouble(textBox3.Text);
            double b = Convert.ToDouble(textBox4.Text);
            double h = Convert.ToDouble(textBox5.Text);

            txt.Text += "Екстраполяційний Метод Адамса:\n";
            txt.Text += String.Format("{0}\t{1}\t\t\t{2}\t\t\t{3}\n", "xi", "yi*", "yi", "yi(точне)");
            double x = a;
            uint quantity = 0;
            double k1 = 0, k2 = 0, k3 = 0, k4 = 0;
            int steps = (int)((b-a)/h);
            double[] vals = new double[steps+1];
            vals[0] = Convert.ToDouble(textBox2.Text);
            txt.Text += String.Format("{0}\t{1}\t{2}\n", x, vals[0], calculateExact(x));

            switch(rank)
            {
                case 2:
                    {
                        //#####P=2
                        for (int i = 1; i < 2; ++i)
                        {
                            k1 = h * calculateFunction(x, vals[i - 1]);
                            k2 = h * calculateFunction(x + h / 3, vals[i - 1] + k1 / 3);
                            k3 = h * calculateFunction(x + 2 * h / 3, vals[i - 1] - k1 / 3 + k2);
                            k4 = h * calculateFunction(x + h, vals[i - 1] + k1 - k2 + k3);
                            vals[i] = vals[i - 1] + (k1 + 3 * k2 + 3 * k3 + k4) / 8;
                            x += h;
                            txt.Text += String.Format("{0}\t{1}\t{2}\n", x, vals[i], calculateExact(x));
                            ++quantity;
                        }
                        for (int i = 2; i <= steps; ++i)
                        {
                            vals[i] = vals[i - 1] + h * (1.5 * calculateFunction(x, vals[i - 1]) - 0.5 * calculateFunction(x - h, vals[i - 2]));
                            x += h;
                            txt.Text += String.Format("{0}\t{1}\t{2}\n", x, vals[i], calculateExact(x));
                            ++quantity;
                        }
                        break;
                    }
                case 3:
                    {
                        //#####P=3
                        for (int i = 1; i < 3; ++i)
                        {
                            k1 = h * calculateFunction(x, vals[i - 1]);
                            k2 = h * calculateFunction(x + h / 3, vals[i - 1] + k1 / 3);
                            k3 = h * calculateFunction(x + 2 * h / 3, vals[i - 1] - k1 / 3 + k2);
                            k4 = h * calculateFunction(x + h, vals[i - 1] + k1 - k2 + k3);
                            vals[i] = vals[i - 1] + (k1 + 3 * k2 + 3 * k3 + k4) / 8;
                            x += h;
                            txt.Text += String.Format("{0}\t{1}\t{2}\n", x, vals[i], calculateExact(x));
                            ++quantity;
                        }
                        for (int i = 3; i <= steps; ++i)
                        {
                            vals[i] = vals[i - 1] + (h / 12) * (23 * calculateFunction(x, vals[i - 1]) - 16 * calculateFunction(x - h, vals[i - 2]) + 5 * calculateFunction(x - 2 * h, vals[i - 3]));
                            x += h;
                            txt.Text += String.Format("{0}\t{1}\t{2}\n", x, vals[i], calculateExact(x));
                            ++quantity;
                        }
                        break;
                    }
                case 4:
                    {
                        //#####P=4
                        for (int i = 1; i < 4; ++i)
                        {
                            k1 = h * calculateFunction(x, vals[i - 1]);
                            k2 = h * calculateFunction(x + h / 3, vals[i - 1] + k1 / 3);
                            k3 = h * calculateFunction(x + 2 * h / 3, vals[i - 1] - k1 / 3 + k2);
                            k4 = h * calculateFunction(x + h, vals[i - 1] + k1 - k2 + k3);
                            vals[i] = vals[i - 1] + (k1 + 3 * k2 + 3 * k3 + k4) / 8;
                            x += h;
                            txt.Text += String.Format("{0}\t{1}\t{2}\n", x, vals[i], calculateExact(x));
                            ++quantity;
                        }
                        for (int i = 4; i <= steps; ++i)
                        {
                            vals[i] = vals[i - 1] + (h / 24) * (55 * calculateFunction(x, vals[i - 1]) - 59 * calculateFunction(x - h, vals[i - 2]) + 37 * calculateFunction(x - 2 * h, vals[i - 3]) - 9 * calculateFunction(x - 3 * h, vals[i - 4]));
                            x += h;
                            txt.Text += String.Format("{0}\t{1}\t{2}\n", x, vals[i], calculateExact(x));
                            ++quantity;
                        }
                        break;
                    }
                case 5:
                    {
                        //#####P=5
                        for (int i = 1; i < 5; ++i)
                        {
                            k1 = h * calculateFunction(x, vals[i - 1]);
                            k2 = h * calculateFunction(x + h / 3, vals[i - 1] + k1 / 3);
                            k3 = h * calculateFunction(x + 2 * h / 3, vals[i - 1] - k1 / 3 + k2);
                            k4 = h * calculateFunction(x + h, vals[i - 1] + k1 - k2 + k3);
                            vals[i] = vals[i - 1] + (k1 + 3 * k2 + 3 * k3 + k4) / 8;
                            x += h;
                            txt.Text += String.Format("{0}\t{1}\t{2}\n", x, vals[i], calculateExact(x));
                            ++quantity;
                        }
                        for (int i = 5; i <= steps; ++i)
                        {
                            vals[i] = vals[i - 1] + (h / 720) * (1901 * calculateFunction(x, vals[i - 1]) - 2774 * calculateFunction(x - h, vals[i - 2]) + 2616 * calculateFunction(x - 2 * h, vals[i - 3]) - 1274 * calculateFunction(x - 3 * h, vals[i - 4]) + 251 * calculateFunction(x - 4 * h, vals[i - 5]));
                            x += h;
                            txt.Text += String.Format("{0}\t{1}\t{2}\n", x, vals[i], calculateExact(x));
                            ++quantity;
                        }
                        break;
                    }
            }
            txt.Text += String.Format("Кількість ітерацій: {0}\n", quantity);
            txt.Text += String.Format("Точність(|y(b)-y*(b)|): {0}", Math.Abs(vals[steps] - calculateExact(x)));
        }

        private void implicitMethod(uint rank)
        {
            TextRange txt = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd);
            txt.Text = "";
            funct = new NCalc.Expression(textBox.Text);
            exact = new NCalc.Expression(textBox1.Text);
            double a = Convert.ToDouble(textBox3.Text);
            double b = Convert.ToDouble(textBox4.Text);
            double h = Convert.ToDouble(textBox5.Text);

            txt.Text += "Інтерполяційний Метод Адамса:\n";
            txt.Text += String.Format("{0}\t{1}\t\t\t{2}\t\t\t{3}\n", "xi", "yi*", "yi", "yi(точне)");
            double x = a;
            uint quantity = 0;
            double k1 = 0, k2 = 0, k3 = 0, k4 = 0;
            int steps = (int)((b-a)/h);
            int steps1 = Convert.ToInt32(textBox6.Text);
            double[] vals = new double[steps + 1];
            vals[0] = Convert.ToDouble(textBox2.Text);
            txt.Text += String.Format("{0}\t{1}\t{2}\n", x, vals[0], calculateExact(x));

            switch(rank)
            {
                case 2:
                    {
                        //#####M=2
                        //With Exmpicit Adams Method p=2
                        for (int i = 1; i < 2; ++i)
                        {
                            k1 = h * calculateFunction(x, vals[i - 1]);
                            k2 = h * calculateFunction(x + h / 3, vals[i - 1] + k1 / 3);
                            k3 = h * calculateFunction(x + 2 * h / 3, vals[i - 1] - k1 / 3 + k2);
                            k4 = h * calculateFunction(x + h, vals[i - 1] + k1 - k2 + k3);
                            vals[i] = vals[i - 1] + (k1 + 3 * k2 + 3 * k3 + k4) / 8;
                            x += h;
                            txt.Text += String.Format("{0}\t{1}\t{2}\n", x, vals[i], calculateExact(x));
                            ++quantity;
                        }
                        for (int i = 2; i <= steps; ++i)
                        {
                            vals[i] = vals[i - 1] + h * (1.5 * calculateFunction(x, vals[i - 1]) - 0.5 * calculateFunction(x - h, vals[i - 2]));

                            double yadamsprev = vals[i];
                            double yadamsnext = 0;
                            double yyy = vals[i - 1];
                            for (int j = 0; j < steps1; ++j)
                            {
                                yadamsnext = vals[i - 1] + (h / 2) * (calculateFunction(x + h, yadamsprev) + calculateFunction(x, vals[i - 1]));
                                yadamsprev = yadamsnext;
                                ++quantity;
                            }
                            yyy = yadamsnext;
                            x += h;
                            txt.Text += String.Format("{0}\t{1}\t{2}\t{3}\n", x, vals[i], yadamsnext, calculateExact(x));
                            vals[i] = yadamsnext;
                            ++quantity;
                        }
                        break;
                    }
                case 3:
                    {
                        //#####M=3
                        //With Exmpicit Adams Method p=2
                        for (int i = 1; i < 2; ++i)
                        {
                            k1 = h * calculateFunction(x, vals[i - 1]);
                            k2 = h * calculateFunction(x + h / 3, vals[i - 1] + k1 / 3);
                            k3 = h * calculateFunction(x + 2 * h / 3, vals[i - 1] - k1 / 3 + k2);
                            k4 = h * calculateFunction(x + h, vals[i - 1] + k1 - k2 + k3);
                            vals[i] = vals[i - 1] + (k1 + 3 * k2 + 3 * k3 + k4) / 8;
                            x += h;
                            txt.Text += String.Format("{0}\t{1}\t{2}\n", x, vals[i], calculateExact(x));
                            ++quantity;
                        }
                        for (int i = 2; i <= steps; ++i)
                        {
                            vals[i] = vals[i - 1] + h * (1.5 * calculateFunction(x, vals[i - 1]) - 0.5 * calculateFunction(x - h, vals[i - 2]));

                            double yadamsprev = vals[i];
                            double yadamsnext = 0;
                            for (int j = 0; j < steps1; ++j)
                            {
                                yadamsnext = vals[i - 1] + (h / 12) * (5 * calculateFunction(x + h, yadamsprev) + 8 * calculateFunction(x, vals[i - 1]) - calculateFunction(x - h, vals[i - 2]));
                                yadamsprev = yadamsnext;
                                ++quantity;
                            }
                            x += h;
                            txt.Text += String.Format("{0}\t{1}\t{2}\t{3}\n", x, vals[i], yadamsnext, calculateExact(x));
                            vals[i] = yadamsnext;
                            ++quantity;
                        }
                        break;
                    }
                case 4:
                    {
                        //#####M=4
                        //With Exmpicit Adams Method p=3
                        for (int i = 1; i < 3; ++i)
                        {
                            k1 = h * calculateFunction(x, vals[i - 1]);
                            k2 = h * calculateFunction(x + h / 3, vals[i - 1] + k1 / 3);
                            k3 = h * calculateFunction(x + 2 * h / 3, vals[i - 1] - k1 / 3 + k2);
                            k4 = h * calculateFunction(x + h, vals[i - 1] + k1 - k2 + k3);
                            vals[i] = vals[i - 1] + (k1 + 3 * k2 + 3 * k3 + k4) / 8;
                            x += h;
                            txt.Text += String.Format("{0}\t{1}\t{2}\n", x, vals[i], calculateExact(x));
                            ++quantity;
                        }
                        for (int i = 3; i <= steps; ++i)
                        {
                            vals[i] = vals[i - 1] + (h / 12) * (23 * calculateFunction(x, vals[i - 1]) - 16 * calculateFunction(x - h, vals[i - 2]) + 5 * calculateFunction(x - 2 * h, vals[i - 3]));

                            double yadamsprev = vals[i];
                            double yadamsnext = 0;
                            for (int j = 0; j < steps1; ++j)
                            {
                                yadamsnext = vals[i - 1] + (h / 24) * (9 * calculateFunction(x + h, yadamsprev) + 19 * calculateFunction(x, vals[i - 1]) - 5*calculateFunction(x - h, vals[i - 2]) + calculateFunction(x-2*h,vals[i-3]));
                                yadamsprev = yadamsnext;
                                ++quantity;
                            }
                            x += h;
                            txt.Text += String.Format("{0}\t{1}\t{2}\t{3}\n", x, vals[i], yadamsnext, calculateExact(x));
                            vals[i] = yadamsnext;
                            ++quantity;
                        }
                        break;
                    }
                case 5:
                    {
                        //#####M=5
                        //With Exmpicit Adams Method p=4
                        for (int i = 1; i < 4; ++i)
                        {
                            k1 = h * calculateFunction(x, vals[i - 1]);
                            k2 = h * calculateFunction(x + h / 3, vals[i - 1] + k1 / 3);
                            k3 = h * calculateFunction(x + 2 * h / 3, vals[i - 1] - k1 / 3 + k2);
                            k4 = h * calculateFunction(x + h, vals[i - 1] + k1 - k2 + k3);
                            vals[i] = vals[i - 1] + (k1 + 3 * k2 + 3 * k3 + k4) / 8;
                            x += h;
                            txt.Text += String.Format("{0}\t{1}\t{2}\n", x, vals[i], calculateExact(x));
                            ++quantity;
                        }
                        for (int i = 4; i <= steps; ++i)
                        {
                            vals[i] = vals[i - 1] + (h / 24) * (55 * calculateFunction(x, vals[i - 1]) - 59 * calculateFunction(x - h, vals[i - 2]) + 37 * calculateFunction(x - 2 * h, vals[i - 3]) - 9 * calculateFunction(x - 3 * h, vals[i - 4]));

                            double yadamsprev = vals[i];
                            double yadamsnext = 0;
                            for (int j = 0; j < steps1; ++j)
                            {
                                yadamsnext = vals[i - 1] + (h / 720) * (251 * calculateFunction(x + h, yadamsprev) + 646 * calculateFunction(x, vals[i - 1]) - 264 * calculateFunction(x - h, vals[i - 2]) + 106*calculateFunction(x - 2 * h, vals[i - 3])-19*calculateFunction(x-3*h, vals[i-4]));
                                yadamsprev = yadamsnext;
                                ++quantity;
                            }
                            x += h;
                            txt.Text += String.Format("{0}\t{1}\t{2}\t{3}\n", x, vals[i], yadamsnext, calculateExact(x));
                            vals[i] = yadamsnext;
                            ++quantity;
                        }
                        break;
                    }
            }
            txt.Text += String.Format("Кількість ітерацій: {0}\n", quantity);
            txt.Text += String.Format("Точність(|y(b)-y*(b)|): {0}", Math.Abs(vals[steps] - calculateExact(x)));
        }

        private void implicitMethodMy(uint rank)
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
            txt.Text += "Метод Адамса:\n";
            txt.Text += String.Format("{0}\t{1}\t\t\t{2}\t\t\t{3}\n", "xi", "yi*", "yi", "yi(точне)");
            double x = a;
            uint quantity = 0;
            double k1 = 0, k2 = 0, k3 = 0, k4 = 0;
            int steps = Convert.ToInt32(textBox6.Text);
            double yadamsnext = 0, yadamsprev = 0;
            
            double yyy = yprev;
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
                    yadamsnext = yyy + (h/2)* (calculateFunction(x + h, yadamsprev)+calculateFunction(x, yyy));
                    yadamsprev = yadamsnext;
                    ++quantity;
                }

                x += h;
                txt.Text += String.Format("{0}\t{1}\t{2}\t{3}\n", x, ynext, yadamsnext, calculateExact(x));
                yprev = ynext;
                yyy = yadamsnext;
                yprev = yadamsnext;
                ++quantity;
            }
            

            //while (x <= b)
            //{

            //    ynext = (yprev + (h / 2) * calculateFunction(x, yprev) / (1 - (h/2) * (x + h) / (Math.Pow(x + h, 2) + 1)));
            //    x += h;
            //    txt.Text += String.Format("{0}\t{1}\t{2}\n", x, ynext, calculateExact(x));
            //    yprev = ynext;
            //    ++quantity;
            //}

            txt.Text += String.Format("Кількість ітерацій: {0}\n", quantity);
            txt.Text += String.Format("Точність(|y(b)-y*(b)|): {0}", Math.Abs(yadamsnext - calculateExact(x)));
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            uint rank = Convert.ToUInt32(comboBox.SelectedIndex);
            if(rank<=3)
            {
                explicitMethod(rank+2);
            }
            else
            {
                implicitMethod(rank-2);
            }
        }

        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            uint rank = Convert.ToUInt32(comboBox.SelectedIndex);
            textBox6.IsEnabled = (rank <= 3)?false:true;
        }
    }
}
