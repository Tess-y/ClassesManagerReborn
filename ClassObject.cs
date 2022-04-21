using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClassesManagerReborn
{
    public class ClassObject
    {
        public CardInfo card;
        public CardType type;
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
            if (!noBlacklist && ((CardType.Entry | CardType.SubClass) & type) != 0)
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
            foreach(CardInfo[] RequiredClassTree in RequiredClassesTree)
            {
                bool flag = true;
                List<CardInfo> playerCards = player.data.currentCards.ToList();
                foreach(CardInfo card in RequiredClassTree)
                {
                    if (playerCards.Contains(card){
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

    }
}
