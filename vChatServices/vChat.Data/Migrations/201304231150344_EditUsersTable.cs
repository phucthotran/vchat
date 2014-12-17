namespace vChat.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EditUsersTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "Picture", c => c.Binary());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "Picture");
        }
    }
}
