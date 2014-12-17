namespace vChat.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserID = c.Int(nullable: false, identity: true),
                        Username = c.String(nullable: false, maxLength: 45),
                        Password = c.String(nullable: false, maxLength: 45),
                        FirstName = c.String(nullable: false, maxLength: 45),
                        LastName = c.String(nullable: false, maxLength: 45),
                        Answer = c.String(nullable: false, maxLength: 50),
                        Birthdate = c.DateTime(nullable: false),
                        Deactive = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        Question_QuestionID = c.Int(),
                    })
                .PrimaryKey(t => t.UserID)
                .ForeignKey("dbo.Question", t => t.Question_QuestionID)
                .Index(t => t.Question_QuestionID);
            
            CreateTable(
                "dbo.Question",
                c => new
                    {
                        QuestionID = c.Int(nullable: false, identity: true),
                        Content = c.String(nullable: false, maxLength: 50),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.QuestionID);
            
            CreateTable(
                "dbo.Conversation",
                c => new
                    {
                        ConversationID = c.Int(nullable: false, identity: true),
                        Message = c.String(nullable: false, maxLength: 1000),
                        Time = c.DateTime(nullable: false),
                        IsRead = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        SentBy_UserID = c.Int(),
                        SendTo_UserID = c.Int(),
                    })
                .PrimaryKey(t => t.ConversationID)
                .ForeignKey("dbo.Users", t => t.SentBy_UserID)
                .ForeignKey("dbo.Users", t => t.SendTo_UserID)
                .Index(t => t.SentBy_UserID)
                .Index(t => t.SendTo_UserID);
            
            CreateTable(
                "dbo.FriendMap",
                c => new
                    {
                        FriendMapID = c.Int(nullable: false, identity: true),
                        UserUserID = c.Int(nullable: false),
                        FriendUserID = c.Int(nullable: false),
                        IsAvailable = c.Boolean(nullable: false),
                        FriendGroup_GroupID = c.Int(),
                    })
                .PrimaryKey(t => t.FriendMapID)
                .ForeignKey("dbo.Users", t => t.UserUserID)
                .ForeignKey("dbo.Users", t => t.FriendUserID)
                .ForeignKey("dbo.FriendGroup", t => t.FriendGroup_GroupID)
                .Index(t => t.UserUserID)
                .Index(t => t.FriendUserID)
                .Index(t => t.FriendGroup_GroupID);
            
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
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.FriendGroup", new[] { "Owner_UserID" });
            DropIndex("dbo.FriendMap", new[] { "FriendGroup_GroupID" });
            DropIndex("dbo.FriendMap", new[] { "FriendUserID" });
            DropIndex("dbo.FriendMap", new[] { "UserUserID" });
            DropIndex("dbo.Conversation", new[] { "SendTo_UserID" });
            DropIndex("dbo.Conversation", new[] { "SentBy_UserID" });
            DropIndex("dbo.Users", new[] { "Question_QuestionID" });
            DropForeignKey("dbo.FriendGroup", "Owner_UserID", "dbo.Users");
            DropForeignKey("dbo.FriendMap", "FriendGroup_GroupID", "dbo.FriendGroup");
            DropForeignKey("dbo.FriendMap", "FriendUserID", "dbo.Users");
            DropForeignKey("dbo.FriendMap", "UserUserID", "dbo.Users");
            DropForeignKey("dbo.Conversation", "SendTo_UserID", "dbo.Users");
            DropForeignKey("dbo.Conversation", "SentBy_UserID", "dbo.Users");
            DropForeignKey("dbo.Users", "Question_QuestionID", "dbo.Question");
            DropTable("dbo.FriendGroup");
            DropTable("dbo.FriendMap");
            DropTable("dbo.Conversation");
            DropTable("dbo.Question");
            DropTable("dbo.Users");
        }
    }
}
