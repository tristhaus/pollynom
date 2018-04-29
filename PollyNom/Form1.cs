using System;
using System.Windows.Forms;

using PollyNom.BusinessLogic; 

namespace PollyNom
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Evaluator evaluator = new Evaluator();
            var list = evaluator.Evaluate();
            textBox1.Text = string.Empty;
            foreach (var value in list)
            {
                textBox1.Text += $"{value.Item1} {value.Item2} \r\n";
            }
            textBox1.Refresh();
        }
    }
}
