using Nop.Core;

namespace Nop.Plugin.Widgets.BlogCategory.Domain
{
    public class BlogCategoryMapping : BaseEntity
    {
        public int BlogId { get; set; }
        public int CategoryId { get; set; }
    }
}
