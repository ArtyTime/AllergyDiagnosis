using System;
using System.ComponentModel.DataAnnotations;

namespace Allergy.Models
{
    public class BaseEntity
    {
        [Key]
        public int Id { get; set; }

        public virtual object Clone()
        {
            return MemberwiseClone();
        }
    }
}
