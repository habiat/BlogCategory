using System;
using Nop.Web.Framework.Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Nop.Plugin.Widgets.BlogCategory.Models
{
    public partial record BlogCategorySearchModel : BaseSearchModel
    {

        public string SearchBlogCategoryName { get; set; }
    }




}
