using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using System;
using UnboundLib;
using UnboundLib.Networking;
using UnboundLib.Utils.UI;
using UnityEngine;
using Photon.Pun;

namespace ClassesManagerReborn
{
    // These are the mods required for our Mod to work
    [BepInDependency("com.willis.rounds.unbound", BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency("pykess.rounds.plugins.moddingutils", BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency("pykess.rounds.plugins.cardchoicespawnuniquecardpatch", BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency("pykess.rounds.plugins.deckcustomization", BepInDependency.DependencyFlags.HardDependency)]
    // Declares our Mod to Bepin
    [BepInPlugin(ModId, ModName, Version)]
    // The game our Mod Is associated with
    [BepInProcess("Rounds.exe")]
    public class Main : BaseUnityPlugin
    {
        private const string ModId = "[INCERT_MOD_ID_HERE_AT_SOME_POINT]";
        private const string ModName = "Classes Manager Reborn";
        public const string Version = "0.0.0";
        public const string ModInitials = "CMR";
        public static Main instance { get; private set; }

        public static ConfigEntry<bool> DEBUG;
        public static ConfigEntry<bool> Force_Class;
        public static ConfigEntry<bool> Ignore_Blacklist;
        public static ConfigEntry<bool> Ensure_Class_Card;
        public static ConfigEntry<bool> Class_War;


        void Awake()
        {
            var harmony = new Harmony(ModId);
            harmony.PatchAll();

            DEBUG = base.Config.Bind<bool>(ModId, "Debug", false, "Enable to turn on concole spam from our mod");
            Force_Class = base.Config.Bind<bool>(ModId, "Force_Class", false, "Enable Force Classes");
            Ignore_Blacklist = base.Config.Bind<bool>(ModId, "Ignore_Blacklist", false, "Allow more then one class per player");
            Ensure_Class_Card = base.Config.Bind<bool>(ModId, "Ensure_Class_Card", false, "Guarantee players in a class will draw a card for that class if able");
            Class_War = base.Config.Bind<bool>(ModId, "Class_War", false, "Prevent players from having the same class");
        }

        void Start()
        {
            instance = this;

            Unbound.RegisterHandshake(ModId, this.OnHandShakeCompleted);

            Unbound.RegisterMenu(ModName, delegate () { }, new Action<GameObject>(this.NewGUI), null, true);
        }

        private void OnHandShakeCompleted()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                NetworkingManager.RPC_Others(typeof(Main), nameof(SyncSettings), new object[] { Force_Class.Value, Ignore_Blacklist.Value, Ensure_Class_Card.Value, Class_War.Value });
            }
        }

        [UnboundRPC]
        private static void SyncSettings(bool host_Force_Class, bool host_Ignore_Blacklist, bool host_Ensure_Class_Card, bool host_Class_War)
        {
            Force_Class.Value = host_Force_Class;
            Ignore_Blacklist.Value = host_Ignore_Blacklist;
            Ensure_Class_Card.Value = host_Ensure_Class_Card;
            Class_War.Value = host_Class_War;
        }

        private void NewGUI(GameObject menu)
        {
            MenuHandler.CreateText(ModId, menu, out _, 60, false, null, null, null, null);

            MenuHandler.CreateToggle(Force_Class.Value, "Enable Force Classes", menu, value => Force_Class.Value = value);
            MenuHandler.CreateToggle(Ignore_Blacklist.Value, "Allow more then one class and subclass per player", menu, value => Ignore_Blacklist.Value = value);
            MenuHandler.CreateToggle(Ensure_Class_Card.Value, "Guarantee players in a class will draw a card for that class if able (NOT YET IMPLMENTED)", menu, value => Ensure_Class_Card.Value = value);
            MenuHandler.CreateToggle(Class_War.Value, "Prevent players from having the same class", menu, value => Class_War.Value = value);


            MenuHandler.CreateText("", menu, out _);
            MenuHandler.CreateToggle(DEBUG.Value, "Debug Mode", menu, value => DEBUG.Value = value, 50, false, Color.red, null, null, null);
        }


        public static void Debug(object message)
        {
            if (DEBUG.Value)
            {
                UnityEngine.Debug.Log($"{ModInitials}=>" + message);
            }
        }

    }
}
