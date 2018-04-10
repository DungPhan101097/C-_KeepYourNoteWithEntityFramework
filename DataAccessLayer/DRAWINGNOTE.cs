namespace DataAccessLayer
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    
    public partial class DRAWINGNOTE
    {
        public DRAWINGNOTE()
        {
            TAGs = new HashSet<TAG>();
        }

        [Key]
        public int IDNote { get; set; }

        [StringLength(100)]
        public string mTitle { get; set; }

        [Column(TypeName = "image")]
        public byte[] mImage { get; set; }

        public DateTime? mAccessTime { get; set; }

        public virtual ICollection<TAG> TAGs { get; set; }


    }
}
