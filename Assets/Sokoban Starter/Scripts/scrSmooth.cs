using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class scrSmooth : MonoBehaviour
{
    private GridObject gridObject;

    public bool forceChain;
    
    // Start is called before the first frame update
    void Start()
    {
        gridObject = GetComponent<GridObject>();
        if (gridObject == null)
        {
            Debug.LogError("GridMovement requires a GridObject component on the same GameObject.");
        }
        
        forceChain = false;
    }

    // Update is called once per frame
    void Update()
    {
        //forceChain = false;
    }

    public void PushedBy(Vector2Int force_source, Vector2Int direction)
    {
        
        if (scrManager.OutOfBound(gridObject.gridPosition + direction))
        {
            return;
        }
        
        //if the force at force_source is in force chain...
        if (scrManager.CheckForInForceChainAtPosition(force_source))
        {
            forceChain = true; //in force chain
        }
        
        //if it is something at the direction it is going
        //if it is smooth
        if (scrManager.CheckForObjectAtGridPosition(gridObject.gridPosition + direction, "smooth"))
        {
            scrManager.PushObjWithSmooths(gridObject.gridPosition, direction); //new
        }

        //if sticky is there
        Vector2Int[] directions = { Vector2Int.down, Vector2Int.left, Vector2Int.up, Vector2Int.right };
        foreach (Vector2Int dir in directions)
        {
            
            //if there is a sticky in the dir going
            
            if (scrManager.CheckForObjectAtGridPosition(gridObject.gridPosition + dir, "sticky"))
            {
                print("scrsmooth: there is a sticky at "+ gridObject.gridPosition + dir);
                //if it is not the sticky right next to
                if (!scrManager.CheckForInForceChainAtPosition(force_source)) //were a ! here: If it is not the source of force, move sticky!!
                {
                    scrManager.StickObjWithStickies(gridObject.gridPosition, direction); //1.Called sticky? Didn't reach here
                }

                else if (scrManager.CheckForInForceChainAtPosition(force_source) &&
                    scrManager.CheckForInForceChainAtPosition(gridObject.gridPosition - direction))
                {
                    scrManager.StickObjWithStickies(gridObject.gridPosition, direction);
                }
            }
        }
        
        //If nothing on up, move
        if (scrManager.CheckForAnyAtGridPosition(gridObject.gridPosition + direction))
        {
            forceChain = false;
            return;
        }
        
        scrManager.Move(direction, gridObject); //moved here
        forceChain = false;
    }
}
