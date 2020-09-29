using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PotionAPI
{
	/// <summary>
	/// An ingredient, collectable in the world and usable in potions
	/// </summary>
	[DebuggerDisplay("{Name,nq}")]
	public class Ingredient
	{
		internal static readonly List<Ingredient> _allIngredients = LoadFromFile(Properties.Resources.IngredientFile);

		public readonly string Name, Obtained;
		public readonly AlchemyEffect[] Effects;
		public readonly float Weight;
		public readonly int Value;

		private Ingredient(string name,
			AlchemyEffect[] effects,
			string weight, string value, string obtained)
		{
			//strings
			this.Name = name;
			this.Obtained = obtained;
			//floats
			this.Weight = Convert.ToSingle(weight);
			//ints
			this.Value = Convert.ToInt32(value);
			//effects
			this.Effects = effects;
		}


		private static List<Ingredient> LoadFromFile(string filepath)
		{
			string NumbersInString(string str)
			{
				StringBuilder builder = new StringBuilder();
				foreach (char c in str)
					if ("0123456789.".Contains(c))
						builder.Append(c);
				return builder.ToString();
			}

			if (CSV.TryRead(filepath, hasHeaders: true, out CSV csv))
			{
				List<Ingredient> ingredients = new List<Ingredient>();

				var headerNumbers = csv.Headers.ToList().Select(header => NumbersInString(header)).Where(str => str.Length > 0).ToList();
				int maxEffect = headerNumbers.Select(n => Convert.ToInt32(n)).Max();

				for (int row = 0; row < csv.Rows; row++)
				{
					//Create an object from each row of the CSV
					AlchemyEffect[] effects = new AlchemyEffect[maxEffect];
					for(int effectNum = 1; effectNum <= maxEffect; effectNum++)
					{
						effects[effectNum - 1] = new AlchemyEffect(
							name:	csv.GetEntry("Effect " + effectNum.ToString() + ": Name", row),
							mag:	csv.GetEntry("Effect " + effectNum.ToString() + ": Magnitude", row),
							dur:	csv.GetEntry("Effect " + effectNum.ToString() + ": Duration", row),
							val:	csv.GetEntry("Effect " + effectNum.ToString() + ": Value", row)
							);
					}

					ingredients.Add(new Ingredient(
						name:		csv.GetEntry("Ingredient", row),
						weight:		csv.GetEntry("Weight", row),
						value:		csv.GetEntry("Value", row),
						obtained:	csv.GetEntry("Obtained", row),
						effects:	effects
						));
				}

				return ingredients;
			}
			else
			{
				return new List<Ingredient>();
			}
		}

		/// <summary>
		/// Fetches an ingredient by name
		/// </summary>
		/// <param name="name">Name of desired <see cref="Ingredient"/></param>
		/// <returns>Ingredient matching the <paramref name="name"/> if successful. Null if unsuccessful</returns>
		public static Ingredient GetIngredient(string name)
			=> _allIngredients.Find(i => i.Name == name);
		
	}
}
