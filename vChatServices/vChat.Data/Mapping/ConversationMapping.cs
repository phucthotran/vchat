using System;
using System.Data.Entity.ModelConfiguration;

namespace vChat.Data.Mapping
{
    internal class ConversationMapping : EntityTypeConfiguration<vChat.Model.Entities.Conversation>
    {
        public ConversationMapping()
        {
            ToTable("Conversation");

            Property(c => c.Message).IsRequired().HasMaxLength(1000);
            Property(c => c.Time).IsRequired();
            Property(c => c.RowVersion).IsRowVersion();

            HasKey(c => c.ConversationID);
        }
    }
}
