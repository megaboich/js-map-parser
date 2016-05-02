using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using JsParser.UI.Helpers;
using JsParser.UI.Properties;
using System.Collections.Specialized;
using System.Diagnostics;

namespace JsParser.UI.UI
{
    public partial class JsParserSettingsControl : UserControl
    {
        private Font _defaultTreeFont;
        private ThemeProvider _themeProvider;

        public JsParserSettingsControl()
        {
            InitializeComponent();

            _defaultTreeFont = Font;
        }

        public Font DefaultTreeFont
        {
            get { return _defaultTreeFont; }
            set { _defaultTreeFont = value; }
        }

        public void InitSettings()
        {
            chTrackActiveItem.Checked = Settings.Default.TrackActiveItem;
            edExtensions.Text = string.Join(" ", Settings.Default.Extensions.OfType<string>().Select(s => s.Replace(".", "")).ToArray());
            chScriptStripEnabled.Checked = Settings.Default.ScriptStripEnabled;
            edScriptStripEtensions.Text = string.Join(" ", Settings.Default.ScriptStripExtensions.OfType<string>().Select(s => s.Replace(".", "")).ToArray());

            var treeFont = Settings.Default.TreeFont ?? _defaultTreeFont;
            btnSelectTreeFont.Text = treeFont.Name + " (" + treeFont.Size + ")";
            btnSelectTreeFont.Font = treeFont;

            taggedFuncLabel2.Font = treeFont;
            taggedFuncLabel3.Font = treeFont;
            taggedFuncLabel4.Font = treeFont;
            taggedFuncLabel5.Font = treeFont;
            taggedFuncLabel6.Font = treeFont;
            colorPicker2.SelectedColor = Settings.Default.taggedFunction2Color;
            colorPicker3.SelectedColor = Settings.Default.taggedFunction3Color;
            colorPicker4.SelectedColor = Settings.Default.taggedFunction4Color;
            colorPicker5.SelectedColor = Settings.Default.taggedFunction5Color;
            colorPicker6.SelectedColor = Settings.Default.taggedFunction6Color;

            numericUpDownMaxParametersLength.Value = Settings.Default.MaxParametersLength;
            numericUpDownMaxParametersLengthInFunctionChain.Value = Settings.Default.MaxParametersLengthInFunctionChain;

            chSendStatistics.Checked = Settings.Default.SendStatistics;
            chFixAspNet.Checked = Settings.Default.FixAspNetTags;
            edFixAspNetExtensions.Text = string.Join(" ", Settings.Default.FixAspNetTagsExtensions.OfType<string>().Select(s => s.Replace(".", "")).ToArray());
            chFixRazor.Checked = Settings.Default.FixRazorSyntax;
            edFixRazorExtensions.Text = string.Join(" ", Settings.Default.FixRazorSyntaxExtensions.OfType<string>().Select(s => s.Replace(".", "")).ToArray());

            edToDoKeyWords.Text = String.Join(", ", Settings.Default.ToDoKeywords.OfType<string>().ToArray());

            chShowErrorNotificationOnTopOfTheEditor.Checked = Settings.Default.ShowErrorsNotificationOnTopOfEditor;

            var themesSerialized = Settings.Default.ThemeSettingsSerialized;
            _themeProvider = ThemeProvider.Deserialize(themesSerialized);

            ReInitThemesComboBox();
        }

        private void ReInitThemesComboBox()
        {
            cbThemePicker.BeginUpdate();
            cbThemePicker.Items.Clear();
            foreach (var item in _themeProvider.GetThemes().Select(t => t.Name))
            {
                cbThemePicker.Items.Add(item);
            }

            cbThemePicker.SelectedItem = _themeProvider.CurrentTheme.Name;
            cbThemePicker.EndUpdate();
        }

        private StringCollection ReadListOfExtensionsFromTextBoxText(string textBoxText)
        {
            var list = new StringCollection();
            list.AddRange(textBoxText.Split(new[] { ' ', ',', ';' }, StringSplitOptions.RemoveEmptyEntries));
            return list;
        }

        public void SaveSettings()
        {
            Settings.Default.TrackActiveItem = chTrackActiveItem.Checked;
            Settings.Default.Extensions = ReadListOfExtensionsFromTextBoxText(edExtensions.Text);

            Settings.Default.ScriptStripEnabled = chScriptStripEnabled.Checked;
            Settings.Default.ScriptStripExtensions = ReadListOfExtensionsFromTextBoxText(edScriptStripEtensions.Text);

            Settings.Default.TreeFont = btnSelectTreeFont.Font;

            Settings.Default.taggedFunction2Color = colorPicker2.SelectedColor;
            Settings.Default.taggedFunction3Color = colorPicker3.SelectedColor;
            Settings.Default.taggedFunction4Color = colorPicker4.SelectedColor;
            Settings.Default.taggedFunction5Color = colorPicker5.SelectedColor;
            Settings.Default.taggedFunction6Color = colorPicker6.SelectedColor;

            Settings.Default.MaxParametersLength = Convert.ToInt32(numericUpDownMaxParametersLength.Value);
            Settings.Default.MaxParametersLengthInFunctionChain = Convert.ToInt32(numericUpDownMaxParametersLengthInFunctionChain.Value);

            Settings.Default.SendStatistics = chSendStatistics.Checked;
            if (Settings.Default.SendStatistics)
            {
                //If user checks this item once - then politic are approved.
                Settings.Default.SendStatisticsPolitic = "Approved";
            }

            Settings.Default.FixAspNetTags = chFixAspNet.Checked;
            Settings.Default.FixAspNetTagsExtensions = ReadListOfExtensionsFromTextBoxText(edFixAspNetExtensions.Text);
            Settings.Default.FixRazorSyntax = chFixRazor.Checked;
            Settings.Default.FixRazorSyntaxExtensions = ReadListOfExtensionsFromTextBoxText(edFixRazorExtensions.Text);

            Settings.Default.ToDoKeywords = new System.Collections.Specialized.StringCollection();
            Settings.Default.ToDoKeywords.AddRange(edToDoKeyWords.Text.Split(new[] { ',', ';' }).Select(p => p.Trim()).ToArray());

            Settings.Default.ShowErrorsNotificationOnTopOfEditor = chShowErrorNotificationOnTopOfTheEditor.Checked;

            Settings.Default.ThemeSettingsSerialized = _themeProvider.Serialize();

            Settings.Default.Save();
        }
       
        private void chScriptStripEnabled_CheckedChanged(object sender, EventArgs e)
        {
            edScriptStripEtensions.Enabled = chScriptStripEnabled.Checked;
        }

        private void chFixAspNet_CheckedChanged(object sender, EventArgs e)
        {
            edFixAspNetExtensions.Enabled = chFixAspNet.Checked;
        }

        private void chFixRazor_CheckedChanged(object sender, EventArgs e)
        {
            edFixRazorExtensions.Enabled = chFixRazor.Checked;
        }

        private void btnAddCustomTheme_Click(object sender, EventArgs e)
        {
            _themeProvider.AddTheme("New theme");
            ReInitThemesComboBox();
        }

        private void btnEditTheme_Click(object sender, EventArgs e)
        {
            var editThemeDialog = new ThemeEditor(_themeProvider.CurrentTheme, (theme) =>
            {
                var name = theme.Name;
                if (name.ToLower() != _themeProvider.CurrentThemeName.ToLower()
                    && _themeProvider.GetThemes().Select(t => t.Name.ToLower()).Contains(name.ToLower()))
                {
                    MessageBox.Show("Name '" + name + "' is already used", "ERROR");
                    return false;
                }
                return true;
            });
            if (editThemeDialog.ShowDialog() == DialogResult.OK)
            {
                _themeProvider.CurrentThemeName = editThemeDialog.Theme.Name;
                ReInitThemesComboBox();
            }
        }

        private void btnRemoveTheme_Click(object sender, EventArgs e)
        {
            _themeProvider.RemoveTheme(_themeProvider.CurrentThemeName);
            ReInitThemesComboBox();
        }

        private void cbThemePicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            _themeProvider.SetCurrent(cbThemePicker.SelectedItem.ToString());
            btnEditTheme.Enabled = !_themeProvider.CurrentTheme.IsPredefined;
            btnRemoveTheme.Enabled = !_themeProvider.CurrentTheme.IsPredefined;
        }

        private void btnSelectTreeFont_Click(object sender, EventArgs e)
        {
            fontDialog1.AllowScriptChange = false;
            fontDialog1.AllowVerticalFonts = false;
            fontDialog1.ShowColor = true;
            fontDialog1.ShowEffects = true;
            fontDialog1.FontMustExist = true;

            if (fontDialog1.ShowDialog() == DialogResult.OK)
            {
                btnSelectTreeFont.Font = fontDialog1.Font;
                btnSelectTreeFont.Text = fontDialog1.Font.Name + " (" + fontDialog1.Font.Size + ")";

                taggedFuncLabel2.Font = fontDialog1.Font;
                taggedFuncLabel3.Font = fontDialog1.Font;
                taggedFuncLabel4.Font = fontDialog1.Font;
                taggedFuncLabel5.Font = fontDialog1.Font;
                taggedFuncLabel6.Font = fontDialog1.Font;
            }
        }

        private void colorPicker2_SelectedColorChanged(object sender, EventArgs e)
        {
            var senderColorPicker = (ColorPicker) sender;
            var lblName = "taggedFuncLabel" + senderColorPicker.Tag;
            Label lbl = (Label)Controls.Find(lblName, true).First();
            lbl.ForeColor = senderColorPicker.SelectedColor;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://github.com/megaboich/js-map-parser");
        }

        private void linkLabel1_Click(object sender, EventArgs e)
        {
            
        }
    }
}
