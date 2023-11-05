using FluentMigrator.Runner.VersionTableInfo;

namespace Notebooks.Infra.Db.Migrations;

[VersionTableMetaData]
// #pragma warning disable CS0618
public class M0000CreateSchema : IVersionTableMetaData
// #pragma warning restore CS0618
{
    public string SchemaName => "notebook";
    public string TableName => "version_info";
    public string ColumnName => "version";
    public string UniqueIndexName => "uc_version";
    public string AppliedOnColumnName => "applied_on";
    public string DescriptionColumnName => "description";
    public bool OwnsSchema => true;
    public object ApplicationContext { get; set; } = null!;
}