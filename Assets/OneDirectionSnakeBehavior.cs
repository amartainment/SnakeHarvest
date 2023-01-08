using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneDirectionSnakeBehavior : SnakeBehavior
{
    [SerializeField] public Vector2 movementDirection;
    [SerializeField] public GameObject ghost;
    // Start is called before the first frame update
   

    public override void AISnakeControls()
    {
       
        base.TryToTurn(movementDirection);
        if(base.stepByStepMode){
        ghost.SetActive(true);
        ghost.transform.position = movementDirection + new Vector2(transform.position.x,transform.position.y);
        }
        //base.AISnakeControls();

    }
}
