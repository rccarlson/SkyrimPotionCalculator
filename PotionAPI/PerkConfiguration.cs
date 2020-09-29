using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PotionAPI
{
    public struct PerkConfiguration
    {
        public readonly int AlchemistPerk;
        public readonly bool PhysicianPerk, BenefactorPerk, PoisonerPerk, SeekerOfShadows;
        public readonly int AlchemySkill, FortifyAlchemy;

        public PerkConfiguration(int AlchemySkill = 15, int AlchemistPerk=0, bool PhysicianPerk=false, bool BenefactorPerk = false, bool PoisonerPerk = false, bool SeekerOfShadows = false, int FortifyAlchemy = 0)
        {
            this.AlchemySkill = AlchemySkill;
            this.PhysicianPerk = PhysicianPerk;
            this.BenefactorPerk = BenefactorPerk;
            this.PoisonerPerk = PoisonerPerk;
            this.AlchemistPerk = AlchemistPerk;
            this.SeekerOfShadows = SeekerOfShadows;
            this.FortifyAlchemy = FortifyAlchemy;
        }

        public static readonly PerkConfiguration Default = new PerkConfiguration();
    }
}
