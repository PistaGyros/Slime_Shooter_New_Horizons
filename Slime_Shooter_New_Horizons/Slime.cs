using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Slime_Shooter_New_Horizons;

public class Slime : Animator
{
    public int slimeID;
    private bool isCollidingWithSlime = false;
    public bool isCollidingWithFence;
    public bool CanBeVacuumed;
    private bool isWithinCorral;
    public List<Texture2D> plortsTexs = new List<Texture2D>();

    public int Hunger = 30;
    public bool hasCollidedWithFood;
    private bool isEating = false;
    private float eatingTime = 2.0f;
    private float eatingTimer = 0.0f;
    private FruitVeggie food;
    public List<PlotBuilding> plotsList;
    private Corral itsCorral = null;
        
    public SlimeOrientation ESlimeOrientation;
    public SlimeStatus ESlimeStatus;
    
    public Slime(int slimeID,
        Texture2D texture,
        Rectangle destinationRectangle,
        Rectangle sourceRectangle,
        List<List<Rectangle>> objectsColRecs,
        float scaleMultiplier) : base(texture, destinationRectangle, sourceRectangle, objectsColRecs, scaleMultiplier)
    {
        this.slimeID = slimeID;
    }
    

    public void ThrowSlime(int quadrantSpawned)
    {
        Throw(destinationRectangle);
        isThrowed = true;
        initQuadrant = quadrantSpawned;
        velocity *= DecideWhatinitQuadrant(quadrantSpawned);
    }
    

    public new void Update(GameTime gameTime, Rectangle playerRec)
    {
        Hunger += (int)gameTime.ElapsedGameTime.TotalSeconds;
        Rectangle collidedSlimeRec = new();
        UpdateAnimator(gameTime);
        var outputOfSlimesChecking = CheckForCollisionsWithSlimes();
        var outputOfFoodChecking = CheckForCollisionsWithFood();
        var outputOfCorralChecking = CheckForCollisionsWithCorral();
        
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

        if (!isCollidingWithFence && outputOfCorralChecking.Item1)
        {
            Console.WriteLine("Is entering corral");
            isCollidingWithFence = true;
            CanBeVacuumed = true;
            itsCorral ??= outputOfCorralChecking.Item2;
        }
        else if (isCollidingWithFence && !outputOfCorralChecking.Item1)
        {
            Console.WriteLine("Slime has entered corral");
            isCollidingWithFence = false;
            CanBeVacuumed = true;
            isWithinCorral = true;
        }

        if (isEating)
        {
            CatchFood();
            eatingTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (eatingTimer >= eatingTime)
            {
                EatFood();
            }
        }
        

        if (isCollidingWithSlime)
        {
            BounceAwayFromSlime(collidedSlimeRec);
        }

        else if (!isCollidingWithSlime)
        {
            UpdateSprite(gameTime, playerRec, destinationRectangle, GetCollisionRectangle());
        }
    }
    
    public new virtual void Draw(SpriteBatch spriteBatch, Vector2 offset)
    {
        Rectangle dest = new Rectangle(
            (int)offset.X + destinationRectangle.X,
            (int)offset.Y + destinationRectangle.Y,
            (int)(destinationRectangle.Width * scaleMultiplier),
            (int)(destinationRectangle.Height * scaleMultiplier));

        // TODO: Figure out enums for flipping slime
        SpriteEffects slimeOrientation = SpriteEffects.None;

        switch (ESlimeOrientation)
        {
            case SlimeOrientation.Left:
                spriteBatch.Draw(texture, dest, GetFrame(currentRow), Color.White,
                    0f, Vector2.Zero, SpriteEffects.None, 0);
                break;
            case SlimeOrientation.Right:
                spriteBatch.Draw(texture, dest, GetFrame(currentRow), Color.White,
                    0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0);
                break;
        }
    }

    public void ChangeActiveAnimRow(SlimeStatus newSlimeStatus)
    {
        ESlimeStatus = newSlimeStatus;
        switch (ESlimeStatus)
        {
            case SlimeStatus.Idle:
                currentRow = 0;
                break;
            case SlimeStatus.BeingVacuumed:
                currentRow = 1;
                break;
            case SlimeStatus.Jumping:
                currentRow = 2;
                break;
        }
    }

    public new void Vacuum(GameTime gameTime)
    {
        VacuumTime(gameTime);
        // Check for any sort of collision
        if (isCollidingWithFence && isWithinCorral && !isThrowed)
        {
            vacuumTime -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            IsVacuumed = false;
            BounceAwayFromFence();
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

    public void BounceAwayFromFence()
    {
        Vector2 centerDestRec = new Vector2(GetCollisionRectangle().X + GetCollisionRectangle().Width / 2,
            GetCollisionRectangle().Y + GetCollisionRectangle().Height / 2);
        Vector2 centerOfCorral = new Vector2(
            itsCorral.destinationRectangle.X + itsCorral.destinationRectangle.Width * itsCorral.scaleMultiplier / 2,
            itsCorral.destinationRectangle.Y + itsCorral.destinationRectangle.Height * itsCorral.scaleMultiplier / 2);
        Vector2 pointVec = new Vector2(centerOfCorral.X - centerDestRec.X, centerOfCorral.Y - centerDestRec.Y);
        destinationRectangle.X += (int)(pointVec.X / 125);
        destinationRectangle.Y += (int)(pointVec.Y / 125);
    }
    
    public new (bool, Rectangle) CheckForCollisionsWithSlimes()
    {
        bool collision = false;
        Rectangle collidedRectangle = new Rectangle();
        foreach (var slime in slimesList)
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

    public new (bool, FruitVeggie) CheckForCollisionsWithFood()
    {
        bool collision = false;
        FruitVeggie collidedFood = null;
        foreach (var fruitVeggie in fruitsVeggiesList)
        {
            if (GetCollisionRectangle().Intersects(fruitVeggie.GetCollisionRectangle()))
            {
                collision = true;
                collidedFood = fruitVeggie;
            }

            collidedFood = fruitVeggie;
        }
        return (collision, collidedFood);
    }
    
    public new (bool, Corral) CheckForCollisionsWithCorral()
    {
        bool collision = false;
        Corral corral = null;
        foreach (var plot in plotsList)
        {
            if (plot.plotsCorral != null)
            {
                foreach (var corralFence in plot.plotsCorral.forceFields)
                {
                    if(GetCollisionRectangle().Intersects(corralFence.GetCollisionRectangle()))
                    {
                        collision = true;
                        if (itsCorral == null)
                            corral = plot.plotsCorral;
                    }    
                }   
            }
        }
        return (collision, corral);
    }

    private void CatchFood()
    {
        food.isThrowed = false;
        food.IsVacuumed = false;
        Rectangle slimeCollRec = GetCollisionRectangle();
        isEating = true;
        food.destinationRectangle.X = slimeCollRec.X + slimeCollRec.Width / 2 - food.destinationRectangle.Width / 2;
        food.destinationRectangle.Y = slimeCollRec.Y + slimeCollRec.Height / 2 - food.destinationRectangle.Height / 2;
    }

    private void EatFood()
    {
        Hunger = 0;
        isEating = false;
        hasCollidedWithFood = false;
        fruitsVeggiesList.Remove(food);
        DropPlort(slimeID + 11);
    }

    private void DropPlort(int plortID)
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