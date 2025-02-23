using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Slime_Shooter_New_Horizons;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private FollowCamera followCamera;

    private Player player;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }
    

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here
        
        followCamera = new FollowCamera(Vector2.Zero);

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // TODO: use this.Content to load your game content here
        Texture2D playerTexture = Content.Load<Texture2D>("white_prototype_rectangle");
        player = new Player(playerTexture, new Rectangle(0, 0, playerTexture.Width * 5, playerTexture.Height * 5), 
            new Rectangle(0, 0, playerTexture.Width, playerTexture.Height), 
            new Vector2(playerTexture.Width, playerTexture.Height), 1);
        
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here
        
        player.Update(gameTime);
        
        followCamera.FollowTarget(player.destinationRectangle,
            new Vector2(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight));

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // TODO: Add your drawing code here
        
        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
        
        player.Draw(_spriteBatch, followCamera.position);
        
        Texture2D playerTexture = Content.Load<Texture2D>("white_prototype_rectangle");
        _spriteBatch.Draw(playerTexture, new Rectangle((int)followCamera.position.X, (int)followCamera.position.Y, playerTexture.Width, playerTexture.Height), Color.White);
        
        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
