using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PotionAPI;

namespace InputTests
{
	[TestClass]
	public class CSVTests
	{
		const int	ExpectedMagicEffects	= 57,
					ExpectedIngredients		= 110;
		static readonly string[] requiredOutputFiles = new string[] {
			PotionAPI.Properties.Resources.MagicEffectFile,
			PotionAPI.Properties.Resources.IngredientFile
		};


		[TestMethod]
		[TestCategory("Files")]
		public void CheckForFiles()
		{
			foreach(string file in requiredOutputFiles)
				if(!System.IO.File.Exists(file))
					throw new AssertFailedException($"File \"{file}\" not found in output");
		}

		[TestMethod]
		[TestCategory("MagicEffect")]
		public void MagicEffectFileCSVGenerationTest() =>
			Assert.IsTrue(CSV.TryRead(PotionAPI.Properties.Resources.MagicEffectFile, true, out CSV effectCSV));

		[TestMethod]
		[TestCategory("Ingredient")]
		public void IngredientFileCSVGenerationTest() =>
			Assert.IsTrue(CSV.TryRead(PotionAPI.Properties.Resources.IngredientFile, true, out CSV ingredientCSV));

		[TestMethod]
		[TestCategory("MagicEffect")]
		public void MagicEffectFileReadTest()
		{
			Assert.IsTrue(CSV.TryRead(PotionAPI.Properties.Resources.MagicEffectFile, true, out CSV effectCSV));
			Assert.AreEqual(expected: effectCSV.GetEntry("Effect (ID)", 0),
				actual: "Cure Disease");
		}

		[TestMethod]
		[TestCategory("Ingredient")]
		public void IngredientFileReadTest()
		{
			Assert.IsTrue(CSV.TryRead(PotionAPI.Properties.Resources.IngredientFile, true, out CSV effectCSV));
			Assert.AreEqual(expected: effectCSV.GetEntry("Ingredient", 0),
				actual: "Abecean Longfin");
		}

		[TestMethod]
		[TestCategory("MagicEffect")]
		public void MagicEffectLoadTest()
		{
			Assert.IsNotNull(MagicEffect._allMagicEffects);
			Assert.AreEqual(ExpectedMagicEffects, MagicEffect._allMagicEffects.Count, $"Expected to find {ExpectedMagicEffects} Magic Effects, but found {MagicEffect._allMagicEffects.Count}");
		}

		[TestMethod]
		[TestCategory("Ingredient")]
		public void IngredientLoadTest()
		{
			Assert.IsNotNull(Ingredient._allIngredients);
			Assert.IsTrue(Ingredient._allIngredients.Count == ExpectedIngredients);
			foreach (Ingredient ingredient in Ingredient._allIngredients)
				Assert.AreEqual(ingredient.Effects.Length, 4);
		}
	}

	[TestClass]
	public class InputValidation
	{
		[TestMethod]
		[TestCategory("MagicEffect")]
		public void MagicEffectNoDuplicates()
		{
			for (int i = 1; i < MagicEffect._allMagicEffects.Count; i++)
				for (int j = 0; j < MagicEffect._allMagicEffects.Count && j < i; j++)
				{
					Assert.AreNotEqual(MagicEffect._allMagicEffects[i].Name, MagicEffect._allMagicEffects[j].Name);
				}

		}

		[TestMethod]
		[TestCategory("Ingredient")]
		public void IngredientNoDuplicates()
		{
			for (int i = 1; i < Ingredient._allIngredients.Count; i++)
				for (int j = 0; j < Ingredient._allIngredients.Count && j < i; j++)
				{
					Assert.AreNotEqual(Ingredient._allIngredients[i].Name, Ingredient._allIngredients[j].Name);
				}

		}

		/// <summary>
		/// Check that no effects are tagged as both beneficial and poisonous
		/// </summary>
		[TestMethod]
		public void IngredientEffectCategoryCheck()
		{
			foreach (MagicEffect eff in MagicEffect._allMagicEffects)
				if (eff.beneficial)
					Assert.IsFalse(eff.poisonous, $"Magic effect {eff.Name} is both beneficial and poisonous");
		}

		[TestMethod]
		public void AllIngredientEffectsValid()
		{
			foreach(Ingredient ingredient in Ingredient._allIngredients)
			{
				Assert.AreEqual(4, ingredient.Effects.Length, $"Ingredient \"{ingredient.Name}\" does not have expected number of effects");
				foreach(AlchemyEffect effect in ingredient.Effects)
				{
					var mgef = MagicEffect.GetMagicEffect(effect.name);
					Assert.IsNotNull(mgef,$"Failed to fetch Magic Effect \"{effect.name}\"");
					Assert.AreEqual(effect.name, mgef.Name, "Incorrect effect was pulled");
				}
			}
		}
	}
}
