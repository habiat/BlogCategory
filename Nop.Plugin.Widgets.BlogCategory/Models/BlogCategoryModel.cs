using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Widgets.BlogCategory.Models
{
    public partial record BlogCategoryModel : BaseNopEntityModel
    {
        public BlogCategoryModel()
        {
            BlogCategoryBreadcrumb = new List<BlogCategoryModel>();
            AvailableCategories = new List<SelectListItem>();
        }
        [NopResourceDisplayName("Plugins.Widgets.BlogCategory.Fields.ParentId")]

        public int ParentId { get; set; }
        [NopResourceDisplayName("Plugins.Widgets.BlogCategory.Fields.PictureId")]
        [UIHint("Picture")]
        public int PictureId { get; set; }
        [NopResourceDisplayName("Plugins.Widgets.BlogCategory.Fields.Name")]
        public string Name { get; set; }
        [NopResourceDisplayName("Plugins.Widgets.BlogCategory.Fields.Tag")]

        public string Tag { get; set; }
        [NopResourceDisplayName("Plugins.Widgets.BlogCategory.Fields.DisplayOrder")]

        public int DisplayOrder { get; set; }
        public string Breadcrumb { get; set; }
        public IList<BlogCategoryModel> BlogCategoryBreadcrumb { get; set; }
        public IList<SelectListItem> AvailableCategories { get; set; }




    }
}
