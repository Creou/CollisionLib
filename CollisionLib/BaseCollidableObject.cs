using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CollisionLib
{
    public abstract class BaseColliableObject : DrawableGameComponent, ICollidable
    {
        private String _textureName;
        private Texture2D _texture;

        private SpriteBatch _spriteBatch;

        private float _friction;
        private float _scale;

        private Vector2 _position;
        private Vector2 _velocity;
        private float _mass;

        public float Mass { get { return _mass; } protected set { _mass = value; } }
        public Vector2 Position { get { return _position; } protected set { _position = value; } }
        public Vector2 Velocity { get { return _velocity; } protected set { _velocity = value; } }

        public float Scale { get { return _scale; } protected set { _scale = value; } }
        public float Friction { get { return _friction; } protected set { _friction = value; } }
        public String TextureName { get { return _textureName; } protected set { _textureName = value; } }
        public Texture2D Texture { get { return _texture; } protected set { _texture = value; } }

        public BaseColliableObject(Game game) : base(game) { }

        public BaseColliableObject(Game game, String textureName, float scale, float mass, Vector2 initialPosition, Vector2 initialVelocity, float friction) :
            base(game)
        {
            TextureName = textureName;
            Scale = scale;
            Mass = mass;
            Position = initialPosition;
            Velocity = initialVelocity;
            Friction = 0.001f;
        }

        public void SetState(Vector2 newPosition, Vector2 newVelocity)
        {
            _position = newPosition;
            _velocity = newVelocity;
        }

        public void SetPosition(Vector2 newPosition)
        {
            SetState(newPosition, Velocity);
        }

        public void SetVelocity(Vector2 newVelocity)
        {
            SetState(Position, newVelocity);
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(this.Game.GraphicsDevice);

            Texture = this.Game.Content.Load<Texture2D>(TextureName);

            base.LoadContent();
        }

        private void ApplyFriction()
        {
            Vector2 velocityNormalized = Velocity;
            velocityNormalized.Normalize();

            float adjustedLength = (Velocity.Length() - _friction);
            if (adjustedLength > 0)
            {
                Velocity = velocityNormalized * adjustedLength;
            }
            else
            {
                Velocity = new Vector2(0, 0);
            }
        }

        public override void Update(GameTime gameTime)
        {
            float newPositionX = (float)(Position.X + (Velocity.X * gameTime.ElapsedGameTime.TotalMilliseconds));
            float newPositionY = (float)(Position.Y + (Velocity.Y * gameTime.ElapsedGameTime.TotalMilliseconds));

            Position = new Vector2(newPositionX, newPositionY);

            ApplyFriction();

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin();

            _spriteBatch.Draw(_texture, Position, null, Color.White, 0, new Vector2(_texture.Width / 2, _texture.Height / 2), _scale, SpriteEffects.None, 0);

            base.Draw(gameTime);

            _spriteBatch.End();
        }
    }
}
