namespace vChat.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EditUsersTable : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Conversation", name: "ReceivedFrom_UserID", newName: "SendTo_UserID");
        }
        
        public override void Down()
        {
            RenameColumn(table: "dbo.Conversation", name: "SendTo_UserID", newName: "ReceivedFrom_UserID");
        }
    }
}
