using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CollisionLib
{
    public interface IColliadableAxisAlignedRectangle : ICollidable
    {
        float Width { get; }
        float Height { get; }
    }
}
