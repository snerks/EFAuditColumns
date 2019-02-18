namespace ContosoUniversityWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UseDateTimeOffsetForAudit : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Student", "CreatedAt", c => c.DateTimeOffset(nullable: false, precision: 7));
            AlterColumn("dbo.Student", "LastModifiedAt", c => c.DateTimeOffset(nullable: false, precision: 7));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Student", "LastModifiedAt", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Student", "CreatedAt", c => c.DateTime(nullable: false));
        }
    }
}
