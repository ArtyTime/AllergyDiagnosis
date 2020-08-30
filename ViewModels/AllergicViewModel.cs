using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Allergy.ViewModels
{
    public class AllergicViewModel
    {
        public int AllergicId { get; set; }

        [Required(ErrorMessage = "The person name is required.")]
        [StringLength(255, ErrorMessage = "Entered value cannot exceed 255 characters")]
        public string Name { get; set; }

        public string Diagnosis { get; set; }
    }
}
