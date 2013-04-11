namespace vChat.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateFriendMapMapping : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.FriendMap", name: "UserUserID", newName: "User_UserID");
            RenameColumn(table: "dbo.FriendMap", name: "FriendUserID", newName: "Friend_UserID");
        }
        
        public override void Down()
        {
            RenameColumn(table: "dbo.FriendMap", name: "Friend_UserID", newName: "FriendUserID");
            RenameColumn(table: "dbo.FriendMap", name: "User_UserID", newName: "UserUserID");
        }
    }
}
