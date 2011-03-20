using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace CollisionLib
{
    public interface ICollidableCircle : ICollidable
    {
        float Diameter { get; }
        float Radius { get; }
    }
}
