using Allergy.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Allergy.Models
{
	[Flags]
	public enum Allergen
	{
		Eggs = 1,
		Peanuts = 2,
		Shellfish = 4,
		Strawberries = 8,
		Tomatoes = 16,
		Chocolate = 32,
		Pollen = 64,
		Cats = 128
	}

	public class Allergies : BaseEntity
	{
		// Constructors 
		public Allergies()
		{
		}

		public Allergies(int id)
		{
			Id = id;
		}

		public Allergies(int id, string personName)
		{
			Id = id;
			Name = personName;
			Score = 0;
		}

		public Allergies(int id, string personName, string allergies)
		{
			Id = id;
			Name = personName;

			var allergens = allergies.ToAllergens();

			Score = (int)allergens;
		}

		public Allergies(int id, string personName, int allergiesScore)
		{
			Id = id;
			Name = personName;
			Score = allergiesScore;
		}

		public Allergies(string personName)
		{
			Name = personName;
			Score = 0;
		}

		public Allergies(string personName, string allergies)
		{
			Name = personName;

			var allergens = allergies.ToAllergens();

			Score = (int)allergens;
		}

		public Allergies(string personName, int allergiesScore)
		{
			Name = personName;
			Score = allergiesScore;
		}

		// Properties

		public string Name { get; set; }

		public int Score { get => (int)_personAllergy; set =>  _personAllergy = (Allergen)value; }

		private Allergen _personAllergy;

		// Methods
		public bool IsAllergicTo(string allergenString)
		{
			var allergen = allergenString.ToAllergens();

			return _personAllergy.HasFlag(allergen);
		}

		public bool IsAllergicTo(Allergen allergen)
		{
			var personsAllergy = (Allergen)Score;

			return personsAllergy.HasFlag(allergen);
		}

		public void AddAllergy(string allergenString)
		{
			var allergen = allergenString.ToAllergens();

			if (_personAllergy.HasFlag(allergen))
				return;

			Score += (int)allergen;
		}

		public void AddAllergy(Allergen allergen)
		{
			if (_personAllergy.HasFlag(allergen))
				return;

			Score += (int)allergen;
		}

		public void DeleteAllergy(string allergenString)
		{
			var allergen = allergenString.ToAllergens();

			if (!_personAllergy.HasFlag(allergen))
				return;

			Score -= (int)allergen;
		}

		public void DeleteAllergy(Allergen allergen)
		{
			if (!_personAllergy.HasFlag(allergen))
				return;

			Score -= (int)allergen;
		}

		public override string ToString()
		{
			var allergensString = _personAllergy.ToString();
			var allergensArray = allergensString.Split(',');

			var resultString = Score == 0
				? $"{Name} has no allergies!"
				: $"{Name} is allergic to {allergensString}.";

			if (allergensArray.Length == 2)
				return resultString.Replace(",", " and");
			else if (allergensArray.Length > 2)
			{
				var lastAllergen = allergensArray.Last();

				return resultString.Replace(lastAllergen, $" and{lastAllergen}");
			}
			else
				return resultString;
		}

		public IEnumerable<Allergen> GetAllergens() => 
			Enum.GetValues(typeof(Allergen))
				.Cast<Allergen>().Where(a => _personAllergy.HasFlag(a));
		
	}
}
