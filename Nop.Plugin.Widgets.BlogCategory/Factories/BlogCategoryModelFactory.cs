using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Plugin.Widgets.BlogCategory.Infrastructure;
using Nop.Plugin.Widgets.BlogCategory.Models;
using Nop.Plugin.Widgets.BlogCategory.Services;
using Nop.Services.Localization;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;

namespace Nop.Plugin.Widgets.BlogCategory.Factories
{
    public class BlogCategoryModelFactory : IBlogCategoryModelFactory
    {
        #region Fields
        private readonly IBlogCategoryService _blogCategoryService;
        private readonly ILocalizationService _localizationService;
        #endregion

        #region Ctor
        public BlogCategoryModelFactory(IBlogCategoryService blogCategoryService, ILocalizationService localizationService)
        {
            _blogCategoryService = blogCategoryService;
            _localizationService = localizationService;
        }
        #endregion

        #region methods

        public virtual async Task<BlogCategoryModel> PrepareBlogCategoryModelAsync(BlogCategoryModel model, Domain.BlogCategory blogCategory)
        {
            if (blogCategory != null)
            {
                //fill in model values from the entity
                if (model == null)
                {
                    model = blogCategory.ToModel<BlogCategoryModel>();
                }
            }
            model.AvailableCategories = await GetCategoryListItem();


            return model;
        }

        public async Task<IList<SelectListItem>> GetCategoryListItem(bool withSpecialDefaultItem = true)
        {
            var categories = await _blogCategoryService.GetAllCategoriesAsync();
            var availableCategories = await categories.SelectAwait(async c => new SelectListItem
            {
                Text = await _blogCategoryService.GetFormattedBreadCrumbAsync(c, categories),
                Value = c.Id.ToString()
            }).ToListAsync();

            if (!withSpecialDefaultItem)
                return availableCategories;

            var defaultItemText = await _localizationService.GetResourceAsync("Admin.Catalog.Categories.Fields.Parent.None");
            availableCategories.Insert(0, new SelectListItem { Text = defaultItemText, Value = "0" });
            return availableCategories;
        }
        #endregion

    }
}
