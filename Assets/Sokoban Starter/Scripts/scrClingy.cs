using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrClingy : MonoBehaviour
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

    public void PulledBy(Vector2Int force_source, Vector2Int direction)
    {
        
        if (scrManager.OutOfBound(gridObject.gridPosition + direction))
        {
            return;
        }

        if (scrManager.CheckForInForceChainAtPosition(gridObject.gridPosition + direction + direction))
        {
            forceChain = true;
            
            //if nothing in front NEW ADD 3:51
            if (scrManager.CheckForAnyAtGridPosition(gridObject.gridPosition + direction))
            {
                forceChain = false;
                return;
            }
            
            print(direction + " is the dir to pull clingy");
            
            scrManager.Move(direction, gridObject);
            
            //do sth to ori
            //if origin's has a clingy
            if (scrManager.CheckForObjectAtGridPosition(gridObject.gridPosition - direction - direction, "clingy"))
            {
                print("there's clingy at the origin");
                scrManager.PullObjWithClingys(force_source - direction, direction);
            }
            
            //if near origin has a sticky
            Vector2Int[] directions = { Vector2Int.down, Vector2Int.left, Vector2Int.up, Vector2Int.right };

            foreach (Vector2Int dir in directions)
            {
                if (scrManager.CheckForObjectAtGridPosition(gridObject.gridPosition - direction + dir, "sticky"))
                {
                    print("there's a sticky at origin at " + (gridObject.gridPosition - direction + dir));
                    print(scrManager.CheckForInForceChainAtPosition(force_source));
                    
                    if (scrManager.CheckForInForceChainAtPosition(force_source))
                    {
                        print("sticky at " + (gridObject.gridPosition - direction + dir) + " tried to move " + direction);
                        scrManager.StickObjWithStickies(gridObject.gridPosition - direction + dir, direction);
                    }
                }
            }
            
            forceChain = false;
        }
    }
}
