using Kitchen;
using KitchenData;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace KitchenFireWalker
{
    [UpdateAfter(typeof(DetermineShoeBonus))]
    [UpdateBefore(typeof(DeterminePlayerSpeed))]
    internal class FlameRoadSystem : GenericSystemBase
    {
        EntityQuery Players;
        EntityQuery Slowers;

        protected override void Initialise()
        {
            base.Initialise();
            Players = GetEntityQuery(new QueryHelper()
                .All(typeof(CPlayer), typeof(CPlayerCosmetics), typeof(CShoeEffect), typeof(CPosition)));
            Slowers = GetEntityQuery(new QueryHelper().All(typeof(CPosition), typeof(CSlowPlayer), typeof(CIsOnFire)).None(typeof(CToolInUse)));
        }

        protected override void OnUpdate()
        {
            var players = Players.ToEntityArray(Allocator.TempJob);
            var slowers = Slowers.ToComponentDataArray<CPosition>(Allocator.Temp);
            using NativeArray<CPlayerCosmetics> playerComestics = Players.ToComponentDataArray<CPlayerCosmetics>(Allocator.Temp);
            using NativeArray<CPosition> playerPosition = Players.ToComponentDataArray<CPosition>(Allocator.Temp);
            for (int i = 0; i < players.Length; i++)
            {
                if (playerComestics[i].Shoe == (PlayerShoe) Mod.PLAYER_SHOE_FIRE_WALKER)
                {
                    ComputePlayerSpeed(players[i], playerPosition[i], slowers);
                }
            }

            players.Dispose();
            slowers.Dispose();
        }

        private void ComputePlayerSpeed(Entity player, CPosition cPosition, NativeArray<CPosition> slowerPositions)
        {
            Vector3 playerVector = cPosition.Position.Rounded();
            foreach (CPosition slowerPosition in slowerPositions)
            {
                Vector3 slowerVector = slowerPosition.Position.Rounded();
                if (playerVector.x == slowerVector.x && playerVector.z == slowerVector.z)
                {
                    EntityManager.SetComponentData(player, new CShoeEffect() { IgnoreMess = true, SpeedModifier = 2f });
                }
                else
                {
                    EntityManager.SetComponentData(player, new CShoeEffect() { IgnoreMess = true, SpeedModifier = 0f });
                }
            }
        }
    }
}
