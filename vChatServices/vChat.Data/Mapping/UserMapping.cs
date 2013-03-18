using System;
using System.Data.Entity.ModelConfiguration;

namespace vChat.Data.Mapping
{
    internal class UserMapping : EntityTypeConfiguration<vChat.Model.Entities.Users>
    {
        public UserMapping()
        {
            ToTable("Users");

            Property(u => u.Username).IsRequired().HasMaxLength(45);
            Property(u => u.Password).IsRequired().HasMaxLength(45);
            Property(u => u.FirstName).IsRequired().HasMaxLength(45);
            Property(u => u.LastName).IsRequired().HasMaxLength(45);
            Property(u => u.Answer).IsRequired().HasMaxLength(50);
            Property(u => u.Birthdate).IsRequired();
            Property(u => u.RowVersion).IsRowVersion();

            HasKey(u => u.UserID);
            HasOptional(u => u.Question).WithOptionalDependent();
            HasMany(u => u.SentMessage).WithOptional(c => c.SentBy).WillCascadeOnDelete(false);
            HasMany(u => u.ReceivedMessage).WithOptional(c => c.SendTo).WillCascadeOnDelete(false);
        }
    }
}
