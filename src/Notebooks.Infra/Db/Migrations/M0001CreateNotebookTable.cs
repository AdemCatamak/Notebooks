using FluentMigrator;

namespace Notebooks.Infra.Db.Migrations;

[Migration(0001)]
public class M0001CreateNotebookTable : Migration
{
    public override void Up()
    {
        Create.Table("Notebooks").InSchema("notebook")
            .WithColumn("Id").AsGuid().PrimaryKey()
            .WithColumn("CreatedOn").AsDateTime2().NotNullable()
            .WithColumn("UpdatedOn").AsDateTime2().NotNullable()
            .WithColumn("Title").AsString().NotNullable()
            .WithColumn("Path").AsString().NotNullable()
            ;

        Create.Index("ix__notebook__Notebooks__Title")
            .OnTable("Notebooks").InSchema("notebook")
            .OnColumn("Title");
    }

    public override void Down()
    {
        Delete.Table("Notebooks").InSchema("notebook");
    }
}