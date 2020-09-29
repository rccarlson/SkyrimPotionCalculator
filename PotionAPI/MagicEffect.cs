using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PotionAPI
{
	/// <summary>
	/// A magic effect 
	/// </summary>
	[DebuggerDisplay("{Name,nq}")]
	public class MagicEffect
	{
		internal static List<MagicEffect> _allMagicEffects = LoadFromFile(Properties.Resources.MagicEffectFile);

		public readonly string Name, Description;
		public readonly float baseCost, baseMag, baseDur;
		public readonly bool beneficial, poisonous, powerAffectsMag, powerAffectsDur;

		private MagicEffect(string name, string description, string baseCost, string baseMagnitude, string baseDuration, string beneficial, string poisonous, string powerAffectsMagnitude, string powerAffectsDuration)
		{
			//strings
			this.Name = name;
			this.Description = description;
			
			//floats
			this.baseCost = Convert.ToSingle(baseCost);
			this.baseMag = Convert.ToSingle(baseMagnitude);
			this.baseDur = Convert.ToSingle(baseDuration);

			//bools
			this.beneficial = beneficial.StartsWith("Y");
			this.poisonous = poisonous.StartsWith("Y");
			this.powerAffectsMag = powerAffectsMagnitude.StartsWith("Y");
			this.powerAffectsDur = powerAffectsDuration.StartsWith("Y");
		}

		private static List<MagicEffect> LoadFromFile(string filepath)
		{
			if(CSV.TryRead(filepath,hasHeaders:true,out CSV csv))
			{
				var magicEffects = new List<MagicEffect>();
				
				for(int i = 0; i < csv.Rows; i++)
				{
					//Create an object from each row of the CSV
					magicEffects.Add(new MagicEffect(
						name:					csv.GetEntry("Effect (ID)", i),
						description:			csv.GetEntry("Description", i),
						baseCost:				csv.GetEntry("Base_Cost", i),
						baseMagnitude:			csv.GetEntry("Base_Mag", i),
						baseDuration:			csv.GetEntry("Base_Dur", i),
						beneficial:				csv.GetEntry("Beneficial", i),
						poisonous:				csv.GetEntry("Poisonous", i),
						powerAffectsMagnitude:	csv.GetEntry("Power Affects Magnitude", i),
						powerAffectsDuration:	csv.GetEntry("Power Affects Duration", i)
						));
				}

				return magicEffects;
			}
			else
			{
				//Failed to load file
				return new List<MagicEffect>();
			}
		}

		/// <summary>
		/// Fetch a <see cref="MagicEffect"/> based on name
		/// </summary>
		/// <param name="name">Name of the desired MagicEffect</param>
		/// <returns><see cref="MagicEffect"/> with a matching name</returns>
		internal static MagicEffect GetMagicEffect(string name)
			=> _allMagicEffects.Find(e => e.Name.CompareTo(name) == 0);
		
	}
}
