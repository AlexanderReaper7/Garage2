using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Garage2.Models.Entities;
using Garage2.Data;
using Microsoft.EntityFrameworkCore;

namespace Garage2.Validators
{

	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]

	public class FirstNameNotEqualToLastNameAttribute : ValidationAttribute
	{
		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
		{
			var member = (Member)validationContext.ObjectInstance;

			if (member.FirstName == member.LastName)
			{
				return new ValidationResult(ErrorMessage ?? "First name and last name cannot be the same.");
			}

			return ValidationResult.Success;
		}
	}

}
