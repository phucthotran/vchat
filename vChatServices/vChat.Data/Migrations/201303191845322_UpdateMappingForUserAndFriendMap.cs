namespace vChat.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateMappingForUserAndFriendMap : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.FriendMap", "User_UserID", c => c.Int());
            AlterColumn("dbo.FriendMap", "Friend_UserID", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.FriendMap", "Friend_UserID", c => c.Int(nullable: false));
            AlterColumn("dbo.FriendMap", "User_UserID", c => c.Int(nullable: false));
        }
    }
}
