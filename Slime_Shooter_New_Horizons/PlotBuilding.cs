using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Slime_Shooter_New_Horizons;

public class PlotBuilding : Building
{
    private Player player;
    private PopUpWindow plotMenu;
    
    public PlotTypes PlotTypes;
    private Rectangle destRec, sourceRec;
    private Texture2D menuBG;
    private SpriteFont font;
    private float scaleMultiplier;

    private Rectangle interactiveRec;
    private bool isPlayerColliding;
    private bool plotMenuOpened;
    
    public PlotBuilding(Texture2D texture, Rectangle destinationRectangle, Rectangle sourceRectangle,
        float scaleMultiplier, Player player, Texture2D menuBG, SpriteFont font) : base(texture, destinationRectangle, sourceRectangle, scaleMultiplier)
    {
        destRec = destinationRectangle;
        sourceRec = sourceRectangle;
        this.menuBG = menuBG;
        this.font = font;
        this.scaleMultiplier = scaleMultiplier;
        this.player = player;
        PlotTypes = PlotTypes.Empty;
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
                Console.WriteLine("Plot menu opened");
                plotMenuOpened = true;
                player.canMove = false;
                OpenPlotMenu(screenRes);
            }
            else if (plotMenuOpened && keyboardState.IsKeyDown(Keys.Escape))
            {
                Console.WriteLine("Plot menu closed");
                plotMenuOpened = false;
                player.canMove = true;
                ClosePlotMenu();
            }
            
        }
        else
            isPlayerColliding = false;

        plotMenu?.Update(gameTime);
    }

    private void CreateInteractiveRec()
    {
        interactiveRec = new Rectangle((int)(destinationRectangle.X - 20 * scaleMultiplier), 
            (int)(destinationRectangle.Y + 80 * scaleMultiplier),
            (int)(26 * scaleMultiplier), (int)(15 * scaleMultiplier));
    }

    private void OpenPlotMenu(Vector2 screenRes)
    {
        plotMenu = new PopUpWindow(new Rectangle((int)screenRes.X, (int)screenRes.Y, 
            (int)(300 * scaleMultiplier), (int)(200 * scaleMultiplier)), menuBG, font);
    }

    private void ClosePlotMenu()
    {
        plotMenu = null;
    }

    public void CreateCorral()
    {
        
    }
}