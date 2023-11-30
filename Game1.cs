using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Super_Zoom_Out
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        Rectangle toriyamaRect;     //create a rectangle to display "toriyamaAd" inside of.
        Texture2D toriyamaAd;       //create a texture2D to store jpg file.
                                    
        bool isZooming = true;      //create variable to check if screen is zooming.

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        //create method that works out percentages so our zoom feature will scroll at the correct ratio.
        public static int GetPercentage(int percentage, float inputValue)
        {
            float fraction = (float)percentage / 100;

            int result = (int)(fraction * inputValue);    //takes result of fraction * inputValue and casts it as an integer type.

            return result;
            
            //return (int)(float)(inputValue * percentage) / 100;   //a more simple solution to work out the percentage.
        }

        protected override void Initialize()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);     //is this line necessary???

            toriyamaRect = new Rectangle(0,     // x position of top left hand corner
                                         0,     // y position of top left hand corner
                                         9480,    // rectangle width
                                         7440);   // rectangle height

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // create a new SpriteBatch, which can be used to draw textures.
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load image content into the "game"
            toriyamaAd = Content.Load<Texture2D>("toriyama_ad");

            // create variables to hold display height and width
            int scaledWidth = toriyamaAd.Width * 10;
            int scaledHeight = toriyamaAd.Height * 10;
            float displayWidth = GraphicsDevice.Viewport.Width;
            float displayHeight = GraphicsDevice.Viewport.Height;
            float rectWidth = scaledWidth;
            float rectHeight = scaledHeight;
            float rectX = 0;
            float rectY = 0;

            // create variables to hold change in height and width.
            float widthChange = GetPercentage(1, rectWidth);
            rectWidth = rectWidth - widthChange;
            rectX = rectX + (widthChange / 2);

            float heightChange = GetPercentage(1, rectHeight);
            rectHeight = rectHeight - heightChange;
            rectY = rectY + (heightChange / 2);


            toriyamaRect = new Rectangle(
                -(int)(scaledWidth / 2) + (int)(displayWidth / 2),
                -(int)(scaledHeight / 2) + (int)(displayHeight / 2),
                (int)rectWidth,
                (int)rectHeight);

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // increase "toryiyamaRect" everytime update() is called.
            // This will create a zoom effect.
            //toriyamaRect.Height--;
            //toriyamaRect.Width--;

            if (isZooming)
            {
                // Calculate the current scale of the image
                float currentScale = (float)toriyamaRect.Width / toriyamaAd.Width;

                // Reduce width and height for zoom out effect, maintaining aspect ratio
                toriyamaRect.Width -= 6; // Adjust the value according to your needs

                // Ensure the width does not go below the original width
                toriyamaRect.Width = Math.Max(toriyamaRect.Width, toriyamaAd.Width);

                toriyamaRect.Height = (int)(toriyamaRect.Width / currentScale); // Maintain aspect ratio

                // Adjust the position to keep the image centered
                toriyamaRect.X = (GraphicsDevice.Viewport.Width - toriyamaRect.Width) / 2; // Center horizontally
                toriyamaRect.Y = (GraphicsDevice.Viewport.Height - toriyamaRect.Height) / 2; // Center vertically

                // Check if the image has filled the screen
                if (toriyamaRect.Width <= GraphicsDevice.Viewport.Width && toriyamaRect.Height <= GraphicsDevice.Viewport.Height)
                {
                    isZooming = false; // Stop zooming
                }
            
                base.Update(gameTime);

            }
        }

        protected override void Draw(GameTime gameTime)
        {
            //GraphicsDevice.Clear(Color.CornflowerBlue);

#if test
            //write a test for the GetPercentage().
            int test;
            test = GetPercentage(10, 800);

            if ((GetPercentage(0, 0) == 0)       &&  // 0 percent of 0
               (GetPercentage(0, 100) == 0)     &&  // 0 percent of 100
               (GetPercentage(50, 100) == 50)   &&  // 50 percent of 100
               (GetPercentage(100, 50) == 50)   &&  // 100 percent of 50
               (GetPercentage(10, 100) == 10))      // 10 percent of 100                               
            {
                GraphicsDevice.Clear(Color.Green);
            }
            else
            {
                GraphicsDevice.Clear(Color.Red);
            }
#endif

            _spriteBatch.Begin();
            _spriteBatch.Draw(toriyamaAd, toriyamaRect, Color.White);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}