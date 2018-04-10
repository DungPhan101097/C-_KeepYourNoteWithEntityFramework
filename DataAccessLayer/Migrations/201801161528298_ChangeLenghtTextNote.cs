namespace DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeLenghtTextNote : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.TEXTNOTEs", "mContent", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TEXTNOTEs", "mContent", c => c.String(maxLength: 2000));
        }
    }
}
