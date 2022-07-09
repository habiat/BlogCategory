using Nop.Core;

namespace Nop.Plugin.Widgets.BlogCategory.Domain
{
    public partial class BlogCategory : BaseEntity
    {
        public int ParentId { get; set; }
        public int PictureId { get; set; }
        public string Name { get; set; }
        public string Tag { get; set; }
        public int DisplayOrder { get; set; } 

    }
}
