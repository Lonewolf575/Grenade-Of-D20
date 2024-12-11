using RimWorld;
using UnityEngine;
using Verse;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ARK_GunOfD20
{
    public class Projectile_GunOfD20 : Bullet
    {
        private Map hitMap;
        private IntVec3 hitPos;

        public ModExtention_D20Gun Props => base.def.GetModExtension<ModExtention_D20Gun>();

        protected override void Impact(Thing hitThing, bool blockedByShield = false)
        {
            hitMap = base.Map;
            hitPos = base.DestinationCell;

            base.Impact(hitThing, blockedByShield);
            //int rand = Math.Abs(Rand.Int % 4);
            int rand = 0;
            switch (rand)
            {
                case (0):
                    {
                        ARK_GrenadeOfD20.D20_Results.effectLightningStrike(hitMap, hitPos);
                        //effect00LightningStrike();
                        break;
                    }
                case (1):
                    {
                        effect01Strip(hitThing);
                        break;
                    }
                case 2:
                    {
                        effect02SummonBoomalopes();
                        break;
                    }
                case 3:
                    {
                        effect03RemoveLimbs(hitThing);
                        break;
                    }
                case 4:
                    {
                        effect04MadThrumbo(hitThing);
                        break;
                    }
                case 5:
                    {
                        effect04MadThrumbo(hitThing);
                        break;
                    }
                default:
                    {
                        string message = "Random Number not valid yet. Number: " + rand.ToString();
                        Messages.Message(message, MessageTypeDefOf.NeutralEvent);
                        break;
                    }
            } 
        }

        

        private void effect00LightningStrike()
        {
            hitMap.weatherManager.eventHandler.AddEvent(new WeatherEvent_LightningStrike(hitMap, hitPos));
        }

        private void effect01Strip(Thing hitThing)
        {
            if (Props != null && hitThing != null && hitThing is Pawn hitPawn)
            {
                hitPawn.Strip(false);
            }
        }

        private void effect02SummonBoomalopes()
        {
            PawnKindDef boomalope = PawnKindDef.Named("Boomalope");
            for (int i = 0; i < 3; i++)
            {
                IntVec3 loc = CellFinder.RandomClosewalkCellNear(hitPos, hitMap, 3);
                ((Pawn)GenSpawn.Spawn(PawnGenerator.GeneratePawn(boomalope), loc, hitMap)).needs.food.CurLevelPercentage = 1f;
            }
        }

        private void effect03RemoveLimbs(Thing hitThing)
        {
            if (Props != null && hitThing != null && hitThing is Pawn hitPawn)
            {
                if (hitPawn.Dead)
                {
                    return;
                }
                List<BodyPartRecord> allParts = hitPawn.def.race.body.AllParts;
                
                HediffSet hSet = hitPawn.health.hediffSet;
                IEnumerable<BodyPartRecord> allHitPawnParts = hSet.GetNotMissingParts();
                foreach (BodyPartRecord part in allHitPawnParts)
                {
                    if (part.Label.Contains("arm") || part.Label.Contains("leg"))
                    {
                        DamageDef vaporize = DamageDefOf.Vaporize;
                        DamageInfo value = new DamageInfo(vaporize, 1000f, 999f, -1f, null, part);
                        Hediff_Injury hediff_Injury = (Hediff_Injury)HediffMaker.MakeHediff(HealthUtility.GetHediffDefFromDamage(vaporize, hitPawn, part), hitPawn, part);
                        hediff_Injury.Severity = 1000f;
                        hitPawn.health.AddHediff(hediff_Injury, part, value);

                        IntVec3 loc = CellFinder.RandomClosewalkCellNear(hitPos, hitMap, 2);

                        string bodyType;
                        if (part.Label.Contains("leg"))
                        {
                            bodyType = "BionicLeg";
                        }
                        else
                        {
                            bodyType = "BionicArm";
                        }

                        ThingDef named = DefDatabase<ThingDef>.GetNamed(bodyType);
                        Thing bodyPartBox = ThingMaker.MakeThing(named);
                        GenPlace.TryPlaceThing(bodyPartBox, loc, hitMap, ThingPlaceMode.Near);
                    }
                }
                /*
                foreach (BodyPartRecord part in allParts)
                {
                    if (part.customLabel.Contains("arm") || part.customLabel.Contains("leg"))
                    {

                    }

                    Messages.Message(part.customLabel, MessageTypeDefOf.NeutralEvent);
                }
                */
            }
        }

        private void effect04MadThrumbo(Thing hitThing)
        {
            PawnKindDef thrumbo = PawnKindDef.Named("Thrumbo");

            IntVec3 loc = CellFinder.RandomClosewalkCellNear(hitPos, hitMap, 3);
            Pawn spawnedThrumbo = (Pawn)GenSpawn.Spawn(PawnGenerator.GeneratePawn(thrumbo), loc, hitMap);
            spawnedThrumbo.needs.food.CurLevelPercentage = .2f;
            spawnedThrumbo.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.Manhunter);
        }
    }
}
