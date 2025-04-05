using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Slime_Shooter_New_Horizons;

public class PlotBuilding : Building
{
    private ContentManager content;
    private Player player;
    public Corral plotsCorral;
    private List<Corral> corralsList;
    public PlotMenuDefault plotMenu;
    public List<int> purchasePlotFacilitiesPrices;

    private Rectangle destRec, sourceRec;
    private Texture2D menuBG;
    private SpriteFont font;
    private float scaleMultiplier;

    private Rectangle interactiveRec;
    private bool isPlayerColliding;
    private bool plotMenuOpened;
    public bool buyBtnPressed;

    public PlotTypes PlotTypes;
    public CorralUpgrades CorralUpgrades;
    public GardenUpgrades GardenUpgrades;

    public event EventHandler NotEnoughMoneyEvent;

    public PlotBuilding(Texture2D texture, Rectangle destinationRectangle, Rectangle sourceRectangle,
        float scaleMultiplier, Player player, Texture2D menuBG, SpriteFont font, ContentManager content, 
        List<Corral> corralsList) :
        base(texture, destinationRectangle, sourceRectangle, scaleMultiplier)
    {
        destRec = destinationRectangle;
        sourceRec = sourceRectangle;
        this.menuBG = menuBG;
        this.font = font;
        this.scaleMultiplier = scaleMultiplier;
        this.content = content;
        this.corralsList = corralsList;
        this.player = player;
        PlotTypes = PlotTypes.Empty;
        CorralUpgrades = CorralUpgrades.None;
        GardenUpgrades = GardenUpgrades.None;
        CreateInteractiveRec();
    }

    public new void Update(GameTime gameTime, Vector2 screenRes)
    {
        KeyboardState keyboardState = Keyboard.GetState();
        if (interactiveRec.Intersects(player.interactRec))
        {
            isPlayerColliding = true;
            if (!plotMenuOpened && keyboardState.IsKeyDown(Keys.E))
            {
                OpenPlotMenu(screenRes);
            }
            else if (plotMenuOpened && keyboardState.IsKeyDown(Keys.Escape))
            {
                ClosePlotMenu();
            }

        }
        else
            isPlayerColliding = false;

        plotMenu?.Update(gameTime);
    }

    private void CreateInteractiveRec()
    {
        interactiveRec = new Rectangle(
            (int)(destinationRectangle.X - 20 * scaleMultiplier),
            (int)(destinationRectangle.Y + 80 * scaleMultiplier),
            (int)(26 * scaleMultiplier),
            (int)(15 * scaleMultiplier)
        );
    }

    private void OpenPlotMenu(Vector2 screenRes)
    {
        Console.WriteLine("Plot menu opened");
        plotMenuOpened = true;
        player.canMove = false;

        Vector2 plotMenuSize = new Vector2(300 * scaleMultiplier, 200 * scaleMultiplier);
        plotMenu = new PlotMenuDefault(new Rectangle(
                (int)(screenRes.X / 2 - plotMenuSize.X / 2),
                (int)(screenRes.Y / 2 - plotMenuSize.Y / 2),
                (int)(plotMenuSize.X), (int)(plotMenuSize.Y)),
            menuBG, font, this, PlotTypes);
    }

    public void ClosePlotMenu()
    {
        Console.WriteLine("Plot menu closed");
        plotMenuOpened = false;
        player.slimeShootTimer = 1f;
        player.canMove = true;
        plotMenu = null;
    }
    

public void PurchaseBtnWasPressed(int selectedFacility)
    {
        switch (PlotTypes)
        {
            case PlotTypes.Empty:
                if (!buyBtnPressed)
                {
                    if (player.coins >= purchasePlotFacilitiesPrices[selectedFacility])
                    {
                        switch (selectedFacility)
                        {
                            case 0:
                                CreateCorral();
                                PlotTypes = PlotTypes.Corral;
                                break;
                            case 1:
                                CreateGarden();
                                PlotTypes = PlotTypes.Garden;
                                break;
                        }
                        player.coins -= purchasePlotFacilitiesPrices[selectedFacility];
                        ClosePlotMenu();
                    }
                    else if (player.coins < purchasePlotFacilitiesPrices[selectedFacility])
                    {
                        NotEnoughMoneyEvent?.Invoke(this, EventArgs.Empty);
                    }
                }

                break;
            case PlotTypes.Corral:
                break;
            case PlotTypes.Garden:
                break;
        }
    }

    private void CreateCorral()
    {
        Texture2D corralTex = content.Load<Texture2D>("corral_deactivated");
        Texture2D forceFieldTexHorizontal = content.Load<Texture2D>("force_field_corral_prototype_anim");
        Texture2D forceFieldTexVertical = content.Load<Texture2D>("force_field_corral_prototype_vertical_anim");
        plotsCorral = new Corral(corralTex,
            new Rectangle((int)(destRec.X + 6 * scaleMultiplier), (int)(destRec.Y - 37 * scaleMultiplier),
                corralTex.Width, corralTex.Height),
            new Rectangle(0, 0, 
                corralTex.Width, corralTex.Height),
            3, forceFieldTexHorizontal, forceFieldTexVertical);
        corralsList.Add(plotsCorral);
    }

    private void CreateGarden()
    {
        
    }
}