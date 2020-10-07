using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PotionAPI
{
	public class Potion
	{
		public readonly Ingredient[] ingredients;
		public readonly List<IngredientEffect> ingredientEffects;
		public readonly List<PotionEffect> effects;
		public readonly PerkConfiguration perks;
		internal readonly int highestValueEffectIndex;
		internal readonly IngredientEffect highestValueEffect;

		public Potion(Ingredient[] ingredients, PerkConfiguration perks)
		{
			this.ingredients = ingredients;
			this.perks = perks;
			ingredientEffects = ProcessIngredients(ingredients);
			highestValueEffect = ingredientEffects.OrderByDescending(effect => effect.value).FirstOrDefault();
		}

		/// <summary>
		/// Find the applicable ingredient effects for use in potion
		/// </summary>
		/// <param name="ingredients">Input ingredients</param>
		/// <returns><see cref="IngredientEffect"/> objects to be converted for use in potion</returns>
		internal List<IngredientEffect> ProcessIngredients(Ingredient[] ingredients)
		{
			var allIngredientEffects = new Dictionary<string, List<IngredientEffect>>();
			foreach (Ingredient ingredient in ingredients)
				foreach (IngredientEffect effect in ingredient.Effects)
				{
					if (allIngredientEffects.ContainsKey(effect.name))
						allIngredientEffects[effect.name].Add(effect);
					else
						allIngredientEffects.Add(
							key: effect.name,
							value: new List<IngredientEffect>() { effect });
				}
			var validIngredientEffectLists = allIngredientEffects.Values.Where(effectList => effectList.Count > 1);
			var maxValueEffects = validIngredientEffectLists.Select(list => list.OrderByDescending(effect => effect.value).First());

			return maxValueEffects.ToList();
		}

		public bool IsValid => effects.Count > 0;
		public bool IsPotion => IsValid && !highestValueEffect.magicEffect.hostile;
		public bool IsPoison => IsValid && highestValueEffect.magicEffect.hostile;


		public class PotionEffect
		{
			private readonly MagicEffect _magicEffect;
			private readonly IngredientEffect _alchemyEffect;

			internal PotionEffect(IngredientEffect ingredientEffect)
			{
				_alchemyEffect = ingredientEffect;
				_magicEffect = MagicEffect.GetMagicEffect(ingredientEffect.name);

				//this.name = ingredientEffect.name;
				//this.description = magicEffect.Description;

				//this.magnitude = ingredientEffect.magnitude;
				//this.duration = ingredientEffect.duration;
				//this.value = ingredientEffect.value;

				//this.Beneficial = magicEffect.beneficial;
				//this.Poisonous = magicEffect.poisonous;
				//this.PowerAffectsMagnitude = magicEffect.powerAffectsMag;
				//this.PowerAffectsDuration = magicEffect.powerAffectsDur;
			}
			public string Name => _alchemyEffect.name;
			public string Description => _magicEffect.Name;
			//public int Magnitude =>
		}
	}
}
