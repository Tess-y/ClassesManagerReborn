using System;
using System.Collections.Generic;
using System.Text;
using HarmonyLib;
using ModdingUtils.Utils;

namespace ClassesManagerReborn.Patchs
{
    [Serializable]
    [HarmonyPatch(typeof(Cards), nameof(Cards.AddCardToPlayer), new Type[] { typeof(Player), typeof(CardInfo), typeof(bool), typeof(string), typeof(float), typeof(float), typeof(bool) })]
    public class AddCardToPlayer
    {
        public static void Prefix(Player player, ref CardInfo card)
        {
            if (ClassesRegistry.Registry.ContainsKey(card))
            {
                CardInfo? requriment = ClassesRegistry.Registry[card].GetMissingClass(player);
                if (requriment != null) card = requriment;
            }
        }
    }
}
