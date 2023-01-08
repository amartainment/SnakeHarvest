using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneDirectionSnakeBehavior : SnakeBehavior
{
    [SerializeField] public Vector2 movementDirection;
    // Start is called before the first frame update
   

    public override void AISnakeControls()
    {
       
        base.TryToTurn(movementDirection);
        //base.AISnakeControls();

    }
}
