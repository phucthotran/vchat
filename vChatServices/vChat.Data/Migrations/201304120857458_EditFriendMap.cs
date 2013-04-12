namespace vChat.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EditFriendMap : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.FriendMap", name: "FriendMap_ID", newName: "FriendGroup_GroupID");            
        }
        
        public override void Down()
        {            
            RenameColumn(table: "dbo.FriendMap", name: "FriendGroup_GroupID", newName: "FriendMap_ID");
        }
    }
}
