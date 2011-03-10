using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CollisionLib
{
    public interface ICollider
    {
        bool CanCollide(ICollidable object1, ICollidable object2);
        void Collide(TimeSpan frameDuration, ICollidable Object1, ICollidable Object2);
    }
}
