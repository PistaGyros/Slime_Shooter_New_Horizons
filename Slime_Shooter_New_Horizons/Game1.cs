using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.OpenGL;

namespace Slime_Shooter_New_Horizons;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private Vector2 screenRes = new (1920, 1080);

    
    // UI related
    private Texture2D itemsAtlas;
    private Texture2D inventoryTex;
    private Texture2D colliderTexture;
    private Texture2D tileMapAtlas;
    private List<Texture2D> listTextures;
    private Texture2D UI_texture;
    private Texture2D coinTex;
    private List<string> itemsNames;
    private Dictionary<int, string> itemsID;
    private List<Rectangle> itemsIDTexturesRec;
    private List<Rectangle> textureTileStore;
    private List<Vector2> objectsColSize;
    private List<List<List<Rectangle>>> objectsColRecs;
    private List<int> sellPrices;
    private List<int> purchasePlotFacilitiesPrices;
    
    private List<Slime> slimeList;
    private List<Plort> plortsList;
    private List<FruitVeggie> fruitsVeggiesList;
    private List<PlotBuilding> plotsList;
    private List<Corral> corralsList;

    private FollowCamera followCamera;
    private TileMap tileMap;
    private Player player;
    private PlortCollector plortSellPoint;
    private DayNightCycle dayNightCycle;

    // TODO: Adjust drawing so it does work properly
    private Triangle triangle;

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

        //triangle = new Triangle(800, 450, 1000, 650, 1000, 250);

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

        textureTileStore = new()
        {
            new Rectangle(0, 0, 16, 16), new Rectangle(16, 0, 16, 16)
        };

        // Price for which can player sold an item based on their objectID in the plot seller
        sellPrices = new List<int>()
        {
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 10, 22, 22, 22, 45
        };

        purchasePlotFacilitiesPrices = new()
        {
            250, 250
        };

        
        // Lists for collider offsets, it goes by objectID then animation row and then specific frame from the row
        objectsColRecs = new List<List<List<Rectangle>>>()
        {
            // Player colliders
            new List<List<Rectangle>>
            {
                new List<Rectangle>()
                {
                    new Rectangle(12, 8, 8, 32), new Rectangle(12, 8, 8, 32),
                    new Rectangle(12, 8, 8, 32), new Rectangle(12, 8, 8, 32),
                    new Rectangle(12, 8, 8, 32), new Rectangle(12, 8, 8, 32)
                },
                new List<Rectangle>()
                {
                    new Rectangle(12, 8, 8, 32), new Rectangle(12, 8, 8, 32),
                    new Rectangle(12, 8, 8, 32), new Rectangle(12, 8, 8, 32),
                    new Rectangle(12, 8, 8, 32), new Rectangle(12, 8, 8, 32)
                },
                new List<Rectangle>()
                {
                    new Rectangle(12, 8, 8, 32), new Rectangle(12, 8, 8, 32),
                    new Rectangle(12, 8, 8, 32), new Rectangle(12, 8, 8, 32),
                    new Rectangle(12, 8, 8, 32), new Rectangle(12, 8, 8, 32)
                },
                new List<Rectangle>()
                {
                    new Rectangle(12, 8, 8, 32), new Rectangle(12, 8, 8, 32),
                    new Rectangle(12, 8, 8, 32), new Rectangle(12, 8, 8, 32),
                    new Rectangle(12, 8, 8, 32), new Rectangle(12, 8, 8, 32)
                }
            },
            
            // TODO: Update objects colliders rectangles according to their animated version
            // Pink Slime object colliders
            new List<List<Rectangle>>()
            {
                new List<Rectangle>()
                {   
                    new Rectangle(3, 11, 16, 11), new Rectangle(3, 10, 16, 11),
                    new Rectangle(4, 9, 14, 12), new Rectangle(3, 12, 16, 10), 
                    new Rectangle(2, 13, 18, 9), new Rectangle(3, 12, 16, 10)
                },
                new List<Rectangle>()
                {
                    new Rectangle(3, 11, 16, 11), new Rectangle(3, 11, 16, 11),
                    new Rectangle(3, 11, 16, 11), new Rectangle(3, 11, 16, 11), 
                    new Rectangle(3, 11, 16, 11), new Rectangle(3, 11, 16, 11)
                },
                new List<Rectangle>()
                {
                    new Rectangle(3, 11, 16, 11), new Rectangle(3, 11, 16, 11),
                    new Rectangle(3, 11, 16, 11), new Rectangle(3, 11, 16, 11), 
                    new Rectangle(3, 11, 16, 11), new Rectangle(3, 11, 16, 11)
                },
                new List<Rectangle>()
                {
                    new Rectangle(3, 11, 16, 11), new Rectangle(3, 11, 16, 11),
                    new Rectangle(3, 11, 16, 11), new Rectangle(3, 11, 16, 11), 
                    new Rectangle(3, 11, 16, 11), new Rectangle(3, 11, 16, 11)
                }
            },
            
            // Rock Slime object colliders
            new List<List<Rectangle>>()
            {
                new List<Rectangle>()
                {
                    new Rectangle(3, 7, 16, 15), new Rectangle(3, 6, 16, 15),
                    new Rectangle(4, 6, 14, 15), new Rectangle(3, 8, 16, 14), 
                    new Rectangle(2, 9, 18, 13), new Rectangle(3, 8, 16, 14)
                    
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
                    new Rectangle(1, 8, 20, 14), new Rectangle(1, 7, 20, 14),
                    new Rectangle(2, 6, 19, 16), new Rectangle(1, 9, 20, 13),
                    new Rectangle(1, 10, 20, 12), new Rectangle(1, 9, 20, 13)
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
                    new Rectangle(0, 9, 22, 13), new Rectangle(0, 8, 22, 13),
                    new Rectangle(0, 7, 22, 13), new Rectangle(0, 6, 22, 13),
                    new Rectangle(0, 7, 22, 13), new Rectangle(0, 8, 22, 13)
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
                new List<Rectangle>()
                {   
                    new Rectangle(3, 11, 16, 11), new Rectangle(3, 10, 16, 11),
                    new Rectangle(4, 9, 14, 12), new Rectangle(3, 12, 16, 10), 
                    new Rectangle(2, 13, 18, 9), new Rectangle(3, 12, 16, 10)
                },
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
        plotsList = new();
        corralsList = new();

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
            // Plorts
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
        
        colliderTexture = Content.Load<Texture2D>("green_square");
        inventoryTex = Content.Load<Texture2D>("inventory");
        itemsAtlas = Content.Load<Texture2D>("items_atlas");
        UI_texture = Content.Load<Texture2D>("UI_tex");
        coinTex = Content.Load<Texture2D>("coin_tex");
        
        
        tileMapAtlas = Content.Load<Texture2D>("tileMapTexture");
        tileMap = new TileMap("Data/tileMap.csv", textureTileStore, tileMapAtlas, 16, 3);
        
        
        Texture2D playerTexture = Content.Load<Texture2D>("bea_walking_spritesheet");
        Vector2 playerSize = new Vector2(32, 40);
        player = new Player(playerTexture, new Rectangle(100, 100, (int)playerSize.X, (int)playerSize.Y), 
            new Rectangle(0, 0, (int)playerSize.X, (int)playerSize.Y), 
            3, objectsColRecs);
        player.SetupAnimator(6, 6, playerSize);
        SpriteFont uiFont = Content.Load<SpriteFont>("Bell MT");
        player.CreateInventory(screenRes, itemsAtlas, inventoryTex, uiFont, itemsID, itemsIDTexturesRec);
        player.CreateStatusBar(screenRes, UI_texture, UI_texture, coinTex, uiFont);
        player.objectsTextures = listTextures;
        player.SetLists(slimeList, plortsList, fruitsVeggiesList);
        player.plots = plotsList;
        
        
        // Init of plort collectors/sell point
        Texture2D plortCollectorTex = Content.Load<Texture2D>("plort_collector");
        plortSellPoint = new PlortCollector(plortCollectorTex,
            new Rectangle(100, 0, plortCollectorTex.Width, plortCollectorTex.Height),
            new Rectangle(0, 0, plortCollectorTex.Width, plortCollectorTex.Height), 3);
        plortSellPoint.player = player;
        plortSellPoint.plortsList = plortsList;
        plortSellPoint.sellPrices = sellPrices;

        Texture2D plotTex = Content.Load<Texture2D>("empty_plot");
        for (int i = 0; i < 5; i++)
        {
            PlotBuilding plot = new PlotBuilding(plotTex,
                new Rectangle(0 + 1000 * i, 500, plotTex.Width, plotTex.Height),
                new Rectangle(0, 0, plotTex.Width, plotTex.Height),
                3, player, UI_texture, uiFont, Content, corralsList);
            plot.purchasePlotFacilitiesPrices = purchasePlotFacilitiesPrices;
            plotsList.Add(plot);
        }

        dayNightCycle = new DayNightCycle(UI_texture);
        dayNightCycle.elapsedTime = 18000;

        //triangle.CreateCollider(GraphicsDevice, colliderTexture);
        //triangle.scaleMultiplier = 1;
        
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            Exit();

        // TODO: Add your update logic here
        
        dayNightCycle.Update(gameTime, screenRes);
        
        player.Update(gameTime, screenRes, (int)dayNightCycle.elapsedTime, dayNightCycle.days);
        
        followCamera.FollowTarget(player.destinationRectangle, screenRes);
        
        tileMap.Update(gameTime);
        
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

        if (plotsList != null)
        {
            foreach (var plot in plotsList)
            {
                plot.Update(gameTime, screenRes);
            }
        }

        if (corralsList != null)
        {
            foreach (var corral in corralsList)
            {
                corral.Update(gameTime);
            }
        }
        
        plortSellPoint.Update(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // TODO: Add your drawing code here
        
        _spriteBatch.Begin(samplerState: SamplerState.PointClamp, blendState: BlendState.NonPremultiplied);

        tileMap.Draw(_spriteBatch, followCamera.position);
        
        
        plortSellPoint.Draw(_spriteBatch, followCamera.position);
        plortSellPoint.DrawCollisionRec(_spriteBatch, colliderTexture, followCamera.position, plortSellPoint.GetCollectorRectangle());
        
        if (slimeList != null)
            foreach (var slime in slimeList)
            {
                slime.DrawCollisionRec(_spriteBatch, colliderTexture, followCamera.position, slime.GetCollisionRectangle());
                slime.Draw(_spriteBatch, followCamera.position);
            }

        if (plortsList != null)
        {
            foreach (var plort in plortsList)
            {
                plort.DrawCollisionRec(_spriteBatch, colliderTexture, followCamera.position, plort.GetCollisionRectangle());
                plort.Draw(_spriteBatch, followCamera.position);
            }
        }

        if (fruitsVeggiesList != null)
        {
            foreach (var fruitVeggie in fruitsVeggiesList)
            {
                fruitVeggie.DrawCollisionRec(_spriteBatch, colliderTexture, followCamera.position, fruitVeggie.GetCollisionRectangle());
                fruitVeggie.Draw(_spriteBatch, followCamera.position);
            }
        }

        if (plotsList != null)
        {
            foreach (var plot in plotsList)
            {
                plot.Draw(_spriteBatch, followCamera.position);
                plot.DrawCollisionRec(_spriteBatch, colliderTexture, followCamera.position, plot.interactiveRec);
                plot.plotMenu?.Draw(_spriteBatch);
            }
        }
        
        if (corralsList != null)
        {
            foreach (var corral in corralsList)
            {
                corral.Draw(_spriteBatch, followCamera.position);
                foreach (var fence in corral.forceFields)
                {
                    fence.DrawCollisionRec(_spriteBatch, colliderTexture, followCamera.position, fence.GetCollisionRectangle());
                    fence.Draw(_spriteBatch, followCamera.position);
                }
            }
        }
        
        player.Draw(_spriteBatch, followCamera.position);
        player.DrawCollisionRec(_spriteBatch, colliderTexture, followCamera.position, player.GetCollisionRectangle());
        if (player.interactRec != Rectangle.Empty)
            player.DrawCollisionRec(_spriteBatch, colliderTexture, followCamera.position, player.interactRec);
        if (player.VacuumConeRecs != null)
            foreach (var rec in player.VacuumConeRecs)
            {
                player.DrawCollisionRec(_spriteBatch, colliderTexture, followCamera.position, rec);   
            }
        
        dayNightCycle.Draw(_spriteBatch);
        
        
        // Draw player UI
        player.inventory.Draw(_spriteBatch, screenRes);
        player.StatusBarsUi.Draw(_spriteBatch);
        
        // Draw plot UI menu
        if (plotsList != null)
        {
            foreach (var plot in plotsList)
            {
                plot.plotMenu?.Draw(_spriteBatch);
            }
        }
        
        //triangle.DrawCollider(_spriteBatch, followCamera.position);
        
        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
