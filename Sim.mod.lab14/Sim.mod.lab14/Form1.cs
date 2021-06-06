using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sim.mod.lab14
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        int N, k;
        double average, variance;
        double average_emp, variance_emp;
        Random r = new Random();


        double[] sample; 
        int[] counter; 
        double[] Fr; 
        double[] X;

        public void average_variance(double[] a)
        {
            average_emp = a.Average();
            variance_emp = 0;
            for (int i = 0; i < N; i++) variance_emp += sample[i] * sample[i];
            variance_emp = variance_emp / (double)N;
            variance_emp -= average_emp * average_emp;
        }

        double ksi(int i)
        {
            int n = i + 1;
            double ksi = 0;
            for (int k = 0; k < n; k++)
            {
                ksi += r.NextDouble();
            }
            ksi -= n / 2;
            ksi = ksi * (Math.Sqrt(12 / n));
            return ksi;
        }

        private void b_start_Click(object sender, EventArgs e)
        {
            double min, max;
            double delta;
            chart1.Series[0]["PointWidth"] = "1";
            N = Convert.ToInt32(textBox3.Text);
            sample = new double[N];
            average = Convert.ToDouble(textBox1.Text);
            variance = Convert.ToDouble(textBox2.Text);

            k = (int)Math.Floor(Math.Log(N) / Math.Log(2) + 1);

            for (int i = 0; i < N; i++)
            {
                sample[i] = Math.Sqrt(variance) * ksi(i) + average;
            }

            counter = new int[k];
            for (int i = 0; i < k; i++)
            {
                counter[i] = 0;
            }
            Fr = new double[k];
            X = new double[k];

            min = sample.Min(); 
            max = sample.Max();
            delta = (max - min) / k;
            for (int i = 0; i < N; i++)
            {
                for (int j = 1; j <= k; j++)
                {
                    if (sample[i] < min + j * delta) 
                    { 
                        counter[j - 1]++; 
                        break; 
                    }
                }
            }
            for (int i = 0; i < k; i++) Fr[i] = counter[i] / (double)N;

            average_variance(sample);
            av.Text = average_emp.ToString("F5");
            var.Text = variance_emp.ToString("F5");
            chart1.Series[0].Points.Clear();
            for (int i = 0; i < k; i++) chart1.Series[0].Points.AddXY(X[i], Fr[i]);

        }
    }
}
