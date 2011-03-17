using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Collections.ObjectModel;
using CollisionLib;

namespace CollisionDetectionDemo
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class CollisionDemo : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager _graphics;
        SpriteBatch _spriteBatch;

        CollisionManager _collisionManager;

        private Ball _ball1;
        private Ball _ball2;
        private AxisAlignedRectangle _rect1;

        public CollisionDemo()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            DisplayMode displayMode = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode;
            this._graphics.PreferredBackBufferFormat = displayMode.Format;
            this._graphics.PreferredBackBufferWidth = (int)(displayMode.Width / 1.1);
            this._graphics.PreferredBackBufferHeight = (int)(displayMode.Height / 1.1);

            _ball1 = new Ball(this, "BallR", 2, 1);
            _ball2 = new Ball(this, "BallB", 2, 1);
            _rect1 = new AxisAlignedRectangle(this, "RectB", 2, 1);
            Reset();

            this.Components.Add(_ball1);
            //this.Components.Add(_ball2);
            this.Components.Add(_rect1);

            IEnumerable<ICollidable> collidableObjects = this.Components.OfType<ICollidable>();

            _collisionManager = new CollisionManager(this, collidableObjects);
        }

        public void Reset()
        {
            _ball1.SetState(new Vector2((int)(this._graphics.PreferredBackBufferWidth / 3.4), (int)(this._graphics.PreferredBackBufferHeight / 5)), new Vector2(this._graphics.PreferredBackBufferWidth / 8400f, this._graphics.PreferredBackBufferHeight / 5250f)); // new Vector2(0.2f, 0.2f));
            _ball2.SetState(new Vector2((int)(this._graphics.PreferredBackBufferWidth / 1.8), (int)(this._graphics.PreferredBackBufferHeight / 1.8)), new Vector2(this._graphics.PreferredBackBufferWidth / -8400f, this._graphics.PreferredBackBufferHeight / -10500f)); // new Vector2(-0.2f, -0.1f));
            _rect1.SetState(new Vector2((int)(this._graphics.PreferredBackBufferWidth / 1.8), (int)(this._graphics.PreferredBackBufferHeight / 1.8)), new Vector2(this._graphics.PreferredBackBufferWidth / -8400f, this._graphics.PreferredBackBufferHeight / -10500f)); // new Vector2(-0.2f, -0.1f));
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                this.Exit();
            }

            // Allow the user to press 'R' to reset the collsion demo.
            if (Keyboard.GetState().IsKeyDown(Keys.R))
            {
                Reset();
            }

            base.Update(gameTime);

            // Apply all the collisions. This is done after the base update so all of the objects have been moved to their new locations for this frame before we collide them.
            _collisionManager.ApplyCollisions(gameTime.ElapsedGameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            base.Draw(gameTime);
        }
    }
}
