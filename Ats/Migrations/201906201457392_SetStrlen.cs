namespace Ats.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SetStrlen : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.InterPersonalInfoes", "ExtCity", c => c.String(maxLength: 40, unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.InterPersonalInfoes", "ExtCity", c => c.String(maxLength: 8000, unicode: false));
        }
    }
}
