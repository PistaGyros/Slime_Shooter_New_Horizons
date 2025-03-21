using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Slime_Shooter_New_Horizons;

public class Inventory : UI
{
    public int sizeScaler = 3;
    private int offset = 10;

    private SpriteFont font;
    private Rectangle backGroundPosition, forGroundPosition;
    private Texture2D backGroundTexture, atlasItemsTexture;
    public int InventorySlotsSize = 4;
    public int MaximumSlotAmount = 30;
    private Dictionary<int, string> itemsNames;
    private List<Rectangle> itemsIDTexturesRec;

    public List<List<int>> inventorySlots = new List<List<int>>();
    public List<Rectangle> slotsRectangles = new List<Rectangle>();
    public int activeSlot = 0;
    public bool IsSlotAvailable;
    
    
    public Inventory(Rectangle backGroundRec, Rectangle forGroundPosition, Texture2D backGroundTexture, Texture2D forGroundTexture,
        SpriteFont font, Dictionary<int, string> itemsNames, List<Rectangle> itemsIDTexturesRec) : base(backGroundRec, backGroundTexture, forGroundTexture, font)
    {
        this.backGroundPosition = backGroundRec;
        this.forGroundPosition = forGroundPosition;
        this.backGroundTexture = backGroundTexture;
        this.atlasItemsTexture = forGroundTexture;
        this.font = font;
        this.itemsNames = itemsNames;
        this.itemsIDTexturesRec = itemsIDTexturesRec;
        CreateInventory();
    }

    public void ChangeSizeScale(int sizeScaler)
    {
        this.sizeScaler = sizeScaler;
    }

    public new virtual void Draw(SpriteBatch spriteBatch, Vector2 screenRes)
    {
        DrawBg(spriteBatch, screenRes);
    }

    // Create blank inventory, list with 4 slots, each slot has and itemID and amount of that item
    private void CreateInventory()
    {
        for (int i = 0; i < InventorySlotsSize; i++)
        {
            List<int> inventorySlot = new List<int>();
            inventorySlot.Add(i + 1); // itemID
            inventorySlot.Add(10); // amount
            inventorySlots.Add(inventorySlot);
            slotsRectangles.Add(new Rectangle());
        }
    }
    
    public void UpdateInventory(int inventoryPosition, int inventoryItemID, int inventoryChange)
    {
        inventorySlots[inventoryPosition][0] = inventoryItemID;
        inventorySlots[inventoryPosition][1] += inventoryChange;
    }

    public int WhichSlotIsAvailable(int itemID)
    {
        // Find if the item is not already in the inventory, if not ...
        for (int i = 0; i < InventorySlotsSize; i++)
        {
            if (inventorySlots[i][0] == itemID)
            {
                // Slot i is available
                return i;
            }
        }
        // ... then find empty slot and if there is no available slot, then ...
        for (int i = 0; i < InventorySlotsSize; i++)
        {
            if (inventorySlots[i][1] == 0)
            {
                // Slot i is available
                return i;
            }
        }
        // ... secret slot (there is no slot 69 [unfortunately], it's a condition)
        return 69;
    }

    public void ChangeActiveSlot(int newActiveSlot)
    {
        activeSlot = newActiveSlot - 1;
    }

    private void DrawBg(SpriteBatch spriteBatch, Vector2 screenRes)
    {
        for (int i = 0; i < InventorySlotsSize; i++)
        {
            float activeSlotScale = activeSlot == i ? 1.1f : 1;
            Vector2 position = new Vector2(
                backGroundPosition.X + offset * i -(backGroundTexture.Width * i * sizeScaler * activeSlotScale - backGroundTexture.Width * i * sizeScaler) + backGroundTexture.Width * i * sizeScaler * activeSlotScale, backGroundPosition.Y);
            Rectangle newRec = new Rectangle((int)position.X, (int)position.Y, 
                (int)(backGroundTexture.Width * sizeScaler * activeSlotScale), 
                (int)(backGroundTexture.Height * sizeScaler * activeSlotScale));
            slotsRectangles.Insert(i, newRec);
            spriteBatch.Draw(backGroundTexture, newRec, Color.White);
            DrawItem(spriteBatch, i, activeSlotScale, position);
        }
        DrawItemName(spriteBatch, inventorySlots[activeSlot][0], screenRes);
    }
    
    private void DrawItem(SpriteBatch spriteBatch, int i, float activeSlotScale, Vector2 bgPosition)
    {
        Vector2 position = new Vector2(
            bgPosition.X + backGroundTexture.Width * sizeScaler * activeSlotScale / 2 - itemsIDTexturesRec[inventorySlots[i][0]].Width * sizeScaler * activeSlotScale / 2,
            bgPosition.Y + backGroundTexture.Height * sizeScaler * activeSlotScale / 2 - itemsIDTexturesRec[inventorySlots[i][0]].Height * sizeScaler * activeSlotScale / 2);
        Rectangle newRec = new Rectangle((int)position.X, (int)position.Y, 
            (int)(itemsIDTexturesRec[inventorySlots[i][0]].Width * sizeScaler * activeSlotScale), (int)(itemsIDTexturesRec[inventorySlots[i][0]].Height * sizeScaler * activeSlotScale));
        if (inventorySlots[i][1] >= 1)
        {
            spriteBatch.Draw(atlasItemsTexture, newRec, itemsIDTexturesRec[inventorySlots[i][0]], Color.White);
            DrawNums(spriteBatch, inventorySlots[i][1], bgPosition, activeSlotScale);
        }
    }

    private void DrawNums(SpriteBatch spriteBatch, int amountOfItems, Vector2 bgPosition, float activeSlotScale)
    {
        int padding = 10;
        Vector2 numPosition = new Vector2(bgPosition.X + backGroundTexture.Width * sizeScaler * activeSlotScale / 2 + padding, 
            bgPosition.Y);
        spriteBatch.DrawString(font, amountOfItems.ToString(), numPosition, Color.Black);
    }

    private void DrawItemName(SpriteBatch spriteBatch, int itemID, Vector2 screenRes)
    {
        Vector2 nameTextPos = new Vector2(screenRes.X / 2 - itemsNames[itemID].Length, screenRes.Y - 50 * sizeScaler);
        spriteBatch.DrawString(font, itemsNames[itemID], nameTextPos, Color.Black);
    }
}