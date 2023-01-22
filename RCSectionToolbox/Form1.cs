using RCSectionToolbox.Materials;
using RCSectionToolbox.RCSectionAnalysis;
using System;
using System.Windows.Forms;

namespace RCSectionToolbox
{

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private RCSection tryParseInput()
        {
            double fc = 0;
            double As1 = 0;
            double As2 = 0;

            if (double.TryParse(heightBox.Text, out double h) && double.TryParse(widthBox.Text, out double b) && double.TryParse(axialBox.Text, out double N)
                && double.TryParse(fyBox.Text, out double fy) && double.TryParse(coverBox.Text, out double c))
            {
                Steel steel = new EurocodeSteel(fy * 1000);

                if (betonBox.SelectedIndex > 0)
                {
                    double.TryParse(betonBox.Text.Substring(1, 2), out fc);
                }
                Concrete concrete = new EurocodeConcrete(fc * 1000);

                if ((diameter1Box.SelectedIndex > 0) && (as1Box.Text != ""))
                {
                    int.TryParse(as1Box.Text, out int numBars);
                    int.TryParse(diameter1Box.Text.Substring(1, 2), out int diameter);
                    As1 = numBars * Math.PI * (diameter / 2) * (diameter / 2) / 1000000;
                }

                if ((diameter2Box.SelectedIndex > 0) && (as2Box.Text != ""))
                {
                    int.TryParse(as2Box.Text, out int numBars);
                    int.TryParse(diameter2Box.Text.Substring(1, 2), out int diameter);
                    As2 = numBars * Math.PI * (diameter / 2) * (diameter / 2) / 1000000;
                }

                return new RCSection(concrete, steel, h, b, N, c, As1, As2);
            }
            else
            {
                throw new ParseError();
            }
        }

        private class ParseError : System.Exception
        {
            public ParseError() : base(String.Format("Error while parsing input from dialog."))
            {

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            RCSection co;
            try
            {
                co = tryParseInput();
            }
            catch (ParseError ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            RCMphiCalculationResults results;

            try
            {
                results = RCMphiCalculation.calculateMphiResults(co);
            }
            catch (SectionAnalysis.CompressionCapacityExceeded ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            catch (SectionAnalysis.TensionCapacityExceeded ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!results.analysisFailed)
            {
                writeResultsToChart(results);
            }
            else
            {
                clearChart();
            }

        }

        private void clearChart()
        {
            chart1.Series[0].Points.Clear();
        }

        private void writeResultsToChart(RCMphiCalculationResults results)
        {
            clearChart();
            for (int i = 0; i < results.Mphi.GetLength(0); i++)
            {
                chart1.Series[0].Points.AddXY(results.Mphi[i, 1], results.Mphi[i, 0]);
            }

            chart1.Invalidate();
        }

        /*
        private void writeResultTextboxSummary(RCMphiCalculationResults results)
        {
            txtSummary.Clear();
            txtSummary.Text += "Reinforcement stresses (%o):" + Environment.NewLine;
            txtSummary.Text += String.Format("es1={0:0.00}, es2={1:0.00}", results._es1 * 1000, results._es2 * 1000) + Environment.NewLine;
            txtSummary.Text += "Reinforcement forces:" + Environment.NewLine;
            txtSummary.Text += String.Format("Fs1={0:0.00}, Fs2={1:0.00}", results._Fs1, results._Fs2) + Environment.NewLine + Environment.NewLine;
            txtSummary.Text += "Concrete stress (%o):" + Environment.NewLine;
            txtSummary.Text += String.Format("ec={0:0.00}", results.ec * 1000) + Environment.NewLine;
            txtSummary.Text += "Concrete force:" + Environment.NewLine;
            txtSummary.Text += String.Format("fc={0:0.00}", results._Fc);
        }*/

        private void Form1_Load(object sender, EventArgs e)
        {
            betonBox.SelectedIndex = 2;
            diameter1Box.SelectedIndex = 2;
            diameter2Box.SelectedIndex = 2;
        }


    }

}
