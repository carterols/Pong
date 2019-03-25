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
    class ScoreBoard {
        public float Width { get; set; }
        public float Height { get; set; }

        private SpriteFont labelFont { get; set; }  //cached font arial20
        private SpriteBatch spriteBatch;  //allows us to write on backbuffer when we need to draw self

        /***********************************************
        * METHOD: ScoreBoard Constructor
        * DESCRIPTION: Initializes class variables
        * RETURN: None
        * *********************************************/
        public ScoreBoard(float width, float height, SpriteBatch spriteBatch, GameContent gameContent) {
            Width = width;
            Height = height;
            this.spriteBatch = spriteBatch;
            labelFont = gameContent.labelFont;
        }

        /***********************************************
        * METHOD: Draw
        * DESCRIPTION: Given a score, draw the score in 
        *   the upper-left corner. If game is over,
        *   relay result to center of screen
        * RETURN: True if game is over, False if not.
        * *********************************************/
        public bool Draw(int[] score) {
            if (score[0] >= 7) {
                spriteBatch.DrawString(labelFont, "YOU WIN! GAME OVER", new Vector2(CenterText("YOU WIN! GAME OVER"), Height / 2), Color.White);
                return true;
            } else if (score[1] >= 7) {
                spriteBatch.DrawString(labelFont, "YOU LOSE! GAME OVER", new Vector2(CenterText("YOU LOSE! GAME OVER"), Height / 2), Color.White);
                return true;
            } else {
                spriteBatch.DrawString(labelFont, score[0].ToString() + " - " + score[1].ToString(), new Vector2(1, 1), Color.White);
                return false;
            }
        }

        /***********************************************
        * METHOD: CenterText
        * DESCRIPTION: Does the calculations to center 
        *   the string on the screen.
        * RETURN: X Position for where to start drawing
        *   the string.
        * *********************************************/
        private int CenterText(string str) {
            Vector2 strSize;
            float width;

            strSize = labelFont.MeasureString(str);
            width = (Width / 2) - (strSize.X / 2);

            return (int)width;      
        }
    }
}
