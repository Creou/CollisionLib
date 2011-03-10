using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace CollisionLib
{
    public interface ICollidable
    {
        Vector2 Position { get; }
        Vector2 Velocity { get; }

        void SetState(Vector2 position, Vector2 velocity);
        void SetPosition(Vector2 position);

        float Mass { get; }
    }
}
