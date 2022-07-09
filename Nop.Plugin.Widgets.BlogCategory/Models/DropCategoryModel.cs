using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Widgets.BlogCategory.Models
{
    public partial record DropCategoryModel : BaseNopModel
    {

        public DropCategoryModel()
        {
            AvailableCategories = new List<SelectListItem>();
            SelectedBlogCategoryIds = new List<int>();
        }

        public IList<SelectListItem> AvailableCategories { get; set; }
        [NopResourceDisplayName("Plugins.Widgets.BlogCategory.Fields.SelectedBlogCategory")]
        public IList<int> SelectedBlogCategoryIds { get; set; }


    }
}
