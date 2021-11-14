using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stride.Core.Mathematics;
using Stride.Input;
using Stride.Engine;

namespace MyGame
{
    public class SphereController : SyncScript
    {
        public List<Keys> KeysLeft { get; } = new List<Keys>();

        public List<Keys> KeysRight { get; } = new List<Keys>();

        public List<Keys> KeysUp { get; } = new List<Keys>();

        public List<Keys> KeysDown { get; } = new List<Keys>();

        public override void Update()
        {
            var isKeyPress = false;

            if (KeysLeft.Any(key => Input.IsKeyDown(key)))
            {
                isKeyPress = true;
                Events.GameEventKey.Broadcast(new Tuple<string, Entity>("Left", Entity));
            }
            if (KeysRight.Any(key => Input.IsKeyDown(key)))
            {
                isKeyPress = true;
                Events.GameEventKey.Broadcast(new Tuple<string, Entity>("Right", Entity));
            }
            if (KeysUp.Any(key => Input.IsKeyDown(key)))
            {
                isKeyPress = true;
                Events.GameEventKey.Broadcast(new Tuple<string, Entity>("Up", Entity));
            }
            if (KeysDown.Any(key => Input.IsKeyDown(key)))
            {
                isKeyPress = true;
                Events.GameEventKey.Broadcast(new Tuple<string, Entity>("Down", Entity));
            }
            if (isKeyPress == false)
            {
                Events.GameEventKey.Broadcast(new Tuple<string, Entity>("Stop", Entity));
            }
        }
    }
}
