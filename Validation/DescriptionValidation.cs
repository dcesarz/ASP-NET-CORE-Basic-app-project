using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace dotnet_project.Validation
{
    public class DescriptionValidation : ValidationAttribute {
		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
		{
			if (value != null)
			{
				string str = value.ToString();
				Regex rgx = new Regex(@"^[a-zA-Z0-9,.!? ]*$");
				int le = str.Length;
				if(le >= 3 && le <= 90 && rgx.IsMatch(str))
				{
					return ValidationResult.Success;
				}
			}
			return new ValidationResult("Zły opis!");
		}
	}
}
