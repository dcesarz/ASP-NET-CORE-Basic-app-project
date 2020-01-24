using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using dotnet_project.Validation;

namespace dotnet_project.Models
{
    public class Item
    {
        public int Id { get; set; }

        [StringLength(30, MinimumLength = 3)]
        [Required]
        [Display(Name = "Item Name")]
        public string Name { get; set; }

        [DescriptionValidation (ErrorMessage = "Wrong length or illegal characters")]
        [Required]
        [Display(Name = "Desc")]
        public string Description { get; set; }

        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        //public Category ItemCategory { get; set; }

        [Display(Name = "Category")]
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
    }
}
