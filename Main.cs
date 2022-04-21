using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using System;
using UnboundLib;
using UnboundLib.Utils.UI;
using UnityEngine;

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
        public static ConfigEntry<bool> DEBUG;
        private const string ModId = "[INCERT_MOD_ID_HERE_AT_SOME_POINT]";
        private const string ModName = "Classes Manager Reborn";
        public const string Version = "0.0.0";
        public const string ModInitials = "CMR";
        public static Main instance { get; private set; }


        void Awake()
        {
            var harmony = new Harmony(ModId);
            harmony.PatchAll();

            DEBUG = base.Config.Bind<bool>(ModId, "Debug", false, "Enable to turn on concole spam from our mod");
        }

        void Start()
        {
            instance = this;
            Unbound.RegisterMenu(ModName, delegate () { }, new Action<GameObject>(this.NewGUI), null, true);



        }

        public static void Debug(object message)
        {
            if (DEBUG.Value)
            {
                UnityEngine.Debug.Log($"{ModInitials}=>" + message);
            }
        }

        private void NewGUI(GameObject menu)
        {
            MenuHandler.CreateText(ModId, menu, out _, 60, false, null, null, null, null);





            GameObject toggle = MenuHandler.CreateToggle(DEBUG.Value, "Debug Mode", menu, delegate (bool value)
            {
                DEBUG.Value = value;
            }, 50, false, Color.red, null, null, null);
        }
    }
}
