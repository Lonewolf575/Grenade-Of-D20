using RimWorld;
using Verse;

namespace ARK_GrenadeOfD20
{
    internal class Proj_GrenadeOfD20 : Projectile_Explosive
    {
        public ModExtention_D20Grenade Props => base.def.GetModExtension<ModExtention_D20Grenade>();

        private int ticksToDetonation;

        public override void Tick()
        {
            base.Tick();
            if (this.ticksToDetonation <= 0)
                return;
            --this.ticksToDetonation;
            if (this.ticksToDetonation > 0)
                return;
            Log.Message("Hello World from Impact!");
            int rand = Rand.Int;
            Messages.Message("Test 1: {rand}", MessageTypeDefOf.NeutralEvent);
            base.Explode();
        }


        protected override void Impact(Thing hitThing, bool blockedByShield = false)
        {
            if (blockedByShield || this.def.projectile.explosionDelay == 0)
            {
                Log.Message("Hello World from Impact!");
                int rand = Rand.Int;
                Messages.Message("Test 1: {rand}", MessageTypeDefOf.NeutralEvent);
                base.Explode();
            }
            else
            {
                this.landed = true;
                this.ticksToDetonation = this.def.projectile.explosionDelay;
                GenExplosion.NotifyNearbyPawnsOfDangerousExplosive((Thing)this, this.def.projectile.damageDef, this.launcher.Faction, this.launcher);
            }
        }
    }
}
