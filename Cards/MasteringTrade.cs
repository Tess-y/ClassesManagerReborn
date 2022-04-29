using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using UnboundLib.Cards;
using UnityEngine;
using ModdingUtils.Extensions;
using UnboundLib;

namespace ClassesManagerReborn.Cards
{
    internal class MasteringTrade : CustomCard
    {
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers)
        {
            cardInfo.GetAdditionalData().canBeReassigned = false;
        }
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            ClassesManager.instance.ExecuteAfterFrames(2, () => {

                List<CardInfo> classes = ClassesRegistry.GetClassInfos(~CardType.Entry);
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
            return "Get a random class card for a class you have.";
        }

        protected override CardInfo.Rarity GetRarity()
        {
            return CardInfo.Rarity.Common;
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
            return "Mastering the Trade";
        }

        public override string GetModName()
        {
            return "CMR";
        }
    }
}
