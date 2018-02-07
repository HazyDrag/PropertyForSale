namespace PropertyForSaleDomainModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Photos", "Advert_ID", "dbo.Adverts");
            DropForeignKey("dbo.Adverts", "User_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Adverts", new[] { "User_Id" });
            DropIndex("dbo.Photos", new[] { "Advert_ID" });
            AlterColumn("dbo.AdTypes", "Name", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.AdTypes", "Description", c => c.String(maxLength: 255));
            AlterColumn("dbo.Adverts", "User_Id", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Photos", "Path", c => c.String(nullable: false, maxLength: 255));
            AlterColumn("dbo.Photos", "Advert_ID", c => c.Int(nullable: false));
            CreateIndex("dbo.Adverts", "User_Id");
            CreateIndex("dbo.Photos", "Advert_ID");
            AddForeignKey("dbo.Photos", "Advert_ID", "dbo.Adverts", "ID", cascadeDelete: true);
            AddForeignKey("dbo.Adverts", "User_Id", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Adverts", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Photos", "Advert_ID", "dbo.Adverts");
            DropIndex("dbo.Photos", new[] { "Advert_ID" });
            DropIndex("dbo.Adverts", new[] { "User_Id" });
            AlterColumn("dbo.Photos", "Advert_ID", c => c.Int());
            AlterColumn("dbo.Photos", "Path", c => c.String());
            AlterColumn("dbo.Adverts", "User_Id", c => c.String(maxLength: 128));
            AlterColumn("dbo.AdTypes", "Description", c => c.String());
            AlterColumn("dbo.AdTypes", "Name", c => c.String());
            CreateIndex("dbo.Photos", "Advert_ID");
            CreateIndex("dbo.Adverts", "User_Id");
            AddForeignKey("dbo.Adverts", "User_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Photos", "Advert_ID", "dbo.Adverts", "ID");
        }
    }
}
