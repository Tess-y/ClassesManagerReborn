using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClassesManagerReborn.Patchs
{
    [Serializable]
    [HarmonyPatch(typeof(ModdingUtils.Utils.Cards), "PlayerIsAllowedCard")]
    public class PlayerIsAllowedCard
    {
        public static void Postfix(ref bool __result, Player player, CardInfo card)
        {
            if (ClassesRegistry.Registry.ContainsKey(card))
            {
                __result = ClassesRegistry.Registry[card].PlayerIsAllowedCard(player);
            }
        }
    }
}
