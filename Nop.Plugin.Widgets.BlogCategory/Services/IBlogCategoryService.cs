using System.Collections.Generic;
using System.Threading.Tasks;
using Nop.Core;
using Nop.Core.Domain.Blogs;
using Nop.Core.Domain.Logging;
using Nop.Plugin.Widgets.BlogCategory.Domain;
using Nop.Plugin.Widgets.BlogCategory.Models;

namespace Nop.Plugin.Widgets.BlogCategory.Services
{
    public interface IBlogCategoryService
    {
        /// <summary>
        /// get all categories
        /// </summary>
        /// <returns>list of blog-categories</returns>
        Task<List<Domain.BlogCategory>> GetAllCategoriesAsync();
        /// <summary>
        /// get blog by id
        /// </summary>
        /// <param name="blogCategoryId"></param>
        /// <returns>category</returns>
        Task<Domain.BlogCategory> GetBlogCategoryByIdAsync(int blogCategoryId);
        /// <summary>
        /// add category
        /// </summary>
        /// <param name="blogCategory"></param>
        /// <returns></returns>
        Task InsertCategoryAsync(Domain.BlogCategory blogCategory);

        /// <summary>
        /// Updates the blogCategory
        /// </summary>
        /// <param name="blogCategory">blogCategory</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task UpdateBlogCategoryAsync(Domain.BlogCategory blogCategory);
        /// <summary>
        /// get categories by blogId
        /// </summary>
        /// <param name="blogId"></param>
        /// <returns>categoryIds list</returns>
        Task<IList<int>> GetBlogCategoryIdsByBlogIdAsync(int blogId);
        Task SaveCategoryMappingsAsync(AddUpdateBlogCategoryModel model);

        /// <summary>
        /// Get formatted category breadcrumb 
        /// Note: ACL and store mapping is ignored
        /// </summary>
        /// <param name="category">Category</param>
        /// <param name="allCategories">All categories</param>
        /// <param name="separator">Separator</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the formatted breadcrumb
        /// </returns>
        Task<string> GetFormattedBreadCrumbAsync(Domain.BlogCategory category, IList<Domain.BlogCategory> allCategories = null,
            string separator = ">>");



        /// <summary>
        /// Inserts activityLogType
        /// </summary>
        /// <param name="activityLogType">activityLogType</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task InsertActivityLogTypeAsync(ActivityLogType activityLogType);

        /// <summary>
        /// Delete activityLogType
        /// </summary>
        /// <param name="activityLogType">activityLogType</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task DeleteActivityLogTypeAsync(ActivityLogType activityLogType);

        /// <summary>
        /// Deletes a blogCategory
        /// </summary>
        /// <param name="blogCategory">blogCategory</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task DeleteBlogCategoryAsync(Domain.BlogCategory blogCategory);




        #region web api methods
        /// <summary>
        /// Gets a category
        /// </summary>
        /// <param name="inputModel">blog Category filter param</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the blogCategory
        /// </returns>
        Task<IList<Domain.BlogCategory>> CategoriesGetBySearch(CategoryInputModel inputModel);

        /// <summary>
        /// Delete activityLogType
        /// </summary>
        /// <param name="categoryIds">categoryIds</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task<IList<BlogPost>> GetBlogsListByCategoryIds(List<int> categoryIds);
        #endregion
    }
}
