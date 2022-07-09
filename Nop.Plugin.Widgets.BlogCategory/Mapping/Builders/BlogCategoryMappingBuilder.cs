using FluentMigrator.Builders.Create.Table;
using Nop.Core.Domain.Blogs;
using Nop.Data.Extensions;
using Nop.Data.Mapping.Builders;

namespace Nop.Plugin.Widgets.BlogCategory.Mapping.Builders
{
    public class BlogCategoryMappingBuilder : NopEntityBuilder<Domain.BlogCategoryMapping>
    {
        public override void MapEntity(CreateTableExpressionBuilder table)
        {
            table
                .WithColumn(nameof(Domain.BlogCategoryMapping.Id)).AsInt32().PrimaryKey().Identity()
                .WithColumn(nameof(Domain.BlogCategoryMapping.CategoryId)).AsInt32().ForeignKey<Domain.BlogCategory>()
                .WithColumn(nameof(Domain.BlogCategoryMapping.BlogId)).AsInt32().ForeignKey<BlogPost>()
                ;
        }
    }
}
