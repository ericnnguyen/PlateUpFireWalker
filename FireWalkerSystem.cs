using Kitchen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Entities;

namespace KitchenFireWalker
{
    internal class FireWalkerSystem : DaySystem
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

        }
    }
}
