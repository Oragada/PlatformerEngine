using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PlatformerEngine.Components
{
    class Obstacle : Component
    {
        protected Obstacle(Texture2D tex, Vector2 pos, GetNearby nearby, GetDisplacement displace)
            : base(tex, pos, nearby,displace)
        {
            Collidable = true;
            Gravity = false;
            Position = pos;

        }
    }
}
