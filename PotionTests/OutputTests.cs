using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PotionAPI;

namespace PotionTests
{
    [TestClass]
    public class OutputTests
    {
        [TestMethod]
        public void ResistFrostPotionTest()
        {
            Potion resistFrost = new Potion(new Ingredient[] {
                Ingredient.GetIngredient("Purple Mountain Flower"),
                Ingredient.GetIngredient("Thistle Branch")
            });
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
            });
            //Value: 86
            //Magnitude: 18
            //Duration: 60
            int dummy = 0;
        }
    }
}
