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
    class Ball {
        public float XPos { get; private set; }
        public float YPos { get; private set; }
        public float XSpeed { get; private set; }
        public float YSpeed { get; private set; }
        public float Height { get; }
        public float Width { get; }
        public float ScreenWidth { get; }
        public float ScreenHeight { get; }
        public float Rotation { get; private set; }
        public bool UseRotation { get; private set; }
        public bool Visible { get; set; }

        public int[] Score { get; set; }

        private Texture2D imgBall;
        private SpriteBatch spriteBatch;
        private GameContent gameContent;

        /***********************************************
         * METHOD: Ball Constructor
         * DESCRIPTION: Initializes class variables
         * RETURN: None
         * *********************************************/
        public Ball(float screenWidth, float screenHeight, SpriteBatch spriteBatch, GameContent gameContent) {
            XPos = 0;
            YPos = 0;
            XSpeed = 0;
            YSpeed = 0;
            Rotation = 0;
            imgBall = gameContent.imgBall;
            Width = imgBall.Width;
            Height = imgBall.Height;
            this.spriteBatch = spriteBatch;
            this.gameContent = gameContent;
            ScreenHeight = screenHeight;
            ScreenWidth = screenWidth;
            Visible = false;
            UseRotation = true;
            Score = new int[2];
            Score[0] = 0;
            Score[1] = 0;
        }

        /***********************************************
        * METHOD: Draw
        * DESCRIPTION: Draws the ball on the screen
        * RETURN: None
        * *********************************************/
        public void Draw() {
            if (Visible == false) {
                return;
            }
            if (UseRotation) {
                Rotation += .1f;
                if (Rotation > 3 * Math.PI) {
                    Rotation = 0;
                }
            }
            spriteBatch.Draw(imgBall, new Vector2(XPos, YPos), null, Color.White, Rotation, new Vector2(Width / 2, Height / 2), 1.0f, SpriteEffects.None, 0);
        }

        /***********************************************
        * METHOD: Launch
        * DESCRIPTION: Starts the round by launching the
        *   ball from the player's paddle
        * RETURN: None
        * *********************************************/
        public void Launch(float x, float y, float xSpeed, float ySpeed) {
            if (Visible == true) {
                return;
            }
            PlaySound(gameContent.startSound);
            Visible = true;
            XPos = x;
            YPos = y;
            XSpeed = xSpeed;
            YSpeed = ySpeed;
        }

        /***********************************************
        * METHOD: Move
        * DESCRIPTION: Moves the ball on the screen. Handles
        *   collisions againsts walls and paddles.
        * RETURN: True if no one has scored, False if 
        *   someone scores
        * *********************************************/
        public bool Move(Paddle paddle, Paddle comPaddle) {
            if (Visible == false) { return false; }

            XPos += XSpeed;
            YPos += YSpeed;

            if (XPos < 1) {
                XPos = 1;
                XSpeed *= -1;
                PlaySound(gameContent.wallBounceSound);
            }
            if (XPos > ScreenWidth - Width + 5) {
                XPos = ScreenWidth - Width + 5;
                XSpeed = XSpeed * -1;
                PlaySound(gameContent.wallBounceSound);
            }
            if (YPos < 1) {
                Visible = false;
                YPos = 0;
                PlaySound(gameContent.missSound);
                Score[0]++;
                return false;
            }
            if (YPos + Height > ScreenHeight) {
                Visible = false;
                YPos = 0;
                PlaySound(gameContent.missSound);
                Score[1]++;
                return false;
            }

            Rectangle paddleRect = new Rectangle((int)paddle.XPos, (int)paddle.YPos, (int)paddle.Width, (int)paddle.Height);
            Rectangle ballRect = new Rectangle((int)XPos, (int)YPos, (int)Width, (int)Height);
            Rectangle comPaddleRect = new Rectangle((int)comPaddle.XPos, (int)comPaddle.YPos, (int)comPaddle.Width, (int)comPaddle.Height);
            // if the ball hits the paddle
            if (HitTest(paddleRect, ballRect)) {
                PlaySound(gameContent.paddleBounceSound);
                SetReboundSpeed(paddle);
                YPos = paddle.YPos - Height + 1;
            } else if (HitTest(comPaddleRect, ballRect)) {
                PlaySound(gameContent.paddleBounceSound);
                SetReboundSpeed(comPaddle);
                YPos = comPaddle.YPos + Height + 1;
            }

            if (XSpeed > 20) {
                XSpeed = 20;
            } else if (XSpeed < -20) {
                XSpeed = -20;
            }
            if (YSpeed > 20) {
                YSpeed = 20;
            } else if (YSpeed < -20) {
                YSpeed = -20;
            }
            return true;           
        }

        /***********************************************
        * METHOD: HitTest
        * DESCRIPTION: Tests if there is a collision between
        *   a paddle and a ball
        * RETURN: True if there is a collision, false if not.
        * *********************************************/
        public static bool HitTest(Rectangle r1, Rectangle r2) {
            if (Rectangle.Intersect(r1,r2) != Rectangle.Empty) {
                return true;
            }
            return false;
        }

        /***********************************************
        * METHOD: PlaySound
        * DESCRIPTION: Plays a sound
        * RETURN: None
        * *********************************************/
        public static void PlaySound(SoundEffect sound) {
            float volume = 1;
            sound.Play(volume, 0.0f, 0.0f);
        }

        /***********************************************
        * METHOD: SetReboundSpeed
        * DESCRIPTION: Sets the rebound speed after a 
        *   paddle collision
        * RETURN: None
        * *********************************************/
        private void SetReboundSpeed(Paddle paddle) {
            XSpeed = (Math.Sign(XSpeed)) * (Math.Abs(XSpeed) + Math.Abs(paddle.XSpeed));
            YSpeed *= -1;
        }
    }
}
