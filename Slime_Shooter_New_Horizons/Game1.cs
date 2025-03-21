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

    
    // UI related
    private Texture2D itemsAtlas;
    private Texture2D inventoryTex;
    private Texture2D colliderTexture;
    private List<Texture2D> listTextures;
    private List<string> itemsNames;
    private Dictionary<int, string> itemsID;
    private List<Rectangle> itemsIDTexturesRec;
    
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
            "Deep Nothingness", "Pink Slime", "Rock Slime", "Tabby Slime", "Fosfor Slime", "Honey Slime", "Slivka",
            "Yahoda", "Mrkva", "Paradayka"
        };
        itemsID = new Dictionary<int, string>();
        for (int i = 0; i < itemsNames.Count; i++)
        {
            itemsID[i] = itemsNames[i];
        }

        itemsIDTexturesRec = new List<Rectangle>()
        {
            new Rectangle(0, 0, 22, 22), new Rectangle(25, 0, 16, 11), 
            new Rectangle(47, 0, 16, 15), new Rectangle(67, 0, 20, 14),
            new Rectangle(88, 0, 22, 13), new Rectangle(113, 0, 16, 11),
            new Rectangle(140, 0, 6, 8), new Rectangle(161, 0, 8, 9),
            new Rectangle(184, 0, 6, 12), new Rectangle(205, 0, 8, 9)
        };
        
        slimeList = new List<Slime>();

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // TODO: use this.Content to load your game content here

        listTextures = new List<Texture2D>()
        {
            null, 
            Content.Load<Texture2D>("pink_slime_new_spritesheet"),
            Content.Load<Texture2D>("rock_slime_new_spritesheet"),
            Content.Load<Texture2D>("tabby_slime_new_spritesheet"),
            Content.Load<Texture2D>("phospor_slime_new_spritesheet"),
            Content.Load<Texture2D>("honey_slime_new_spritesheet"),
            Content.Load<Texture2D>("slivka_fruit"),
            Content.Load<Texture2D>("yahoda_fruit"),
            Content.Load<Texture2D>("mrkva_veggie"),
            Content.Load<Texture2D>("paradayka_veggie")
        };
        
        colliderTexture = Content.Load<Texture2D>("collider_texture");
        inventoryTex = Content.Load<Texture2D>("inventory");
        itemsAtlas = Content.Load<Texture2D>("items_atlas");
        
        
        Texture2D playerTexture = Content.Load<Texture2D>("spr_player_1_left_idle");
        player = new Player(playerTexture, new Rectangle(0, 0, playerTexture.Width * 5, playerTexture.Height * 5), 
            new Rectangle(0, 0, playerTexture.Width, playerTexture.Height), 
            1, new Vector2(playerTexture.Width * 5, playerTexture.Height * 5), colliderTexture);
        SpriteFont uiFont = Content.Load<SpriteFont>("Bell MT");
        player.CreateInventory(screenRes, itemsAtlas, inventoryTex, uiFont, itemsID, itemsIDTexturesRec);
        player.slimeTextures = listTextures;
        

        Texture2D corralTex = Content.Load<Texture2D>("corral_deactivated");
        Texture2D forceFieldTexHorizontal = Content.Load<Texture2D>("force_field_corral_prototype_anim");
        Texture2D forceFieldTexVertical = Content.Load<Texture2D>("force_field_corral_prototype_anim_vertical");
        corral = new Corral(corralTex,
            new Rectangle(0, 0, corralTex.Width, corralTex.Height),
            new Rectangle(0, 0, corralTex.Width, corralTex.Height),
            3, colliderTexture, forceFieldTexHorizontal, forceFieldTexVertical);

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
                slime.Update(gameTime, player.destinationRectangle, slimeList);
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
        
        corral.Draw(_spriteBatch, followCamera.position);
        
        player.Draw(_spriteBatch, followCamera.position);
        
        foreach (var fence in corral.forceFields)
        {
            fence.Draw(_spriteBatch, followCamera.position);
        }
        
        player.inventory.Draw(_spriteBatch, screenRes);
        
        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
