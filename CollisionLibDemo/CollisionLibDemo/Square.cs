using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CollisionLib;
using Microsoft.Xna.Framework;

namespace CollisionDetectionDemo
{
    class Square : BaseColliableObject, IColliadableAxisAlignedRectangle
    {
        public float Height { get { return Texture.Height * Scale; } }
        public float Width { get { return Texture.Width * Scale; } }

        public Square(Game game, String texture, float scale, float mass)
            : this(game, texture, scale, mass, Vector2.Zero, Vector2.Zero)
        {
        }

        public Square(Game game, String texture, float scale, float mass, Vector2 initialPosition, Vector2 initialVelocity)
            : base(game, texture, scale, mass, initialPosition, initialVelocity, 0.001f)
        {
        }
    }
}
