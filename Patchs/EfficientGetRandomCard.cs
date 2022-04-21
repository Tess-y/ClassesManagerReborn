using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using HarmonyLib;
using UnboundLib;
using UnityEngine;

namespace ClassesManagerReborn.Patchs
{
    [Serializable]
    [HarmonyPatch]
    public class EfficientGetRandomCard
    {
        public static bool PrefixMthod(CardChoice cardChoice, ref GameObject __result)
        {
            if (Main.Force_Class.Value)
            {
                // this is a more efficient version of the above method that gauruntees that cards drawn will be valid on the first try
                Player player = PickingPlayer(cardChoice);
                if (player == null || player.data.currentCards.Intersect(ClassesRegistry.ClassInfos).Any()) { return true; }
                Main.Debug("Forcing Class");
                CardInfo[] validCards = ClassesRegistry.ClassInfos.Where(c => ModdingUtils.Utils.Cards.instance.PlayerIsAllowedCard(player, c) && GetRelativeRarity(c) > 0f).ToArray();

                try
                {
                    if ((bool)CardChoiceVisuals.instance.GetFieldValue("isShowinig"))
                    {
                        List<string> spawnedCards = ((List<GameObject>)CardChoice.instance.GetFieldValue("spawnedCards")).Select(obj => obj.GetComponent<CardInfo>().cardName).ToList();
                        validCards = validCards.Where(c => c.categories.Contains(CardChoiceSpawnUniqueCardPatch.CustomCategories.CustomCardCategories.CanDrawMultipleCategory) || !spawnedCards.Contains(c.cardName)).ToArray();
                    }
                }
                catch (NullReferenceException)
                {
                    validCards = ClassesRegistry.ClassInfos.Where(c => ModdingUtils.Utils.Cards.instance.PlayerIsAllowedCard(player, c) && GetRelativeRarity(c) > 0f).ToArray();
                }

                // if there are no valid cards immediately return the Null Card from SpawnUniqueCardPatch
                if (!validCards.Any())
                {
                    __result = ((CardInfo)typeof(CardChoiceSpawnUniqueCardPatch.CardChoiceSpawnUniqueCardPatch).GetField("NullCard", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null)).gameObject;
                    return false;
                }

                GameObject result = null;
                float num = 0f;
                for (int i = 0; i < validCards.Length; i++)
                {
                    num += GetRelativeRarity(validCards[i]);
                }
                float num2 = UnityEngine.Random.Range(0f, num);
                for (int j = 0; j < validCards.Length; j++)
                {
                    num2 -= GetRelativeRarity(validCards[j]);

                    if (num2 <= 0f)
                    {
                        result = validCards[j].gameObject;
                        break;
                    }
                }
                __result = result;
                return false;
            }
            return true;
        }


        internal static float GetRelativeRarity(CardInfo card)
        {
            return (float)Main.instance.GetRelativeRarity.Invoke(null,new object[] { card });
        }

        internal static Player PickingPlayer(CardChoice cardChoice) //thefted from original
        {
            Player player = null;
            try
            {
                if ((PickerType)cardChoice.GetFieldValue("pickerType") == PickerType.Team)
                {
                    player = PlayerManager.instance.GetPlayersInTeam(cardChoice.pickrID).FirstOrDefault();
                }
                else
                {
                    player = (cardChoice.pickrID < PlayerManager.instance.players.Count() && cardChoice.pickrID >= 0) ? PlayerManager.instance.players[cardChoice.pickrID] : null;
                }
            }
            catch
            {
                player = null;
            }

            return player;
        }
    }
}
