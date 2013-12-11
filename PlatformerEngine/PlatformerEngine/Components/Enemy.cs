using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PlatformerEngine.Components
{
    class Enemy : Creature
    {
        public int ScoreValue { get; private set; }

        private int move;

        public Enemy(Texture2D tex, Vector2 position, int scoreValue, int startMove, GetNearby nearby, GetDisplacement displace)
            : base(tex, position, nearby, displace)
        {
            ScoreValue = scoreValue;
            if (startMove != 0)
            {
                move = startMove;
            }
            else
            {
                move = -250;
            }
            


            Velocity = new Vector2(move, 0);
        }

        public override void CollidedWith(Component other, GameTime time)
        {
            base.CollidedWith(other, time);
        }

        //TODO: NOT COMPLETED
    }
}
