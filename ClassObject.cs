using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClassesManagerReborn
{
    public class ClassObject
    {
        public CardInfo card;
        public CardType type { internal set; get; }
        public CardInfo[][] RequiredClassesTree;
        private bool noBlacklist = false;
        private List<CardInfo> whiteList = new List<CardInfo>();


        public List<CardInfo> GetwhiteList { get { if (((CardType.Card | CardType.Branch) & type) != 0) return null; if (noBlacklist) return ClassesRegistry.GetClassInfos(type); return whiteList; } }

        public ClassObject(CardInfo card, CardType type, CardInfo[][] requiredClassesTree)
        {
            this.card = card;
            this.type = type;
            RequiredClassesTree = requiredClassesTree;
        }


        public void WhitelistAll(bool value = true)
        {
            noBlacklist = value;
        }

        public void Whitelist(CardInfo card)
        {
            noBlacklist = false;
            if(!whiteList.Contains(card))
                whiteList.Add(card);
        }

        public void dewhitelist(CardInfo card)
        {
            noBlacklist = false;
            whiteList.Remove(card);
        }

        public bool PlayerIsAllowedCard(Player player)
        {
            if (Main.Class_War.Value && type == CardType.Entry)
            {
                foreach(Player p in PlayerManager.instance.players)
                {
                    if (p != player && p.data.currentCards.Contains(card)) return false;
                }
            }
            if (!Main.Ignore_Blacklist.Value && !noBlacklist && ((CardType.Entry | CardType.SubClass) & type) != 0)
            {
                if (whiteList.Any())
                {
                    if (player.data.currentCards.Where(c => !whiteList.Contains(c)).Intersect(ClassesRegistry.GetClassInfos(type)).Any()) return false;
                }
                else
                {
                    if (player.data.currentCards.Intersect(ClassesRegistry.GetClassInfos(type)).Any()) return false;
                }
            }
            if (type == CardType.Entry) return true;
            foreach(CardInfo[] RequiredClassTree in RequiredClassesTree)
            {
                bool flag = true;
                List<CardInfo> playerCards = player.data.currentCards.ToList();
                foreach(CardInfo card in RequiredClassTree)
                {
                    if (playerCards.Contains(card)){
                        playerCards.Remove(card);
                    }
                    else
                    {
                        flag = false;
                        break;
                    }
                }
                if (flag) return true;
            }

            return false;
        }

        internal CardInfo? GetMissingClass(Player player)
        {
            List<CardInfo> cardInfos = new List<CardInfo>();
            bool first = true;

            foreach (CardInfo[] RequiredClassTree in RequiredClassesTree)
            {
                List<CardInfo> missing = new List<CardInfo>();
                List<CardInfo> playerCards = player.data.currentCards.ToList();
                foreach (CardInfo card in RequiredClassTree)
                {
                    if (playerCards.Contains(card))
                    {
                        playerCards.Remove(card);
                    }
                    else
                    {
                        missing.Add(card);
                    }
                }
                if (first || cardInfos.Count > missing.Count)
                {
                    first = false;
                    cardInfos = missing;
                } 
            }

            return cardInfos.Any() ? cardInfos[0] : null;
        }

        public static CardInfo[][] TecTreeHelper(CardInfo[] cards, int required_count)
        {
            List<CardInfo[]> ret = new List<CardInfo[]>();
            List<int> counts = new List<int>(); for(int i = 0; i<required_count; i++) counts.Add(0);
            while (counts[0] < cards.Length)
            {
                List<CardInfo> cardInfos = new List<CardInfo>();
                foreach(int i in counts) cardInfos.Add(cards[i]);
                ret.Add(cardInfos.ToArray());
                counts[counts.Count - 1]++;
                for (int i = counts.Count -1; i <= 0; i--)
                {
                    if (counts[i] == cards.Length && i!=0)
                    {
                        counts[i] = 0;
                        counts[i-1]++;
                    }
                }
            }
            return ret.ToArray();
        }

    }
}
