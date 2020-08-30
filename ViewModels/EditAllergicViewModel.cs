using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace Allergy.ViewModels
{
    public class EditAllergicViewModel : AllergicViewModel
    {
        public List<SelectListItem> AvailableAllergens { get; set; }

        public IEnumerable<string> SelectedAllergens { get; set; }
    }
}
