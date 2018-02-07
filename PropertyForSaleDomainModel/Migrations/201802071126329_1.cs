namespace PropertyForSaleDomainModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Adverts", "User_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Adverts", new[] { "User_Id" });
            AlterColumn("dbo.Adverts", "User_Id", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.Adverts", "User_Id");
            AddForeignKey("dbo.Adverts", "User_Id", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Adverts", "User_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Adverts", new[] { "User_Id" });
            AlterColumn("dbo.Adverts", "User_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.Adverts", "User_Id");
            AddForeignKey("dbo.Adverts", "User_Id", "dbo.AspNetUsers", "Id");
        }
    }
}
