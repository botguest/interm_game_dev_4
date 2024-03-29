using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrSticky : MonoBehaviour
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
        
    }

    public void StickedBy(Vector2Int force_source, Vector2Int direction) //this was called twice
    {

        bool push_buffer = false;
        Vector2Int push_buffer_source = new Vector2Int(0, 0);
        bool stick_buffer = false;

        if (scrManager.OutOfBound(gridObject.gridPosition + direction))
        {
            return;
        }
        
        Vector2Int[] directions = { Vector2Int.down, Vector2Int.left, Vector2Int.up, Vector2Int.right };

        foreach (Vector2Int dir in directions)
        {
            if (!scrManager.CheckForInForceChainAtPosition(gridObject.gridPosition + dir))
            {
                forceChain = true;

                if (scrManager.CheckForObjectAtGridPosition(gridObject.gridPosition + dir, "sticky") &&
                    (dir == direction))
                {
                    scrManager.StickObjWithStickies(gridObject.gridPosition, direction);
                }
                
                //if it is sticky
                else if (scrManager.CheckForObjectAtGridPosition(gridObject.gridPosition + dir, "sticky"))
                {
                    stick_buffer = true;
                    push_buffer_source = gridObject.gridPosition;
                    
                }

                if (scrManager.CheckForObjectAtGridPosition(gridObject.gridPosition + dir, "smooth") && (dir == direction))
                {
                    scrManager.PushObjWithSmooths(gridObject.gridPosition, direction);
                }
                
                //if it is smooth
                else if (scrManager.CheckForObjectAtGridPosition(gridObject.gridPosition + dir, "smooth"))
                {
                    print("push smooth invoked by sticky");
                    //simple patch
                    push_buffer = true;
                    push_buffer_source = gridObject.gridPosition;
                }
                
            }
        }

        //if nothing in the direction going, move itself.
        if (!scrManager.CheckForAnyAtGridPosition(gridObject.gridPosition + direction))
        {
            print("sticky moved "+ direction + " once");
            scrManager.Move(direction, gridObject); //this line was ran twice
            
            //if in behind the move has a clingy
            if (scrManager.CheckForObjectAtGridPosition(gridObject.gridPosition - direction - direction, "clingy"))
            {
                scrManager.PullObjWithClingys(gridObject.gridPosition - direction, direction);
            }

            //if in behind has a smooth.
            if (scrManager.CheckForObjectAtGridPosition(gridObject.gridPosition - direction - direction, "smooth"))
            {
                //called
                scrManager.PushObjWithSmooths(gridObject.gridPosition - direction, direction);
            }
            
            if (push_buffer && (push_buffer_source!=gridObject.gridPosition))
            {
                scrManager.PushObjWithSmooths(gridObject.gridPosition, direction); //need to do it again later.
            }

            if (stick_buffer && (push_buffer_source != gridObject.gridPosition))
            {
                scrManager.StickObjWithStickies(gridObject.gridPosition, direction);
            }
            
            forceChain = false;
            return;
        }

        print("sticky not moved");
        forceChain = false;
    }
}

