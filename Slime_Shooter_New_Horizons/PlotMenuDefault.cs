using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Slime_Shooter_New_Horizons;

public class PlotMenuDefault : UI
{
    private PlotBuilding itsPlot;
    
    private Rectangle bgRec;
    private Texture2D bgTex;
    private SpriteFont font;

    private Rectangle exitBtnRec, buyBtnRec, leftArrowKeyRec, rightArrowKeyRec, facilityPictureRec;
    private List<string> facilitiesNames = new()
    {
        "Corral",
        "Garden",
        "Pond",
        "Storage"
    };
    private int selectedFacility = 0;
    private const int numAllAvailableFacilities = 2;
    private bool errorMessageEventInvoked = false;
    
    private PlotMenu plotMenu;
    private PlotTypes itsPlotType;
    
    public PlotMenuDefault(Rectangle bgRectangle, Texture2D backGroundTexture, SpriteFont font, PlotBuilding itsPlot, 
        PlotTypes itsPlotType)
    : base(bgRectangle, backGroundTexture, font)
    {
        bgRec = bgRectangle;
        bgTex = backGroundTexture;
        this.font = font;
        this.itsPlot = itsPlot;
        this.itsPlotType = itsPlotType;
        this.itsPlot.NotEnoughMoneyEvent += ItsPlotOnNotEnoughMoneyEvent;
        InitUIRecs();
    }
    
    private void ItsPlotOnNotEnoughMoneyEvent(object sender, EventArgs e)
    {
        errorMessageEventInvoked = true;
    }
    
    private void InitUIRecs()
    {
        Vector2 exitBtnSize = new Vector2(14, 14);
        exitBtnRec = new Rectangle((int)(bgRec.X + bgRec.Width - exitBtnSize.X * SizeScaler), 
            bgRec.Y, 
            (int)(exitBtnSize.X * SizeScaler), (int)(exitBtnSize.Y * SizeScaler));
        Vector2 buyBtnSize = new Vector2(40, 20);
        buyBtnRec = new Rectangle(
            (int)(bgRec.X + (float)bgRec.Width / 2 - buyBtnSize.X * SizeScaler / 2 + 10),
            bgRec.Y + bgRec.Height - 50 * SizeScaler, 
            (int)buyBtnSize.X * SizeScaler, (int)buyBtnSize.Y * SizeScaler);
        Vector2 arrowKeySize = new Vector2(4, 8);
        leftArrowKeyRec = new((int)(buyBtnRec.X - 10 * SizeScaler - arrowKeySize.X * SizeScaler),
            buyBtnRec.Y, (int)arrowKeySize.X * SizeScaler, (int)arrowKeySize.Y * SizeScaler);
        rightArrowKeyRec = new((int)(buyBtnRec.X + buyBtnRec.Width + 10 * SizeScaler),
            buyBtnRec.Y, (int)arrowKeySize.X * SizeScaler, (int)arrowKeySize.Y * SizeScaler);
        Vector2 pictureSize = new(8, 8);
        facilityPictureRec = new(
            (int)(bgRec.X + (float)bgRec.Width / 2 - pictureSize.X * 4 * SizeScaler / 2),
            bgRec.Y + 50 * SizeScaler, 
            (int)(pictureSize.X * 4 * SizeScaler), (int)(pictureSize.Y * 4f * SizeScaler)
            );
    }

    public new void Update(GameTime gameTime)
    {
        itsPlotType = itsPlot.PlotTypes;
        
        // Check for mouse clicking
        MouseState mouseState = Mouse.GetState();
        Rectangle mouseClickRec = new Rectangle(mouseState.X, mouseState.Y, 1, 1);
        if (mouseState.LeftButton == ButtonState.Pressed)
        {
            errorMessageEventInvoked = false;
            if (exitBtnRec.Contains(mouseClickRec))
                itsPlot.ClosePlotMenu();
            else if (buyBtnRec.Contains(mouseClickRec))
            {
                itsPlot.PurchaseBtnWasPressed(selectedFacility);
                itsPlot.buyBtnPressed = true;
            }
            else if (leftArrowKeyRec.Contains(mouseClickRec))
                selectedFacility = selectedFacility > 0 ? selectedFacility - 1 : selectedFacility;
            else if (rightArrowKeyRec.Contains(mouseClickRec))
                selectedFacility = selectedFacility < numAllAvailableFacilities - 1 ? selectedFacility + 1 : selectedFacility;
        }
        else if (mouseState.LeftButton == ButtonState.Released)
            itsPlot.buyBtnPressed = false;
    }

    public new void Draw(SpriteBatch spriteBatch)
    {
        // Draw bg
        spriteBatch.Draw(bgTex, bgRec, new Rectangle(2, 0, 1, 1), Color.White);
        
        // Draw exit button
        DrawButtons(spriteBatch);

        switch (itsPlotType)
        {
            case PlotTypes.Empty:
                // Draw facility
                DrawFacilitiesMenu(spriteBatch);
                if (errorMessageEventInvoked)
                    DrawErrorMessage(spriteBatch);
                break;
            case PlotTypes.Corral:
                
                break;
        }
    }

    private void DrawButtons(SpriteBatch spriteBatch)
    {
        // Exit button
        spriteBatch.Draw(bgTex, exitBtnRec, new Rectangle(3, 0, 7, 7), Color.White);
        
        // Buy button
        spriteBatch.Draw(bgTex, buyBtnRec, new Rectangle(4, 0, 1, 1), Color.White);
        spriteBatch.Draw(bgTex, 
            new Rectangle(buyBtnRec.X + 2 * SizeScaler, buyBtnRec.Y + 2 * SizeScaler, 
                buyBtnRec.Width - 4 * SizeScaler, buyBtnRec.Height - 4 * SizeScaler), 
            new Rectangle(2, 0, 1, 1), Color.White);
        string text = "Purchase";
        Vector2 textPos = new Vector2(buyBtnRec.X + (float)buyBtnRec.Width / 2 - font.MeasureString(text).X / 2, 
            buyBtnRec.Y + (float)buyBtnRec.Height / 2);
        spriteBatch.DrawString(font, text, textPos, Color.Black);
        
        // Left Right keys
        spriteBatch.Draw(bgTex, leftArrowKeyRec, new Rectangle(10, 0, 4, 8), Color.White);
        spriteBatch.Draw(bgTex, rightArrowKeyRec, new Rectangle(10, 0, 4, 8), Color.White,
            0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);
    }

    private void DrawFacilitiesMenu(SpriteBatch spriteBatch)
    {
        // Draw text
        string text = facilitiesNames[selectedFacility];
        Vector2 textPos = new(
            facilityPictureRec.X + (float)facilityPictureRec.Width / 2 - font.MeasureString(text).X / 2,
            facilityPictureRec.Y - 10 * SizeScaler);
        spriteBatch.DrawString(font, text, textPos, Color.Black);
        switch (selectedFacility) {
            
            case 0:
                // Draw corral
                spriteBatch.Draw(bgTex, facilityPictureRec, new Rectangle(14, 0, 8, 8), Color.White);
                break;
            case 1:
                // Draw garden
                spriteBatch.Draw(bgTex, facilityPictureRec, new Rectangle(22, 0, 8, 8), Color.White);
                break;
            case 2:
                // Draw pond
                break;
            case 3:
                // Draw storage
                break;
        }
    }

    private void DrawCorralUpgradeMenu()
    {
        
    }

    private void DrawErrorMessage(SpriteBatch spriteBatch)
    {
        string errorText = "Not enough money";
        Vector2 textPos = new Vector2(buyBtnRec.X + (float)buyBtnRec.Width / 2 - font.MeasureString(errorText).X / 2,
            buyBtnRec.Y - 10 * SizeScaler);
        spriteBatch.DrawString(font, errorText, textPos, Color.Red);
    }
}