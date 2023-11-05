using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Notebooks.Domain;

namespace Notebooks.Infra.Db.EntityConfigurations;

public class NotebookTypeConfiguration : IEntityTypeConfiguration<Notebook>
{
    public void Configure(EntityTypeBuilder<Notebook> builder)
    {
        builder.ToTable("Notebooks", "notebook");

        builder.HasKey(notebook => notebook.Id);

        builder.Property(notebook => notebook.Id).HasColumnName("Id");
        builder.Property(notebook => notebook.CreatedOn).HasColumnName("CreatedOn");
        builder.Property(notebook => notebook.UpdatedOn).HasColumnName("UpdatedOn");
        builder.Property(notebook => notebook.Title).HasColumnName("Title");
        builder.Property(notebook => notebook.Path).HasColumnName("Path");
    }
}