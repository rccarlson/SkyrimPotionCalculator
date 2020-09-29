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
		public readonly List<AlchemyEffect> ingredientEffects;
		public readonly List<PotionEffect> effects;
		public readonly PerkConfiguration perks;

		public Potion(Ingredient[] ingredients, PerkConfiguration perks)
		{
			this.ingredients = ingredients;
			this.perks = perks;
			ingredientEffects = ProcessIngredients(ingredients);
		}

		/// <summary>
		/// Find the applicable ingredient effects for use in potion
		/// </summary>
		/// <param name="ingredients">Input ingredients</param>
		/// <returns><see cref="AlchemyEffect"/> objects to be converted for use in potion</returns>
		internal List<AlchemyEffect> ProcessIngredients(Ingredient[] ingredients)
		{
			// Create list of all alchemical effects from ingredients
			List<AlchemyEffect> alchEffects = new List<AlchemyEffect>();
			foreach (Ingredient ingredient in ingredients)
				alchEffects.AddRange(ingredient.Effects);

			// Create list of effects that occur at least twice, ordered by priority
			var applicableEffects = from effect in alchEffects where alchEffects.Count(e => e.name == effect.name) > 1 orderby effect.Priority descending select effect;

			// Take the first of each effect type from the list
			List<AlchemyEffect> potionEffects = new List<AlchemyEffect>();
			foreach (AlchemyEffect effect in applicableEffects)
				if (!potionEffects.Any(e => e.name == effect.name))
					potionEffects.Add(effect);

			return potionEffects;
		}

		public class PotionEffect
		{
			private readonly MagicEffect _magicEffect;
			private readonly AlchemyEffect _alchemyEffect;

			internal PotionEffect(AlchemyEffect ingredientEffect)
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
