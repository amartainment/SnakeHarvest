using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform oppositeWall;
    public Vector3 spawningOffset; 
    public WallType myWallType; 
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector3 ReturnSpawnPosition(Vector3 hitPosition) {
        switch(myWallType) {
            case WallType.top:
            return new Vector3(0,oppositeWall.position.y,0)  + new Vector3(hitPosition.x,spawningOffset.y,0);
            //break;
            case WallType.right:
            return new Vector3(oppositeWall.position.x,0,0) + new Vector3(spawningOffset.x,hitPosition.y,0);
           // break;
            case WallType.left:
            return new Vector3(oppositeWall.position.x,0,0) + new Vector3(spawningOffset.x,hitPosition.y,0);
            //break;
            case WallType.bottom:
            return new Vector3(0,oppositeWall.position.y,0) + new Vector3(hitPosition.x,spawningOffset.y,0);
            //break;
            default:
            return Vector3.zero;
           // break;


        }
        
    }

     public enum WallType {
            top,
            right,
            bottom,
            left
        }
}
