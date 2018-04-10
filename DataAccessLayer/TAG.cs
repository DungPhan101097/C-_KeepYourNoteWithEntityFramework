using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace DataAccessLayer
{
    public partial class TAG
    {
        public TAG()
        {
            DRAWINGNOTEs = new HashSet<DRAWINGNOTE>();
            TEXTNOTEs = new HashSet<TEXTNOTE>();
        }

        public TAG(string content) : this()
        {
            mContent = content;
            mAccessTime = DateTime.Now;
        }

        [Key]
        public int IDTag { get; set; }

        [Index(IsUnique = true)]
        [StringLength(100)]
        public string mContent { get; set; }

        public DateTime? mAccessTime { get; set; }
        
        public virtual ICollection<DRAWINGNOTE> DRAWINGNOTEs { get; set; }
        
        public virtual ICollection<TEXTNOTE> TEXTNOTEs { get; set; }

        public override string ToString()
        {
            string res = "IDTag: " + IDTag + " - mContent: " + mContent + " - mAccessTime: " + mAccessTime.ToString();

            return res;
        }

        public bool compareTag(TAG myTag)
        {
            if (this.IDTag == myTag.IDTag)
                return true;
            return false;

        }

        public void assignTag(TAG newTag)
        {
            this.mContent = newTag.mContent;
            this.mAccessTime = newTag.mAccessTime;
        }

        public int countNumberNote()
        {
            return DRAWINGNOTEs.Count + TEXTNOTEs.Count;
        }

        public List<TEXTNOTE> getAllTextNote()
        {
            List<TEXTNOTE> listPreview = new List<TEXTNOTE>();
            foreach (TEXTNOTE note in TEXTNOTEs)
            {
                listPreview.Add(note);
            }

            return listPreview;
        }

        

        //public List<TEXTNOTE> getAllPreviewTextNote()
        //{
        //    List<TEXTNOTE> listPreview = new List<TEXTNOTE>();
        //    foreach (TEXTNOTE note in TEXTNOTEs)
        //    {
        //        listPreview.Add(note.showPreView());
        //    }

        //    return listPreview;
        //}

        //public List<TEXTNOTE> getAllPreviewTextNote()
        //{
        //    List<TEXTNOTE> listPreview = new List<TEXTNOTE>();
        //    foreach (TEXTNOTE note in TEXTNOTEs)
        //    {
        //        listPreview.Add(note.showPreView());
        //    }

        //    return listPreview;
        //}
    }
}
