using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PlatformerEngine.Components
{
    class Projectile : Component
    {
        public int Speed { get; private set; }
        public float Direction { get; private set; }

        public Projectile(Texture2D tex, Vector2 pos, int speed, float dir, GetNearby nearby, GetDisplacement displace)
            : base(tex, pos, nearby, displace)
        {
            Speed = speed;
            Direction = dir;
            Velocity = new Vector2((float) (Math.Cos(Math.PI - Direction) * Speed), (float) (Math.Sin(Math.PI-Direction)*Speed));
            Collidable = true;
            Gravity = false;
        }

        public override void Update(GameTime time)
        {
            base.Update(time);
            //TODO: collision system

            //TODO: remove if outside view
        }

        public override void Draw(GameTime time, SpriteBatch batch)
        {
            batch.Draw(Texture, CollisionArea, null, Color.White, (float) (Direction - Math.PI),
                       new Vector2(Texture.Height/2, Texture.Width/2), SpriteEffects.None, 0);

        }

        public override void CollidedWith(Component other, GameTime time)
        {
            //TODO: collision results
            base.CollidedWith(other, time);
        }
    }
}
