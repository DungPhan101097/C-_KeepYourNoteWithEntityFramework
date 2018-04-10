namespace DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ReInitDatabase : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DRAWINGNOTEs",
                c => new
                    {
                        IDNote = c.Int(nullable: false, identity: true),
                        mTitle = c.String(maxLength: 100),
                        mImage = c.Binary(storeType: "image"),
                        mAccessTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.IDNote);
            
            CreateTable(
                "dbo.TAGs",
                c => new
                    {
                        IDTag = c.Int(nullable: false, identity: true),
                        mContent = c.String(maxLength: 100),
                        mAccessTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.IDTag)
                .Index(t => t.mContent, unique: true);
            
            CreateTable(
                "dbo.TEXTNOTEs",
                c => new
                    {
                        IdNote = c.Int(nullable: false, identity: true),
                        mTitle = c.String(maxLength: 100),
                        mContent = c.String(maxLength: 2000),
                        mAccessTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.IdNote);
            
            CreateTable(
                "dbo.TAGDRAWINGNOTEs",
                c => new
                    {
                        TAG_IDTag = c.Int(nullable: false),
                        DRAWINGNOTE_IDNote = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.TAG_IDTag, t.DRAWINGNOTE_IDNote })
                .ForeignKey("dbo.TAGs", t => t.TAG_IDTag, cascadeDelete: true)
                .ForeignKey("dbo.DRAWINGNOTEs", t => t.DRAWINGNOTE_IDNote, cascadeDelete: true)
                .Index(t => t.TAG_IDTag)
                .Index(t => t.DRAWINGNOTE_IDNote);
            
            CreateTable(
                "dbo.TEXTNOTETAGs",
                c => new
                    {
                        TEXTNOTE_IdNote = c.Int(nullable: false),
                        TAG_IDTag = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.TEXTNOTE_IdNote, t.TAG_IDTag })
                .ForeignKey("dbo.TEXTNOTEs", t => t.TEXTNOTE_IdNote, cascadeDelete: true)
                .ForeignKey("dbo.TAGs", t => t.TAG_IDTag, cascadeDelete: true)
                .Index(t => t.TEXTNOTE_IdNote)
                .Index(t => t.TAG_IDTag);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TEXTNOTETAGs", "TAG_IDTag", "dbo.TAGs");
            DropForeignKey("dbo.TEXTNOTETAGs", "TEXTNOTE_IdNote", "dbo.TEXTNOTEs");
            DropForeignKey("dbo.TAGDRAWINGNOTEs", "DRAWINGNOTE_IDNote", "dbo.DRAWINGNOTEs");
            DropForeignKey("dbo.TAGDRAWINGNOTEs", "TAG_IDTag", "dbo.TAGs");
            DropIndex("dbo.TEXTNOTETAGs", new[] { "TAG_IDTag" });
            DropIndex("dbo.TEXTNOTETAGs", new[] { "TEXTNOTE_IdNote" });
            DropIndex("dbo.TAGDRAWINGNOTEs", new[] { "DRAWINGNOTE_IDNote" });
            DropIndex("dbo.TAGDRAWINGNOTEs", new[] { "TAG_IDTag" });
            DropIndex("dbo.TAGs", new[] { "mContent" });
            DropTable("dbo.TEXTNOTETAGs");
            DropTable("dbo.TAGDRAWINGNOTEs");
            DropTable("dbo.TEXTNOTEs");
            DropTable("dbo.TAGs");
            DropTable("dbo.DRAWINGNOTEs");
        }
    }
}
