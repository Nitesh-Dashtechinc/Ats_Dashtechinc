namespace Ats.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFieldHydrabad : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.InterPersonalInfoes", "ExtCity", c => c.String(maxLength: 8000, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.InterPersonalInfoes", "ExtCity");
        }
    }
}
