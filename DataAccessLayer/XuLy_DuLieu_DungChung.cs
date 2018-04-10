using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;


namespace DataAccessLayer
{
    public class XuLy_DuLieu_DungChung
    {
        protected static string connectString;
        protected static string pathToDatabase;

        public XuLy_DuLieu_DungChung()
        {

        }

        public static void Init()
        {
            pathToDatabase = @"E:\LapTrinh\TAILIEU_2017\LTWIN_TDQUANG_2017\Exercise\KeepMyNotes\DataBase\QUANLY_TAGNOTE.mdf";

            connectString = @"Data Source=(localdb)\v11.0;" +
                                        "AttachDbFilename=" + pathToDatabase;
        }

        public DataTable ReadTable(String tableName, DataSet dataSet, String condition = "", String order = "")
        {
            DataTable Kq = new DataTable(tableName);
            dataSet.Tables.Add(Kq);
            String Chuoi_lenh = "Select * From " + tableName;
            if(condition != "")
            {
                Chuoi_lenh += " Where " + condition;
            }

            if (order != "")
            {
                Chuoi_lenh += " Order by " + order;
            }

            SqlDataAdapter Bo_thich_ung = new SqlDataAdapter(Chuoi_lenh, connectString);
            Bo_thich_ung.Fill(dataSet, "TAG");
            Bo_thich_ung.FillSchema(Kq, SchemaType.Source);

            return Kq;
        }
        
    }
}
