using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PotionAPI
{
	/// <summary>
	/// The alchemical effect produced by an ingredient
	/// </summary>
	[DebuggerDisplay("{name,nq}")]
	public struct AlchemyEffect
	{
		public readonly string name, description;
		public readonly int magnitude, duration, value;
		public readonly MagicEffect magicEffect;

		/// <summary>
		/// Create an alchemical effect by defining its properties
		/// </summary>
		/// <param name="name">Effect name</param>
		/// <param name="mag">Base magnitude</param>
		/// <param name="dur">Base duration</param>
		/// <param name="val">Base value</param>
		internal AlchemyEffect(string name, string mag, string dur, string val)
		{
			magicEffect = MagicEffect.GetMagicEffect(name);

			this.name = name;
			this.description = magicEffect.Description;

			this.magnitude = Convert.ToInt32(mag);
			this.duration = Convert.ToInt32(dur);
			this.value = Convert.ToInt32(val);
		}

		/// <summary>
		/// Ingredient priority to determine which ingredients are used for potion effect determination
		/// </summary>
		public int Priority => value;
	}
}
