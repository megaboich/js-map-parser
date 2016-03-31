using JsParser.Core.Code;
using JsParser.Core.Infrastructure;
using JsParser.Core.Parsers;
using JsParser.Package.Infrastructure;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace JsParser.Package.UI
{
    /// <summary>
    /// Interaction logic for ErrorsNotificationControl.xaml
    /// </summary>
    public partial class ErrorsNotificationControl : UserControl, IWpfTextViewMargin, ITextViewMargin
    {
        public const string MarginName = "JSParserErrorsNotificationMargin";
        private IWpfTextView _textView;
        private bool _isDisposed = false;
        private string _docFilePath;
        private ICodeProvider _codeProvider;

        public ErrorsNotificationControl(IWpfTextView wpfTextView)
        {
            _textView = wpfTextView;

            this.Height = 0;
            this.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;

            if (JsParser.UI.Properties.Settings.Default.ShowErrorsNotificationOnTopOfEditor)
            {

                Microsoft.VisualStudio.Text.ITextDocument document;
                if (wpfTextView == null
                    || wpfTextView.TextDataModel == null
                    || wpfTextView.TextDataModel.DocumentBuffer == null
                    || wpfTextView.TextDataModel.DocumentBuffer.Properties == null
                    || (!wpfTextView.TextDataModel.DocumentBuffer.Properties.TryGetProperty(typeof(Microsoft.VisualStudio.Text.ITextDocument), out document))
                    || document == null
                    || document.TextBuffer == null
                )
                {
                    // Perform a monstroneous null-check while extracting document object. 
                    // There were couple of issues probably caused of non error checking : http://code.google.com/p/js-addin/issues/detail?id=32, http://code.google.com/p/js-addin/issues/detail?id=30

                    // There is no document, so just empty initialize without any functionality.
                }
                else
                {
                    _docFilePath = document.FilePath;
                    document.FileActionOccurred += document_FileActionOccurred;
                    JsParserEventsBroadcaster.Subscribe(JsParserEventsHandler, _docFilePath);
                }
            }

            InitializeComponent();
        }

        private void document_FileActionOccurred(object sender, TextDocumentFileActionEventArgs e)
        {
            if (e.FileActionType.HasFlag(FileActionTypes.ContentSavedToDisk)
                || e.FileActionType.HasFlag(FileActionTypes.DocumentRenamed))
            {
                if (e.FilePath != _docFilePath)
                {
                    //Need to update registration
                    JsParserEventsBroadcaster.UpdateSubscription(_docFilePath, e.FilePath);
                    _docFilePath = e.FilePath;
                }
            }
        }

        public void JsParserEventsHandler(JsParserEvent args)
        {
            _codeProvider = args.Code;

            if (args is JsParserErrorsNotificationArgs)
            {
                SetErrors(((JsParserErrorsNotificationArgs)args));
                return;
            }
        }

        private void ErrorsDetailsCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox)
            {
                var cmb = (ComboBox)sender;
                var val = cmb.SelectedValue;
                if (val is ErrorMessage)
                {
                    var errorMessage = (ErrorMessage)val;
                    if (_codeProvider != null)
                    {
                        _codeProvider.SelectionMoveToLineAndOffset(errorMessage.StartLine, errorMessage.StartColumn + 1);
                        _codeProvider.SetFocus();
                    }
                }
            }
        }

        public void SetErrors(JsParserErrorsNotificationArgs args)
        {
            var errors = args.Errors;
            if (errors.Any())
            {
                SummaryMessageLabel.Content = string.Format(" Javascript Map Parser: {0} errors found.", errors.Count());
                ErrorsDetailsCombobox.SelectionChanged -= ErrorsDetailsCombobox_SelectionChanged;
                ErrorsDetailsCombobox.ItemsSource =
                    errors
                    .Select(e => new
                        {
                            Text = "Line: " + e.StartLine + ", Message: " + e.Message.SplitWordsByCamelCase() + ".",
                            Item = e,
                        })
                        .ToList();
                ErrorsDetailsCombobox.SelectedIndex = 0;
                ChangeHeightTo(30d);
                ErrorsDetailsCombobox.SelectionChanged += ErrorsDetailsCombobox_SelectionChanged;
            }
            else
            {
                ErrorsDetailsCombobox.ItemsSource = Enumerable.Empty<ErrorMessage>();
                SummaryMessageLabel.Content = string.Empty;
                ChangeHeightTo(0d);
            }
        }

        private void ChangeHeightTo(double newHeight)
        {
            if (this._textView.Options.GetOptionValue<bool>(DefaultWpfViewOptions.EnableSimpleGraphicsId))
            {
                base.Height = newHeight;
                return;
            }
            var doubleAnimation = new DoubleAnimation(base.Height, newHeight, new Duration(TimeSpan.FromMilliseconds(175.0)));
            Storyboard.SetTarget(doubleAnimation, this);
            Storyboard.SetTargetProperty(doubleAnimation, new PropertyPath(FrameworkElement.HeightProperty));
            new Storyboard
            {
                Children = 
                {
                    doubleAnimation
                }
            }.Begin(this);
        }

        private void ThrowIfDisposed()
        {
            if (_isDisposed)
                throw new ObjectDisposedException(MarginName);
        }

        #region IWpfTextViewMargin Members

        /// <summary>
        /// The <see cref="Sytem.Windows.FrameworkElement"/> that implements the visual representation
        /// of the margin.
        /// </summary>
        public System.Windows.FrameworkElement VisualElement
        {
            // Since this margin implements Canvas, this is the object which renders
            // the margin.
            get
            {
                ThrowIfDisposed();
                return this;
            }
        }

        #endregion

        #region ITextViewMargin Members

        public double MarginSize
        {
            // Since this is a horizontal margin, its width will be bound to the width of the text view.
            // Therefore, its size is its height.
            get
            {
                ThrowIfDisposed();
                return this.ActualHeight;
            }
        }

        public bool Enabled
        {
            // The margin should always be enabled
            get
            {
                ThrowIfDisposed();
                return true;
            }
        }

        /// <summary>
        /// Returns an instance of the margin if this is the margin that has been requested.
        /// </summary>
        /// <param name="marginName">The name of the margin requested</param>
        /// <returns>An instance of EditorMargin1 or null</returns>
        public ITextViewMargin GetTextViewMargin(string marginName)
        {
            return (marginName == MarginName) ? (IWpfTextViewMargin)this : null;
        }

        public void Dispose()
        {
            JsParserEventsBroadcaster.Unsubscribe(JsParserEventsHandler, _docFilePath);

            if (!_isDisposed)
            {
                GC.SuppressFinalize(this);
                _isDisposed = true;
            }
        }
        #endregion
    }
}
