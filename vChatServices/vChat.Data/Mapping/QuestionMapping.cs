using System;
using System.Data.Entity.ModelConfiguration;

namespace vChat.Data.Mapping
{
    internal class QuestionMapping : EntityTypeConfiguration<vChat.Model.Entities.Question>
    {
        public QuestionMapping()
        {
            ToTable("Question");

            Property(q => q.Content).IsRequired().HasMaxLength(50);
            Property(q => q.RowVersion).IsRowVersion();

            HasKey(q => q.QuestionID);
        }
    }
}
