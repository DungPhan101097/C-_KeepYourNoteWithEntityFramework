namespace DataAccessLayer
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Text;

    public partial class TEXTNOTE
    {
        const int MAX_PREVIEW = 50;
        public TEXTNOTE()
        {
            TAGs = new HashSet<TAG>();
        }

        public TEXTNOTE(string title, string content) : this()
        {
            mTitle = title;
            mContent = content;
            mAccessTime = DateTime.Now;
        }

        [Key]
        public int IdNote { get; set; }

        [StringLength(100)]
        public string mTitle { get; set; }

        [StringLength(20000000)]
        public string mContent { get; set; }
       
        public DateTime? mAccessTime { get; set; }

        public virtual ICollection<TAG> TAGs { get; set; }

        public override string ToString()
        {
            string res = "IDNote: " + IdNote + " - mTitle: " + mTitle + " - mContent: " + mContent + " - mccessTime: " + mAccessTime.ToString();

            return res;
        }

        public void assignTextNote(TEXTNOTE newNote)
        {
            this.IdNote = newNote.IdNote;
            this.mTitle = newNote.mTitle;
            if(newNote.mContent != null)
                this.mContent = newNote.mContent.Clone().ToString();
            this.mAccessTime = newNote.mAccessTime;
        }

        public string showPreView()
        {
            string preview = "";
            preview += this.mAccessTime.ToString() + "\n";
            if (this.mTitle.Length > MAX_PREVIEW)
            {
                preview += this.mTitle.Substring(0, MAX_PREVIEW);
                preview += "...";
            }
            else
            {
                preview += this.mTitle;
            }
            preview += "\n";

            if (this.mContent.Length > MAX_PREVIEW)
            {
                preview += this.mContent.Substring(0, MAX_PREVIEW);
            }
            else
            {
                preview += this.mContent;
                preview += "...";
            }
            return preview;
        }

        public void ShowFullTextNote()
        {
            Console.WriteLine("Title: " + this.mTitle);
            Console.WriteLine("Content: " + this.mContent);
            Console.WriteLine("Access time: " + this.mAccessTime.ToString());
        }

        public List<TAG> getAllTagPreTextNote()
        {
            List<TAG> listTag = new List<TAG>();
            foreach(TAG tag in TAGs)
            {
                listTag.Add(tag);
            }
            return listTag;
        }
    }
}
