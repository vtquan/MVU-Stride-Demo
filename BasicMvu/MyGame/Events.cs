using Stride.Engine;
using Stride.Engine.Events;
using System;

namespace MyGame
{
    public static class Events
    {
        public static readonly EventKey<string> PlayerEventKey = new();
        public static readonly EventReceiver<string> PlayerEventListener = new(PlayerEventKey, EventReceiverOptions.Buffered);
    }
}