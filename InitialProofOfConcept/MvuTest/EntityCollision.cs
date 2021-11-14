using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stride.Core.Mathematics;
using Stride.Input;
using Stride.Engine;
using static MvuTest.MVU.Game;
using static MvuTest.MVU.Message;
using static MvuTest.MVU.Events;
using Stride.Core;
using Stride.Games;

namespace MvuTest
{
    public class EntityCollision : AsyncScript
    {
        // Declared public member fields and properties will show in the game studio
        protected GameCompositionRoot MvuGame;

        public override async Task Execute()
        {
            MvuGame = (GameCompositionRoot)Services.GetService<IGame>();

            var trigger = Entity.Get<PhysicsComponent>();
            trigger.ProcessCollisions = true;

            while (Game.IsRunning)
            {
                // Do stuff every new frame
                var firstCollision = await trigger.NewCollision();
                //MvuGame.SendCollisionMessage();
                MessageEventKey.Broadcast(new Tuple<string, Entity>("Collision", Entity));
                //await Script.NextFrame();
            }
        }
    }
}
