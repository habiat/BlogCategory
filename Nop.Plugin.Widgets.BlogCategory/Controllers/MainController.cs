using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nop.Core.Domain.Blogs;
using Nop.Plugin.Widgets.BlogCategory.Authorization.Attributes;
using Nop.Plugin.Widgets.BlogCategory.Infrastructure;
using Nop.Plugin.Widgets.BlogCategory.Models;
using Nop.Plugin.Widgets.BlogCategory.Services;

namespace Nop.Plugin.Widgets.BlogCategory.Controllers
{

    [AuthorizePermission("PublicStoreAllowNavigation")]
    public class MainController : BaseApiController
    {
        private readonly IBlogCategoryService _blogCategoryService;

        public MainController(IBlogCategoryService blogCategoryService)
        {
            _blogCategoryService = blogCategoryService;
        }
        /// <summary>
        ///    fetch categories by tag,parentId,category name
        /// </summary>
        [HttpGet]
        [Route("/api/categories", Name = "GetCategories")]
        public async Task<ApiResult<List<Domain.BlogCategory>>> GetCategories(CategoryInputModel inputInputModel)
        {
            return Ok(await _blogCategoryService.CategoriesGetBySearch(inputInputModel));
        }
        /// <summary>
        /// get blogs by category Ids
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("/api/GetBlogsList", Name = "GetBlogsListByCategoryIds")]
        public async Task<ApiResult<List<BlogPost>>> GetBlogsListByCategoryIds(List<int> ids)
        {
            return Ok(await _blogCategoryService.GetBlogsListByCategoryIds(ids));
        }
    }
}
