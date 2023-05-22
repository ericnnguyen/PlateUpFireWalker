using HarmonyLib;
using Kitchen;
using KitchenData;
using KitchenLib.Utils;
using System;
using System.Linq;
using System.Reflection;
using UnityEngine;
using static Kitchen.PlayerShoeSubview;

namespace KitchenFireWalker
{
    [HarmonyPatch]
    internal static class PlayerShoeSubView_Patch
    {
        private static MethodInfo GetPrefabMethod = ReflectionUtils.GetMethod<LocalViewRouter>("GetPrefab");
        [HarmonyPatch(typeof(LocalViewRouter), "GetPrefab", new Type[] { typeof(ViewType) })]
        [HarmonyPostfix]
        static void GetPrefab_PostFix(ref GameObject __result, ref LocalViewRouter __instance, ViewType view_type)
        {
            if (view_type == ViewType.Player)
            {
                PlayerShoeSubview playerShoeSubview = __result?.GetChild("Shoes")?.GetComponent<PlayerShoeSubview>();
                if (playerShoeSubview != null && playerShoeSubview.Prefabs?.Count > 0)
                {
                    ShoePrefab fireWalkers = playerShoeSubview.Prefabs.FirstOrDefault(prefab => prefab.Shoe == (PlayerShoe)Mod.PLAYER_SHOE_FIRE_WALKER);
                    if (fireWalkers.Shoe == (PlayerShoe)Mod.PLAYER_SHOE_FIRE_WALKER) return;

                    ShoePrefab trainerPrefab = playerShoeSubview.Prefabs.FirstOrDefault(prefab => prefab.Shoe == PlayerShoe.Trainers);
                    if (trainerPrefab.Shoe != PlayerShoe.Trainers) return;

                    GameObject container = new GameObject("Hider");
                    container.SetActive(false);
                    GameObject fireWalker = UnityEngine.Object.Instantiate(trainerPrefab.Prefab, __result.transform, false);
                    fireWalker.transform.SetParent(container.transform);

                    var materials = new Material[1];
                    materials[0] = MaterialUtils.GetExistingMaterial("Plastic - Shiny Red");

                    MaterialUtils.ApplyMaterial(fireWalker, "Shoe/Body", materials);
                    MaterialUtils.ApplyMaterial(fireWalker, "Shoe/Sole", materials);
                    playerShoeSubview.Prefabs.Add(new ShoePrefab { Prefab = fireWalker, Shoe = (PlayerShoe)Mod.PLAYER_SHOE_FIRE_WALKER });
                }
            }
        }
    }
}