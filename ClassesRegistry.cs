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

        public static ClassObject Regester(CardInfo card, CardType type)
        {
            if(type != CardType.Entry)
            {
                throw new ArgumentException("Non-entry cards require a requirment tree to resester");
            }
            return Regester(card, type, new CardInfo[] { });
        }
        public static ClassObject Regester(CardInfo card, CardType type, CardInfo RequiredClass)
        {
            return Regester(card, type, new CardInfo[] { RequiredClass });
        }
        public static ClassObject Regester(CardInfo card, CardType type, CardInfo[] RequiredClassTree)
        {
            return Regester(card, type, new CardInfo[][] { RequiredClassTree });
        }
        public static ClassObject Regester(CardInfo card, CardType type, CardInfo[][] RequiredClassesTree)
        {
            if (Registry.ContainsKey(card))
            {
                throw new ArgumentException($"Card {card.cardName} has already been regestered");
            }
            ClassObject classObject = new ClassObject(card, type, RequiredClassesTree);
            Registry.Add(card, classObject);
            if(type == CardType.Entry) ClassInfos.Add(card);
            if (card.allowMultiple)
                classObject.Whitelist(card);
            return classObject;
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
        Entry = 0x1,
        SubClass = 0x2,
        Branch = 0x4,
        Card = 0x8,
    }
}
