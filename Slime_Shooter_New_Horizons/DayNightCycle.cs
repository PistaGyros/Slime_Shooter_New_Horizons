using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Slime_Shooter_New_Horizons;

public class DayNightCycle
{
    public double elapsedTime;
    public int days;
    private Texture2D globalLightFakeTexture;
    private Vector2 screenRes;
    
    public DayNight dayOrNight;
    // Alpha value
    private float currentGlobalLightIntensity = 0;
    
    
    public DayNightCycle(Texture2D globalLightFakeTexture)
    {
        this.globalLightFakeTexture = globalLightFakeTexture;
    }

    public new void Update(GameTime gameTime, Vector2 screenRes)
    {
        this.screenRes = screenRes;
        elapsedTime += gameTime.ElapsedGameTime.TotalSeconds * 60;
        if (elapsedTime > 86400)
        {
            elapsedTime = 0;
            days++;
        }
        
        DayOrNight();
    }

    public new void Draw(SpriteBatch spriteBatch)
    {
        Color nightColor = new Color(Color.MidnightBlue, currentGlobalLightIntensity);
        spriteBatch.Draw(
            globalLightFakeTexture, 
            new Rectangle(-(int)screenRes.X, -(int)screenRes.Y, (int)(screenRes.X * 100), (int)(screenRes.Y * 100)),
            new Rectangle(0, 0, 1, 1), 
            nightColor);
    }

    private void DayOrNight()
    {
        switch (elapsedTime)
        {
            case >= 18000 and <= 72000:
                dayOrNight = DayNight.Day;
                currentGlobalLightIntensity = 0f;       
                break;
            case < 18000 or > 72000:
                dayOrNight = DayNight.Night;
                currentGlobalLightIntensity = 0.5f;
                break;
        }
    }
}