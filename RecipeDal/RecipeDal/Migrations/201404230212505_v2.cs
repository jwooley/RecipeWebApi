namespace RecipeDal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v2 : DbMigration
    {
        public override void Up()
        {
            
            DropColumn("dbo.Categories", "DisplayOrder");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Categories", "DisplayOrder", c => c.Long());
        }
    }
}
