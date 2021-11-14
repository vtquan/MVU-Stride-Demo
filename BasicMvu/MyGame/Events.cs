using Stride.Engine;
using Stride.Engine.Events;
using System;

namespace MyGame
{
    public static class Events
    {
        public static readonly EventKey<Tuple<string, Entity>> GameEventKey = new();
        public static readonly EventReceiver<Tuple<string, Entity>> gameListener = new(GameEventKey, EventReceiverOptions.Buffered);
    }
}