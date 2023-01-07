using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SnakeBehavior : MonoBehaviour {
	// Did the snake eat something?
	bool ate = false;

    public bool playerSnake = false;

	//Did user died?
	public bool isDied = false;

	// Tail Prefab
	public GameObject tailPrefab;

    public float movementTick = 0.3f;

	// Current Movement Direction
	// (by default it moves to the right)
	Vector2 dir = Vector2.right;
    Vector3 eulerRotationAngles = new Vector3(0,0,0);
    public Transform headSpriteTransform;

	// Keep Track of Tail
	List<TailBehavior> tail = new List<TailBehavior>();

    public Transform myTailTransform;
    public TailBehavior myTail = null;

	// Use this for initialization
	void Start () {
		// Move the Snake every 300ms
		InvokeRepeating("Move", movementTick, movementTick); 
	}

	// Update is called once per frame
	void Update () {
		if (!isDied) {
            if(playerSnake) {
			// Move in a new Direction?
			if (Input.GetKey (KeyCode.D)) {
				TryToTurn(Vector2.right);
                eulerRotationAngles = new Vector3(0,0,-90);
            }
			else if (Input.GetKey (KeyCode.S)) {
				TryToTurn(-Vector2.up);    // '-up' means 'down'
                eulerRotationAngles = new Vector3(0,0,-180);
            }
			else if (Input.GetKey (KeyCode.A)) {
				TryToTurn(-Vector2.right);// '-right' means 'left'
                eulerRotationAngles = new Vector3(0,0,90);
                
            }
			else if (Input.GetKey (KeyCode.W)) {
				TryToTurn(Vector2.up);
                eulerRotationAngles = new Vector3(0,0,0);
            }
            } 

		} else {
            if(playerSnake) {
			if (Input.GetKey(KeyCode.R)){
				//clear the tail
				tail.Clear();

				//reset to origin
				transform.position = new Vector3(0, 0, 0);

				//make snake alive
				isDied = false;
			}
            }
		}
	}

    private void AISnakeControls() {
        float randomFloat = Random.Range(0.00f,1.00f);
        Vector2 randomDirection = new Vector2(0,0);
        switch (randomFloat) {
            case float f when f>=0 && f<0.25f:
            randomDirection = Vector2.up;
            break;
            case float f when f>=0.25f && f<0.5f:
            randomDirection = Vector2.right;
            break;
            case float f when f>=0.5f && f<0.75f:
            randomDirection = -Vector2.up;
            break;
            case float f when f>=0.75f && f<=1f:
            randomDirection = -Vector2.right;
            break;
            default:
            break;
        }
        TryToTurn(randomDirection);
    }

    private void TryToTurn(Vector2 direction) {
        Collider2D checkForTailCollider = Physics2D.OverlapCircle(new Vector2(transform.position.x,transform.position.y)+direction,0.1f);
        if(checkForTailCollider!=null)  {

            if(checkForTailCollider.GetComponent<TailBehavior>()!=null) {
                
            } else {
                dir = direction;
                headSpriteTransform.up = dir;
            }

        } else {
            dir = direction;
            headSpriteTransform.up = dir;
        }

        
    }

	void Move() {
		if (!isDied) {
            if(!playerSnake)  {
                AISnakeControls();
            }
			// Save current position (gap will be here)
			Vector2 v = transform.position;

	
			transform.Translate (dir);
            
			// Ate something? Then insert new Element into gap
			if (ate) {
				// Load Prefab into the world
				GameObject g = (GameObject)Instantiate (tailPrefab,v,Quaternion.identity);
                TailBehavior newTail = g.GetComponent<TailBehavior>(); 
                newTail.SetIndex(tail.Count, this, tail);
                newTail.SetColor(headSpriteTransform.GetComponent<SpriteRenderer>().color);
                
				// Keep track of it in our tail list
				tail.Add(newTail);
                if(tail.Count>1) {
                newTail.SetHead(tail[tail.IndexOf(newTail)-1].transform);
                
                } else {
                    myTail = newTail;
                    newTail.SetHead(transform);

                }
                //g.transform.parent = transform;
              

				// Reset the flag
				ate = false;
			
         
            
		}
        if(myTail!=null){
        PassPositionToTail(v);
        }
	}
    }

    void PassPositionToTail(Vector2 myOldPosition) {
        myTail.MoveToPosition(myOldPosition);
        

    }

	void OnTriggerEnter2D(Collider2D coll) {
		// Food?
		if (coll.name.StartsWith("Food")) {
			// Get longer in next Move call
			ate = true;

			// Remove the Food
			Destroy(coll.gameObject);
		}
        
        else if(coll.name.StartsWith("Hole")) {
            Destroy(coll.gameObject);
            if(tail.Count>=1) {
            tail[tail.Count-2].myTail =null;    
            Destroy(tail[tail.Count-1].gameObject);
            tail.RemoveAt(tail.Count-1);
            } else {
                isDied = true;
            }
        } else if(coll.gameObject.GetComponent<TailBehavior>()!=null) {
            if(coll.gameObject.GetComponent<TailBehavior>().myParentSnake == this) {
			isDied = true;
            } else {
                ate = true;
                coll.gameObject.GetComponent<TailBehavior>().KillMyself();
            }

		} 
	}
}