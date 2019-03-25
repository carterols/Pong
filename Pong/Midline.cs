using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Pong {
    class Midline {
        public float Width { get; set; } //width of game
        public float Height { get; set; } //height of game

        private Texture2D imgPixel { get; set; }  //cached image single pixel we'll use to draw the border lines
        private SpriteBatch spriteBatch;  //allows us to write on backbuffer when we need to draw self

        /***********************************************
        * METHOD: Midline Constructor
        * DESCRIPTION: Initializes class variables
        * RETURN: None
        * *********************************************/
        public Midline (float width, float height, SpriteBatch spriteBatch, GameContent gameContent) {
            Width = width;
            Height = height;
            imgPixel = gameContent.imgPixel;
            this.spriteBatch = spriteBatch;
        }

        /***********************************************
        * METHOD: Draw
        * DESCRIPTION: Draws the Midline onto the screen
        * RETURN: None
        * *********************************************/
        public void Draw() {
            spriteBatch.Draw(imgPixel, new Rectangle(0, (int)(Height / 2), (int)Width - 1, 1), Color.Green);
        }
    }
}
