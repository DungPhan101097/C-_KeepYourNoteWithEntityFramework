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
    using callbacks;

    /// <summary>
    /// Interaction logic for SingleViewTag.xaml
    /// </summary>
    public partial class SingleViewTag : UserControl
    {
        const int WIDTH = 100;
        public KeyValuePair<string, List<TAG>> entry { get; set; }
        public Dictionary<string, TAG> entryDetailList { get; set; }
        public int heightPerTag { get; set; }
        public int heightLabel { get; set; }
        public string selectedTag { get; set; }
        public ViewAllTagClickCallback viewAllTagClickCallbackIns;


        public SingleViewTag(ViewAllTagClickCallback callback, 
            KeyValuePair<string, List<TAG>> entry, int heightPerTag, int heightLabel)
        {
            InitializeComponent();
            this.viewAllTagClickCallbackIns = callback;
            entryDetailList = new Dictionary<string, TAG>();

            singleViewContainer.Height = (entry.Value.Count * heightPerTag + heightLabel);
            singleViewContainer.Width = WIDTH;

            singleViewTag.Height = (entry.Value.Count * heightPerTag + heightLabel);
            singleViewTag.Width = WIDTH;

            this.heightPerTag = heightPerTag;
            this.heightLabel = heightLabel;
            this.entry = entry;
            
            lbTitle.Content = entry.Key;
            lbTitle.Height = heightLabel;
            lbTitle.Width = WIDTH;

            foreach (TAG tag in entry.Value)
            {
                ListBoxItem item = new ListBoxItem();
                item.Content = tag.mContent + " (" + tag.countNumberNote() + ")";
                item.Height = heightPerTag;
                item.Width = WIDTH;

                if (!entryDetailList.ContainsKey(tag.mContent))
                {
                    entryDetailList.Add(tag.mContent, tag);
                }
                lbxTags.Items.Add(item);
            }
        }
        
        private void lbxTags_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox lstbox = sender as ListBox;
            var selItem = (ListBoxItem)(lstbox.SelectedItem);
            selectedTag = (string) selItem.Content;
            selectedTag = selectedTag.Substring(0, selectedTag.LastIndexOf("(") - 1);
            viewAllTagClickCallbackIns.onTagClick(this.entryDetailList[selectedTag]);
        }
    }
}
