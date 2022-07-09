using FluentMigrator;
using Nop.Data.Migrations;

namespace Nop.Plugin.Widgets.BlogCategory.Data
{
    [SkipMigrationOnUpdate]
    [NopMigration("2022/06/19 09:30:17", "Widgets.BlogCategory base schema")]
    public class SchemaMigration : AutoReversingMigration
    {
        protected IMigrationManager _migrationManager;

        public SchemaMigration(IMigrationManager migrationManager)
        {
            _migrationManager = migrationManager;
        }

        public override void Up()
        {
            _migrationManager.BuildTable<Domain.BlogCategory>(Create);
            _migrationManager.BuildTable<Domain.BlogCategoryMapping>(Create);
        }
    }
}