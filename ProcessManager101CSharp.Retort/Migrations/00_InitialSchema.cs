namespace Migrations;

using FluentMigrator;

[Migration(0)]
public class InitialSchema : Migration
{
    public override void Up()
    {
        // =============================
        // Table: checkpoints
        // -----------------------------
        Create.Table("checkpoints")
            .WithColumn("id").AsString(50).PrimaryKey().NotNullable().Indexed()
            .WithColumn("commit_position").AsInt64().NotNullable()
            .WithColumn("prepare_position").AsInt64().NotNullable();
    }

    public override void Down()
    {
        // Do nothing!
    }
}