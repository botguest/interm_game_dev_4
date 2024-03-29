using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class scrManager : MonoBehaviour
{
    
    public static List<scrSticky> FindAllStickiesNearPosition(Vector2Int position)
    {
        List<scrSticky> foundStickies = new List<scrSticky>();
        
        // Get all scrSticky instances in the scene
        scrSticky[] allStickies = FindObjectsOfType<scrSticky>();

        // Define the relative positions to check (up, down, left, right)
        Vector2Int[] directions = new Vector2Int[]
        {
            new Vector2Int(0, 1), // Up
            new Vector2Int(0, -1), // Down
            new Vector2Int(-1, 0), // Left
            new Vector2Int(1, 0)  // Right
        };

        // Iterate through all scrSticky instances
        foreach (scrSticky sticky in allStickies)
        {
            Vector2Int stickyPosition = sticky.GetComponent<GridObject>().gridPosition;

            // Check if the sticky's position is one of the four directions from the specified position
            foreach (Vector2Int dir in directions)
            {
                if (position + dir == stickyPosition)
                {
                    foundStickies.Add(sticky);
                }
            }
        }

        return foundStickies;
    }
    

    public static bool CheckForInForceChainAtPosition(Vector2Int position)
    {
        if (CheckForAnyAtGridPosition(position) == false)
        { 
            //debugging
            print("Nuthing here" + position + ", not even blocks");
            
            return false;
        }

        //there is something at the position. Now check if the position is in force chain
        GridObject[] allGridObjects = FindObjectsOfType<GridObject>(); //here!!
        foreach (GridObject obj in allGridObjects)
        {
            if (obj.GetComponent<scrPlayer>() != null && obj.gridPosition == position)
            {
                if (obj.GetComponent<scrPlayer>().forceChain)
                {
                    print("hay player fc at " + position);
                    return true;
                }
            }
            
            else if (obj.GetComponent<scrSmooth>() != null && obj.gridPosition == position)
            {
                if (obj.GetComponent<scrSmooth>().forceChain)
                {
                    print("hay smooth fc at " + position);
                    return true;
                }
            }
            
            else if (obj.GetComponent<scrClingy>() != null && obj.gridPosition == position)
            {
                if (obj.GetComponent<scrClingy>().forceChain)
                {
                    print("hay clingy fc at " + position);
                    return true;
                }
            }
            
            else if (obj.GetComponent<scrSticky>() != null && obj.gridPosition == position)
            {
                if (obj.GetComponent<scrSticky>().forceChain)
                {
                    print("hay sticky fc at " + position);
                    return true;
                }
            }
        }
        
        print("no obj around " + position + " has force chain");
        return false;
    }
    
    public static bool CheckForObjectAtGridPosition(Vector2Int position, string tag)
    {
        // Find all objects of type GridObject
        GridObject[] allGridObjects = FindObjectsOfType<GridObject>();
        
        foreach (GridObject obj in allGridObjects)
        {
            // Check if the object's grid position matches the target position and has the specified tag
            if (obj.gridPosition == position && obj.CompareTag(tag))
            {
                print("hay " + tag + " at " + position);
                return true;
            }
            
        }
        return false; 
    }

    public static bool CheckForAnyAtGridPosition(Vector2Int position)
    {
        if (CheckForObjectAtGridPosition(position, "wall"))
        {
            return true;
        }
        
        if (CheckForObjectAtGridPosition(position, "sticky"))
        {
            return true;
        }
        
        if (CheckForObjectAtGridPosition(position, "smooth"))
        {
            return true;
        }
        
        if (CheckForObjectAtGridPosition(position, "player"))
        {
            return true;
        }
        
        if (CheckForObjectAtGridPosition(position, "clingy"))
        {
            return true;
        }
        
        return false; 
    }

    //New Version!
    public static bool PushObjWithSmooths(Vector2Int force_source, Vector2Int direction)
    {
        scrSmooth[] objWithScrSmooths = FindObjectsOfType<scrSmooth>();
        foreach (scrSmooth objSmooth in objWithScrSmooths)
        {
            objSmooth.PushedBy(force_source, direction);
        }

        return true;
    }

    public static bool PullObjWithClingys(Vector2Int force_source, Vector2Int direction)
    {
        scrClingy[] objWithScrClingy = FindObjectsOfType<scrClingy>();
        foreach (scrClingy objClingy in objWithScrClingy)
        {
            objClingy.PulledBy(force_source, direction);
        }

        return true;
    }

    public static bool StickObjWithStickies(Vector2Int force_source, Vector2Int direction)
    {
        scrSticky[] objWithScrSticky = FindObjectsOfType<scrSticky>();
        foreach (scrSticky objSticky in objWithScrSticky)
        {
            objSticky.StickedBy(force_source, direction);
        }
        return true;
    }

    public static void Move(Vector2Int move_direction, GridObject gridObject)
    {
        gridObject.gridPosition = gridObject.gridPosition + move_direction;
    }

    public static bool OutOfBound(Vector2Int position)
    {
        if (position.x > GridMaker.reference.dimensions.x ||
            position.x < 1 ||
            position.y > GridMaker.reference.dimensions.y ||
            position.y < 1)
        {
            return true;
        }

        return false;
    }
}
