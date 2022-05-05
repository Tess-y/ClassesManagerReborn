using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

namespace ClassesManagerReborn.Util
{
    internal class Legend : MonoBehaviour
    {
        public static Color color = new Color(1, 1, 0, 1);
        public void Update()
        {
            List<GameObject> rarityText = ClassNameMono.FindObjectsInChildren(gameObject, "RarityText(Clone)");
            if(rarityText.Count > 0)
            {
                rarityText[0].GetComponent<TextMeshProUGUI>().color = color;
                rarityText[0].GetComponent<TextMeshProUGUI>().text = rarityText[0].GetComponent<TextMeshProUGUI> ().text.Replace("Rare", "Legendary");
            }
        }
    }
}
