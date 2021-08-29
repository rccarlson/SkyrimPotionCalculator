using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PotionAPI;

namespace PotionTests
{
	[TestClass]
	public class OutputTests
	{
		//TODO: Paralysis and Frenzy potions may break system

		[TestMethod]
		public void ResistFrostPotionTest()
		{
			Potion resistFrost = new Potion(new Ingredient[] {
				Ingredient.GetIngredient("Purple Mountain Flower"),
				Ingredient.GetIngredient("Thistle Branch")
			},
			perks: new PerkConfiguration(AlchemySkill:100, FortifyAlchemy: 0));
			//Value: 86
			//Magnitude: 18
			//Duration: 60
		}

		[TestMethod]
		public void CustomPotionTest()
		{
			Potion potion = new Potion(new Ingredient[] {
				Ingredient.GetIngredient("Purple Mountain Flower"),
				Ingredient.GetIngredient("Thistle Branch")
			}, perks: new PerkConfiguration(AlchemySkill: 100, FortifyAlchemy: 1194248));
			//Value: 485
			//Magnitude: 100
			//Duration: 34
		}

		[TestMethod]
		public void PrioritizedSingleEffectPotionTest()
		{
			Potion damageHealthPotion = new Potion(new Ingredient[] {
				Ingredient.GetIngredient("Jarrin Root"),
				Ingredient.GetIngredient("Falmer Ear")
			}, perks: new PerkConfiguration(AlchemySkill: 100));
			//Value: 581
			//Magnitude: 1200
			//Duration: 0
		}

		[TestMethod]
		public void RunAllFileTests()
        {
			var csv = CSV.Read("VerifiedPotionResults.csv", true);
			for(int i = 0; i < csv.Rows; i++)
            {
				PerkConfiguration perks = new PerkConfiguration(
					AlchemySkill: Convert.ToInt32(csv.GetEntry("Alchemy Skill", i)),
					AlchemistPerk: Convert.ToInt32(csv.GetEntry("Alchemist Perk", i)),
					PhysicianPerk: csv.GetEntry("Physician Perk", i) == "Y" ? true : false,
					BenefactorPerk: csv.GetEntry("Benefactor Perk", i) == "Y" ? true : false,
					PoisonerPerk: csv.GetEntry("Poisoner Perk", i) == "Y" ? true : false,
					FortifyAlchemy: Convert.ToInt32(csv.GetEntry("Fortify Alchemy", i))
					);

				var ingredients = new List<Ingredient>();
				for(int j = 0; j < 3; j++)
                {
					var ingredient = Ingredient.GetIngredient(csv.GetEntry($"Ingredient {j+1}", i));
					if (ingredient is object)
						ingredients.Add(ingredient);
				}

				var potion = new Potion(ingredients, perks);

				string expectedName = csv.GetEntry("Potion Name", i);
				var expectedValue = Convert.ToInt32(csv.GetEntry("Potion Value", i));
				var expectedMagnitude = Convert.ToDouble(csv.GetEntry("Potion Magnitude", i));
				var expectedDuration = Convert.ToInt32(csv.GetEntry("Potion Duration", i));

				Assert.AreEqual(actual: potion.Name, expected: expectedName);
				//AssertWithinPercentage(actual: potion.Value)74077
            }
        }
		private double GetDifference(double a, double b) => Math.Abs(a - b) / ((a + b) / 2);
		private void AssertWithinPercentage(double actual, double expected, double margin)
        {
			double diff = GetDifference(actual, expected);
			if (diff > margin)
				throw new AssertFailedException($"Expected: {expected}\n" +
					$"Actual: {actual}\n" +
					$"Difference ({diff}) was greater than maximum ({margin})");
        }
	}
}
