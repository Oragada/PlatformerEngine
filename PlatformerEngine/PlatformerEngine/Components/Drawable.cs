using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using PlatformerEngine.Components;

namespace PlatformerEngine.Components
{
    abstract class Drawable : Entity
    {
        private Texture2D Texture { get; set; }
    }
}
