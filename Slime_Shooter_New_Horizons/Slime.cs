using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Slime_Shooter_New_Horizons;

public class Slime(
    Texture2D texture,
    Rectangle destinationRectangle,
    Rectangle sourceRectangle,
    List<List<Rectangle>> objectsColRecs,
    float scaleMultiplier)
    : Animator(texture, destinationRectangle, sourceRectangle, objectsColRecs, scaleMultiplier)
{
    public int slimeID;
    private bool isCollidingWithSlime = false;
    public List<Texture2D> plortsTexs = new List<Texture2D>();

    public int Hunger = 30;
    public bool hasCollidedWithFood;
    private bool isEating = false;
    private float eatingTime = 2.0f;
    private float eatingTimer = 0.0f;
    private FruitVeggie food;


    public void ThrowSlime(int quadrantSpawned)
    {
        Throw(destinationRectangle);
        isThrowed = true;
        initQuadrant = quadrantSpawned;
        velocity *= DecideWhatinitQuadrant(quadrantSpawned);
    }
    

    public new void Update(GameTime gameTime, Rectangle playerRec, List<Slime> slimeList, List<Plort> plortsList,
        List<FruitVeggie> fruitVeggieList)
    {
        Hunger += (int)gameTime.ElapsedGameTime.TotalSeconds;
        Rectangle collidedSlimeRec = new();
        UpdateAnimator(gameTime);
        var outputOfSlimesChecking = CheckForCollisionsWithSlimes(slimeList);
        var outputOfFoodChecking = CheckForCollisionsWithFood(fruitVeggieList);
        
        if (outputOfSlimesChecking.Item1)
        {
            IsVacuumed = false;
            isThrowed = false;
            isCollidingWithSlime = true;
            collidedSlimeRec = outputOfSlimesChecking.Item2;
        }
        else
        {
            isCollidingWithSlime = false;
        }

        if (!hasCollidedWithFood && Hunger >= 30)
        {

            if (outputOfFoodChecking.Item1)
            {
                hasCollidedWithFood = true;
                food = outputOfFoodChecking.Item2;
                CatchFood();
            }
            else
            {
                hasCollidedWithFood = false;
            }
        }

        if (isEating)
        {
            CatchFood();
            eatingTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (eatingTimer >= eatingTime)
            {
                EatFood(plortsList, fruitVeggieList);
            }
        }
        

        if (isCollidingWithSlime)
        {
            BounceAwayFromSlime(collidedSlimeRec);
        }

        else if (!isCollidingWithSlime)
        {
            UpdateSprite(gameTime, playerRec);
        }
    }

    private void BounceAwayFromSlime(Rectangle badSlime)
    {
        Vector2 centerDestRec = new Vector2(GetCollisionRectangle().X + GetCollisionRectangle().Width / 2,
            GetCollisionRectangle().Y + GetCollisionRectangle().Height / 2);
        Vector2 centerBadSlimeRec = new Vector2(badSlime.X + badSlime.Width / 2, badSlime.Y + badSlime.Height / 2);
        Vector2 pointVec = new Vector2(centerDestRec.X - centerBadSlimeRec.X, centerDestRec.Y - centerBadSlimeRec.Y);
        destinationRectangle.X += (int)pointVec.X;
        destinationRectangle.Y += (int)pointVec.Y;
    }
    
    public new (bool, Rectangle) CheckForCollisionsWithSlimes(List<Slime> slimeList)
    {
        bool collision = false;
        Rectangle collidedRectangle = new Rectangle();
        foreach (var slime in slimeList)
        {
            if (this != slime)
                if(GetCollisionRectangle().Intersects(slime.GetCollisionRectangle()))
                {
                    collision = true;
                    collidedRectangle = slime.GetCollisionRectangle();
                    slime.isThrowed = false;
                }
        }

        return (collision, collidedRectangle);
    }

    public new (bool, FruitVeggie) CheckForCollisionsWithFood(List<FruitVeggie> fruitVeggieList)
    {
        bool collision = false;
        FruitVeggie collidedFood = null;
        foreach (var fruitVeggie in fruitVeggieList)
        {
            if (GetCollisionRectangle().Contains(fruitVeggie.GetCollisionRectangle()))
            {
                collision = true;
                collidedFood = fruitVeggie;
            }

            collidedFood = fruitVeggie;
        }
        return (collision, collidedFood);
    }

    private void CatchFood()
    {
        food.isThrowed = false;
        food.IsVacuumed = false;
        Rectangle slimeCollRec = GetCollisionRectangle();
        isEating = true;
        food.destinationRectangle.X = slimeCollRec.X + slimeCollRec.Width / 2 - food.destinationRectangle.Width / 2;
        food.destinationRectangle.Y = slimeCollRec.Y + slimeCollRec.Height / 2 - food.destinationRectangle.Height / 2;
        Console.WriteLine(food.destinationRectangle);
    }

    private void EatFood(List<Plort> plortsList, List<FruitVeggie> fruitVeggiesList)
    {
        Hunger = 0;
        isEating = false;
        hasCollidedWithFood = false;
        fruitVeggiesList.Remove(food);
        DropPlort(slimeID + 11, plortsList);
    }

    private void DropPlort(int plortID, List<Plort> plortsList)
    {
        Texture2D plortTexture = plortsTexs[plortID];
        Plort droppedPlort = new Plort(plortID, plortTexture,
            new Rectangle(destinationRectangle.X + destinationRectangle.Width / 2, destinationRectangle.Y, 
                plortTexture.Width, plortTexture.Height),
            new Rectangle(0, 0, plortTexture.Width, plortTexture.Height), 2);
        Random rnd = new Random();
        droppedPlort.ThrowPlort(rnd.Next(1, 5));
        plortsList.Add(droppedPlort);
    }
}