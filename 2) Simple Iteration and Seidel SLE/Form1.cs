using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace _2__Simple_Iteration_and_Seidel_SLE
{

	public partial class Form1 : Form
	{
		public static double[] multiplyMatrixAndVec(double[,] matrix, double[] vec)
		{
			double[] multi = new double[vec.Length];
			for (uint i = 0; i < matrix.GetLength(0); ++i)
			{
				for (uint j = 0; j < matrix.GetLength(1); ++j)
				{
					multi[i] += matrix[i, j] * vec[j];
				}
			}
			return multi;
		}
		public static string printMatrix<T>(T[,] matrix)
		{
			string res = "";
			for (uint i = 0; i < matrix.GetLength(0); ++i)
			{
				for (uint j = 0; j < matrix.GetLength(1); ++j)
				{
					res += String.Format("{0}\t", matrix[i, j]);
				}
				res += String.Format("\n");
			}
			return res;
		}
		public static string printMatrix<T>(T[,] matrix, T[] vec)
		{
			string res = "";
			for (uint i = 0; i < matrix.GetLength(0); ++i)
			{
				for (uint j = 0; j < matrix.GetLength(1); ++j)
				{
					res += String.Format("{0}\t", matrix[i, j]);
				}
				res += String.Format(vec[i].ToString());
				res += String.Format("\n");
			}
			return res;
		}
		public static string printVec<T>(T[] vec)
		{
			string res = "";
			for (uint i = 0; i < vec.Length; ++i)
			{
				res += String.Format("{0}\t", vec[i]);
			}
			res += String.Format("\n");
			return res;
		}
		public static double normaVec(double[] vec)
		{
			double norma = Math.Abs(vec[0]);
			for (int i = 1; i < vec.Length; ++i)
			{
				if (Math.Abs(vec[i]) > norma)
				{
					norma = Math.Abs(vec[i]);
				}
			}
			return norma;
		}

		private int methodType;
		private double[,] matrixA;
		private double[] vectorB;
		private double precision;
		private const int size = 4;
		public Form1()
		{
			InitializeComponent();
			comboBox1.SelectedIndex = 0;
			methodType = Convert.ToInt32(comboBox1.SelectedIndex);
			textBox1.Text = "0,001";

			/*
			matrixA = new double[size, size]
			{
				{ 8,1,1,-1 },
				{ 1,5,-1,-1 },
				{ 1,-1,5,1 },
				{ 2,1,-1,10 }
			};
			*/

			/*
			matrixA = new double[size, size]
			{
				{ 0.12,-1,0.32,-0.18 },
				{ 0.08,-0.12,-0.77,0.32 },
				{ 0.25,0.22,0.14,-1 },
				{ -0.77,-0.14,0.06,-0.12 }
			};
			*/

			matrixA = new double[size, size]
			{
				{ 25,3,5,4 },
				{ 3,25,4,5 },
				{ 4,5,25,3 },
				{ 5,4,5,25 }
			};

			//vectorB = new double[size]{9,4,18,41};
			//vectorB = new double[size] { 0.72, 0.58, -1.56, -1.21 };
			vectorB = new double[size] { 1.11, 1.24, 1.55, 1.16 };
			for (int i = 0; i < size; ++i)
			{
				for (int j = 0; j < size; ++j)
				{
					tableLayoutPanel1.Controls.Add(new TextBox() { Text = Convert.ToString(matrixA[i, j]) }, j, i);
				}
				tableLayoutPanel1.Controls.Add(new TextBox() { Text = Convert.ToString(vectorB[i]) }, 4, i);
				tableLayoutPanel2.Controls.Add(new Label() { Text = String.Format("x{0}", i + 1) }, 0, i);
				tableLayoutPanel2.Controls.Add(new TextBox() { }, 1, i);
			}
		}

		private void button1_Click(object sender, EventArgs e)
		{
			richTextBox1.Text = "";
			precision = Convert.ToDouble(textBox1.Text);
			for (int i = 0; i < size; ++i)
			{
				for (int j = 0; j < size; ++j)
				{
					matrixA[i, j] = Convert.ToDouble(tableLayoutPanel1.GetControlFromPosition(j, i).Text);
				}
				vectorB[i] = Convert.ToDouble(tableLayoutPanel1.GetControlFromPosition(size, i).Text);
			}
			double[,] matrixC = new double[size, size];
			double[] vectorD = new double[size];
			for (int i = 0; i < size; ++i)
			{
				for (int j = 0; j < size; ++j)
				{
					if (i == j)
					{
						matrixC[i, j] = 0;
					}
					else
					{
						matrixC[i, j] = -(matrixA[i, j] / matrixA[i, i]);
					}
				}
				vectorD[i] = vectorB[i] / matrixA[i, i];
			}
			richTextBox1.Text += "Матриця C:\n" + printMatrix(matrixC);
			richTextBox1.Text += "\nВектор d:\n" + printVec(vectorD);
			richTextBox1.Text += "\nПеревірка достатньої умови збіжності:\n";
			double normaC = 0;
			for (int i = 0; i < size; ++i)
			{
				double sum = 0;
				for (int j = 0; j < size; ++j)
				{
					sum += Math.Abs(matrixC[i, j]);
				}
				if (sum > normaC)
				{
					normaC = sum;
				}
				richTextBox1.Text += String.Format("∑|c{0}j|={1}{2}\n", i + 1, sum, (sum < 1) ? "<1" : ">1");
			}
			richTextBox1.Text += String.Format("\nНорма матриці C:\n||C||={0}\n", normaC);

			double[] curX = new double[size];
			for (uint i = 0; i < size; ++i)
			{
				curX[i] = vectorD[i];
			}
			richTextBox1.Text += String.Format("\nПочаткове наближення x0=d:\n{0}", printVec(curX));
			if (methodType == 0)
			{
				double[] nextX = new double[size];
				uint count = 0;
				double normaNextXCurX = 0;
				do
				{
					double[] CmultCurX = multiplyMatrixAndVec(matrixC, curX);
					for (int i = 0; i < size; ++i)
					{
						nextX[i] = vectorD[i] + CmultCurX[i];
					}
					richTextBox1.Text += String.Format("\nНаближення №{0} x{0}=d+C*x{1}:\n{2}", count + 1, count++, printVec(nextX));
					double[] subXNextXCur = new double[size];
					for (int i = 0; i < size; ++i)
					{
						subXNextXCur[i] = nextX[i] - curX[i];
						curX[i] = nextX[i];
					}
					normaNextXCurX = normaVec(subXNextXCur);
					richTextBox1.Text += String.Format("Норма ||x{0}-x{1}||={2}\n", count, count - 1, normaNextXCurX);
				} while (((normaC / (1 - normaC)) * normaNextXCurX) >= precision);
				richTextBox1.Text += String.Format("\nРозвязок:::::\n{0}\nКількість ітерацій={1}", printVec(nextX), count);
				richTextBox1.Text += String.Format("\nКількість ітерацій(апостеріорна оцінка): {0}\n", Math.Ceiling((Math.Log10((precision * (1 - normaC)) / (normaVec(vectorD))) / Math.Log10(normaC)) - 1));
			}
			else if (methodType == 1)
			{
				double[] nextX = new double[size];
				double[] subXNextXCur = new double[size];
				uint count = 0;
				double normaNextXCurX = 0;
				do
				{
					for (int i = 0; i < size; ++i)
					{
						double sum1 = 0, sum2 = 0;
						for (int j = 0; j < i; ++j)
						{
							sum1 += matrixC[i, j] * nextX[j];
						}
						for (int j = i; j < size; ++j)
						{
							sum2 += matrixC[i, j] * curX[j];
						}
						nextX[i] = vectorD[i] + sum1 + sum2;
					}
					richTextBox1.Text += String.Format("\nНаближення №{0} x{0}=d+C*x{0}+C*x{1}:\n{2}", count + 1, count++, printVec(nextX));

					for (int i = 0; i < size; ++i)
					{
						subXNextXCur[i] = nextX[i] - curX[i];
						curX[i] = nextX[i];
					}
					normaNextXCurX = normaVec(subXNextXCur);
					richTextBox1.Text += String.Format("Норма ||x{0}-x{1}||={2}\n", count, count - 1, normaNextXCurX);
				} while (normaNextXCurX >= ((normaC / (1 - normaC)) * precision));
				richTextBox1.Text += String.Format("\nРозвязок:::::\n{0}\nКількість ітерацій={1}", printVec(nextX), count);
			}
			for(int i=0; i< size; ++i)
			{
				tableLayoutPanel2.GetControlFromPosition(1, i).Text = curX[i].ToString();
            }
		}

		private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
		{
			methodType = Convert.ToInt32(comboBox1.SelectedIndex);
		}
	}
}
