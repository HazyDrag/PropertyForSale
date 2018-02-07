namespace PropertyForSaleDomainModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AdvertsEdit : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Adverts", "Name", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Adverts", "Town", c => c.String(maxLength: 50));
            AlterColumn("dbo.Adverts", "Description", c => c.String(maxLength: 400));
            AlterColumn("dbo.Adverts", "Type", c => c.String(nullable: false, maxLength: 40));
            DropColumn("dbo.Adverts", "Photo");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Adverts", "Photo", c => c.String());
            AlterColumn("dbo.Adverts", "Type", c => c.String());
            AlterColumn("dbo.Adverts", "Description", c => c.String());
            AlterColumn("dbo.Adverts", "Town", c => c.String());
            AlterColumn("dbo.Adverts", "Name", c => c.String(maxLength: 255));
        }
    }
}
