using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using Vector2Int = UnityEngine.Vector2Int;

public class scrPlayer : MonoBehaviour
{
    private GridObject gridObject;

    public bool forceChain;

    // Start is called before the first frame update
    void Start()
    {
        //testing events & listeners
        //test_trigger.AddListener(print_success_2);
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
        InputCheckNew();
        forceChain = false;
    }

    private void InputCheckNew()
    {
        Vector2Int dir = new Vector2Int(0, 0);

        if (Input.GetKeyDown(KeyCode.W))
        {
            dir = Vector2Int.down;
        }

        else if (Input.GetKeyDown(KeyCode.A))
        {
            dir = Vector2Int.left;
        }

        else if (Input.GetKeyDown(KeyCode.S))
        {
            dir = Vector2Int.up;
        }

        else if (Input.GetKeyDown(KeyCode.D))
        {
            dir = Vector2Int.right;
        }

        if (dir != new Vector2Int(0, 0) && !scrManager.OutOfBound(gridObject.gridPosition + dir))
        {
            //Do something to the thing at where about to hit.
            //if it is smooth at up, smooth move up
            if (scrManager.CheckForObjectAtGridPosition(gridObject.gridPosition + dir, "smooth"))
            {
                forceChain = true;
                //pushing up all obj con scr smooths
                scrManager.PushObjWithSmooths(gridObject.gridPosition, dir);
            } //1: did not go here

            //if it is sticky
            if (scrManager.CheckForObjectAtGridPosition(gridObject.gridPosition + dir,
                    "sticky")) //original pos's down has sticky
            {
                scrManager.StickObjWithStickies(gridObject.gridPosition, dir);
            }

            //if something still top
            if (scrManager.CheckForAnyAtGridPosition(gridObject.gridPosition + dir))
            {
                return;
            }

            //Move
            scrManager.Move(dir, gridObject);

            //if there is a sticky near by ori, sticky move too 1: likely here
            if (scrManager.FindAllStickiesNearPosition(gridObject.gridPosition - dir).Count != 0) //if there are nearby
            {
                forceChain = true;

                System.Collections.Generic.List<scrSticky> allStickies =
                    scrManager.FindAllStickiesNearPosition(gridObject.gridPosition - dir);
                print("length of list of all stickies near position: " + allStickies.Count);
                for (int i = 0; i < allStickies.Count; i++)
                {
                    scrManager.StickObjWithStickies(gridObject.gridPosition, dir);
                }
            }

            //if it is clingy at the bottom
            if (scrManager.CheckForObjectAtGridPosition(gridObject.gridPosition - dir - dir, "clingy"))
            {
                forceChain = true;
                //pulling up all obj from bottom
                scrManager.PullObjWithClingys(gridObject.gridPosition, dir);
            }
        }
    }
}