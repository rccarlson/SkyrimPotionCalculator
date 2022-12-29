using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PotionAPI
{
	/// <summary>
	/// Class to handle all potion calculations and properites
	/// </summary>
	[DebuggerDisplay("{Name,nq}")]
	public class Potion
	{
		const float IngredientMult = 4.0f,
			SkillFactor = 1.5f;

		public readonly Ingredient[] ingredients;
		public readonly List<IngredientEffect> ingredientEffects;
		public readonly List<PotionEffect> effects;
		public readonly PerkConfiguration perks;
		internal readonly IngredientEffect highestValueEffect;

		public Potion(IEnumerable<Ingredient> ingredients, PerkConfiguration perks)
		{
			this.ingredients = ingredients.Distinct().ToArray();
			this.perks = perks;
			ingredientEffects = ProcessIngredients(this.ingredients);
			highestValueEffect = ingredientEffects.OrderByDescending(effect => effect.value).FirstOrDefault();

			foreach(IngredientEffect effect in ingredientEffects)
            {
				GetPotionEffect(effect, perks);
            }
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

		public bool IsValid => ingredientEffects.Count > 0;
		public bool IsPotion => IsValid && !highestValueEffect.magicEffect.hostile;
		public bool IsPoison => IsValid && highestValueEffect.magicEffect.hostile;

		public string Name
        {
            get
            {
				if (!IsValid) return "Invalid Potion";
				
				if (IsPotion) return $"Potion of {highestValueEffect.name}";
				else return $"Poison of {highestValueEffect.name}";
			}
        }
		public string Description
        {
            get
            {
				throw new NotImplementedException(nameof(Description));
            }
        }

		internal float GetPowerFactor(IngredientEffect ingredientEffect, PerkConfiguration perks)
        {
			float physicianPerkMultiplier =
				perks.PhysicianPerk
				&& (ingredientEffect.name == "Restore Health"
				||  ingredientEffect.name == "Restore Magicka"
				||  ingredientEffect.name == "Restore Stamina")
				? 1.25f : 1.0f;

			float benefactorPerkMultiplier =
				perks.BenefactorPerk
				&& IsPotion
				&& ingredientEffect.magicEffect.beneficial
				? 1.25f : 1.0f;

			float poisonerPerkMultiplier =
				perks.PoisonerPerk
				&& IsPoison
				&& ingredientEffect.magicEffect.HasKeyword("MagicAlchHarmful")
				? 1.25f : 1.0f;

			Debug.Assert(!(benefactorPerkMultiplier > 1.0f && poisonerPerkMultiplier > 1.0f),
				$"Both benefactor and poisoner perks were applied to ingredient effect '{ingredientEffect.name}'");

			float powerFactor = IngredientMult
				* (1.0f + (SkillFactor - 1.0f) * perks.AlchemySkill / 100.0f)
				* (1.0f + perks.FortifyAlchemy / 100.0f)
				* (1.0f + perks.AlchemistPerk / 100.0f)
				* physicianPerkMultiplier * benefactorPerkMultiplier * poisonerPerkMultiplier;

			return powerFactor;
		}

		internal PotionEffect GetPotionEffect(IngredientEffect ingredientEffect, PerkConfiguration perks)
        {
			var powerFactor = GetPowerFactor(ingredientEffect, perks);

			float magnitude = ingredientEffect.magicEffect.noMagnitude ? 0 : ingredientEffect.magnitude;
			var magnitudeFactor = ingredientEffect.magicEffect.powerAffectsMag ? powerFactor : 1.0f;
			magnitude *= magnitudeFactor;
			magnitude = (float)Math.Round(magnitude);

			float duration = ingredientEffect.magicEffect.noDuration ? 0 : ingredientEffect.duration;
			var durationFactor = ingredientEffect.magicEffect.powerAffectsDur ? powerFactor : 1.0f;
			duration *= durationFactor;

			//magnitudeFactor = 1; //Used in the wiki calculations
			//if (magnitude < 0) magnitudeFactor = magnitude;
			//durationFactor = 1; //Used in the wiki calculations
			//if (duration < 0) durationFactor = duration / 10.0f;

			var magnitudeFactor2 = 1.0f;
			if (magnitude > 0) magnitudeFactor2 = magnitude;
			var durationFactor2 = 1.0f;
			//if (duration > 0) durationFactor2 = duration / 10.0f;
			var value2 = ingredientEffect.value * Math.Pow(magnitudeFactor2 * durationFactor2, 1.1f);

			var magnitudeCost = Math.Pow(magnitudeFactor, 1.1);
			var durationCost = Math.Pow(durationFactor, 1.1);
			var value = ingredientEffect.value * magnitudeCost * durationCost;

			return new PotionEffect(ingredientEffect: ingredientEffect,
				magnitude: ingredientEffect.magicEffect.noMagnitude ? 0 : (int)Math.Floor(magnitude),
				duration: ingredientEffect.magicEffect.noDuration ? 0 : (int)Math.Floor(duration),
				value: 0.0f);
        }

		[DebuggerDisplay("{Name,nq}")]
		public struct PotionEffect
		{
			private readonly MagicEffect _magicEffect;
			private readonly IngredientEffect _alchemyEffect;
			public readonly int Magnitude, Duration;
			public readonly float Value;

			internal PotionEffect(IngredientEffect ingredientEffect, int magnitude = 0, int duration = 0, float value = 0)
			{
				_alchemyEffect = ingredientEffect;
				_magicEffect = MagicEffect.GetMagicEffect(ingredientEffect.name);

				Magnitude = magnitude;
				Duration = duration;
				Value = value;
			}
			public string Name => _alchemyEffect.name;
			public string Description => _magicEffect.Name;
			//public int Magnitude =>
		}
	}
}
