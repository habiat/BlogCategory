using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Widgets.BlogCategory.Models
{
    public class CategoryInputModel
    {
        public int ParentId { get; set; }
        public string TagName { get; set; }
        public string CategoryName { get; set; }
    }
}
