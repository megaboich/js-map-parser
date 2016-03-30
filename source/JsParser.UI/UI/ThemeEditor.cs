using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;

namespace JsParser.UI.UI
{
    public partial class ThemeEditor : Form
    {
        public Theme Theme;
        public Predicate<Theme> Validator;

        public ThemeEditor(Theme theme, Predicate<Theme> validator)
        {
            Theme = theme;
            Validator = validator;
            InitializeComponent();
            InitControls(Theme);
        }

        private void InitControls(Theme theme)
        {
            txtThemeName.Text = theme.Name;

            cpControlBackground.SelectedColor = theme.Colors.ControlBackground;
            cpControlText.SelectedColor = theme.Colors.ControlText;
            cpGridLines.SelectedColor = theme.Colors.GridLines;
            cpHighlightBackground.SelectedColor = theme.Colors.HighlightBackground;
            cpHighlightInactiveBkground.SelectedColor = theme.Colors.HighlightInactiveBackground;
            cpHighlightInactiveText.SelectedColor = theme.Colors.HighlightInactiveText;
            cpHighlightText.SelectedColor = theme.Colors.HighlightText;
            cpMenuBackground.SelectedColor = theme.Colors.MenuBackground;
            cpWindowBackground.SelectedColor = theme.Colors.WindowBackground;
            cpWindowText.SelectedColor = theme.Colors.WindowText;
            cpTabText.SelectedColor = theme.Colors.TabText;
            cpLineNumbersText.SelectedColor = theme.Colors.LineNumbersText;
        }

        private bool ApplyControls(Theme theme)
        {
            theme.Colors.ControlBackground = cpControlBackground.SelectedColor;
            theme.Colors.ControlText = cpControlText.SelectedColor;
            theme.Colors.GridLines = cpGridLines.SelectedColor;
            theme.Colors.HighlightBackground = cpHighlightBackground.SelectedColor;
            theme.Colors.HighlightInactiveBackground = cpHighlightInactiveBkground.SelectedColor;
            theme.Colors.HighlightInactiveText = cpHighlightInactiveText.SelectedColor;
            theme.Colors.HighlightText = cpHighlightText.SelectedColor;
            theme.Colors.MenuBackground = cpMenuBackground.SelectedColor;
            theme.Colors.WindowBackground = cpWindowBackground.SelectedColor;
            theme.Colors.WindowText = cpWindowText.SelectedColor;
            theme.Colors.TabText = cpTabText.SelectedColor;
            theme.Colors.LineNumbersText = cpLineNumbersText.SelectedColor;
            
            theme.Name = txtThemeName.Text;
            return Validator(theme);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var clone = Theme.LoadFromXml(Theme.SaveToXml());
            if (ApplyControls(clone))
            {
                Validator = t => true;
                ApplyControls(Theme);
                DialogResult = DialogResult.OK;
            }
            else
            {
                DialogResult = DialogResult.None;
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    File.WriteAllText(saveFileDialog1.FileName, Theme.SaveToXml().ToString());
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "ERROR");
                }
            }
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    var theme = Theme.LoadFromXml(XElement.Parse(File.ReadAllText(openFileDialog1.FileName)));
                    InitControls(theme);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "ERROR");
                }
            }
        }
    }
}
