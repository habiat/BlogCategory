using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Plugin.Widgets.BlogCategory.Models;

namespace Nop.Plugin.Widgets.BlogCategory.Factories
{
    public interface IBlogCategoryModelFactory
    {
        Task<BlogCategoryModel> PrepareBlogCategoryModelAsync(BlogCategoryModel model, Domain.BlogCategory blogCategory);
        Task<IList<SelectListItem>> GetCategoryListItem(bool withSpecialDefaultItem = true);
    }
}
