using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClassesManagerReborn
{
    public static class ClassesRegistry
    {
        internal static Dictionary<CardInfo, ClassObject> Registry = new Dictionary<CardInfo, ClassObject>();
        internal static List<CardInfo> ClassInfos = new List<CardInfo>();

        /// <summary>
        /// Resesters a card with the manager
        /// </summary>
        /// <param name="card">The card to regester</param>
        /// <param name="type">The type of card being regestered</param>
        /// <param name="CardLimit">The max number of copies a player is alowed to have, 0 for infinite</param>
        public static ClassObject Register(CardInfo card, CardType type, int CardLimit = 0)
        {
            if(type != CardType.Entry)
            {
                throw new ArgumentException("Non-entry cards require a requirment tree to resester");
            }
            return Register(card, type, new CardInfo[] { }, CardLimit);
        }

        /// <summary>
        /// Resesters a card with the manager
        /// </summary>
        /// <param name="card">The card to regester</param>
        /// <param name="type">The type of card being regestered</param>
        /// <param name="RequiredClass">The Card a player must have inorder to get this card</param>
        /// <param name="CardLimit">The max number of copies a player is alowed to have, 0 for infinite</param>
        public static ClassObject Register(CardInfo card, CardType type, CardInfo RequiredClass, int CardLimit = 0)
        {
            return Register(card, type, new CardInfo[] { RequiredClass }, CardLimit);
        }

        /// <summary>
        /// Resesters a card with the manager
        /// </summary>
        /// <param name="card">The card to regester</param>
        /// <param name="type">The type of card being regestered</param>
        /// <param name="RequiredClassTree">The Cards a player must have inorder to get this card</param>
        /// <param name="CardLimit">The max number of copies a player is alowed to have, 0 for infinite</param>
        public static ClassObject Register(CardInfo card, CardType type, CardInfo[] RequiredClassTree, int CardLimit = 0)
        {
            return Register(card, type, new CardInfo[][] { RequiredClassTree }, CardLimit);
        }

        /// <summary>
        /// Resesters a card with the manager
        /// </summary>
        /// <param name="card">The card to regester</param>
        /// <param name="type">The type of card being regestered</param>
        /// <param name="RequiredClassesTree">A list of lists of cards, a player must have all the cards on at least on of the lists to get the card</param>
        /// <param name="CardLimit">The max number of copies a player is alowed to have, 0 for infinite</param>
        public static ClassObject Register(CardInfo card, CardType type, CardInfo[][] RequiredClassesTree, int CardLimit = 0)
        {
            if (Registry.ContainsKey(card))
            {
                throw new ArgumentException($"Card {card.cardName} has already been Registered");
            }
            ClassObject classObject = new ClassObject(card, type, RequiredClassesTree, card.allowMultiple? CardLimit : 1);
            Registry.Add(card, classObject);
            if(type == CardType.Entry) ClassInfos.Add(card);
            if (card.allowMultiple)
                classObject.Whitelist(card);
            return classObject;
        }

        public static ClassObject Get(CardInfo card)
        {
            if (Registry.ContainsKey(card)) return Registry[card];
            return null;
        }

        public static List<ClassObject> GetClassObjects(CardType type)
        {
            return Registry.Values.Where(v => (type & v.type) != 0).ToList();
        }

        public static List<CardInfo> GetClassInfos(CardType type)
        {
            return Registry.Values.Where(v => (type & v.type) != 0).Select<ClassObject,CardInfo>(v => v.card).ToList();
        }

    }

    [Flags]
    public enum CardType
    {
        Entry = 0x1, //The entry point of a class, normally players can only have one Entry card.
        SubClass = 0x2, //The entry point of a subclass, normally players can only have one SubClass card.
        Branch = 0x4, //Indicats that there are other cards locked behind this one.
        Card = 0x8, //A normal card in the class system
    }
}
