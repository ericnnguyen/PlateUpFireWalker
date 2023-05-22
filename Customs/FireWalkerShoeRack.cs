using Kitchen;
using KitchenData;
using KitchenLib.Customs;
using KitchenLib.References;
using KitchenLib.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace KitchenFireWalker
{
    internal class FireWalkerShoeRack : CustomAppliance
    {
        public override int BaseGameDataObjectID => ApplianceReferences.ShoeRackTrainers;
        public override string UniqueNameID => "firewalkershoerack";
        ///public override GameObject Prefab => GameData.Main.GetPrefab(BaseGameDataObjectID) ?? PrefabBuilder.CreateEmptyPrefab("Provider - Shoes - FireWalkers"); 
        static GameObject _prefabCache = null;
        public override GameObject Prefab
        {
            get
            {
                if (_prefabCache == null)
                {
                    GameObject container = new GameObject("Hider");
                    container.SetActive(false);
                    _prefabCache = Object.Instantiate((GDOUtils.GetExistingGDO(BaseGameDataObjectID) as Appliance).Prefab);
                    _prefabCache.transform.SetParent(container.transform);
                }
                return _prefabCache;
            }
        }
        public override List<IApplianceProperty> Properties => new List<IApplianceProperty>()
        {
            new CShoeSelector()
            {
                Shoe = (PlayerShoe) Mod.PLAYER_SHOE_FIRE_WALKER,
                Available = 1,
                Max = 1
            },
        };
        public override List<Appliance.ApplianceProcesses> Processes { get => ((Appliance)GDOUtils.GetExistingGDO(ApplianceReferences.ShoeRackTrainers)).Processes; protected set => base.Processes = value; }
        public override bool IsNonInteractive => false;
        public override OccupancyLayer Layer => OccupancyLayer.Default;
        public override EntryAnimation EntryAnimation => EntryAnimation.Placement;
        public override ExitAnimation ExitAnimation => ExitAnimation.Destroy;
        public override bool IsPurchasable => true;
        public override bool IsPurchasableAsUpgrade => true;
        public override DecorationType ThemeRequired => DecorationType.Null;
        public override ShoppingTags ShoppingTags => ShoppingTags.Misc;
        public override RarityTier RarityTier => RarityTier.Uncommon;
        public override PriceTier PriceTier => PriceTier.Medium;
        public override bool StapleWhenMissing => false;
        public override bool SellOnlyAsDuplicate => false;
        public override bool PreventSale => false;
        public override bool IsNonCrated => false;

        public override List<(Locale, ApplianceInfo)> InfoList => InfoList = new List<(Locale, ApplianceInfo)>()
        {
            (Locale.English, new ApplianceInfo()
            {
                Name = "Fire Walkers",
                Description = "When you need more fire",
                Sections = new List<Appliance.Section>()
                {
                    new Appliance.Section()
                    {
                        Title = "Ignore Mess",
                        Description = "Ignores messes but without the lower speed"
                    },
                    new Appliance.Section()
                    {
                        Title = "Burning Mess Spreader",
                        Description = "Spreads a burning mess with every step"
                    },
                    new Appliance.Section()
                    {
                        Title = "Flame Road",
                        Description = "Move extremely fast when walking over a burning mess"
                    }
                }
            })
        };

        public override List<Appliance> Upgrades => new List<Appliance>()
        {
            GDOUtils.GetExistingGDO(ApplianceReferences.FireExtinguisherHolder) as Appliance
        };

        bool isRegistered = false;

        public override void OnRegister(Appliance gameDataObject)
        {
            base.OnRegister(gameDataObject);

            if (!isRegistered)
            {
                ApplyMaterials();
                //ApplyComponents();
                isRegistered = true;
            }
        }

        private void ApplyMaterials()
        {
            var materials = new Material[1];
            materials[0] = MaterialUtils.GetExistingMaterial("Plastic - Shiny Red");
            MaterialUtils.ApplyMaterial(Prefab, "GameObject/Shoe - Trainer/Shoe/Body", materials);
            MaterialUtils.ApplyMaterial(Prefab, "GameObject/Shoe - Trainer (1)/Shoe/Body", materials);

            materials[0] = MaterialUtils.GetExistingMaterial("Plastic - Shiny Red");
            MaterialUtils.ApplyMaterial(Prefab, "GameObject/Shoe - Trainer/Shoe/Sole", materials);
            MaterialUtils.ApplyMaterial(Prefab, "GameObject/Shoe - Trainer (1)/Shoe/Sole", materials);
        }

        private void ApplyComponents()
        {
        }
    }
}
