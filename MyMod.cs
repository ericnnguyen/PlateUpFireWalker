﻿using Kitchen;
using KitchenMods;
using Unity.Collections;
using Unity.Entities;

namespace MyMod
{
    public class SetEverythingOnFire : GenericSystemBase, IModSystem
    {
        private EntityQuery Appliances;

        struct CHasBeenSetOnFire : IModComponent { }

        protected override void Initialise()
        {
            base.Initialise();
            Appliances = GetEntityQuery(new QueryHelper()
                    .All(typeof(CAppliance))
                    .None(
                        typeof(CFire),
                        typeof(CIsOnFire),
                        typeof(CFireImmune),
                        typeof(CHasBeenSetOnFire)
                    ));
        }

        protected override void OnUpdate()
        {
            var appliances = Appliances.ToEntityArray(Allocator.TempJob);
            foreach (var appliance in appliances)
            {
                EntityManager.AddComponent<CIsOnFire>(appliance);
                EntityManager.AddComponent<CHasBeenSetOnFire>(appliance);
            }
            appliances.Dispose();
        }
    }
}