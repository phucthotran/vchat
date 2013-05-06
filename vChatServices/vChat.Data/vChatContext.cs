using System;
using System.Data.Entity;
using vChat.Model.Entities;
using vChat.Data.Mapping;

namespace vChat.Data
{
    public class vChatContext : DbContext
    {
        public vChatContext()
        //: base("vChatDB")
        {
            Configuration.ProxyCreationEnabled = false;
            Configuration.LazyLoadingEnabled = false;
            System.Data.Entity.Database.SetInitializer<vChatContext>(new CreateDatabaseIfNotExists<vChatContext>());
        }

        //public vChatContext()
        //    : base(GetConnection(@"C:\Connect.txt"))
        //{
        //    Configuration.ProxyCreationEnabled = false;
        //    Configuration.LazyLoadingEnabled = false;
        //    System.Data.Entity.Database.SetInitializer<vChatContext>(new CreateDatabaseIfNotExists<vChatContext>());
        //}

        //Temp Func
        private static String GetConnection(String FileName)
        {
            using (System.IO.FileStream fs = new System.IO.FileStream(FileName, System.IO.FileMode.Open, System.IO.FileAccess.Read))
            {
                System.IO.StreamReader sReader = new System.IO.StreamReader(fs);
                String Connection = sReader.ReadLine();

                return Connection;
            }
        }    

        public DbSet<Users> Users { get; set; }
        public DbSet<Question> Question { get; set; }
        public DbSet<Conversation> Conversation { get; set; }
        public DbSet<FriendGroup> FriendGroup { get; set; }
        public DbSet<FriendMap> FriendMap { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new UserMapping());
            modelBuilder.Configurations.Add(new QuestionMapping());
            modelBuilder.Configurations.Add(new ConversationMapping());
            modelBuilder.Configurations.Add(new FriendGroupMapping());
            modelBuilder.Configurations.Add(new FriendMapMapping());
        }
    }
}
