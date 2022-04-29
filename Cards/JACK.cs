using System;
using System.Collections.Generic;
using System.Text;
using UnboundLib.Cards;
using UnityEngine;
using ModdingUtils.Extensions;
using UnboundLib;

namespace ClassesManagerReborn.Cards
{
    internal class JACK : CustomCard
    {
        internal static CardInfo card;
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers)
        {
            cardInfo.allowMultiple = false;
            cardInfo.GetAdditionalData().canBeReassigned = false;
        }
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            ClassesManager.instance.ExecuteAfterFrames(2, () => { 
                List<CardInfo> classes = ClassesRegistry.GetClassInfos(CardType.Entry);
                foreach (CardInfo card in classes)
                {
                    if(!player.data.currentCards.Contains(card))
                        ModdingUtils.Utils.Cards.instance.AddCardToPlayer(player, card, addToCardBar: true);
                }
            });
        }

        protected override GameObject GetCardArt()
        {
            return null;
        }

        protected override string GetDescription()
        {
            return "Master of none";
        }

        protected override CardInfo.Rarity GetRarity()
        {
            return CardInfo.Rarity.Rare;
        }

        protected override CardInfoStat[] GetStats()
        {
            return new CardInfoStat[] { };
        }

        protected override CardThemeColor.CardThemeColorType GetTheme()
        {
            return CardThemeColor.CardThemeColorType.NatureBrown;
        }

        protected override string GetTitle()
        {
            return "JACK";
        }

        public override string GetModName()
        {
            return "CMR";
        }
    }
}
