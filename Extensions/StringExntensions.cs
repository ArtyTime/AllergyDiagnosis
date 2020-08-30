using System;
using System.Linq;
using Allergy.Models;

namespace Allergy.Extensions
{
    public static class StringExntensions
    {
		public static Allergen ToAllergens(this string allergensString)
		{
			var allergensArray = allergensString.Split(' ');

			var allergens = allergensArray
				.Select(x =>
				{
					Enum.TryParse<Allergen>(x, out var outenum);
					return outenum;
				}).Aggregate((prev, next) => prev | next);

			return allergens;
		}
	}
}
