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
    private Vector2 screenRes = new Vector2(1280, 720);

    private Texture2D slimeTexture;
    
    // UI related
    private Texture2D itemsAtlas;
    private List<string> itemsNames;
    private Dictionary<string, int> itemsID;
    
    private List<Slime> slimeList;

    private FollowCamera followCamera;
    private Inventory inventory;

    private Player player;
    private Corral corral;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        _graphics.PreferredBackBufferWidth = (int)screenRes.X;
        _graphics.PreferredBackBufferHeight = (int)screenRes.Y;
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }
    

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here
        
        followCamera = new FollowCamera(Vector2.Zero);
        
        // UI
        itemsNames = new List<string>()
        {
            "Pink Slime", "Rock Slime"
        };
        itemsID = new Dictionary<string, int>();
        for (int i = 0; i < itemsNames.Count; i++)
        {
            itemsID[itemsNames[i]] = i;
        }
        //inventory = new(new Vector2(screenRes.X / 2 - 200f, screenRes.Y - 100f), );

        slimeList = new List<Slime>();

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // TODO: use this.Content to load your game content here
        
        slimeTexture = Content.Load<Texture2D>("pink_slime_jumping_spritesheet-export");
        itemsAtlas = Content.Load<Texture2D>("items_atlas");

        Slime littleSlime = new Slime(slimeTexture, new Rectangle(50, 50, 20, 20),
            new Rectangle(0, 0, 20, 20),
            2, new Vector2(slimeTexture.Width, slimeTexture.Height),
            6, 6, new Vector2(20, 20), 1, 1);
        slimeList.Add(littleSlime);
        
        Texture2D playerTexture = Content.Load<Texture2D>("spr_player_1_left_idle");
        player = new Player(playerTexture, new Rectangle(0, 0, playerTexture.Width * 5, playerTexture.Height * 5), 
            new Rectangle(0, 0, playerTexture.Width, playerTexture.Height), 
            1, new Vector2(playerTexture.Width, playerTexture.Height), slimeTexture);

        Texture2D corralTex = Content.Load<Texture2D>("corral_deactivated");
        Texture2D forceFieldTexHorizontal = Content.Load<Texture2D>("force_field_corral_prototype_anim");
        Texture2D forceFieldTexVertical = Content.Load<Texture2D>("force_field_corral_prototype_anim_vertical");
        corral = new Corral(corralTex,
            new Rectangle(0, 0, corralTex.Width, corralTex.Height),
            new Rectangle(0, 0, corralTex.Width, corralTex.Height),
            3, forceFieldTexHorizontal, forceFieldTexVertical);

    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here
        
        player.Update(gameTime, slimeList, followCamera.position, 
            new Vector2(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight));
        
        followCamera.FollowTarget(player.destinationRectangle,
            new Vector2(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight));
        
        if (slimeList != null)
            foreach (var slime in slimeList)
            {
                slime.Update(gameTime);
            }
        
        corral.Update(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // TODO: Add your drawing code here
        
        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

        if (slimeList != null)
            foreach (var slime in slimeList)
            {
                slime.Draw(_spriteBatch, followCamera.position);
            }
        
        player.Draw(_spriteBatch, followCamera.position);
        
        corral.Draw(_spriteBatch, followCamera.position);
        foreach (var fence in corral.forceFields)
        {
            fence.Draw(_spriteBatch, followCamera.position);
        }
        
        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
