using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PlatformerEngine.Components
{
    public abstract class Component
    {
        protected readonly Texture2D Texture;
        protected readonly GetNearby nearby;
        //public ComponentManager manager { get; private set; }

        public bool Gravity { get; set; }
        public bool Collidable { get; set; }

        public Vector2 Position { get; set; }
        public GetDisplacement Displacement { get; set; }
        public Vector2 Velocity { get; set; }

        public virtual Rectangle CollisionArea
        {
            get { return new Rectangle((int)Position.X, (int)Position.Y, Texture.Bounds.Width, Texture.Bounds.Height); }
        }

        public virtual void Draw(GameTime time, SpriteBatch batch)
        {
            batch.Draw(Texture, CollisionArea, Color.White);
        }

        public virtual void Update(GameTime time)
        {
            Position += Velocity * (float)time.ElapsedGameTime.TotalSeconds;
        }

        protected Component(Texture2D tex, Vector2 position, GetNearby nearby, GetDisplacement displacement)
        {
            Position = position;
            Displacement = displacement;
            Velocity = new Vector2();
            Collidable = true;
            Texture = tex;
            this.nearby = nearby;

            //manager = man
        }

        public virtual void CollidedWith(Component other, GameTime time) { }

        //GraphicsDevice graphDevice {get { return PlatformEngine.GraphicsDevice; }}
        public bool LegalMove(Vector2 move)
        {
            Rectangle rect = this.CollisionArea;
            Rectangle newArea = new Rectangle(rect.X + ComponentManager.FloatMax(move.X),rect.Y + ComponentManager.FloatMax(move.Y), rect.Width, rect.Height);

            return nearby(this).Any(e => e.CollisionArea.Intersects(CollisionArea) && !e.Equals(this));
        }

        public Rectangle GetScreenBox(Texture2D texture)
        {
            return new Rectangle((int)(Position.X - Displacement().X),
                                               (int)(720 - Position.Y - texture.Height/2 - Displacement().Y),
                                               texture.Width/2, texture.Height/2);
        }
    }

    public delegate List<Component> GetNearby(Component comp);

    public delegate Vector2 GetDisplacement();
}
