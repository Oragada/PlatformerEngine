using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PlatformerEngine.Components
{
    public abstract class Creature : Component
    {

        public bool JustGotHit { get; private set; }

        public bool FacingRight { get; private set; }

        public int RunFade { get; protected set; }
        public int Acceleration { get; protected set; }
        public int MaxSpeed { get; protected set; }

        public int JumpBoost { get; private set; }

        public Creature(Texture2D tex, Vector2 position, GetNearby nearby, GetDisplacement displace)
            : base(tex, position, nearby, displace)
        {
            FacingRight = true;

            Gravity = true;
            Collidable = true;

            RunFade = 900;
            Acceleration = 1000;
            MaxSpeed = 100;
            JumpBoost = 500;
        }


        public virtual Rectangle CollisionArea
        {
            get { return new Rectangle((int)Position.X, (int)Position.Y, Texture.Bounds.Width/2, Texture.Bounds.Height/2); }
        }

        public override void Draw(GameTime time, SpriteBatch batch)
        {
            int tl = 100;
            Rectangle texSelect;
            if (InAir((float) time.ElapsedGameTime.TotalSeconds))
            {
                texSelect = Velocity.Length() > 0.001 ? new Rectangle(tl, tl, tl, tl) : new Rectangle(0,tl,tl,tl);
            }
            else
            {
                texSelect = Velocity.Length() > 0.001 ? new Rectangle(tl, 0, tl, tl) : new Rectangle(0, 0, tl, tl);
            }

            Rectangle truePos = GetScreenBox(Texture);

            batch.Draw(Texture,truePos,texSelect, Color.White,0,new Vector2(0), FacingRight ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
        }

        public override void Update(GameTime time)
        {
            JustGotHit = false;
            base.Update(time);
            Act(time);

            //Speed
            if (Math.Abs(Velocity.Length() - 0) < 0.001) return; //No movement, so we avoid a lot of calculations

            double secTime = time.ElapsedGameTime.TotalSeconds;

            int slowDown = (int) (RunFade*secTime);

            if (!InAir((float)secTime))
            {
                //Stopping
                if ((Velocity.X > 0 && Velocity.X - slowDown < 0) || (Velocity.X < 0 && Velocity.X - slowDown > 0))
                {
                    Velocity = new Vector2(0, Velocity.Y);
                }
                else //Slow down
                {
                    Velocity -= new Vector2(Velocity.X > 0 ? slowDown : -slowDown, 0);
                }
            }

            //Position
            int change = (int) (Velocity.X*secTime);
            Vector2 run = new Vector2(change, 0);

            if (LegalMove(run))
            {
                Position += run;
            }
            else //collision
            {
                //List<Component> Nearby = new List<Component>();
                 
                List<Component> hit =
                    this.nearby(this).Where(e => e.Collidable && e.CollisionArea.Intersects(this.CollisionArea)).ToList();
                foreach (Component component in hit)
                {
                    CollidedWith(component, time);
                    component.CollidedWith(component, time);
                }
                
                //TODO: brake on collision?
                //Velocity = new Vector2(0, Velocity.Y);
            }
        }

        protected bool InAir(float time)
            {
                return !IsOnSomething(time);
            }

        protected bool IsOnSomething(float time)
        {
            return (
                       Position.Y <= 0 || //means that botton is always solid
                       LegalMove(Velocity + new Vector2(0, -PlatformEngine.GRAVITY*time*time)) //will hit something if fall TODO: modify? see ComponentManager Gravity
                   );

        }

        public virtual void Act(GameTime time)
        {
            FacingRight = Velocity.X >= 0;
        }

        public virtual void Jump(float time)
        {
            if(!InAir(time)) Velocity += new Vector2(0,JumpBoost);
        }

    }
}
