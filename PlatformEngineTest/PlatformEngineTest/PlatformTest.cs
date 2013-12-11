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
using PlatformerEngine;
using PlatformerEngine.Components;

namespace PlatformEngineTest
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class PlatformTest : PlatformEngine
    {
        private Texture2D HeroTex;
        private Texture2D EnemyTex;

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            base.LoadContent();
            HeroTex = Content.Load<Texture2D>("HeroShip");
            EnemyTex = Content.Load<Texture2D>("Enemy");

            cm.AddComponent(new Player(HeroTex, new Vector2(100, 100),cm.GetNearbyComps,cm.GetDisplacement));
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
        }

    }
}
