using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using dotnet_project.Validation;

namespace dotnet_project.Models
{
    public class Category
    {
        public int Id { get; set; }

        [StringLength(30, MinimumLength = 3)]
        [Required]
        [Display(Name = "Category Name")]
        public string Name { get; set; }

        [DescriptionValidation(ErrorMessage = "Wrong length or illegal characters")]
        [Required]
        [Display(Name = "Desc")]
        public string Description { get; set; }

        public virtual ICollection<Item> Items { get; set; }
    }
}
