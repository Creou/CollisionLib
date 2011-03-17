using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;
using CollisionLib;

namespace CollisionDetectionDemo
{
    public class Ball : BaseColliableObject, ICollidableCircle
    {
        public float Diameter { get { return Texture.Width * Scale; } }

        

        public Ball(Game game, String texture, float scale, float mass)
            : this(game, texture, scale, mass, Vector2.Zero, Vector2.Zero)
        {
        }

        public Ball(Game game, String texture, float scale, float mass, Vector2 initialPosition, Vector2 initialVelocity)
            : base(game, texture, scale, mass, initialPosition, initialVelocity, 0.001f)
        {
        }
    }
}
