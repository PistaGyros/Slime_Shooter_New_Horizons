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
    private List<Vector2> objectsColSize;
    private List<List<List<Vector2>>> animationOffSets;
    private List<List<List<Rectangle>>> objectsColRecs;
    
    private List<Slime> slimeList;
    private List<Plort> plortsList;
    private List<FruitVeggie> fruitsVeggiesList;
    private List<int> sellPrices;

    private FollowCamera followCamera;

    private Player player;
    private Corral corral;
    private PlortCollector plortSellPoint;

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
            // Slimes
            "Deep Nothingness", "Pink Slime", "Rock Slime", "Tabby Slime", "Fosfor Slime", "Honey Slime", "", "", "", "", "",
            // Plorts
            "", "Pink Plort", "Rock Plort", "Tabby Plort", "Fosfor Plort", "Honey Plort", "", "", "", "",
            // Fruits and veggies
            "", "Slivka", "Yahoda", "Mrkva", "Paradayka"
        };
        itemsID = new Dictionary<int, string>();
        for (int i = 0; i < itemsNames.Count; i++)
        {
            itemsID[i] = itemsNames[i];
        }

        itemsIDTexturesRec = new List<Rectangle>()
        {
            new Rectangle(0, 0, 22, 22), 
            // Slimes
            new Rectangle(25, 0, 16, 11), new Rectangle(47, 0, 16, 15), 
            new Rectangle(67, 0, 20, 14), new Rectangle(88, 0, 22, 13), 
            new Rectangle(113, 0, 16, 11), new Rectangle(), new Rectangle(),new Rectangle(),new Rectangle(),
            new Rectangle(),
            // Plorts
            new Rectangle(), new Rectangle(28, 22, 10, 10), new Rectangle(51, 22, 10, 10), 
            new Rectangle(72, 22, 10, 10), new Rectangle(94, 22, 10, 10), new Rectangle(116, 22, 10, 10), 
            new Rectangle(), new Rectangle(), new Rectangle(), new Rectangle(),
            // Fruits and vegies
            new Rectangle(), new Rectangle(30, 44, 6, 8), new Rectangle(51, 44, 8, 9),
            new Rectangle(74, 44, 6, 12), new Rectangle(95, 44, 8, 9)
        };

        sellPrices = new List<int>()
        {
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 10, 22, 22, 22, 45
        };

        
        // Lists for collider offsets, it goes by objectID then animation row and then specific frame from the row
        objectsColRecs = new List<List<List<Rectangle>>>()
        {
            // Empty object col
            new List<List<Rectangle>>(){new List<Rectangle>(){new Rectangle()}},
            // Pink Slime object colliders
            new List<List<Rectangle>>()
            {
                new List<Rectangle>(){new Rectangle(3, 11, 16, 11), new Rectangle(3, 11, 16, 11),
                    new Rectangle(3, 11, 16, 11), new Rectangle(3, 11, 16, 11), 
                    new Rectangle(3, 11, 16, 11), new Rectangle(3, 11, 16, 11)},
                new List<Rectangle>(){new Rectangle(3, 11, 16, 11), new Rectangle(3, 11, 16, 11),
                    new Rectangle(3, 11, 16, 11), new Rectangle(3, 11, 16, 11), 
                    new Rectangle(3, 11, 16, 11), new Rectangle(3, 11, 16, 11)},
                new List<Rectangle>(){new Rectangle(3, 11, 16, 11), new Rectangle(3, 11, 16, 11),
                    new Rectangle(3, 11, 16, 11), new Rectangle(3, 11, 16, 11), 
                    new Rectangle(3, 11, 16, 11), new Rectangle(3, 11, 16, 11)},
                new List<Rectangle>(){new Rectangle(3, 11, 16, 11), new Rectangle(3, 11, 16, 11),
                    new Rectangle(3, 11, 16, 11), new Rectangle(3, 11, 16, 11), 
                    new Rectangle(3, 11, 16, 11), new Rectangle(3, 11, 16, 11)}
            },
            
            // Rock Slime object colliders
            new List<List<Rectangle>>()
            {
                new List<Rectangle>()
                {
                    new Rectangle(3, 7, 16, 15), new Rectangle(3, 7, 16, 15),
                    new Rectangle(3, 7, 16, 15), new Rectangle(3, 7, 16, 15), 
                    new Rectangle(3, 7, 16, 15), new Rectangle(3, 7, 16, 15)
                    
                },
                new List<Rectangle>()
                {
                    new Rectangle(3, 7, 16, 15), new Rectangle(3, 7, 16, 15),
                    new Rectangle(3, 7, 16, 15), new Rectangle(3, 7, 16, 15), 
                    new Rectangle(3, 7, 16, 15), new Rectangle(3, 7, 16, 15)
                    
                },
                new List<Rectangle>()
                {
                    new Rectangle(3, 7, 16, 15), new Rectangle(3, 7, 16, 15),
                    new Rectangle(3, 7, 16, 15), new Rectangle(3, 7, 16, 15), 
                    new Rectangle(3, 7, 16, 15), new Rectangle(3, 7, 16, 15)
                    
                },
                new List<Rectangle>()
                {
                    new Rectangle(3, 7, 16, 15), new Rectangle(3, 7, 16, 15),
                    new Rectangle(3, 7, 16, 15), new Rectangle(3, 7, 16, 15), 
                    new Rectangle(3, 7, 16, 15), new Rectangle(3, 7, 16, 15)
                    
                }
            },
            
            // Tabby Slime object colliders
            new List<List<Rectangle>>()
            {
                new List<Rectangle>()
                {
                    new Rectangle(1, 8, 20, 14), new Rectangle(1, 8, 20, 14),
                    new Rectangle(1, 8, 20, 14), new Rectangle(1, 8, 20, 14),
                    new Rectangle(1, 8, 20, 14), new Rectangle(1, 8, 20, 14)
                },
                new List<Rectangle>()
                {
                    new Rectangle(1, 8, 20, 14), new Rectangle(1, 8, 20, 14),
                    new Rectangle(1, 8, 20, 14), new Rectangle(1, 8, 20, 14),
                    new Rectangle(1, 8, 20, 14), new Rectangle(1, 8, 20, 14)
                },
                new List<Rectangle>()
                {
                    new Rectangle(1, 8, 20, 14), new Rectangle(1, 8, 20, 14),
                    new Rectangle(1, 8, 20, 14), new Rectangle(1, 8, 20, 14),
                    new Rectangle(1, 8, 20, 14), new Rectangle(1, 8, 20, 14)
                },
                new List<Rectangle>()
                {
                    new Rectangle(1, 8, 20, 14), new Rectangle(1, 8, 20, 14),
                    new Rectangle(1, 8, 20, 14), new Rectangle(1, 8, 20, 14),
                    new Rectangle(1, 8, 20, 14), new Rectangle(1, 8, 20, 14)
                }
            },
            
            // Fosfor Slime object colliders
            new List<List<Rectangle>>()
            {
                new List<Rectangle>()
                {
                    new Rectangle(0, 9, 22, 13), new Rectangle(0, 9, 22, 13),
                    new Rectangle(0, 9, 22, 13), new Rectangle(0, 9, 22, 13),
                    new Rectangle(0, 9, 22, 13), new Rectangle(0, 9, 22, 13)
                },
                new List<Rectangle>()
                {
                    new Rectangle(0, 9, 22, 13), new Rectangle(0, 9, 22, 13),
                    new Rectangle(0, 9, 22, 13), new Rectangle(0, 9, 22, 13),
                    new Rectangle(0, 9, 22, 13), new Rectangle(0, 9, 22, 13)
                },
                new List<Rectangle>()
                {
                    new Rectangle(0, 9, 22, 13), new Rectangle(0, 9, 22, 13),
                    new Rectangle(0, 9, 22, 13), new Rectangle(0, 9, 22, 13),
                    new Rectangle(0, 9, 22, 13), new Rectangle(0, 9, 22, 13)
                },
                new List<Rectangle>()
                {
                    new Rectangle(0, 9, 22, 13), new Rectangle(0, 9, 22, 13),
                    new Rectangle(0, 9, 22, 13), new Rectangle(0, 9, 22, 13),
                    new Rectangle(0, 9, 22, 13), new Rectangle(0, 9, 22, 13)
                }
            },
            
            // Honey Slime object colliders
            new List<List<Rectangle>>()
            {
                new List<Rectangle>(){new Rectangle(3, 11, 16, 11), new Rectangle(3, 11, 16, 11),
                    new Rectangle(3, 11, 16, 11), new Rectangle(3, 11, 16, 11), 
                    new Rectangle(3, 11, 16, 11), new Rectangle(3, 11, 16, 11)},
                new List<Rectangle>(){new Rectangle(3, 11, 16, 11), new Rectangle(3, 11, 16, 11),
                    new Rectangle(3, 11, 16, 11), new Rectangle(3, 11, 16, 11), 
                    new Rectangle(3, 11, 16, 11), new Rectangle(3, 11, 16, 11)},
                new List<Rectangle>(){new Rectangle(3, 11, 16, 11), new Rectangle(3, 11, 16, 11),
                    new Rectangle(3, 11, 16, 11), new Rectangle(3, 11, 16, 11), 
                    new Rectangle(3, 11, 16, 11), new Rectangle(3, 11, 16, 11)},
                new List<Rectangle>(){new Rectangle(3, 11, 16, 11), new Rectangle(3, 11, 16, 11),
                    new Rectangle(3, 11, 16, 11), new Rectangle(3, 11, 16, 11), 
                    new Rectangle(3, 11, 16, 11), new Rectangle(3, 11, 16, 11)}
            }
        };

        
        slimeList = new List<Slime>();
        plortsList = new List<Plort>();
        fruitsVeggiesList = new List<FruitVeggie>();

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // TODO: use this.Content to load your game content here

        listTextures = new List<Texture2D>()
        {
            null, 
            // Slimes
            Content.Load<Texture2D>("pink_slime_new_spritesheet"),
            Content.Load<Texture2D>("rock_slime_new_spritesheet"),
            Content.Load<Texture2D>("tabby_slime_new_spritesheet"),
            Content.Load<Texture2D>("phospor_slime_new_spritesheet"),
            Content.Load<Texture2D>("honey_slime_new_spritesheet"),
            null, null, null, null, null, 
            //Plorts
            null,
            Content.Load<Texture2D>("pink_plort"),
            Content.Load<Texture2D>("rock_plort"),
            Content.Load<Texture2D>("tabby_plort"), 
            Content.Load<Texture2D>("phospor_plort"),
            Content.Load<Texture2D>("honey_plort"), null, null, null, null,
            // Fruits and veggies
            null,
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
            1, objectsColRecs);
        SpriteFont uiFont = Content.Load<SpriteFont>("Bell MT");
        player.CreateInventory(screenRes, itemsAtlas, inventoryTex, uiFont, itemsID, itemsIDTexturesRec);
        player.objectsTextures = listTextures;
        player.SetLists(slimeList, plortsList, fruitsVeggiesList);
        
        // Init of corral
        Texture2D corralTex = Content.Load<Texture2D>("corral_deactivated");
        Texture2D forceFieldTexHorizontal = Content.Load<Texture2D>("force_field_corral_prototype_anim");
        Texture2D forceFieldTexVertical = Content.Load<Texture2D>("force_field_corral_prototype_anim_vertical");
        corral = new Corral(corralTex,
            new Rectangle(0, 0, corralTex.Width, corralTex.Height),
            new Rectangle(0, 0, corralTex.Width, corralTex.Height),
            3, colliderTexture, forceFieldTexHorizontal, forceFieldTexVertical);
        
        // Init of plort collectors/sell point
        Texture2D plortCollectorTex = Content.Load<Texture2D>("plort_collector");
        plortSellPoint = new PlortCollector(plortCollectorTex,
            new Rectangle(50, -300, plortCollectorTex.Width, plortCollectorTex.Height),
            new Rectangle(0, 0, plortCollectorTex.Width, plortCollectorTex.Height), 3);
        plortSellPoint.player = player;
        plortSellPoint.plortsList = plortsList;
        plortSellPoint.sellPrices = sellPrices;
        
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here
        
        player.Update(gameTime, new Vector2(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight));
        
        followCamera.FollowTarget(player.GetCollisionRectangle(),
            new Vector2(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight));
        
        if (slimeList != null)
            foreach (var slime in slimeList)
            {
                slime.Update(gameTime, player.GetCollisionRectangle());
            }

        if (plortsList != null)
        {
            foreach (var plort in plortsList)
            {
                plort.Update(gameTime, player.GetCollisionRectangle());
            }
        }

        if (fruitsVeggiesList != null)
        {
            foreach (var fruitVeggie in fruitsVeggiesList)
            {
                fruitVeggie.Update(gameTime, player.GetCollisionRectangle());
            }
        }
        
        corral.Update(gameTime);
        plortSellPoint.Update(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // TODO: Add your drawing code here
        
        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

        plortSellPoint.Draw(_spriteBatch, followCamera.position);
        
        if (slimeList != null)
            foreach (var slime in slimeList)
            {
                slime.Draw(_spriteBatch, followCamera.position);
            }

        if (plortsList != null)
        {
            foreach (var plort in plortsList)
            {
                plort.Draw(_spriteBatch, followCamera.position);
            }
        }

        if (fruitsVeggiesList != null)
        {
            foreach (var fruitVeggie in fruitsVeggiesList)
            {
                fruitVeggie.Draw(_spriteBatch, followCamera.position);
            }
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
