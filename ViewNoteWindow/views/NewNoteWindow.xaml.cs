using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using DataAccessLayer;
using System.IO;

namespace ViewNoteWindow
{
    /// <summary>
    /// Interaction logic for NewNoteWindow.xaml
    /// </summary>
    public partial class NewNoteWindow : Window
    {
        public TEXTNOTE newNote { get; set;}
        private string strListTag = null;
        private List<string> listNameTag;
        public EditText edittextIns { get; set; }
        private XuLy_DuLieu_TextNote curProcessTextNote;
        private XuLy_DuLieu_Tag curProcessTag;

        public NewNoteWindow (XuLy_DuLieu_Tag xlTag, XuLy_DuLieu_TextNote xlTextNote)
        {
            InitializeComponent();

            addNoteWindow.Left = 400;
            addNoteWindow.Top = 200;

            listNameTag = new List<string>();
            newNote = new TEXTNOTE();
            newNote.mAccessTime = DateTime.Now;

            curProcessTextNote = xlTextNote;
            curProcessTag = xlTag;

            infoContentNote.Children.Clear();
            edittextIns = new EditText(newNote);
            infoContentNote.Children.Add(edittextIns);
        }

        public NewNoteWindow(TAG selectedTag, XuLy_DuLieu_Tag xlTag, XuLy_DuLieu_TextNote xlTextNote)
        {
            InitializeComponent();
            listNameTag = new List<string>();
            newNote = new TEXTNOTE();

            infoContentNote.Children.Clear();
            infoContentNote.Children.Add(new EditText(newNote));

            curProcessTextNote = xlTextNote;
            curProcessTag = xlTag;
        }
        

        private void txtTitleNote_TextChanged(object sender, TextChangedEventArgs e)
        {
            newNote.mTitle = txtTitleNote.Text;

        }

        private void txtlistTag_TextChanged(object sender, TextChangedEventArgs e)
        {
            strListTag = txtlistTag.Text;
        }

        private void btnDone_Click(object sender, RoutedEventArgs e)
        {
            // Kiem tra dieu kien
            newNote.mContent = edittextIns.textNoteIns.mContent;
            if (newNote.mContent != null)
            {
                
                // Thay doi mau nut

                // tao list tag name.
                List<string> listNameTag = new List<string>();
                if (strListTag != null)
                {
                    strListTag = strListTag.Trim();
                    while (strListTag.IndexOf(',') >= 0)
                    {
                        string tmp = (strListTag.Substring(0, strListTag.IndexOf(','))).Trim();
                        listNameTag.Add(tmp);
                        strListTag = strListTag.Remove(0, strListTag.IndexOf(',') + 1);
                    }
                    if (strListTag.Trim().Length > 0)
                    {
                        listNameTag.Add(strListTag.Trim());
                    }
                }

                // Cap nhat danh sach tag cho newNote.
                if (listNameTag.Count > 0)
                {
                    foreach (string str in listNameTag)
                    {
                        TAG curTag = curProcessTag.getTagByContent(str);
                        if (curTag == null)
                        {
                            curTag = new TAG(str);
                            curProcessTag.insertTAG(curTag);
                        }
                        newNote.TAGs.Add(curTag);
                    }
                    using (var memoryStream = new MemoryStream())
                    {
                        var currentTextRange = edittextIns.getCurrentRichText();
                        currentTextRange.Save(memoryStream, DataFormats.Xaml);
                        newNote.mContent = UTF8Encoding.UTF8.GetString(memoryStream.ToArray());
                    }
                    curProcessTextNote.insertTextNote(newNote);
                        
                    
                }
            }

        }
    }
}
