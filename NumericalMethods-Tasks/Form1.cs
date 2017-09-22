using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NCalc;

namespace NumericalMethods_Tasks
{
	public partial class Form1 : Form
	{
		private string phi = "5 / 6 * Sin([X])";
		private string phiShtrih = "5 / 6 * Cos([X])";
		private Expression phiExpression;
		private Expression phiShtrihExpression;
		public Form1()
		{
			InitializeComponent();
			phiExpression = new Expression(phi);
			phiShtrihExpression = new Expression(phiShtrih);
			textBox1.Text = phi;
			textBox2.Text = phiShtrih;
			textBox3.Text = "1";
			textBox4.Text = "0,00001";
		}
		private double functionPhiFromX(double x)
		{
			phiExpression.Parameters["X"] = x;
			return (double)phiExpression.Evaluate();
		}
		private double functionPhiShtrihFromX(double x)
		{
			phiShtrihExpression.Parameters["X"] = x;
			return (double)phiShtrihExpression.Evaluate();
		}
		/*
		private double functionPhiFromX(double x)
		{
			return (double)5 / 6 * Math.Sin(x);
		}
		private double functionPhiShtrihFromX(double x)
		{
			return (double)5 / 6 * Math.Cos(x);
		}
		*/
		private void button1_Click(object sender, EventArgs e)
		{
			richTextBox1.Text = "";
			richTextBox1.Text += "Перевірка достатньої умови збіжності ітераційного процесу:";
			phiExpression = new Expression(textBox1.Text);
			phiShtrihExpression = new Expression(textBox2.Text);

			bool coincides = true;
			for(double i=1; i<=10; ++i)
			{
				double xChecking = functionPhiShtrihFromX(i);
				richTextBox1.Text += String.Format("\n |φ'({0})|={1}{2}", i, xChecking, (Math.Abs(xChecking) < 1) ? "<1" : ">1");
				if (Math.Abs(xChecking) >= 1)
				{
					coincides = false;
					break;
				}
			}
			if (!coincides)
			{
				richTextBox1.Text += "\n Достатня умова збіжності |φ'(x)|<1 не виконується!!!!";
			}
			else
			{
				richTextBox1.Text += "\n Достатня умова збіжності |φ'(x)|<1 виконується.\n";
				double precision = Convert.ToDouble(textBox4.Text);
				double prevX = Convert.ToDouble(textBox3.Text);
				uint count = 0;
				double curX = 0;
				double prevXRem;
				do
				{
					curX = functionPhiFromX(prevX);
					richTextBox1.Text += String.Format("\nНаближення №{1} x({1})=φ(x({0}))\nx({1})={2}\n", count, ++count, curX);
					prevXRem = prevX;
					prevX = curX;
				} while (Math.Abs(curX - prevXRem) >= precision);
				richTextBox1.Text += String.Format("\nРозв'язок:::::::::::\n x={0}\nКількість ітерацій: {1}", Math.Round(curX, 5), count);
				textBox5.Text = Convert.ToString(Math.Round(curX, 5));
				textBox6.Text = Convert.ToString(count);
			}
		}
	}
}
