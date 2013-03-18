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
                        ReceivedFrom_UserID = c.Int(),
                    })
                .PrimaryKey(t => t.ConversationID)
                .ForeignKey("dbo.Users", t => t.SentBy_UserID)
                .ForeignKey("dbo.Users", t => t.ReceivedFrom_UserID)
                .Index(t => t.SentBy_UserID)
                .Index(t => t.ReceivedFrom_UserID);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.Conversation", new[] { "ReceivedFrom_UserID" });
            DropIndex("dbo.Conversation", new[] { "SentBy_UserID" });
            DropIndex("dbo.Users", new[] { "Question_QuestionID" });
            DropForeignKey("dbo.Conversation", "ReceivedFrom_UserID", "dbo.Users");
            DropForeignKey("dbo.Conversation", "SentBy_UserID", "dbo.Users");
            DropForeignKey("dbo.Users", "Question_QuestionID", "dbo.Question");
            DropTable("dbo.Conversation");
            DropTable("dbo.Question");
            DropTable("dbo.Users");
        }
    }
}
