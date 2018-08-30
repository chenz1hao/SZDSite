namespace SZD.WebSite.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modify5 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "Desc", c => c.String());
            AddColumn("dbo.Solutions", "Desc", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Solutions", "Desc");
            DropColumn("dbo.Products", "Desc");
        }
    }
}
