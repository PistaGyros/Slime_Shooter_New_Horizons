﻿using System;
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
    
    private Rectangle backGroundPosition, forGroundPosition;
    private Texture2D backGroundTexture, atlasItemsTexture;
    private int itemTexSize = 22;
    private int inventorySlotsSize = 4;
    private Dictionary<string, int> itemsID;
    private Dictionary<int, Rectangle> itemsIDTextures = new Dictionary<int, Rectangle>();

    private List<List<int>> inventorySlots = new List<List<int>>();
    private int activeSlot = 0;
    
    
    public Inventory(Rectangle backGroundRec, Rectangle forGroundPosition, Texture2D backGroundTexture, Texture2D forGroundTexture,
        Dictionary<string, int> itemsID) : base(backGroundRec, backGroundTexture, forGroundTexture)
    {
        this.backGroundPosition = backGroundRec;
        this.forGroundPosition = forGroundPosition;
        this.backGroundTexture = backGroundTexture;
        this.atlasItemsTexture = forGroundTexture;
        this.itemsID = itemsID;
        AssignTexturesToIDs();
        CreateInventory();
    }

    public void ChangeSizeScale(int sizeScaler)
    {
        this.sizeScaler = sizeScaler;
    }

    public new virtual void Draw(SpriteBatch spriteBatch)
    {
        DrawBg(spriteBatch);
    }
    
    private void AssignTexturesToIDs()
    {
        int id = 0;
        for (int y = 0; y < atlasItemsTexture.Height / itemTexSize; y++)
        {
            for (int x = 0; x < atlasItemsTexture.Width / itemTexSize; x++)
            {
                itemsIDTextures.Add(id, new Rectangle(x * itemTexSize, y * itemTexSize, itemTexSize, itemTexSize));
                id++;
            }
        }
    }

    // Create blank inventory, list with 4 slots, each slot has and itemID and amount of that item
    private void CreateInventory()
    {
        for (int i = 0; i < inventorySlotsSize; i++)
        {
            List<int> inventorySlot = new List<int>();
            inventorySlot.Add(4); // itemID
            inventorySlot.Add(1); // amount
            inventorySlots.Add(inventorySlot);
        }
    }
    
    public void UpdateInventory(int inventoryPosition, int inventoryItemID, int inventoryItemAmount)
    {
        List<int> newInventorySlot = new List<int>();
        newInventorySlot.Add(inventoryItemID);
        newInventorySlot.Add(inventoryItemAmount);
        inventorySlots[inventoryPosition] = newInventorySlot;
    }

    public void ChangeActiveSlot(int newActiveSlot)
    {
        activeSlot = newActiveSlot - 1;
    }

    private void DrawBg(SpriteBatch spriteBatch)
    {
        for (int i = 0; i < inventorySlotsSize; i++)
        {
            float activeSlotScale = activeSlot == i ? 1.1f : 1;
            Vector2 position = new Vector2(
                backGroundPosition.X + offset * i -(backGroundTexture.Width * i * sizeScaler * activeSlotScale - backGroundTexture.Width * i * sizeScaler) + backGroundTexture.Width * i * sizeScaler * activeSlotScale, backGroundPosition.Y);
            Rectangle newRec = new Rectangle((int)position.X, (int)position.Y, 
                (int)(backGroundTexture.Width * sizeScaler * activeSlotScale), 
                (int)(backGroundTexture.Height * sizeScaler * activeSlotScale));
            spriteBatch.Draw(backGroundTexture, newRec, Color.White);
            DrawItems(spriteBatch, i, activeSlotScale, position);
        }
    }
    
    private void DrawItems(SpriteBatch spriteBatch, int i, float activeSlotScale, Vector2 bgPosition)
    {
        Vector2 position = new Vector2(
            bgPosition.X + backGroundTexture.Width * sizeScaler * activeSlotScale / 2 - itemTexSize * sizeScaler * activeSlotScale / 2,
            forGroundPosition.Y);
        Rectangle newRec = new Rectangle((int)position.X, (int)position.Y, 
            (int)(itemTexSize * sizeScaler * activeSlotScale), (int)(itemTexSize * sizeScaler * activeSlotScale));
        spriteBatch.Draw(atlasItemsTexture, newRec, itemsIDTextures[inventorySlots[i][0]], Color.White);
    }
}