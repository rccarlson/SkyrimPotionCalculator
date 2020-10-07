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
		public readonly float baseCost;
		public readonly bool hostile, detrimental, noMagnitude, noDuration, beneficial, poisonous, powerAffectsMag, powerAffectsDur;
		public readonly List<string> keywords;

		private MagicEffect(string name, string description, string baseCost, string hostile, string detrimental, string noMagnitude, string noDuration, string powerAffects, string keywords)
		{
			//strings
			this.Name = name;
			this.Description = description;

			//floats
			this.baseCost = Convert.ToSingle(baseCost);

			//bools
			this.hostile = hostile.StartsWith("Y");
			this.detrimental = detrimental.StartsWith("Y");
			this.noMagnitude = noMagnitude.StartsWith("Y");
			this.noDuration = noDuration.StartsWith("Y");

			if (powerAffects == "Magnitude")
			{
				this.powerAffectsMag = true;
				this.powerAffectsDur = false;
			}
			else if (powerAffects == "Duration")
			{
				this.powerAffectsMag = false;
				this.powerAffectsDur = true;
			}
			else
				throw new FormatException($"Bad \"Power Affects\" input: \"{powerAffects}\"");

			this.keywords = keywords.Split(';').ToList();

			beneficial = this.keywords.Contains("MagicAlchBeneficial");
			
		}

		private static List<MagicEffect> LoadFromFile(string filepath)
		{
			if(CSV.TryRead(filepath,hasHeaders:true,out CSV csv))
			{
				var magicEffects = new List<MagicEffect>();
				
				for(int i = 0; i < csv.Rows; i++)
				{
					magicEffects.Add(new MagicEffect(
						name:                   csv.GetEntry("Effect (ID)", i),
						description:			csv.GetEntry("Description", i),
						baseCost:				csv.GetEntry("Base_Cost", i),
						hostile:				csv.GetEntry("Hostile", i),
						detrimental:			csv.GetEntry("Detrimental", i),
						noMagnitude:			csv.GetEntry("No Magnitude", i),
						noDuration:				csv.GetEntry("No Duration", i),
						powerAffects:			csv.GetEntry("Power Affects", i),
						keywords:				csv.GetEntry("Keywords", i)
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
