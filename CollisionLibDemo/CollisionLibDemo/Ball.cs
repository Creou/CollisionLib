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
    public class Ball : DrawableGameComponent, ICollidableCircle
    {
        private Texture2D _ballTexture;
        private SpriteBatch _spriteBatch;

        private Vector2 _position;
        private Vector2 _velocity;
        private float _friction;

        private float _scale;
        private float _mass;

        public Vector2 Position { get { return _position; } }
        public Vector2 Velocity { get { return _velocity; } }
        public float Mass { get { return _mass; } }
        public float Diameter { get { return _ballTexture.Width * _scale; } }

        private String _texture;

        public Ball(Game game, String texture, float scale, float mass)
            : this(game, texture, scale, mass, Vector2.Zero, Vector2.Zero)
        {
        }

        public Ball(Game game, String texture, float scale, float mass, Vector2 initialPosition, Vector2 initialVelocity)
            : base(game)
        {
            _texture = texture;
            _scale = scale;
            _mass = mass;
            _position = initialPosition;
            _velocity = initialVelocity;
            _friction = 0.001f;
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

            _ballTexture = this.Game.Content.Load<Texture2D>(_texture);

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            _position.X = (float)(_position.X + (_velocity.X * gameTime.ElapsedGameTime.TotalMilliseconds));
            _position.Y = (float)(_position.Y + (_velocity.Y * gameTime.ElapsedGameTime.TotalMilliseconds));
            ApplyBallFriction();

            base.Update(gameTime);
        }

        private void ApplyBallFriction()
        {
            Vector2 velocityNormalized = _velocity;
            velocityNormalized.Normalize();

            float adjustedLength = (_velocity.Length() - _friction);
            if (adjustedLength > 0)
            {
                _velocity = velocityNormalized * adjustedLength;
            }
            else
            {
                _velocity = new Vector2(0, 0);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin();

            _spriteBatch.Draw(_ballTexture, _position, null, Color.White, 0, new Vector2(_ballTexture.Width / 2, _ballTexture.Height / 2), _scale, SpriteEffects.None, 0);

            base.Draw(gameTime);

            _spriteBatch.End();
        }
    }
}
