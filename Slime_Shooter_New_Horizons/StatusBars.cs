using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Slime_Shooter_New_Horizons;

public class StatusBars : UI
{
    private Vector2 screenRes;
    public int CoinsStatus;
    public float StaminaStatus;
    public int HealthStatus;
    private int actualTime;
    private int days;
    
    private Vector2 position;
    private Texture2D backGroundTexture, forGroundTexture, coinTex;
    private SpriteFont font;
    
    public StatusBars(Vector2 position, Texture2D backGroundTexture, Texture2D forGroundTexture, SpriteFont font,
        Texture2D coinTex) :
        base(position, backGroundTexture, forGroundTexture, font)
    {
        this.position = position;
        this.backGroundTexture = backGroundTexture;
        this.forGroundTexture = forGroundTexture;
        this.font = font;
        this.coinTex = coinTex;
        ChangeSizeScale(3);
    }

    public new void Update(GameTime gameTime, Vector2 screenRes, int CoinsStatus, int StaminaStatus, int HealthStatus, 
        int actualTime, int days)
    {
        this.screenRes = screenRes;
        this.CoinsStatus = CoinsStatus;
        this.StaminaStatus = StaminaStatus;
        this.HealthStatus = HealthStatus;
        this.actualTime = actualTime;
        this.days = days;
    }

    public new void Draw(SpriteBatch spriteBatch)
    {
        DrawCoins(spriteBatch);
        DrawStamina(spriteBatch);
        DrawTime(spriteBatch);
    }

    private void DrawCoins(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(coinTex, 
            new Rectangle((int)position.X, (int)position.Y, coinTex.Width * SizeScaler, coinTex.Height * SizeScaler),
            new Rectangle(0, 0, coinTex.Width, coinTex.Height), Color.White);
        Vector2 coinStringPos = new Vector2(position.X + coinTex.Width * SizeScaler + 5 * SizeScaler, 
            position.Y + coinTex.Height / 3 * SizeScaler);
        spriteBatch.DrawString(font, CoinsStatus.ToString(), coinStringPos, Color.Black);
    }

    private void DrawStamina(SpriteBatch spriteBatch)
    {
        Vector2 staminaBarPos = new Vector2(position.X, position.Y + 15 * SizeScaler);
        spriteBatch.Draw(forGroundTexture,
            new Rectangle((int)staminaBarPos.X, (int)staminaBarPos.Y, 
                (int)(StaminaStatus / 100 * 50 * SizeScaler), 10 * SizeScaler),
            new Rectangle(0, 0, 1, 1), Color.White);
        Vector2 staminaStringPos = new Vector2(staminaBarPos.X, staminaBarPos.Y + 2.5f * SizeScaler);
        spriteBatch.DrawString(font, StaminaStatus.ToString(), staminaStringPos, Color.White);
    }

    private void DrawHealth(SpriteBatch spriteBatch)
    {
        
    }

    private void DrawTime(SpriteBatch spriteBatch)
    {
        int minutes = actualTime % 3600 / 60;
        int hours = actualTime / 3600;
        string timeString = "Time: " + hours.ToString() + ":" + minutes.ToString();
        Vector2 timeTextPos = new Vector2(screenRes.X - 50 * SizeScaler, 50 * SizeScaler);
        spriteBatch.DrawString(font, timeString, timeTextPos, Color.Black);
        string daysString = "Day  " + days;
        Vector2 daysTextPos = new Vector2(screenRes.X - 25 * SizeScaler, timeTextPos.Y);
        spriteBatch.DrawString(font, daysString, daysTextPos, Color.Black);
    }
}