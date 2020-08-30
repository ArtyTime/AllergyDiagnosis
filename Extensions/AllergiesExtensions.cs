using Allergy.Models;
using Allergy.ViewModels;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;

namespace Allergy.Extensions
{
    public static class AllergiesExtensions
    {
        public static AllergicsListViewModel ToAllergicsListViewModel(this List<Allergies> allergics) => 
            new AllergicsListViewModel 
            {
                Allergics = allergics.Select(a => a.ToAllergicViewModel()).ToList()
            };
        

        public static AllergicViewModel ToAllergicViewModel(this Allergies allergic) => 
            new AllergicViewModel
            {
                AllergicId = allergic.Id,
                Name = allergic.Name,
                Diagnosis = allergic.ToString()
            };

        public static EditAllergicViewModel ToEditAllergicViewModel(this Allergies allergic) =>
            new EditAllergicViewModel
            {
                AllergicId = allergic.Id,
                Name = allergic.Name,
                Diagnosis = allergic.ToString(),
                SelectedAllergens = allergic.GetPersonAllergensIds().Select(a => a.ToString()),
                AvailableAllergens = GetAllAllergensAsListItems()
            };

        public static List<SelectListItem> GetAllAllergensAsListItems()
        {
            return Enum.GetValues(typeof(Allergen)).Cast<Allergen>()
                .Select(a => new SelectListItem
                {
                    Text = a.ToString(),
                    Value = ((int)a).ToString()
                }).ToList();
        }
        
        public static IEnumerable<int> GetPersonAllergensIds(this Allergies allergic)
        {
            var personAllergensList = allergic.GetAllergens();

            var resultAllergensIds = personAllergensList.Select(a =>
            {
                return ((int)a);
            });

            return resultAllergensIds;
        }
    }
}
