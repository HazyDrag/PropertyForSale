namespace PropertyForSaleDomainModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AspNetUsers", "PhoneNumber", c => c.String(nullable: false, maxLength: 11));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AspNetUsers", "PhoneNumber", c => c.String(nullable: false));
        }
    }
}
