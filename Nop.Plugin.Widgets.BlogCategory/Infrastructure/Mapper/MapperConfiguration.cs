using AutoMapper;
using Nop.Core.Infrastructure.Mapper;
using Nop.Plugin.Widgets.BlogCategory.Models;

namespace Nop.Plugin.Widgets.BlogCategory.Infrastructure.Mapper
{
   public class MapperConfiguration : Profile, IOrderedMapperProfile
    {
        #region Ctor
        public MapperConfiguration()
        {
            CreateMap<Domain.BlogCategory, BlogCategoryModel>()
                .ForMember(model => model.Breadcrumb, options => options.Ignore())
                .ForMember(model => model.BlogCategoryBreadcrumb, options => options.Ignore());
                CreateMap<BlogCategoryModel, Domain.BlogCategory>();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Order of this mapper implementation
        /// </summary>
        public int Order => 1;

        #endregion
    }
}
