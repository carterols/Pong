using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Pong {
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        GameContent gameContent;

        private Paddle paddle;
        private ComputerPaddle comPaddle;
        private GameBorder gameBorder;
        private Midline midline;
        private Ball ball;
        private ScoreBoard scoreBoard;
        private int ScreenWidth = 0;
        private int ScreenHeight = 0;
        private bool GameEnd = false;
        private MouseState oldMouseState;
        private KeyboardState oldKeyboardState;
        private bool readyToServeBall = true;

        public Game1() {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize() {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent() {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            gameContent = new GameContent(Content);
            ScreenWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            ScreenHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;

            // Make the screen 502 x 700 max if larger than that
            if (ScreenWidth >= 502) {
                ScreenWidth = 502;
            } 
            if (ScreenHeight >= 700) {
                ScreenHeight = 700;
            }

            graphics.PreferredBackBufferWidth = ScreenWidth;
            graphics.PreferredBackBufferHeight = ScreenHeight;
            graphics.ApplyChanges();

            int paddleX = (ScreenWidth - gameContent.imgPaddle.Width) / 2; // center the paddle on the screen
            int paddleY = ScreenHeight - 100; // 100 pixels from the bottom
            int comPaddleY = 100;
            paddle = new Paddle(paddleX, paddleY, ScreenWidth, spriteBatch, gameContent);
            comPaddle = new ComputerPaddle(paddleX, comPaddleY, ScreenWidth, spriteBatch, gameContent);
            gameBorder = new GameBorder(ScreenWidth, ScreenHeight, spriteBatch, gameContent);
            midline = new Midline(ScreenWidth, ScreenHeight, spriteBatch, gameContent);
            ball = new Ball(ScreenWidth, ScreenHeight, spriteBatch, gameContent);
            scoreBoard = new ScoreBoard(ScreenWidth, ScreenHeight, spriteBatch, gameContent);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent() {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime) {
            if (IsActive == false) {
                return;
            }

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            KeyboardState newKeyboardState = Keyboard.GetState();
            MouseState newMouseState = Mouse.GetState();

            comPaddle.MoveTo(ball.XPos);
            if (oldMouseState.X != newMouseState.X) {
                if (newMouseState.X >= 0 || newMouseState.X < ScreenWidth && newMouseState.Y >= 0 && newMouseState.Y < ScreenHeight) {
                    paddle.MoveTo(newMouseState.X);
                }
            } else {
                paddle.XSpeed = 0;
            }

            // left click
            if (newMouseState.LeftButton == ButtonState.Released && oldMouseState.LeftButton == ButtonState.Pressed && oldMouseState.X == newMouseState.X && oldMouseState.Y == newMouseState.Y && readyToServeBall) {
                ServeBall();
            }

            //process keyboard events                           
            if (newKeyboardState.IsKeyDown(Keys.Left)) {
                paddle.MoveLeft();
            }
            if (newKeyboardState.IsKeyDown(Keys.Right)) {
                paddle.MoveRight();
            }
            if (oldKeyboardState.IsKeyUp(Keys.Space) && newKeyboardState.IsKeyDown(Keys.Space) && readyToServeBall) {
                ServeBall();
            }

            oldMouseState = newMouseState; // this saves the old state      
            oldKeyboardState = newKeyboardState;
            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.Black);

            if (GameEnd) {
                System.Threading.Thread.Sleep(3000);
                GameEnd = false;
            }
            spriteBatch.Begin();
            paddle.Draw();
            comPaddle.Draw();
            midline.Draw();
            gameBorder.Draw();
           
            if (ball.Visible) {
                bool inPlay = ball.Move(paddle, comPaddle);
                if (inPlay) {
                    ball.Draw();
                } else {
                    readyToServeBall = true;
                }
            }
            if (scoreBoard.Draw(ball.Score)) {
                ball.Score[0] = 0;
                ball.Score[1] = 0;
                GameEnd = true;
            }
           
            spriteBatch.End();
            base.Draw(gameTime);
        }

        /***********************************************
        * METHOD: ServeBall
        * DESCRIPTION: Serves the ball from the player's
        *   paddle. 
        * RETURN: None
        * *********************************************/
        private void ServeBall() {
            readyToServeBall = false;
            Random rand = new Random();
            float ballX = paddle.XPos + (paddle.Width / 2);
            float ballY = paddle.YPos - ball.Height;
            int randXSpeed = rand.Next(3, 20) - 9;
            ball.Launch(ballX, ballY, randXSpeed, -8);

        }
    }
}
