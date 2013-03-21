using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;

namespace vChat.Data.Mapping
{
    internal class FriendGroupMapping : EntityTypeConfiguration<vChat.Model.Entities.FriendGroup>
    {
        public FriendGroupMapping()
        {
            ToTable("FriendGroup");

            Property(g => g.Name).IsRequired().HasMaxLength(45);
            Property(g => g.RowVersion).IsRowVersion();
            HasOptional(g => g.Owner).WithOptionalDependent();

            HasKey(g => g.GroupID);
        }
    }
}
