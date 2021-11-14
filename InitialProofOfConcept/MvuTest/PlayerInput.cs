using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stride.Core.Mathematics;
using Stride.Input;
using Stride.Engine;
using Stride.Engine.Events;
using static MvuTest.MVU.Events;
using Stride.Physics;

namespace MvuTest
{
    public class PlayerInput : SyncScript
    {
        // Declared public member fields and properties will show in the game studio
        private bool jumpButtonDown = false;

        public float DeadZone { get; set; } = 0.25f;

        public CameraComponent Camera { get; set; }

        /// <summary>
        /// Multiplies move movement by this amount to apply aim rotations
        /// </summary>
        public float MouseSensitivity = 100.0f;

        public List<Keys> KeysLeft { get; } = new List<Keys>();

        public List<Keys> KeysRight { get; } = new List<Keys>();

        public List<Keys> KeysUp { get; } = new List<Keys>();

        public List<Keys> KeysDown { get; } = new List<Keys>();

        public List<Keys> KeysJump { get; } = new List<Keys>();

        public override void Start()
        {
            // Initialization of the script.
        }

        public override void Update()
        {
            var isKeyPress = false;
            // Do stuff every new frame
            if (KeysLeft.Any(key => Input.IsKeyDown(key)))
            {
                isKeyPress = true;
                MessageEventKey.Broadcast(new Tuple<string, Entity>("Left", Entity));
            }
            if (KeysRight.Any(key => Input.IsKeyDown(key)))
            {
                isKeyPress = true;
                MessageEventKey.Broadcast(new Tuple<string, Entity>("Right", Entity));
            }
            if (KeysUp.Any(key => Input.IsKeyDown(key)))
            {
                isKeyPress = true;
                MessageEventKey.Broadcast(new Tuple<string, Entity>("Up", Entity));
            }
            if (KeysDown.Any(key => Input.IsKeyDown(key)))
            {
                isKeyPress = true;
                MessageEventKey.Broadcast(new Tuple<string, Entity>("Down", Entity));
            }
            if (KeysJump.Any(key => Input.IsKeyDown(key)))
            {
                isKeyPress = true;
                MessageEventKey.Broadcast(new Tuple<string, Entity>("Jump", Entity));
            }
            else if (Entity.Get<CharacterComponent>().IsGrounded)
            {
                MessageEventKey.Broadcast(new Tuple<string, Entity>("Grounded", Entity));
            }
            if (isKeyPress == false)
            {
                MessageEventKey.Broadcast(new Tuple<string, Entity>("NoMovement", Entity));
            }
        }
    }
}
