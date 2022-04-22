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
            if (ClassesManager.Double_Odds.Value)
            {
                if(ClassesRegistry.Registry.ContainsKey(card) && ClassesRegistry.Registry[card].type != CardType.Entry)
                {
                    __result *= 2;
                }
            }
        }
    }
}
