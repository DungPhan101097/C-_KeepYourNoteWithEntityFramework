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
using System.Windows.Navigation;
using System.Windows.Shapes;
using DataAccessLayer;

namespace ViewNoteWindow
{
    using enums;
    using System.Data.Entity.Validation;
    using System.IO;

    /// <summary>
    /// Interaction logic for ViewTagDetail.xaml
    /// </summary>
    public partial class ViewTagDetail : UserControl, Xl_TextNote_Observer
    {
        public string tagName { get; set; }
        public int height { get; set; }
        public int width { get; set; }
        public XuLy_DuLieu_Tag xlTag { get; set; }
        public XuLy_DuLieu_TextNote xlTextNote { get; set; }
        public TEXTNOTE curNote { get; set; }
        public EditText currentEditText { get; set; }
        public TAG selectedTag { get; set; }

        public ViewTagDetail(XuLy_DuLieu_Tag aXlTag, XuLy_DuLieu_TextNote axlNote,
            string atagName, int aheight, int awidth, ENUM_VIEW typeView = ENUM_VIEW.VIEW_BY_NONE)
        {
            InitializeComponent();

            tagName = atagName;
            height = aheight;
            width = awidth;

            MainViewTagDetail.Height = height;
            MainViewTagDetail.Width = width;

            tagDetailContainer.Height = height;
            tagDetailContainer.Width = width;

            xlTag = aXlTag;
            xlTextNote = axlNote;
            xlTextNote.registerNote(this);

            refreshUINote();
        }

        public void createListViewAllNotesSort()
        {
            selectedTag =  xlTag.getTagByContent(tagName);

            List<TEXTNOTE> listTnote;
            if (tagName.Equals("ViewAllNote") == true)
            {
                listTnote = xlTextNote.getAllTextNote();
            }
            else
            {
                listTnote =  xlTag.getAllTextNoteByTag(tagName); 
            }

            List<TEXTNOTE> listNoteClone = new List<TEXTNOTE>();
            foreach(TEXTNOTE note in listTnote)
            {
                TEXTNOTE newNote = new TEXTNOTE();
                newNote.assignTextNote(note);
                listNoteClone.Add(newNote);
            }

            // Dieu chinh lai cach hien thi
            // Load xaml text
            foreach(TEXTNOTE curNote in listNoteClone)
            {
                RichTextBox rtbContent = new RichTextBox();

                if (curNote.mContent != null)
                {

                    TextRange textRange = new TextRange(rtbContent.Document.ContentStart, rtbContent.Document.ContentEnd);
                    using (MemoryStream memStream = GetMemoryStreamFromString(curNote.mContent))
                    {
                        textRange.Load(memStream, DataFormats.Xaml);
                        curNote.mContent = textRange.Text;
                    }
                }
            }

            // Them vao list view. 

           lvAllNote.ItemsSource = listNoteClone;
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


        private void lvAllNote_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Lay duoc note dang duoc chon la cai nao.
            var selectedItem = lvAllNote.SelectedItem as TEXTNOTE;
            curNote = selectedItem;

            var selectedNote = xlTextNote.getTextNoteByID(selectedItem);

            if (selectedNote == null)
            {
                return;
            }

            noteInfoEditDown.Children.Clear();
            currentEditText = new EditText(selectedNote);
            noteInfoEditDown.Children.Add(currentEditText);

            List<TAG> listTag = selectedNote.getAllTagPreTextNote();
            List<string> listContentTag = new List<string>();
            foreach(TAG tag in listTag)
            {
                listContentTag.Add(tag.mContent);
            }

            cbxListTag.ItemsSource = listContentTag;
            tbxTitleNote.Text = selectedNote.mTitle;
            tbxAccessNote.Text = selectedNote.mAccessTime.ToString();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            using (var memoryStream = new MemoryStream())
            {
                var currentTextRange = currentEditText.getCurrentRichText();
                currentTextRange.Save(memoryStream, DataFormats.Xaml);
                if (curNote == null)
                {
                    return;
                }
                curNote.mContent = UTF8Encoding.UTF8.GetString(memoryStream.ToArray());

                xlTextNote.modifyTextNote(curNote, curNote);
            }
        }

        private void btnNewNote_Click(object sender, RoutedEventArgs e)
        {
            // Mo man hinh new note moi.
            NewNoteWindow newNoteWindow = new NewNoteWindow(selectedTag,xlTag, xlTextNote);
            newNoteWindow.ShowDialog();
        }

        public void refreshUINote()
        {
            removeUINote();
            createListViewAllNotesSort();

        }

        public void removeUINote()
        {
            lvAllNote.ItemsSource = null;
        }

        public void presentStackResultSearch(List<TEXTNOTE> listTextNote)
        {
            // Clear the list
            //resultStack.Children.Clear();

            //// Add the result
            //foreach (var obj in listTextNote)
            //{
            //    TextBlock textBlock = new TextBlock();
            //    textBlock.Text = obj;
            //    // A little style...
            //    textBlock.Margin = new Thickness(2, 3, 2, 3);
            //    textBlock.Cursor = Cursors.Hand;

            //    // Mouse events
            //    textBlock.MouseLeftButtonUp += (s, ev) =>
            //    {
            //        // Them call refresh UI all tag và show detail tag lên
            //        // Thêm behaviour tương tự khi người dùng nhấn nút search
            //        txtsearchNote.Text = (s as TextBlock).Text;

            //        // Change UI
            //        removeUINote();
            //        List<TEXTNOTE> selectedNote = xlTextNote.findTextNotesByKeyWord(txtsearchNote.Text);
            //        TEXTNOTE noteFirst = selectedNote.First();
            //        noteInfoEditDown.Children.Clear();
            //        currentEditText = new EditText(noteFirst);
            //        noteInfoEditDown.Children.Add(currentEditText);

            //        List<TAG> listTag = noteFirst.getAllTagPreTextNote();
            //        List<string> listContentTag = new List<string>();
            //        foreach (TAG tag in listTag)
            //        {
            //            listContentTag.Add(tag.mContent);
            //        }

            //        cbxListTag.ItemsSource = listContentTag;
            //        tbxTitleNote.Text = noteFirst.mTitle;
            //        tbxAccessNote.Text = noteFirst.mAccessTime.ToString();

            //        //string headerTag = txtsearchNote.Text;
            //        //content.Children.Remove(curUserControl);
            //        //curUserControl = new ViewTagDetail(xlTag, xlTextNote, headerTag, (int)content.Height, (int)content.Width);
            //        //content.Children.Add(curUserControl);

            //        resultStack.Children.Clear();
            //        borderResult.Visibility = System.Windows.Visibility.Collapsed;
            //    };

            //    textBlock.MouseEnter += (s, ev) =>
            //    {
            //        TextBlock b = s as TextBlock;
            //        b.Background = Brushes.PeachPuff;
            //    };

            //    textBlock.MouseLeave += (s, ev) =>
            //    {
            //        TextBlock b = s as TextBlock;
            //        b.Background = Brushes.Transparent;
            //    };

            //    // Add to the panel
            //    resultStack.Children.Add(textBlock);

            //}
        }

        private void txtsearchNote_TextChanged(object sender, TextChangedEventArgs e)
        {
            //List<TEXTNOTE> listTextNote = xlTextNote.getAllTextNote();

            //List<TEXTNOTE> listNoteClone = new List<TEXTNOTE>();
            //foreach (TEXTNOTE note in listTextNote)
            //{
            //    TEXTNOTE newNote = new TEXTNOTE();
            //    newNote.assignTextNote(note);
            //    listNoteClone.Add(newNote);
            //}

            //// Dieu chinh lai cach hien thi
            //// Load xaml text
            //foreach (TEXTNOTE curNote in listNoteClone)
            //{
            //    RichTextBox rtbContent = new RichTextBox();

            //    if (curNote.mContent != null)
            //    {

            //        TextRange textRange = new TextRange(rtbContent.Document.ContentStart, rtbContent.Document.ContentEnd);
            //        using (MemoryStream memStream = GetMemoryStreamFromString(curNote.mContent))
            //        {
            //            textRange.Load(memStream, DataFormats.Xaml);
            //            curNote.mContent = textRange.Text;
            //        }
            //    }
            //}
            //List<
            //foreach(TEXTNOTE note in listNoteClone)
            //{
            //    if(!note.mContent.Contains(txtsearchNote.Text))
            //    {
            //        listNoteContent.Add(note.mContent);
            //        listNoteClone.RemoveAt()
            //    }
            //}

            //if (listNoteContent.Count == 0)
            //{
            //    // Clear
            //    resultStack.Children.Clear();
            //    borderResult.Visibility = System.Windows.Visibility.Collapsed;
            //}
            //else
            //{
            //    borderResult.Visibility = System.Windows.Visibility.Visible;
            //    presentStackResultSearch(listNoteContent);

            //}

        }

        private void btnSearchNote_Click(object sender, RoutedEventArgs e)
        {
            //List<TEXTNOTE> ListNote = xlTextNote.findTextNotesByKeyWord(txtsearchNote.Text);
            //if (ListNote.Count == 0)
            //{
            //    // Clear
            //    resultStack.Children.Clear();
            //    borderResult.Visibility = System.Windows.Visibility.Collapsed;
            //    MessageBox.Show("This note isn't exist in database!");
            //}
            //else
            //{
            //    borderResult.Visibility = System.Windows.Visibility.Visible;
                
            //    presentStackResultSearch(ListNote);

            //    // Change UI.
            //    //string headerTag = txtsearchNote.Text;
            //    //content.Children.Remove(curUserControl);
            //    //curUserControl = new ViewTagDetail(xlTag, xlTextNote, headerTag, (int)content.Height, (int)content.Width);
            //    //content.Children.Add(curUserControl);
            //}
            //resultStack.Children.Clear();
            //borderResult.Visibility = System.Windows.Visibility.Collapsed;
        }
    }
}


//switch(typeView)
//{
//    case ENUM_VIEW.VIEW_BY_TITLE:
//        listTnote.Sort(delegate (TEXTNOTE t1, TEXTNOTE t2) { return t1.mTitle.CompareTo(t2.mTitle); });
//        break;
//    case ENUM_VIEW.VIEW_BY_ASCDATE:
//        listTnote.Sort(delegate (TEXTNOTE t1, TEXTNOTE t2)
//        {
//            return t1.mAccessTime.ToString().CompareTo(t2.mAccessTime.ToString()) ;
//            //return result;
//        });
//        break;
//    case ENUM_VIEW.VIEW_BY_DESCDATE:
//        listTnote.Sort(delegate (TEXTNOTE t1, TEXTNOTE t2)
//        {
//            return t1.mAccessTime.ToString().CompareTo(t2.mAccessTime.ToString());
//            //return result;
//        });
//        break;

//}
