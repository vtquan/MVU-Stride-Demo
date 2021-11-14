using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stride.Core.Mathematics;
using Stride.Engine.Events;

namespace MvuTest
{
    public static class Events
    {
        public static readonly EventKey<Vector3> MoveDirectionEventKey = new();

        public static readonly EventKey<Vector2> CameraDirectionEventKey = new();

        public static readonly EventKey<bool> JumpEventKey = new();

        public static readonly EventReceiver<Vector3> moveDirectionEvent = new(MoveDirectionEventKey);

        public static readonly EventReceiver<bool> jumpEvent = new(JumpEventKey);
    }
}
