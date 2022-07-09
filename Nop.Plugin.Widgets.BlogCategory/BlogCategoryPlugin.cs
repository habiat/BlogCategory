using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Routing;
using Nop.Core;
using Nop.Core.Domain.Cms;
using Nop.Core.Domain.Logging;
using Nop.Plugin.Widgets.BlogCategory.Services;
using Nop.Services.Cms;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Nop.Services.Plugins;
using Nop.Web.Framework;
using Nop.Web.Framework.Infrastructure;
using Nop.Web.Framework.Menu;

namespace Nop.Plugin.Widgets.BlogCategory
{
    public class BlogCategoryPlugin : BasePlugin, IWidgetPlugin, IAdminMenuPlugin
    {

        #region Fields

        private readonly WidgetSettings _widgetSettings;
        private readonly ILocalizationService _localizationService;
        private readonly ISettingService _settingService;
        private readonly IWebHelper _webHelper;
        private readonly ICustomerActivityService _customerActivityService;
        private readonly IBlogCategoryService _blogCategoryService;

        #endregion

        #region Ctor
        public BlogCategoryPlugin(
            WidgetSettings widgetSettings,
            ILocalizationService localizationService,
            ISettingService settingService,
            IWebHelper webHelper,
            ICustomerActivityService customerActivityService,
            IBlogCategoryService blogCategoryService)
        {
            _widgetSettings = widgetSettings;
            _localizationService = localizationService;
            _settingService = settingService;
            _webHelper = webHelper;
            _customerActivityService = customerActivityService;
            _blogCategoryService = blogCategoryService;
        }
        #endregion

        #region Methods
        public Task ManageSiteMapAsync(SiteMapNode rootNode)
        {
            var menuItem = new SiteMapNode()
            {
                SystemName = "Widgets.BlogCategory",
                Title = "Blog Category",
                ControllerName = "BlogCategory",
                ActionName = "Configure",
                IconClass = "far fa-dot-circle",
                Visible = true,
                RouteValues = new RouteValueDictionary() { { "area", AreaNames.Admin } },
            };
            var pluginNode = rootNode.ChildNodes.FirstOrDefault(x => x.SystemName == "Content Management");
            if (pluginNode != null)
                pluginNode.ChildNodes.Add(menuItem);
            else
                rootNode.ChildNodes.Add(menuItem);

            return Task.CompletedTask;
        }

        /// <summary>
        /// Gets a configuration page URL
        /// </summary>
        public override string GetConfigurationPageUrl()
        {
            return $"{_webHelper.GetStoreLocation()}Admin/BlogCategory/Configure";
        }
        public Task<IList<string>> GetWidgetZonesAsync()
        {
            return Task.FromResult<IList<string>>(new List<string> { PublicWidgetZones.BlogListPageBeforePost, AdminWidgetZones.BlogPostDetailsBlock });
        }

        public string GetWidgetViewComponentName(string widgetZone)
        {
            return "WidgetsBlogCategory";
        }

        /// <summary>
        /// Install plugin
        /// </summary>
        /// <returns>A task that represents the asynchronous operation</returns>
        public override async Task InstallAsync()
        {
            await _localizationService.AddLocaleResourceAsync(new Dictionary<string, string>
            {
                ["Plugins.Widgets.BlogCategory.Configuration"] = "Configuration",
                ["Plugins.Widgets.BlogCategory.List"] = "Category List",
                ["Plugins.Widgets.BlogCategory.CreateOrUpdate.Title"] = "Create or Edit blog category",
                ["Plugins.Widgets.BlogCategory.ActivityLog.DeleteBlogCategory"] = "Deleted a Blog category ('{0}')",
                ["Plugins.Widgets.BlogCategory.ActivityLog.UpdateBlogCategory"] = "updated a Blog category ('{0}')",
                ["Plugins.Widgets.BlogCategory.ActivityLog.CreateBlogategory"] = "created a Blog category ('{0}')",
                ["Plugins.Widgets.BlogCategory.Deleted"] = "The Blog Category has been deleted successfully.",
                ["Plugins.Widgets.BlogCategory.Updated"] = "The Blog Category has been updated successfully.",
                ["Plugins.Widgets.BlogCategory.Created"] = "The Blog Category has been Created successfully.",
                ["Plugins.Widgets.BlogCategory.Fields.SelectedBlogCategory"] = "Blog Category",
                ["Plugins.Widgets.BlogCategory.Fields.Name"] = "Category Name",
                ["Plugins.Widgets.BlogCategory.Fields.Tag"] = "Tag",
                ["Plugins.Widgets.BlogCategory.Fields.DisplayOrder"] = "DisplayOrder",
                ["Plugins.Widgets.BlogCategory.Fields.ParentId"] = "ParentId",
                ["Plugins.Widgets.BlogCategory.Fields.PictureId"] = "Select Picture",
            });

            var activityLog = new Dictionary<string, string>()
            {
                {"DeleteBlogCategory","Delete Blog Category"},
                {"UpdateBlogCategory","UpdateBlog Category"},
                {"CreateBlogCategory","Create Blog Category"},
            };

            foreach (var systemKeyword in activityLog)
            {
                var activityLogType = (await _customerActivityService.GetAllActivityTypesAsync()).FirstOrDefault(type => type.SystemKeyword.Equals(systemKeyword.Key));
                if (activityLogType == null)
                {
                    var acType = new ActivityLogType()
                    {
                        Name = systemKeyword.Value,
                        Enabled = true,
                        SystemKeyword = systemKeyword.Key
                    };
                    await _blogCategoryService.InsertActivityLogTypeAsync(acType);
                }
            }
            await base.InstallAsync();
        }

        /// <summary>
        /// Uninstall plugin
        /// </summary>
        /// <returns>A task that represents the asynchronous operation</returns>
        public override async Task UninstallAsync()
        {
            _widgetSettings.ActiveWidgetSystemNames.Remove(BlogCategoryDefaults.SystemName);
            await _settingService.SaveSettingAsync(_widgetSettings);

            await _localizationService.DeleteLocaleResourcesAsync("Plugins.Widgets.BlogCategory");
            var activityLog = new Dictionary<string, string>()
            {
                {"DeleteBlogCategory","Delete Blog Category"},
                {"UpdateBlogCategory","UpdateBlog Category"},
                {"CreateBlogCategory","Create Blog Category"},
            };

            foreach (var systemKeyword in activityLog)
            {
                var activityLogType = (await _customerActivityService.GetAllActivityTypesAsync()).FirstOrDefault(type => type.SystemKeyword.Equals(systemKeyword.Key));
                if (activityLogType != null)
                    await _blogCategoryService.DeleteActivityLogTypeAsync(activityLogType);
            }

            await base.UninstallAsync();

        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets a value indicating whether to hide this plugin on the widget list page in the admin area
        /// </summary>
        public bool HideInWidgetList => false;

        #endregion

    }
}
