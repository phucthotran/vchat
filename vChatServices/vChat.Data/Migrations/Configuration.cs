namespace vChat.Data.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<vChat.Data.vChatContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(vChat.Data.vChatContext context)
        {
            if (context.Database.Exists())
                return;

            context.Question.AddOrUpdate(
                new Model.Entities.Question { Content = "Nơi bạn sinh ra" },
                new Model.Entities.Question { Content = "Trường tiểu học của bạn" },
                new Model.Entities.Question { Content = "Trường trung học (cấp 2 hoặc 3) của bạn" },
                new Model.Entities.Question { Content = "Trường đại học của bạn" },
                new Model.Entities.Question { Content = "Tên người yêu" }
                );

            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}
