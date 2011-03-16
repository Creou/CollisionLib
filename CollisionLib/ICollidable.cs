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

        void SetState(Vector2 newPosition, Vector2 newVelocity);
        void SetPosition(Vector2 newPosition);
        void SetVelocity(Vector2 newVelocity);

        float Mass { get; }
    }
}
