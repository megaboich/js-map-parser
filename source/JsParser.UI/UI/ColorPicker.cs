using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace JsParser.UI.UI
{
    public partial class ColorPicker : UserControl
    {
        private bool _skipTextBoxEvent = false;

        [Description("SelectedColorChanged")]
        [Category("CatBehavior")]
        public event EventHandler SelectedColorChanged;

        public Color SelectedColor
        {
            get { return panel1.BackColor; }
            set
            {
                panel1.BackColor = value;
                _skipTextBoxEvent = true;
                textBox1.Text = ColorTranslator.ToHtml(panel1.BackColor);
                _skipTextBoxEvent = false;
                if (SelectedColorChanged != null)
                {
                    SelectedColorChanged(this, EventArgs.Empty);
                }
            }
        }

        public ColorPicker()
        {
            InitializeComponent();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel1_Click(object sender, EventArgs e)
        {
            colorDialog1.Color = SelectedColor;
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                SelectedColor = colorDialog1.Color;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (_skipTextBoxEvent)
            {
                return;
            }

            var text = textBox1.Text;
            try
            {
                var color = ColorTranslator.FromHtml(text);
                panel1.BackColor = color;
            }
            catch
            {
            }
        }
    }
}
