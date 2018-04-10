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

namespace ViewNoteWindow
{
    /// <summary>
    /// Interaction logic for NewTagWindow.xaml
    /// </summary>
    public partial class NewTagWindow : Window
    {
        public TAG newTag { get; set; }
        private XuLy_DuLieu_Tag curProcessTag;

        public NewTagWindow(XuLy_DuLieu_Tag xlTag)
        {
            InitializeComponent();

            newTagWindows.Left = 400;
            newTagWindows.Top = 200;

            curProcessTag = xlTag;
            newTag = new TAG();
        }

        private void tagName_TextChanged(object sender, TextChangedEventArgs e)
        {
            newTag.mContent = tagName.Text;
            newTag.mAccessTime = DateTime.Now;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // OK.
            if (tagName.Text != "")
            {
                if (curProcessTag.getTagByContent(newTag.mContent) == null)
                {
                    curProcessTag.insertTAG(newTag);
                    MessageBox.Show("Create new tag successfuly!");
                    newTagWindows.Close();
                }
                else
                {
                    MessageBox.Show("This tag is exists in database! Please try again!");
                }
            }
            else
                MessageBox.Show("Create new tag failure!");
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            // Cancel.
            newTagWindows.Close();
        }
        
    }
}
