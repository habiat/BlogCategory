using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nop.Plugin.Widgets.BlogCategory.Factories;
using Nop.Plugin.Widgets.BlogCategory.Infrastructure;
using Nop.Plugin.Widgets.BlogCategory.Models;
using Nop.Plugin.Widgets.BlogCategory.Services;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Nop.Services.Media;
using Nop.Services.Messages;
using Nop.Services.Security;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Models.DataTables;
using Nop.Web.Framework.Mvc.Filters;

namespace Nop.Plugin.Widgets.BlogCategory.Controllers
{
    [Area(AreaNames.Admin)]
    [AuthorizeAdmin]
    [AutoValidateAntiforgeryToken]
    public class BlogCategoryController : BasePluginController
    {
        #region Fields

        private readonly ILocalizationService _localizationService;
        private readonly IPermissionService _permissionService;
        private readonly IBlogCategoryService _blogCategoryService;
        private readonly IBlogCategoryModelFactory _blogCategoryModelFactory;
        private readonly IPictureService _pictureService;
        private readonly ICustomerActivityService _customerActivityService;
        private readonly INotificationService _notificationService;


        #endregion

        #region Ctor
        public BlogCategoryController(IPermissionService permissionService,
            ILocalizationService localizationService,
            IBlogCategoryService blogCategoryService,
            IBlogCategoryModelFactory blogCategoryModelFactory,
            IPictureService pictureService,
            ICustomerActivityService customerActivityService,
            INotificationService notificationService)
        {

            _permissionService = permissionService;
            _localizationService = localizationService;
            _blogCategoryService = blogCategoryService;
            _blogCategoryModelFactory = blogCategoryModelFactory;
            _pictureService = pictureService;
            _customerActivityService = customerActivityService;
            _notificationService = notificationService;
        }


        #endregion

        #region methods
        [HttpGet]
        public virtual async Task<IActionResult> List()
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageWidgets))
                return AccessDeniedView();
            var model = new BlogCategorySearchModel();
            return View("~/Plugins/Widgets.BlogCategory/Views/List.cshtml", model);
        }
        [HttpPost]
        public virtual async Task<IActionResult> BlogCategoryGetAll(BlogCategoryModel searchModel)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageWidgets))
                return AccessDeniedView();
            try
            {
                var categories = await _blogCategoryService.GetAllCategoriesAsync();
                var items = new List<BlogCategoryModel>();
                foreach (var cat in categories)
                {
                    var categoryModel = cat.ToModel<BlogCategoryModel>();
                    categoryModel.Breadcrumb = await _blogCategoryService.GetFormattedBreadCrumbAsync(cat, categories);
                    items.Add(categoryModel);
                }

                return Ok(new DataTablesModel
                {
                    Data = items

                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        public virtual async Task<IActionResult> CreateOrUpdate(int? id)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageWidgets))
                return AccessDeniedView();

            if (id == null)
            {
                //prepare model
                var newModel = await _blogCategoryModelFactory.PrepareBlogCategoryModelAsync(new BlogCategoryModel(), null);
                return View("~/Plugins/Widgets.BlogCategory/Views/CreateOrUpdate.cshtml", newModel);
            }
            //try to get a blog category with the specified id
            var category = await _blogCategoryService.GetBlogCategoryByIdAsync((int)id);
            var model = await _blogCategoryModelFactory.PrepareBlogCategoryModelAsync(null, category);

            return View("~/Plugins/Widgets.BlogCategory/Views/CreateOrUpdate.cshtml", model);
        }
        [HttpPost]
        public virtual async Task<IActionResult> CreateOrUpdate(BlogCategoryModel model)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageWidgets))
                return AccessDeniedView();

            if (ModelState.IsValid)
            {
                if (model.Id > 0)
                {
                    //try to get a blog category with the specified id
                    var blogCategory = await _blogCategoryService.GetBlogCategoryByIdAsync(model.Id);
                    if (blogCategory == null)
                        return RedirectToAction("List");

                    var prevPictureId = blogCategory.PictureId;

                    blogCategory = model.ToEntity<Domain.BlogCategory>();


                    await _blogCategoryService.UpdateBlogCategoryAsync(blogCategory);

                    #region picture-update
                    //delete an old picture (if deleted or updated)
                    if (prevPictureId > 0 && prevPictureId != blogCategory.PictureId)
                    {
                        var prevPicture = await _pictureService.GetPictureByIdAsync(prevPictureId);
                        if (prevPicture != null)
                            await _pictureService.DeletePictureAsync(prevPicture);
                    }
                    //update picture seo file name
                    await UpdatePictureSeoNamesAsync(blogCategory);

                    await _customerActivityService.InsertActivityAsync("UpdateBlogCategory",
                        string.Format(await _localizationService.GetResourceAsync("Plugins.Widgets.BlogCategory.ActivityLog.UpdateBlogCategory"), blogCategory.Name), blogCategory);

                    _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Plugins.Widgets.BlogCategory.Updated"));


                    #endregion

                }
                else
                {
                    //var blogCategory = new Domain.BlogCategory();
                    var blogCategory = model.ToEntity<Domain.BlogCategory>() ?? new Domain.BlogCategory();

                    await _blogCategoryService.InsertCategoryAsync(blogCategory);

                    //update picture seo file name
                    await UpdatePictureSeoNamesAsync(blogCategory);

                    await _customerActivityService.InsertActivityAsync("CreateBlogCategory",
                        string.Format(await _localizationService.GetResourceAsync("Plugins.Widgets.BlogCategory.ActivityLog.CreateBlogategory"), blogCategory.Name), blogCategory);

                    _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Plugins.Widgets.BlogCategory.Created"));
                }


                return RedirectToAction("List");

            }
            //prepare model
            model = await _blogCategoryModelFactory.PrepareBlogCategoryModelAsync(model, null);

            //if we got this far, something failed, redisplay form
            return View("~/Plugins/Widgets.BlogCategory/Views/List.cshtml", model);

        }
        protected virtual async Task UpdatePictureSeoNamesAsync(Domain.BlogCategory blogCategory)
        {
            var picture = await _pictureService.GetPictureByIdAsync(blogCategory.PictureId);
            if (picture != null)
                await _pictureService.SetSeoFilenameAsync(picture.Id, await _pictureService.GetPictureSeNameAsync(blogCategory.Name));
        }
        public virtual async Task<IActionResult> BlogCategoryDelete(int id)
        {

            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageWidgets))
                return AccessDeniedView();

            //try to get a blogCategory with the specified id
            var blogCategory = await _blogCategoryService.GetBlogCategoryByIdAsync(id);
            if (blogCategory == null)
                return RedirectToAction("List");

            await _blogCategoryService.DeleteBlogCategoryAsync(blogCategory);

            //activity log
            await _customerActivityService.InsertActivityAsync("DeleteBlogCategory",
                string.Format(await _localizationService.GetResourceAsync("Plugins.Widgets.BlogCategory.ActivityLog.DeleteBlogCategory"), blogCategory.Name), blogCategory);

            _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Plugins.Widgets.BlogCategory.Deleted"));


            return View("~/Plugins/Widgets.BlogCategory/Views/List.cshtml");
        }
        public async Task<IActionResult> Configure()
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageWidgets))
                return AccessDeniedView();
            return RedirectToAction("List");
        }

        #endregion

        #region widget methods

        public async Task<IActionResult> BlogCategoryMapUpdate(AddUpdateBlogCategoryModel model)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageWidgets))
                return AccessDeniedView();

            await _blogCategoryService.SaveCategoryMappingsAsync(model);
            return Json(new { result = "success" });
        }

        #endregion

    }
}
