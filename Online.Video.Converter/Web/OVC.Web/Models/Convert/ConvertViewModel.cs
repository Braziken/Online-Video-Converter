using System.Collections.Generic;
using System.Web.Mvc;

namespace OVC.Web.Models.Convert
{
    public class ConvertViewModel
    {
        public IEnumerable<SelectListItem> AvailableFormatTypes { get; set; }

        public string SelectedFormatType { get; set; }
    }
}
