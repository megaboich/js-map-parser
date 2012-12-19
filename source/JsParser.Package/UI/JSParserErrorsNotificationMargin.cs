using System;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Editor;
using JsParser.Core.Parsers;
using System.Collections.Generic;
using System.Windows.Media.Animation;
using System.Windows;
using Microsoft.VisualStudio.Text;
using JsParser.Package.UI;

namespace JsParser.Package
{
    /// <summary>
    /// A class detailing the margin's visual definition including both size and content.
    /// </summary>
    public class JSParserErrorsNotificationMargin : Canvas, IWpfTextViewMargin
    {
        public const string MarginName = "JSParserErrorsNotificationMargin";
        private IWpfTextView _textView;
        private bool _isDisposed = false;

        private Label _textLabel;
        private string _docFilePath;

        /// <summary>
        /// Creates a <see cref="JSParserErrorsNotificationMargin"/> for a given <see cref="IWpfTextView"/>.
        /// </summary>
        /// <param name="textView">The <see cref="IWpfTextView"/> to attach the margin to.</param>
        public JSParserErrorsNotificationMargin(IWpfTextView textView)
        {
            _textView = textView;

            this.Height = 0;
            this.ClipToBounds = true;
            this.Background = new SolidColorBrush(Colors.DarkRed);

            _textLabel = new Label();
            _textLabel.Background = new SolidColorBrush(Colors.DarkRed);
            _textLabel.Foreground = new SolidColorBrush(Colors.White);
            _textLabel.Content = "";
            this.Children.Add(_textLabel);

            //Get full path of dicument
            ITextDocument document;
            textView.TextDataModel.DocumentBuffer.Properties.TryGetProperty(typeof(ITextDocument), out document);
            _docFilePath = document.FilePath;

            ErrorNotificationCommunicator.SubscribeForErrors(ErrorsHandler, _docFilePath);
        }

        public void ErrorsHandler(JsParser.UI.UI.NavigationTreeView.ErrorsNotificationArgs args)
        {
            SetErrors(args.Errors);
        }

        public void SetErrors(IEnumerable<ErrorMessage> errors)
        {
            if (errors.Any())
            {
                _textLabel.Background = new SolidColorBrush(Colors.DarkRed);
                _textLabel.Content = string.Format(" Javascript Parser: {0} errors found.", errors.Count());
                ChangeHeightTo(24d);
            }
            else
            {
                _textLabel.Background = new SolidColorBrush(Colors.White);
                _textLabel.Content = string.Empty;
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
            return (marginName == JSParserErrorsNotificationMargin.MarginName) ? (IWpfTextViewMargin)this : null;
        }

        public void Dispose()
        {
            ErrorNotificationCommunicator.UnsubscribeFromErrors(ErrorsHandler, _docFilePath);

            if (!_isDisposed)
            {
                GC.SuppressFinalize(this);
                _isDisposed = true;
            }
        }
        #endregion
    }
}
