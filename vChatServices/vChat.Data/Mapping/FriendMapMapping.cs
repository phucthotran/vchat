using System;
using System.Data.Entity.ModelConfiguration;

namespace vChat.Data.Mapping
{
    internal class FriendMapMapping : EntityTypeConfiguration<vChat.Model.Entities.FriendMap>
    {
        public FriendMapMapping()
        {
            ToTable("FriendMap");

            //HasRequired(f => f.User).WithMany(u => u.FriendsFake).HasForeignKey(f => f.UserUserID).WillCascadeOnDelete(false);
            //HasRequired(f => f.Friend).WithMany(u => u.Friends).HasForeignKey(f => f.FriendUserID).WillCascadeOnDelete(false);

            //HasRequired(f => f.User).WithMany(u => u.FriendsFake).WillCascadeOnDelete(false);
            //HasRequired(f => f.Friend).WithMany(u => u.Friends).WillCascadeOnDelete(false);
            Property(f => f.RowVersion).IsRowVersion();
            //HasOptional(f => f.FriendGroup).WithOptionalDependent();

            HasKey(f => f.FriendMapID);
        }
    }
}
