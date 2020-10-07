using System;
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
			int dummy = 0;
		}

		[TestMethod]
		public void PrioritizedSingleEffectPotionTest()
		{
			Potion damageHealthPotion = new Potion(new Ingredient[] {
				Ingredient.GetIngredient("Jarrin Root"),
				Ingredient.GetIngredient("Falmer Ear")
			}, perks: new PerkConfiguration(AlchemySkill: 100));
			//Value: 86
			//Magnitude: 18
			//Duration: 60
			int dummy = 0;
		}
	}
}
