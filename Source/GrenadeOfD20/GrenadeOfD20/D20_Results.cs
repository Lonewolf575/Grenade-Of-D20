using RimWorld;
using UnityEngine;
using Verse;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ARK_GrenadeOfD20
{
    public static class D20_Results
    {
        /*
            TypeEffects: position Effects
        */
        public static void effectLightningStrike(Map hitMap, IntVec3 hitPos)
        {
            hitMap.weatherManager.eventHandler.AddEvent(new WeatherEvent_LightningStrike(hitMap, hitPos));
        }
    }
}
