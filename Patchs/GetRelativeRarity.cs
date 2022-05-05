using System;
using System.Collections.Generic;
using System.Text;
using HarmonyLib;

namespace ClassesManagerReborn.Patchs
{
    [Serializable]
    [HarmonyPatch]
    public class GetRelativeRarity
    {
        public static void PostfixMthod(ref float __result, CardInfo card)
        {
            if (ClassesRegistry.Registry.ContainsKey(card) && ClassesRegistry.Registry[card].RequiredClassesTree[0].Length != 0 && (ClassesRegistry.Registry[card].type & CardType.NonClassCard) == 0)
            {
                __result *= ClassesManager.Class_Odds.Value;
            }
            else if(card == Cards.JACK.card)
            {
                __result /= 3f;
            }
        }
    }
}
