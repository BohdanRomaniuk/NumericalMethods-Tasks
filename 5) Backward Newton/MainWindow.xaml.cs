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

namespace _5__Backward_Newton
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private NCalc.Expression exp;
        private double[] vectorX;
        private double[] vectorY;
        private double[] polynomial;
        public MainWindow()
        {
            InitializeComponent();
            exp = new NCalc.Expression(function.Text);
            string[] xValues = textBox1.Text.Split(' ');
            vectorX = new double[xValues.Length];
            vectorY = new double[xValues.Length];
            for(int i=0; i<xValues.Length; ++i)
            {
                vectorX[i] = Convert.ToDouble(xValues[i]);
            }
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
        private double calculateFx(double x)
        {
            exp.Parameters["X"] = x;
            return (double)exp.Evaluate();
        }
        private void button_Click(object sender, RoutedEventArgs e)
        {
            exp = new NCalc.Expression(function.Text);
            for(int i=0; i<vectorX.Length; ++i)
            {
                vectorY[i] = calculateFx(vectorX[i]);
                textBox3.Text += vectorY[i] + " ";
            }
        }
        private double calculateSplitSub(int from, int to, double[] x_vector, double[] y_vector)
        {
            if(Math.Abs(to-from)==1)
            {
                return (y_vector[to] - y_vector[from]) / (x_vector[to] - x_vector[from]);
            }
            return (calculateSplitSub(from + 1, to, vectorX, vectorY) - calculateSplitSub(from,to-1, vectorX, vectorY)) / (x_vector[to]  - x_vector[from]);
        }
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            polynomial = new double[1]{ vectorY[vectorY.Length-1]};
            int count = 1;
            for (int i=vectorX.Length; i>=0; --i)
            {
                double[] new_pol = new double[2] { 1, -vectorY[vectorY.Length - 1] };
                for(int j=0; j<count; ++j)
                {

                }
                ++count;
                new_pol = multiplyPolynoms(new_pol, new double[1] { calculateSplitSub(vectorX.Length - 1, i, vectorX, vectorY) });
            }
        }
    }
}
