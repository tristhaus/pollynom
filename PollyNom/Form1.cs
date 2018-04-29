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
            if(string.IsNullOrWhiteSpace(inputBox.Text))
            {
                return;
            }
            Evaluator evaluator = new Evaluator(inputBox.Text);
            var list = evaluator.Evaluate();
            outputBox.Text = string.Empty;
            foreach (var value in list)
            {
                outputBox.Text += $"{value.Item1} {value.Item2} \r\n";
            }
            outputBox.Refresh();
        }
    }
}
