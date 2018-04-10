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

using WordCloudGen = WordCloud.WordCloud;

namespace ViewNoteWindow
{
    /// <summary>
    /// Interaction logic for ViewStatisticsWindow.xaml
    /// </summary>
    public partial class ViewStatisticsWindow : Window
    {
        private XuLy_DuLieu_Tag xlTag;
        private XuLy_DuLieu_TextNote xlTextNote;

        public ViewStatisticsWindow(XuLy_DuLieu_Tag axlTag, XuLy_DuLieu_TextNote axlTextNote)
        {
            InitializeComponent();
            xlTag = axlTag;
            xlTextNote = axlTextNote;

            createWordCloud();
        }

        public void createWordCloud()
        {
            List<string> listTagName = new List<string>();
            List<int> listFreqs = new List<int>();
            List<Tuple<TAG, int>> listNumNotePerTag = new List<Tuple<TAG, int>>();

            listNumNotePerTag = xlTag.statisticNumberNotePerTag();
            int maxCount = listNumNotePerTag.Max(t => t.Item2);
            maxCount++;

            listNumNotePerTag.Sort(delegate (Tuple<TAG, int> tuple1, Tuple<TAG, int> tuple2)
            {
                return tuple2.Item2.CompareTo(tuple1.Item2);
            });

            foreach(var entry in listNumNotePerTag)
            {
                listTagName.Add(entry.Item1.mContent);
                listFreqs.Add(maxCount - entry.Item2);
            }

            var wc = new WordCloudGen(400, 400);
            System.Drawing.Image wordCloudImage = wc.Draw(listTagName, listFreqs);

            // Đổi từ kiểu dữ liệu Drawing.Image sang ImageSource ==> bi
            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            wordCloudImage.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            ms.Seek(0, System.IO.SeekOrigin.Begin);
            bi.StreamSource = ms;
            bi.EndInit();

            // Đặt hình mới được tạo vào (Đây là hình Word Cloud) 
            wordCloud.Source = bi;
        }

        
    }
}
