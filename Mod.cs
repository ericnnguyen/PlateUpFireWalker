using KitchenData;
using KitchenLib;
using KitchenLib.Event;
using KitchenLib.Utils;
using KitchenMods;
using PreferenceSystem;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using ApplianceLib;

// Namespace should have "Kitchen" in the beginning
namespace KitchenFireWalker
{
    public class Mod : BaseMod, IModSystem
    {
        // GUID must be unique and is recommended to be in reverse domain name notation
        // Mod Name is displayed to the player and listed in the mods menu
        // Mod Version must follow semver notation e.g. "1.2.3"
        public const string MOD_GUID = "Nghia.PlateUp.FireWalker";
        public const string MOD_NAME = "FireWalker";
        public const string MOD_VERSION = "0.69.1";
        public const string MOD_AUTHOR = "Nghia";
        public const string MOD_GAMEVERSION = ">=1.1.4";
        // Game version this mod is designed for in semver
        // e.g. ">=1.1.3" current and all future
        // e.g. ">=1.1.3 <=1.2.3" for all from/until

        // Boolean constant whose value depends on whether you built with DEBUG or RELEASE mode, useful for testing
#if DEBUG
        public const bool DEBUG_MODE = true;
#else
        public const bool DEBUG_MODE = false;
#endif

        public static AssetBundle Bundle;

        public const string FIRE_WALKER_ITEM_PROVIDER_ID = "fireWalkerItemProviderEnabled";
        public const string FIRE_WALKER_ID = "fireWalkerEnabled";

        public static PreferenceSystemManager PManager;

        public Mod() : base(MOD_GUID, MOD_NAME, MOD_AUTHOR, MOD_VERSION, MOD_GAMEVERSION, Assembly.GetExecutingAssembly()) { }

        protected override void OnInitialise()
        {
            LogWarning($"{MOD_GUID} v{MOD_VERSION} in use!");

            ////UpdateUpgrades();
        }

        private void UpdateUpgrades()
        {
            List<Appliance> appliances = new List<Appliance>();
            Appliance fireWalkerItemProvider = (Appliance)GDOUtils.GetCustomGameDataObject<FireWalkerShoeRack>().GameDataObject;
            if (fireWalkerItemProvider != null)
            {
                appliances.Add(fireWalkerItemProvider);
            }
        }

        private void AddGameData()
        {
            LogInfo("Attempting to register game data...");

            AddGameDataObject<FireWalkerShoeRack>();

            LogInfo("Done loading game data.");
        }

        protected override void OnUpdate()
        {
        }

        protected override void OnPostActivate(KitchenMods.Mod mod)
        {
            // TODO: Uncomment the following if you have an asset bundle.
            // TODO: Also, make sure to set EnableAssetBundleDeploy to 'true' in your ModName.csproj

            // LogInfo("Attempting to load asset bundle...");
            // Bundle = mod.GetPacks<AssetBundleModPack>().SelectMany(e => e.AssetBundles).First();
            // LogInfo("Done loading asset bundle.");

            // Register custom GDOs
            AddGameData();
            PManager = new PreferenceSystemManager(MOD_GUID, MOD_NAME);

            // Perform actions when game data is built
            Events.BuildGameDataEvent += delegate (object s, BuildGameDataEventArgs args)
            {
                args.gamedata.ProcessesView.Initialise(args.gamedata);
            };
        }

        #region Logging
        public static void LogInfo(string _log) { Debug.Log($"[{MOD_NAME}] " + _log); }
        public static void LogWarning(string _log) { Debug.LogWarning($"[{MOD_NAME}] " + _log); }
        public static void LogError(string _log) { Debug.LogError($"[{MOD_NAME}] " + _log); }
        public static void LogInfo(object _log) { LogInfo(_log.ToString()); }
        public static void LogWarning(object _log) { LogWarning(_log.ToString()); }
        public static void LogError(object _log) { LogError(_log.ToString()); }
        #endregion
    }
}
