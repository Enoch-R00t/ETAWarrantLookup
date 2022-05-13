using Microsoft.AspNetCore.Mvc.Rendering;

using System.Collections.Generic;

namespace ETAWarrantLookup.ViewModels
{
    public class WarrantSearchViewModel
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Race { get; set; }
        public string Sex { get; set; }
        public string Court { get; set; }

        public IEnumerable<SelectListItem> CourtList { get; set; }
        public IEnumerable<SelectListItem> SexList { get; set; }
        public IEnumerable<SelectListItem> RaceList { get; set; }
    }
}
