using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DataAccessLayer;

namespace ViewNoteWindow
{
    /// <summary>
    /// Interaction logic for EditText.xaml
    /// </summary>
    public partial class EditText : UserControl
    {
        public TEXTNOTE textNoteIns { get; set; }
        public string contentRichText { get; set; }

        public EditText(TEXTNOTE curNote)
        {
            InitializeComponent();
            textNoteIns = new TEXTNOTE();
            textNoteIns.assignTextNote(curNote);

            // Load xaml text
            if (textNoteIns.mContent != null)
            {
                TextRange textRange = new TextRange(rtbEditor.Document.ContentStart, rtbEditor.Document.ContentEnd);
                using (MemoryStream memStream = GetMemoryStreamFromString(textNoteIns.mContent))
                {
                    textRange.Load(memStream, DataFormats.Xaml);
                }
            }
            cmbFontFamily.ItemsSource = Fonts.SystemFontFamilies.OrderBy(f => f.Source);
            cmbFontSize.ItemsSource = new List<double>() { 8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24, 26, 28, 36, 48, 72 };
            cmbFontFamily.SelectedIndex = 0;
            cmbFontSize.SelectedIndex = 0;
        }

        private MemoryStream GetMemoryStreamFromString(string mContent)
        {
                MemoryStream stream = new MemoryStream();
                StreamWriter writer = new StreamWriter(stream);
                writer.Write(mContent);
                writer.Flush();
                stream.Position = 0;
                return stream;
        
        }

        private void rtbEditor_SelectionChanged(object sender, RoutedEventArgs e)
        {
            object temp = rtbEditor.Selection.GetPropertyValue(Inline.FontWeightProperty);
            btnBold.IsChecked = (temp != DependencyProperty.UnsetValue) && (temp.Equals(FontWeights.Bold));
            temp = rtbEditor.Selection.GetPropertyValue(Inline.FontStyleProperty);

            btnItalic.IsChecked = (temp != DependencyProperty.UnsetValue) && (temp.Equals(FontStyles.Italic));
            temp = rtbEditor.Selection.GetPropertyValue(Inline.TextDecorationsProperty);
            btnUnderline.IsChecked = (temp != DependencyProperty.UnsetValue) && (temp.Equals(TextDecorations.Underline));

            temp = rtbEditor.Selection.GetPropertyValue(Inline.FontFamilyProperty);
            cmbFontFamily.SelectedItem = temp;
            temp = rtbEditor.Selection.GetPropertyValue(Inline.FontSizeProperty);
            if (temp.ToString() != DependencyProperty.UnsetValue.ToString())
            {
                cmbFontSize.Text = temp.ToString();
            }

        }

        private void cmbFontFamily_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbFontFamily.SelectedItem != null)
                rtbEditor.Selection.ApplyPropertyValue(Inline.FontFamilyProperty, cmbFontFamily.SelectedItem);
        }

        private void cmbFontSize_TextChanged(object sender, TextChangedEventArgs e)
        { 
            if (cmbFontSize.Text != DependencyProperty.UnsetValue.ToString())
            {
                rtbEditor.Selection.ApplyPropertyValue(Inline.FontSizeProperty, cmbFontSize.Text);
            }
            else
            {
                cmbFontSize.SelectedValue = "";
            }
        }

        private void rtbEditor_TextChanged(object sender, TextChangedEventArgs e)
        {
            contentRichText = new TextRange(rtbEditor.Document.ContentStart, rtbEditor.Document.ContentEnd).Text;
            textNoteIns.mContent = contentRichText;
            
        }

        public TextRange getCurrentRichText()
        {
            return new TextRange(rtbEditor.Document.ContentStart, rtbEditor.Document.ContentEnd);
        }
    }
}
