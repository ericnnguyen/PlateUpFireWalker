using Kitchen;
using KitchenData;
using KitchenLib.References;
using Unity.Collections;
using Unity.Entities;

namespace KitchenFireWalker
{
    internal class BurningMessSystem : DaySystem
    {
        EntityQuery Players;

        protected override void Initialise()
        {
            base.Initialise();
            Players = GetEntityQuery(new QueryHelper()
                .All(typeof(CPlayer), typeof(CPlayerCosmetics), typeof(CPosition)));
        }

        protected override void OnUpdate()
        {
            using NativeArray<CPosition> playerPositions = Players.ToComponentDataArray<CPosition>(Allocator.Temp);
            using NativeArray<CPlayerCosmetics> playerComestics = Players.ToComponentDataArray<CPlayerCosmetics>(Allocator.Temp);
            for (int i = 0; i < playerPositions.Length; i++)
            {
                CPlayerCosmetics cosmetics = playerComestics[i];
                if (cosmetics.Shoe != (PlayerShoe) Mod.PLAYER_SHOE_FIRE_WALKER) continue;
                CPosition position = playerPositions[i];
                if (GetOccupant(position, OccupancyLayer.Default) != default || GetOccupant(position, OccupancyLayer.Floor) != default) continue;

                Entity newMess = EntityManager.CreateEntity();
                Set(newMess, new CPosition(position.Position.Rounded()));
                Set(newMess, default(CMess));
                Set(newMess, default(CIsOnFire));
                Set(newMess, new CCreateAppliance()
                {
                    ID = ApplianceReferences.MessKitchen1,
                    ForceLayer = OccupancyLayer.Floor
                });
            }
        }
    }
}
