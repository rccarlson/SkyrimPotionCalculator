using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PotionAPI;

namespace InputTests
{
	[TestClass]
	public class CSVTests
	{
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
			Assert.IsTrue(MagicEffect._allMagicEffects.Count == 55);
		}

		[TestMethod]
		[TestCategory("Ingredient")]
		public void IngredientLoadTest()
		{
			Assert.IsNotNull(Ingredient._allIngredients);
			Assert.IsTrue(Ingredient._allIngredients.Count == 110);
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
				else
					Assert.IsTrue(eff.poisonous, $"Magic effect {eff.Name} is neither beneficial nor poisonous");
		}
	}
}
