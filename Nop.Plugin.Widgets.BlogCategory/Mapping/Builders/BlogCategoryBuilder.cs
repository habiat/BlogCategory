using FluentMigrator.Builders.Create.Table;
using Nop.Data.Mapping.Builders;

namespace Nop.Plugin.Widgets.BlogCategory.Mapping.Builders
{
    public class BlogCategoryBuilder : NopEntityBuilder<Domain.BlogCategory>
    {
        public override void MapEntity(CreateTableExpressionBuilder table)
        {
            table
                .WithColumn(nameof(Domain.BlogCategory.Id)).AsInt32().PrimaryKey().Identity()
                .WithColumn(nameof(Domain.BlogCategory.Name)).AsString(200).NotNullable()
                .WithColumn(nameof(Domain.BlogCategory.Tag)).AsString(200).Nullable()
                .WithColumn(nameof(Domain.BlogCategory.DisplayOrder)).AsInt32()
                .WithColumn(nameof(Domain.BlogCategory.ParentId)).AsInt32()
                .WithColumn(nameof(Domain.BlogCategory.PictureId)).AsInt32().Nullable()
            ;

        }
    }
}
