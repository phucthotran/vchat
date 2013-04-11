namespace vChat.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EditSomeTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FriendMap", "RowVersion", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
            AddColumn("dbo.FriendGroup", "RowVersion", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.FriendGroup", "RowVersion");
            DropColumn("dbo.FriendMap", "RowVersion");
        }
    }
}
