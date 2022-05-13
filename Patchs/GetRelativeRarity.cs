using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using HarmonyLib;
using UnboundLib.Utils;

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
                UnityEngine.Debug.Log($"hit on card {card.cardName}");
                __result *= ClassesManager.Class_Odds.Value;
            }
            else if (card.GetComponent<Util.Legend>() != null)
            {
                __result /= 3f;
            }
        }
    }
    [Serializable]
    [HarmonyPatch]
    public class GetRarityAsPerc
    {
        public static void PostfixMthod(ref float __result, CardInfo card)
        {
            float cardR = (float)ClassesManager.instance.GetRelativeRarity.Invoke(null, new object[] { card });
            float totalR = ((ObservableCollection<CardInfo>)typeof(CardManager).GetField("activeCards", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null))
                .ToList().Select(c => (float)ClassesManager.instance.GetRelativeRarity.Invoke(null, new object[] { c })).Sum();
            __result = cardR / totalR;


            UnityEngine.Debug.Log($"{__result}:{cardR}/{totalR}");
        }
    }
}
