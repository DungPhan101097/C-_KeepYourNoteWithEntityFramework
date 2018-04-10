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
using System.Collections;
using System.Data.SqlClient;
using DataAccessLayer;
using WinForms = System.Windows.Forms;

namespace ViewNoteWindow
{
    using callbacks;
    using enums;
    using System.IO;
    using System.Runtime.InteropServices;

    public partial class MainWindow : Window, MainWindowTagClickCallback, Xl_Tag_Observer, Xl_TextNote_Observer
    {
        public Dictionary<string, List<TAG>> lstOfTags;
        public UserControl curUserControl = null;

        public Style headerStyle { get; set; }
        public Style subHeaderStyle { get; set; }
        public Style childStyle { get; set; }

        public TreeViewItem selectedTag { get; set; }
        private System.Windows.Forms.NotifyIcon notifier = new System.Windows.Forms.NotifyIcon();

        static NoteDBDung noteDB;
        static XuLy_DuLieu_Tag xlTag;
        static XuLy_DuLieu_TextNote xlTextNote;

        public MainWindow()
        {
            InitializeComponent();
            createStyleForTreeItem();
            noteDB = new NoteDBDung();
            xlTag = new XuLy_DuLieu_Tag(noteDB);
            xlTextNote = new XuLy_DuLieu_TextNote(noteDB);
            
            xlTag.register(this);
            xlTextNote.registerNote(this);
            
            if (noteDB.TAGs.Count() == 0)
            {
                initDumpDatabase();
            }

            refreshUI();
            content.Height = MainViewWindow.Height - menu.Height - content.Margin.Top;
            content.Width = MainViewWindow.Width - FirstCollumn.Width.Value;
            curUserControl = new ViewAllTags(xlTag, xlTextNote, this, (int)content.Height, (int)content.Width);
            content.Children.Add(curUserControl);

            ShowInTaskbar = true;
            //MainViewWindow.WindowState = System.Windows.WindowState.Minimized;
            this.notifier.MouseDown += new WinForms.MouseEventHandler(notifier_MouseDown);
            this.notifier.Icon = new System.Drawing.Icon("note.ico");
            this.notifier.Visible = true;

            notifier.Text = "Keep notes";
            notifier.ShowBalloonTip(500, "Keep notes Notification", "Keep notes is comming!",
            System.Windows.Forms.ToolTipIcon.Info);

        }

        private void initDumpDatabase()
        {
            xlTag.insertTAG(new TAG("Vật Lý"));
            xlTag.insertTAG(new TAG("Toán Học"));
            xlTag.insertTAG(new TAG("Ngữ Văn"));
            xlTag.insertTAG(new TAG("Thể Dục"));
            xlTag.insertTAG(new TAG("Lịch Sử"));

            noteDB.SaveChanges();
        }


        // ... { GLOBAL HOOK }
        [DllImport("user32.dll")]
        static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc callback, IntPtr hInstance, uint threadId);

        [DllImport("user32.dll")]
        static extern bool UnhookWindowsHookEx(IntPtr hInstance);

        [DllImport("user32.dll")]
        static extern IntPtr CallNextHookEx(IntPtr idHook, int nCode, int wParam, IntPtr lParam);

        [DllImport("kernel32.dll")]
        static extern IntPtr LoadLibrary(string lpFileName);

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern short GetKeyState(int nVirtKey);

        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        const int WH_KEYBOARD_LL = 13;
        const int WM_KEYDOWN = 0x100;

        public const int VK_LCONTROL = 0xA2;
        public const int VK_RCONTROL = 0xA3;

        private LowLevelKeyboardProc _proc = hookProc;

        private static IntPtr hhook = IntPtr.Zero;

        public void SetHook()
        {
            IntPtr hInstance = LoadLibrary("User32");
            hhook = SetWindowsHookEx(WH_KEYBOARD_LL, _proc, hInstance, 0);
        }

        public static void UnHook()
        {
            UnhookWindowsHookEx(hhook);
        }

        [StructLayout(LayoutKind.Sequential)]
        public class KbLLHookStruct
        {
            public int vkCode;
            public int scanCode;
            public int flags;
            public int time;
            public int dwExtraInfo;
        }

        public static IntPtr hookProc(int code, IntPtr wParam, IntPtr lParam)
        {
            if (code >= 0 && wParam == (IntPtr)WM_KEYDOWN)
            {
                int vkCode = Marshal.ReadInt32(lParam);
                //////ОБРАБОТКА НАЖАТИЯ
                if (vkCode.ToString() == "162")
                {
                    NewNoteWindow newNoteWindow = new NewNoteWindow(xlTag, xlTextNote);
                    newNoteWindow.WindowState = WindowState.Normal;
                    newNoteWindow.ShowDialog();
                }
            }
            return CallNextHookEx(hhook, code, (int)wParam, lParam);
        }

        void notifier_MouseDown(object sender, WinForms.MouseEventArgs e)
        {
            if (e.Button == WinForms.MouseButtons.Right)
            {
                ContextMenu menu = (ContextMenu)this.FindResource("NotifierContextMenu");
                menu.IsOpen = true;
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            // View notes
            refreshUI();
            content.Height = MainViewWindow.Height - menu.Height - content.Margin.Top;
            content.Width = MainViewWindow.Width - FirstCollumn.Width.Value;
            curUserControl = new ViewAllTags(xlTag, xlTextNote, this, (int)content.Height, (int)content.Width);
            content.Children.Add(curUserControl);
            this.WindowState = System.Windows.WindowState.Normal;

        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            // view statistics
            ViewStatisticsWindow viewStatisticsWindow = new ViewStatisticsWindow(xlTag, xlTextNote);
            viewStatisticsWindow.WindowState = System.Windows.WindowState.Normal;
            viewStatisticsWindow.ShowDialog();
        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            // Close app
            MainViewWindow.Close();
        }


        public void createStyleForTreeItem()
        {
            // Header style.
            headerStyle = new Style(typeof(TreeViewItem));
            headerStyle.Setters.Add(new Setter
            {
                Property = TextElement.FontSizeProperty,
                Value = 30.0
            });
            headerStyle.Setters.Add(new Setter
            {
                Property = TextElement.FontFamilyProperty,
                Value = new FontFamily("Segoe UI")
            });
            headerStyle.Setters.Add(new Setter
            {
                Property = TextElement.FontWeightProperty,
                Value = FontWeights.Light
            });
            headerStyle.Setters.Add(new Setter
            {
                Property = TextElement.ForegroundProperty,
                Value = Brushes.Blue
            });
            headerStyle.Setters.Add(new Setter
            {
                Property = TextElement.FocusableProperty,
                Value = false
            });

            // Sub header style.
            subHeaderStyle = new Style(typeof(TreeViewItem));
            subHeaderStyle.Setters.Add(new Setter
            {
                Property = TextElement.FontSizeProperty,
                Value = 14.0
            });
            subHeaderStyle.Setters.Add(new Setter
            {
                Property = TextElement.FontFamilyProperty,
                Value = new FontFamily("Segoe UI")
            });
            subHeaderStyle.Setters.Add(new Setter
            {
                Property = TextElement.FontWeightProperty,
                Value = FontWeights.Light
            });
            subHeaderStyle.Setters.Add(new Setter
            {
                Property = TextElement.ForegroundProperty,
                Value = Brushes.LightBlue
            });

            // Child style.
            childStyle = new Style(typeof(TreeViewItem));
            childStyle.Setters.Add(new Setter
            {
                Property = TextElement.FontSizeProperty,
                Value = 14.0
            });
            childStyle.Setters.Add(new Setter
            {
                Property = TextElement.FontFamilyProperty,
                Value = new FontFamily("Segoe UI")
            });
            childStyle.Setters.Add(new Setter
            {
                Property = TextElement.FontWeightProperty,
                Value = FontWeights.Light
            });
            childStyle.Setters.Add(new Setter
            {
                Property = TextElement.ForegroundProperty,
                Value = Brushes.Black
            });
        }

        public void LoadTreeByTitile(List<TAG> listTag)
        {
            // mac dinh laf sort list tag theo mContent.
            listTag.Sort(delegate (TAG tag1, TAG tag2) { return tag1.mContent.CompareTo(tag2.mContent); });
            lstOfTags = new Dictionary<string, List<TAG>>();

            foreach (TAG tag in listTag)
            {
                string firstChar = tag.mContent[0].ToString();
                if (!lstOfTags.ContainsKey(firstChar))
                {
                    lstOfTags.Add(firstChar, new List<TAG>());
                }
                lstOfTags[firstChar].Add(tag);
            }

            // Them vo tree view.
            tvListTag.Items.Clear();
            TreeViewItem root = new TreeViewItem();
            root.Header = "TAGS";
            root.Style = headerStyle;
            root.IsExpanded = true;
            
            tvListTag.Items.Add(root);

            foreach (var entry in lstOfTags)
            {
                TreeViewItem nodeitem = new TreeViewItem();
                nodeitem.Header = entry.Key;
                nodeitem.IsExpanded = true;
                nodeitem.Style = childStyle;

                foreach(TAG tag in entry.Value)
                {
                    nodeitem.Items.Add(new TreeViewItem() { Header = tag.mContent + " (" + tag.countNumberNote() + ")"});
                }
                root.Items.Add(nodeitem);
            }
            
        }

        public void LoadTreeView()
        {
            // Dau tien la lay tat ca cac tag len.
            List<TAG> listTag = xlTag.getAllTAG();

            LoadTreeByTitile(listTag);
        }

        private void btnNewTag_Click(object sender, RoutedEventArgs e)
        {
            content.Children.Remove(curUserControl);
            curUserControl = new ViewTagDetail(xlTag, xlTextNote, selectedTag.Header.ToString(), (int)content.Height, (int)content.Width);
            content.Children.Add(curUserControl);
        }

        private void tvListTag_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            selectedTag = (TreeViewItem)e.NewValue;
            if (selectedTag != null)
            {
                content.Children.Remove(curUserControl);
                string headerTag = selectedTag.Header.ToString().Substring(0, selectedTag.Header.ToString().LastIndexOf("(") - 1);
                curUserControl = new ViewTagDetail(xlTag, xlTextNote, headerTag, (int)content.Height, (int)content.Width);
                content.Children.Add(curUserControl);
            }
        }

        public void onTagClick(TAG selectedTag)
        {
            // Process selected tag
            content.Children.Remove(curUserControl);
            curUserControl = new ViewTagDetail(xlTag, xlTextNote, selectedTag.mContent, (int)content.Height, (int)content.Width);
            content.Children.Add(curUserControl);
        }

        private void menuExit_Click(object sender, RoutedEventArgs e)
        {
            MainViewWindow.Close();
        }

        private void menuNewNote_Click(object sender, RoutedEventArgs e)
        {
            NewNoteWindow newNoteWindow = new NewNoteWindow(xlTag, xlTextNote);
            newNoteWindow.ShowDialog();
        }

        private void menuNewTag_Click(object sender, RoutedEventArgs e)
        {
            NewTagWindow newTagWindow = new NewTagWindow(xlTag);
            newTagWindow.ShowDialog();
        }

        private void menuViewAllNote_Click(object sender, RoutedEventArgs e)
        {
            string headerTag = "ViewAllNote";
            content.Children.Remove(curUserControl);
            curUserControl = new ViewTagDetail(xlTag, xlTextNote, headerTag, (int)content.Height, (int)content.Width);
            content.Children.Add(curUserControl);
        }

        private void menuViewAllTag_Click(object sender, RoutedEventArgs e)
        {
            content.Height = MainViewWindow.Height - menu.Height;
            content.Width = MainViewWindow.Width - FirstCollumn.Width.Value;
            content.Children.Remove(curUserControl);
            curUserControl = new ViewAllTags(xlTag, xlTextNote, this, (int)content.Height, (int)content.Width);
            content.Children.Add(curUserControl);
        }

        private void menuStatistics_Click(object sender, RoutedEventArgs e)
        {
            ViewStatisticsWindow viewStatisticsWindow = new ViewStatisticsWindow(xlTag, xlTextNote);
            viewStatisticsWindow.ShowDialog();
        }

        private void sortTitle_Click(object sender, RoutedEventArgs e)
        {

        }

        public void refreshUI()
        {
            ClearTreeView();
            LoadTreeView();
        }

        private void ClearTreeView()
        {
            if(tvListTag.Items.Count > 0)
                tvListTag.Items.Clear();
        }

        public void refreshUINote()
        {
            ClearTreeView();
            LoadTreeView();
        }
        
        public void presentStackResultSearch(List<TAG> listTag)
        {
            // Clear the list
            resultStack.Children.Clear();

            // Add the result
            foreach (var obj in listTag)
            {
                TextBlock textBlock = new TextBlock();
                textBlock.Text = obj.mContent;
                // A little style...
                textBlock.Margin = new Thickness(2, 3, 2, 3);
                textBlock.Cursor = Cursors.Hand;

                // Mouse events
                textBlock.MouseLeftButtonUp += (s, ev) =>
                {
                    // Them call refresh UI all tag và show detail tag lên
                    // Thêm behaviour tương tự khi người dùng nhấn nút search
                    txtSearchTag.Text = (s as TextBlock).Text;

                    string headerTag = txtSearchTag.Text;
                    content.Children.Remove(curUserControl);
                    curUserControl = new ViewTagDetail(xlTag, xlTextNote, headerTag, (int)content.Height, (int)content.Width);
                    content.Children.Add(curUserControl);
                    
                    resultStack.Children.Clear();
                    borderResult.Visibility = System.Windows.Visibility.Collapsed;
                };

                textBlock.MouseEnter += (s, ev) =>
                {
                    TextBlock b = s as TextBlock;
                    b.Background = Brushes.PeachPuff;
                };

                textBlock.MouseLeave += (s, ev) =>
                {
                    TextBlock b = s as TextBlock;
                    b.Background = Brushes.Transparent;
                };

                // Add to the panel
                resultStack.Children.Add(textBlock);

            }
        }

        private void btnSearchTag_Click(object sender, RoutedEventArgs e)
        {
            TAG tag = xlTag.getTagByContent(txtSearchTag.Text);
            if (tag == null)
            {
                // Clear
                resultStack.Children.Clear();
                borderResult.Visibility = System.Windows.Visibility.Collapsed;
                MessageBox.Show("This tag isn't exist in database!");
            }
            else
            {
                borderResult.Visibility = System.Windows.Visibility.Visible;
                List<TAG> listTmp = new List<TAG>();
                listTmp.Add(tag);
                presentStackResultSearch(listTmp);

                // Change UI.
                string headerTag = txtSearchTag.Text;
                content.Children.Remove(curUserControl);
                curUserControl = new ViewTagDetail(xlTag, xlTextNote, headerTag, (int)content.Height, (int)content.Width);
                content.Children.Add(curUserControl);
            }
            resultStack.Children.Clear();
            borderResult.Visibility = System.Windows.Visibility.Collapsed;
        }
        

        private void txtSearchTag_TextChanged(object sender, TextChangedEventArgs e)
        {
            List<TAG> listTag;
            if (txtSearchTag.Text == "")
            {
                listTag = xlTag.getAllTAG();
            }
            else
                    listTag = xlTag.findTagsByKeyWord(txtSearchTag.Text);

            if (listTag.Count == 0)
            {
                // Clear
                resultStack.Children.Clear();
                borderResult.Visibility = System.Windows.Visibility.Collapsed;
            }
            else
            {
                borderResult.Visibility = System.Windows.Visibility.Visible;
                presentStackResultSearch(listTag);

            }

        }

        private void MainViewWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
           UnHook();
        }

        private void MainViewWindow_Loaded(object sender, RoutedEventArgs e)
        {
           SetHook();
        }
    }
}
