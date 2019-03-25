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
    class Paddle {
        public const int SpeedModifier = 7;
        public float XPos { get; set; }
        public float YPos { get; set; }
        public float XSpeed { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }
        public float ScreenWidth { get; set; }

        private Texture2D imgPaddle { get; set; }
        private SpriteBatch SpriteBatch;

        /***********************************************
        * METHOD: Paddle Constructor
        * DESCRIPTION: Initializes class variables
        * RETURN: None
        * *********************************************/
        public Paddle(float x, float y, float screenWidth, SpriteBatch spriteBatch, GameContent gameContent) {
            XPos = x;
            YPos = y;
            XSpeed = 0;
            imgPaddle = gameContent.imgPaddle;
            Width = imgPaddle.Width;
            Height = imgPaddle.Height;
            SpriteBatch = spriteBatch;
            ScreenWidth = screenWidth;
        }

        /***********************************************
        * METHOD: Draw  
        * DESCRIPTION: Draws the Paddle onto the screen
        * RETURN: None
        * *********************************************/
        public void Draw() {
            SpriteBatch.Draw(imgPaddle, new Vector2(XPos, YPos), null, Color.White, 0, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0);
        }

        /***********************************************
        * METHOD: MoveLeft
        * DESCRIPTION: Moves the paddle left by the const
        *   SpeedModifier. This is used when user is moving
        *   with the keyboard arrows and not the mouse.
        * RETURN: None
        * *********************************************/
        public void MoveLeft() {
            XPos = XPos - SpeedModifier;
            XSpeed = -SpeedModifier;
            if (XPos < 1) {
                XPos = 1;
                XSpeed = 0;
            }
        }

        /***********************************************
        * METHOD: MoveRight
        * DESCRIPTION: Moves the paddle right by the const
        *   SpeedModifier. This is used when user is moving
        *   with the keyboard arrows and not the mouse.
        * RETURN: None
        * *********************************************/
        public void MoveRight() {
            XPos = XPos + SpeedModifier;
            XSpeed = SpeedModifier;
            if ((XPos + Width) > ScreenWidth) {
                XPos = ScreenWidth - Width;
                XSpeed = 0;
            }
        }

        /***********************************************
        * METHOD: MoveTo
        * DESCRIPTION: Moves the paddle to a passed in X
        *   coordinate. This is used when the user is using
        *   the keyboard
        * RETURN: None
        * *********************************************/
        public void MoveTo(float x) {
            float xBefore = XPos;

            if (x >= 0) {
                if (x < ScreenWidth - Width) {
                    XPos = x;
                } else {
                    XPos = ScreenWidth - Width;
                }
            } else {
                XPos = 0;
            }

            XSpeed = XPos - xBefore;
        }
    }
}
