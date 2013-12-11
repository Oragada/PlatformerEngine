using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PlatformerEngine.Components
{
    public class ComponentManager
    {
        private const float SCROLL = 0.3f;

        public GraphicsDevice Device { get; set; }
        private List<Component> Components;
        private List<Component> toBeAdded;
        private List<Component> toBeRemoved;

        private Random rand;

        public Vector2 Displacement { get; private set; }
        public int Score { get; private set; }
        public int Count { get { return Components.Count; } }

        public ComponentManager(GraphicsDevice device)
        {
            Device = device;
            Components = new List<Component>();
            toBeAdded = new List<Component>();
            toBeRemoved = new List<Component>();

            rand = new Random();
        }

        public void AddComponent(Component comp)
        {
            toBeAdded.Add(comp);
        }

        public void RemoveComponent(Component comp)
        {
            toBeRemoved.Add(comp);
        }

        public void Update(GameTime time)
        {
            toBeAdded.ForEach(e => Components.Add(e));
            toBeAdded.Clear();

            toBeRemoved.ForEach(e => Components.Add(e));
            toBeRemoved.Clear();

            Components.ForEach(e => e.Update(time));
            Gravity(time);
            
            DoScroll(time);
        }

        private void DoScroll(GameTime time)
        {
            Rectangle screen = Device.Viewport.Bounds;

            Player player = Components.First(e => e is Player) as Player;

            if (Displacement.X + screen.Width - player.CollisionArea.Center.X < screen.Width * SCROLL)
            {
                Displacement = Displacement + new Vector2((float)(player.MaxSpeed * time.ElapsedGameTime.TotalSeconds), 0);
                //foreach (Component e in Component((int)Displacement.X + 2 * screen.Width)) AddComponent(e);
            }
            else if (Displacement.X + screen.Width - player.CollisionArea.Center.X > screen.Width * (1 - SCROLL) && Displacement.X > 0)
            {
                Displacement = new Vector2((float)(Displacement.X - player.MaxSpeed * time.ElapsedGameTime.TotalSeconds), Displacement.Y);
            }
        }

        public void Draw(GameTime time, SpriteBatch batch)
        {
            Components.Where(e => !OutsideView(e)).ToList().ForEach(e => e.Draw(time, batch));
        }

        void Gravity(GameTime time)
        {
            float secTime = (float) time.ElapsedGameTime.TotalSeconds;
            int gravity = (int) (PlatformEngine.GRAVITY*secTime);

            foreach (Component component in Components.Where(e => e.Gravity))
            {
                int fall = (int) (component.Velocity.Y*secTime);

                if (component.LegalMove(new Vector2(0, fall)*secTime)) //Can keep falling
                {
                    component.Velocity += new Vector2(0, -gravity);
                }
                else //stop fall
                {
                    component.Velocity = new Vector2(component.Velocity.X, 0);

                    //collisions
                    foreach (Component otherComp in ComponentsWithin(component, new Vector2(0, fall) * secTime).Where(e => !e.Equals(component)))
                    {
                        component.CollidedWith(otherComp,time);
                    }

                    //Move the final bit. Adds the remaining pixels required for a perfect fit
                    while (!component.LegalMove(new Vector2(0, fall)) && fall < 0) fall++;
                }

                //Move
                Vector2 vMove = new Vector2(0, fall);
                if (component.LegalMove(vMove))
                {
                    component.Position += vMove;
                }
            }
        }

        public List<Component> GetNearbyComps(Component comp)
        {
            return Components.Where(e => (e.Position - comp.Position).Length() > 400).ToList();
        }

        public Component[] ComponentsWithin(Component component, Vector2 move)
        {
            Rectangle rect = component.CollisionArea;
            Rectangle newArea = new Rectangle(rect.X + FloatMax(move.X),rect.Y + FloatMax(move.Y), rect.Width, rect.Height);
            return ComponentsWithin(newArea);
        }

        private Component[] ComponentsWithin(Rectangle newArea)
        {
            return Components.Where(e => e.Collidable && e.CollisionArea.Intersects(newArea)).ToArray();
        }

        public static int FloatMax(float input)
        {
            return (int) (input >= 0.0f ? Math.Ceiling(input) : Math.Floor(input));
        }

        public bool OutsideView(Component comp)
        {
            //TODO OutsideView
            return false;
        }

        public Vector2 GetDisplacement()
        {
            return Displacement;
        }
    }
}
