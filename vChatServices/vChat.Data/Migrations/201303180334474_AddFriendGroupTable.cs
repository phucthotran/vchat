namespace vChat.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFriendGroupTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FriendGroup",
                c => new
                    {
                        GroupID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 45),
                        Owner_UserID = c.Int(),
                    })
                .PrimaryKey(t => t.GroupID)
                .ForeignKey("dbo.Users", t => t.Owner_UserID)
                .Index(t => t.Owner_UserID);
            
            AddColumn("dbo.Users", "Group_GroupID", c => c.Int());
            AddForeignKey("dbo.Users", "Group_GroupID", "dbo.FriendGroup", "GroupID");
            CreateIndex("dbo.Users", "Group_GroupID");
        }
        
        public override void Down()
        {
            DropIndex("dbo.FriendGroup", new[] { "Owner_UserID" });
            DropIndex("dbo.Users", new[] { "Group_GroupID" });
            DropForeignKey("dbo.FriendGroup", "Owner_UserID", "dbo.Users");
            DropForeignKey("dbo.Users", "Group_GroupID", "dbo.FriendGroup");
            DropColumn("dbo.Users", "Group_GroupID");
            DropTable("dbo.FriendGroup");
        }
    }
}
