using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nop.Plugin.Widgets.BlogCategory.Factories;
using Nop.Plugin.Widgets.BlogCategory.Models;
using Nop.Plugin.Widgets.BlogCategory.Services;
using Nop.Web.Framework.Components;

namespace Nop.Plugin.Widgets.BlogCategory.Components
{
    [ViewComponent(Name = "WidgetsBlogCategory")]
    public class WidgetBlogCategoryViewComponent : NopViewComponent
    {
        private readonly IBlogCategoryModelFactory _blogCategoryModelFactory;
        private readonly IBlogCategoryService _blogCategoryService;
        public WidgetBlogCategoryViewComponent(IBlogCategoryModelFactory blogCategoryModelFactory, IBlogCategoryService blogCategoryService)
        {
            _blogCategoryModelFactory = blogCategoryModelFactory;
            _blogCategoryService = blogCategoryService;
        }
        public async Task<IViewComponentResult> InvokeAsync(string widgetZone, object additionalData)
        {
           // int blogId = Convert.ToInt32(additionalData.Id);
            var blogId = additionalData is Web.Areas.Admin.Models.Blogs.BlogPostModel entityModel ? entityModel.Id : 0;


            var model = new DropCategoryModel();
            model.AvailableCategories = await _blogCategoryModelFactory.GetCategoryListItem(withSpecialDefaultItem:false);
            model.SelectedBlogCategoryIds = await _blogCategoryService.GetBlogCategoryIdsByBlogIdAsync(blogId);
            return View("~/Plugins/Widgets.BlogCategory/Views/DropDownCategories.cshtml", model);
        }
    }
}
