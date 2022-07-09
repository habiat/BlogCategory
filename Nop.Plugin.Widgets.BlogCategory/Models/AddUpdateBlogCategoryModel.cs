using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Web.Framework.Models;

namespace Nop.Plugin.Widgets.BlogCategory.Models
{
    public  record AddUpdateBlogCategoryModel
    {
        #region Ctor
        public AddUpdateBlogCategoryModel()
        {
            selectedCategoryIds = new List<int>();
        }
        #endregion
        public IList<int> selectedCategoryIds { get; set; }
        public int BlogId { get; set; }
        
    }
}
