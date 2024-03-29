using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrWall : MonoBehaviour
{
    private GridObject gridObject;
    
    // Start is called before the first frame update
    void Start()
    {
        gridObject = GetComponent<GridObject>();
        if (gridObject == null)
        {
            Debug.LogError("GridMovement requires a GridObject component on the same GameObject.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
