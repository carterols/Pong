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
    class ComputerPaddle : Paddle {
        /***********************************************
        * METHOD: ComputerPaddle Constructor
        * DESCRIPTION: Initializes class variables. Uses 
        *   base constructor
        * RETURN: None
        * *********************************************/
        public ComputerPaddle(float x, float y, float screenWidth, SpriteBatch spriteBatch, GameContent gameContent) : base(x, y, screenWidth, spriteBatch, gameContent) { }

        /***********************************************
        * METHOD: MoveTo Override
        * DESCRIPTION: Overrides MoveTo method in Paddle
        *   class
        * RETURN: None
        * *********************************************/
        new public void MoveTo(float x) {
            float xBefore = XPos;
            XPos = x - (Width / 2);
            XSpeed = XPos - xBefore;

            // below makes the Computer a little less perfect by limiting their top speed
            if (XPos > xBefore) { // if we moved to the right
                if (XSpeed > 15) {
                    XPos = xBefore + 15;
                }
            } else { // if we moved to the left
                if (XSpeed < -15) {
                    XPos = xBefore - 15;
                }
            }

            if (XPos < 1) {
                XPos = 1;
                XSpeed = 0;
            } else if (XPos + Width > ScreenWidth) {
                XPos = ScreenWidth - Width;
                XSpeed = 0;
            }

        }
    }
}
