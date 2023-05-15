using Kitchen;
using KitchenData;
using KitchenLib.References;
using KitchenMods;
using Unity.Collections;
using Unity.Entities;

namespace KitchenFireWalker
{
    public class BurningMessSystem : DaySystem
    {
        EntityQuery Players;

        protected override void Initialise()
        {
            base.Initialise();
            Players = GetEntityQuery(new QueryHelper()
                .All(typeof(CPlayer), typeof(CPosition)));
        }

        protected override void OnUpdate()
        {
            using NativeArray<CPosition> playerPositions = Players.ToComponentDataArray<CPosition>(Allocator.Temp);
            for (int i = 0; i < playerPositions.Length; i++)
            {
                CPosition position = playerPositions[i];
                if (GetOccupant(position, OccupancyLayer.Default) != default || GetOccupant(position, OccupancyLayer.Floor) != default)
                    continue;

                Entity newMess = EntityManager.CreateEntity();
                Set(newMess, new CPosition(position.Position.Rounded()));
                Set(newMess, default(CMess));
                Set(newMess, default(CIsOnFire));
                Set(newMess, default(CBurningMess));
                Set(newMess, new CCreateAppliance()
                {
                    ID = ApplianceReferences.MessKitchen1,
                    ForceLayer = OccupancyLayer.Floor
                });
            }
        }
    }
}
