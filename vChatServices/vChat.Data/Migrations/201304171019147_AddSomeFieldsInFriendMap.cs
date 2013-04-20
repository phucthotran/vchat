namespace vChat.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSomeFieldsInFriendMap : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FriendMap", "IsAccepted", c => c.Boolean(nullable: false));
            AddColumn("dbo.FriendMap", "IsIgnored", c => c.Boolean(nullable: false));
            DropColumn("dbo.FriendMap", "IsAvailable");
        }
        
        public override void Down()
        {
            AddColumn("dbo.FriendMap", "IsAvailable", c => c.Boolean(nullable: false));
            DropColumn("dbo.FriendMap", "IsIgnored");
            DropColumn("dbo.FriendMap", "IsAccepted");
        }
    }
}
