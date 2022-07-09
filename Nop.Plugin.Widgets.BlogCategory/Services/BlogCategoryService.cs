using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nop.Core;
using Nop.Core.Domain.Blogs;
using Nop.Core.Domain.Logging;
using Nop.Data;
using Nop.Plugin.Widgets.BlogCategory.Domain;
using Nop.Plugin.Widgets.BlogCategory.Models;

namespace Nop.Plugin.Widgets.BlogCategory.Services
{
    public class BlogCategoryService : IBlogCategoryService
    {
        #region Fields
        private readonly IRepository<Domain.BlogCategory> _blogCategoryRepository;
        private readonly IRepository<BlogCategoryMapping> _blogCategoryMappingRepository;
        private readonly IRepository<ActivityLogType> _activityLogTypeRepository;
        private readonly IRepository<BlogPost> _blogPostRepository;
        #endregion

        #region Ctor
        public BlogCategoryService(IRepository<Domain.BlogCategory> blogCategoryRepository,
            IRepository<BlogCategoryMapping> blogCategoryMappingRepository,
            IRepository<ActivityLogType> activityLogTypeRepository,
            IRepository<BlogPost> blogPostRepository)
        {
            _blogCategoryRepository = blogCategoryRepository;
            _blogCategoryMappingRepository = blogCategoryMappingRepository;
            _activityLogTypeRepository = activityLogTypeRepository;
            _blogPostRepository = blogPostRepository;
        }
        #endregion

        #region methods

        public virtual async Task<List<Domain.BlogCategory>> GetAllCategoriesAsync()
        {
            var blogCategories = await _blogCategoryRepository.GetAllAsync(async query =>
            {
                return query.OrderBy(c => c.ParentId).ThenBy(c => c.DisplayOrder).ThenBy(c => c.Id);
            });

            //paging
            return await blogCategories.ToListAsync();
        }

        /// <summary>
        /// Gets a category
        /// </summary>
        /// <param name="blogCategoryId">blog Category identifier</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the blogCategory
        /// </returns>
        public virtual async Task<Domain.BlogCategory> GetBlogCategoryByIdAsync(int blogCategoryId)
        {
            return await _blogCategoryRepository.GetByIdAsync(blogCategoryId, cache => default);
        }
        /// <summary>
        /// Inserts blogCategory
        /// </summary>
        /// <param name="blogCategory">blogCategory</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task InsertCategoryAsync(Domain.BlogCategory blogCategory)
        {
            await _blogCategoryRepository.InsertAsync(blogCategory);
        }

        /// <summary>
        /// Updates the blogCategory
        /// </summary>
        /// <param name="blogCategory">blogCategory</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task UpdateBlogCategoryAsync(Domain.BlogCategory blogCategory)
        {
            if (blogCategory == null)
                throw new ArgumentNullException(nameof(blogCategory));

            //validate category hierarchy
            var parentCategory = await GetBlogCategoryByIdAsync(blogCategory.ParentId);
            while (parentCategory != null)
            {
                if (blogCategory.Id == parentCategory.Id)
                {
                    blogCategory.ParentId = 0;
                    break;
                }

                parentCategory = await GetBlogCategoryByIdAsync(parentCategory.ParentId);
            }

            await _blogCategoryRepository.UpdateAsync(blogCategory);
        }

        /// <summary>
        /// Deletes a blogCategory
        /// </summary>
        /// <param name="blogCategory">blogCategory</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task DeleteBlogCategoryAsync(Domain.BlogCategory blogCategory)
        {
            await _blogCategoryRepository.DeleteAsync(blogCategory);
        }

        public virtual async Task SaveCategoryMappingsAsync(AddUpdateBlogCategoryModel model)
        {
            var existingBlogCategories = await GetBlogCategoriesBlogIdAsync(model.BlogId);

            //delete categories
            foreach (var existingBlogCategoryMapping in existingBlogCategories)
                if (!model.selectedCategoryIds.Contains(existingBlogCategoryMapping.CategoryId))
                    await _blogCategoryMappingRepository.DeleteAsync(existingBlogCategoryMapping);

            //add categories
            foreach (var categoryId in model.selectedCategoryIds)
            {
                if (FindProductCategory(existingBlogCategories, model.BlogId, categoryId) == null)
                {
                    await _blogCategoryMappingRepository.InsertAsync(new BlogCategoryMapping()
                    {
                        BlogId = model.BlogId,
                        CategoryId = categoryId
                    });
                }
            }
        }
        /// <summary>
        /// get categories by blogId
        /// </summary>
        /// <param name="blogId"></param>
        /// <returns>categoryIds list</returns>
        public virtual async Task<IList<int>> GetBlogCategoryIdsByBlogIdAsync(int blogId)
        {
            if (blogId == 0)
                return new List<int>();

            return await _blogCategoryMappingRepository.Table.Where(p => p.BlogId == blogId).Select(p => p.CategoryId)
                .ToListAsync();
        }

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
        public virtual async Task<string> GetFormattedBreadCrumbAsync(Domain.BlogCategory category, IList<Domain.BlogCategory> allCategories = null,
            string separator = ">>")
        {
            var result = string.Empty;

            var breadcrumb = await GetCategoryBreadCrumbAsync(category, allCategories, true);
            for (var i = 0; i <= breadcrumb.Count - 1; i++)
            {
                var categoryName = breadcrumb[i].Name;
                result = string.IsNullOrEmpty(result) ? categoryName : $"{result} {separator} {categoryName}";
            }

            return result;
        }

        /// <summary>
        /// Inserts activityLogType
        /// </summary>
        /// <param name="activityLogType">activityLogType</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task InsertActivityLogTypeAsync(ActivityLogType activityLogType)
        {
            await _activityLogTypeRepository.InsertAsync(activityLogType);
        }
        /// <summary>
        /// Delete activityLogType
        /// </summary>
        /// <param name="activityLogType">activityLogType</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task DeleteActivityLogTypeAsync(ActivityLogType activityLogType)
        {
            await _activityLogTypeRepository.DeleteAsync(activityLogType);
        }


        #endregion

        #region web api methods
        /// <summary>
        /// Gets a category
        /// </summary>
        /// <param name="inputModel">blog Category filter param</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the blogCategory
        /// </returns>
        public virtual async Task<IList<Domain.BlogCategory>> CategoriesGetBySearch(CategoryInputModel inputModel)
        {
            return await _blogCategoryRepository.GetAllAsync(query =>
            {
                if (inputModel.ParentId > 0)
                {
                    query = query.Where(p => p.ParentId == inputModel.ParentId);
                }

                if (!string.IsNullOrEmpty(inputModel.TagName))
                {
                    query = query.Where(p => p.Tag.Contains(inputModel.TagName));
                }

                if (!string.IsNullOrEmpty(inputModel.CategoryName))
                {
                    query = query.Where(p => p.Name.Contains(inputModel.CategoryName));
                }

                return query;
            });


        }
        /// <summary>
        /// get blogs by category Ids
        /// </summary>
        /// <param name="categoryIds">categoryIds</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task<IList<BlogPost>> GetBlogsListByCategoryIds(List<int> categoryIds)
        {
            if (categoryIds == null || categoryIds.Count == 0)
                return null;

            var data = (from c in _blogCategoryMappingRepository.Table
                        join b in _blogPostRepository.Table on c.BlogId equals b.Id
                        where categoryIds.Contains(c.CategoryId)
                        select b
                ).ToListAsync();
            return await data;
        }


        #endregion

        #region private-methods
        private async Task<IList<BlogCategoryMapping>> GetBlogCategoriesBlogIdAsync(int blogId)
        {
            if (blogId == 0)
                return new List<BlogCategoryMapping>();

            return await _blogCategoryMappingRepository.Table.Where(p => p.BlogId == blogId)
                .ToListAsync();
        }
        /// <summary>
        /// Returns a BlogCategory that has the specified values
        /// </summary>
        /// <param name="source">Source</param>
        /// <param name="blogId">Product identifier</param>
        /// <param name="categoryId">Category identifier</param>
        /// <returns>A BlogCategory that has the specified values; otherwise null</returns>
        private BlogCategoryMapping FindProductCategory(IList<BlogCategoryMapping> source, int blogId, int categoryId)
        {
            foreach (var blogCategory in source)
                if (blogCategory.BlogId == blogId && blogCategory.CategoryId == categoryId)
                    return blogCategory;

            return null;
        }

        /// <summary>
        /// Get category breadcrumb 
        /// </summary>
        /// <param name="category">Category</param>
        /// <param name="allCategories">All categories</param>
        /// <param name="showHidden">A value indicating whether to load hidden records</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the category breadcrumb 
        /// </returns>
        private async Task<IList<Domain.BlogCategory>> GetCategoryBreadCrumbAsync(Domain.BlogCategory category, IList<Domain.BlogCategory> allCategories = null, bool showHidden = false)
        {
            if (category == null)
                throw new ArgumentNullException(nameof(category));

            var result = new List<Domain.BlogCategory>();
            var alreadyProcessedCategoryIds = new List<int>();

            while (category != null && //not null
                   !alreadyProcessedCategoryIds.Contains(category.Id)) //prevent circular references
            {
                result.Add(category);

                alreadyProcessedCategoryIds.Add(category.Id);

                category = allCategories != null
                    ? allCategories.FirstOrDefault(c => c.Id == category.ParentId)
                    : await GetBlogCategoryByIdAsync(category.ParentId);
            }

            result.Reverse();
            return result;
        }


        #endregion
    }
}
