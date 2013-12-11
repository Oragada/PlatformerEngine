using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PlatformerEngine.Components
{
    public class Player : Creature
    {
        private static Keys[] KEY_JUMP = new Keys[]{Keys.W, Keys.Up };
        private static Keys[] KEY_LEFT = new Keys[]{Keys.A, Keys.Left };
        private static Keys[] KEY_RIGHT = new Keys[]{Keys.D, Keys.Right };
        private static Keys[] KEY_SHOOT = new Keys[]{Keys.Space};

        public int Score { get; private set; }

        private bool lastSpace = false;
        private int fadeShoot;
        private int fadeShootDuration = 500;

        public Player(Texture2D tex, Vector2 position, GetNearby nearby, GetDisplacement displace)
            : base(tex, position, nearby, displace)
        {
            RunFade = 900;
            Acceleration = 1400;
            MaxSpeed = 350;
        }

        public void AddScore(int amount)
        {
            Score += amount;
        }

        public override void Act(GameTime gameTime)
        {

            base.Act(gameTime);
            float time = (float) gameTime.ElapsedGameTime.TotalSeconds;
            if (fadeShoot > 0)
            {
                fadeShoot -= (int) gameTime.ElapsedGameTime.TotalMilliseconds;
            }
            if (fadeShoot < 0)
            {
                fadeShoot = 0;
            }

            Vector2 increase = new Vector2(Acceleration*time, 0);

            if(KeyDown(KEY_RIGHT))
            {
                if (Velocity.X < MaxSpeed) Velocity += increase;
            }
            if (KeyDown(KEY_LEFT))
            {
                if (Velocity.X > -MaxSpeed) Velocity -= increase;
            }
            if (KeyDown(KEY_JUMP) && !lastSpace && IsOnSomething(time))
            {
                Jump(time);
            }
            /*if (KeyDown(KEY_SHOOT) && fadeShoot == 0)
            {
                Shoot(time);
            }*/

            lastSpace = KeyDown(KEY_JUMP);
        }

        private bool KeyDown(Keys[] keys)
        {
            return keys.Any(k => Keyboard.GetState().IsKeyDown(k));
        }


    }
}
