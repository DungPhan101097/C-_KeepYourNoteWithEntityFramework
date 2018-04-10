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
    /// Interaction logic for ViewAllTags.xaml
    /// </summary>
    public partial class ViewAllTags : UserControl, ViewAllTagClickCallback, Xl_Tag_Observer, Xl_TextNote_Observer
    {
        const int heightPerTag = 25;
        const int heightLabel = 30;
        const int widthOfSingleTag = 150;
        const int preventiveHeight = 10;

        public Dictionary<string, List<TAG>> childList;
        public int heightViewTags { get; set; }
        public int widthViewTags { get; set; }
        public string selectedTagName { get; set; }
        public XuLy_DuLieu_Tag obs { get; set; }
        private XuLy_DuLieu_TextNote obsTextNote;
        
        public ViewAllTags(XuLy_DuLieu_Tag xlTag, XuLy_DuLieu_TextNote xlTextNote, MainWindowTagClickCallback callback,
             int height, int width)
        {
            InitializeComponent();
            this.heightViewTags = height;
            this.widthViewTags = width;

            gridMain.Height = heightViewTags;
            gridMain.Width = widthViewTags;

            obs = xlTag;
            obsTextNote = xlTextNote;

            obs.register(this);
            obsTextNote.registerNote(this);

            refreshUI();

            this.mainWindowTagClickCallbackIns = callback;
        }
        

        public void userControl_GetDataFromChild(string atagName)
        {
            selectedTagName = atagName;
        }

        public void createGridParent()
        {
            childList = new Dictionary<string, List<TAG>>();
            List<TAG> listTag = obs.getAllTAG();
            listTag.Sort(delegate (TAG t1, TAG t2)
            {
                return t1.mContent.CompareTo(t2.mContent);
            });
            
            foreach (TAG tag in listTag)
            {
                string firstChar = tag.mContent[0].ToString();
                if (!childList.ContainsKey(firstChar))
                {
                    childList.Add(firstChar, new List<TAG>());
                }
                childList[firstChar].Add(tag);
            }

            int gridHeight = (int)gridMain.Height;
            
            StackPanel sp = new StackPanel();
            sp.Height = gridHeight;
            sp.Width = widthOfSingleTag;
            sp.Orientation = Orientation.Vertical;
            sp.VerticalAlignment = VerticalAlignment.Top;

            foreach (var entry in childList)
            {
                int numOfTags = entry.Value.Count;
                if (gridHeight - (numOfTags * heightPerTag + heightLabel) > 150)
                {
                    gridHeight -= (numOfTags * heightPerTag + heightLabel + preventiveHeight);
                    SingleViewTag curSingleViewTag = new SingleViewTag(this, entry, heightPerTag, heightLabel);
                    
                    sp.Children.Add(curSingleViewTag);
                }
                else
                {
                    gridHeight = (int)gridMain.Height - (numOfTags * heightPerTag + heightLabel);
                    stackParent.Children.Add(sp);
                    sp = new StackPanel();
                    sp.Height = gridHeight;
                    sp.Width = widthOfSingleTag;
                    sp.VerticalAlignment = VerticalAlignment.Top;
                    var curSingleViewTag = new SingleViewTag(this, entry, heightPerTag, heightLabel);
                   
                    sp.Children.Add(curSingleViewTag);
                }
            }
            stackParent.Children.Add(sp);
        }

        public MainWindowTagClickCallback mainWindowTagClickCallbackIns { get; set; }

        public void onTagClick(TAG selectedTag)
        {
            // Process selected tag click
            // delegate MainWindowTagClickCallback process
            mainWindowTagClickCallbackIns.onTagClick(selectedTag);
        }

        public void refreshUI()
        {
            clearGridParent();
            createGridParent();
        }

        private void clearGridParent()
        {
            if(stackParent.Children.Count > 0)
                stackParent.Children.Clear();
        }

        public void refreshUINote()
        {
            clearGridParent();
            createGridParent();
        }
    }
}
