using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SnakeBehavior : MonoBehaviour {
	// Did the snake eat something?
	bool ate = false;

    public bool playerSnake = false;
    public bool stepByStepMode = false;

    public int lowerLimit = 2;

    public int upperLimit = 5;

    public bool startWithTails =false;
    public int startingTailCount = 2;

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
	public List<TailBehavior> tail = new List<TailBehavior>();

    public Transform myTailTransform;
    
    public GameManager myManager;
    public TailBehavior myTail = null;

    public bool movementCouroutinRunning = false;

	// Use this for initialization
	public void Start () {
        PullValuesFromGM(1);
    
        if(startWithTails) {
            InstantiateWithTail(startingTailCount);
        }
	}

    public IEnumerator KeepMovingEnumerator() {
        movementCouroutinRunning =true;
        yield return new WaitForSeconds(movementTick);
        Move();
        movementCouroutinRunning = false;
    }

    private void OnEnable() {
        MyEventSystem.worldEnd += CheckTailAndDie;
        MyEventSystem.ResetHeroEvent += PullValuesFromGM;
        MyEventSystem.speedChange += PullNewSpeedFromGM;
    }

    private void OnDisable() {
        MyEventSystem.worldEnd -= CheckTailAndDie;
        MyEventSystem.ResetHeroEvent -= PullValuesFromGM;
        MyEventSystem.speedChange -= PullNewSpeedFromGM;
    }

    private void OnDestroy() {
        MyEventSystem.worldEnd -= CheckTailAndDie;
        MyEventSystem.ResetHeroEvent -= PullValuesFromGM;
         MyEventSystem.speedChange -= PullNewSpeedFromGM;
    }

    public void PullValuesFromGM(int i) {
        myManager = GameObject.Find("_GM").GetComponent<GameManager>();
        stepByStepMode = myManager.stepByStepMode;
        lowerLimit = myManager.returnRandomTarget();
        Debug.Log("setting lowerlimit to" + lowerLimit);
        upperLimit = myManager.requiredHigherLimit;
        movementTick = myManager.movementTick;
    }

    public void PullNewSpeedFromGM(float f) {
        movementTick = f;
    }

	// Update is called once per frame
	public void Update () {

        if(stepByStepMode) {
            SetByStepControls();
        } else {
            ContinousMotionControls();
		    if(!movementCouroutinRunning) {
                StartCoroutine(KeepMovingEnumerator());
            }
        } 
        
	
	}

    public void CheckTailAndDie(int i) {
        if((tail.Count + 1) != lowerLimit ) {
            Debug.Log(gameObject.name + "snake died with "+ (tail.Count() + 1)+" against required: "+ lowerLimit);
            DieAsASnake(1);

        } else {
            
                //or update stats if alive
            PullValuesFromGM(1);
            if(playerSnake){
                MyEventSystem.levelComplete(1);
            }
        }

        
    }

    

    public void InstantiateWithTail(int tailCount)
    {
        for(int i=0;i<tailCount;i++) {
        GameObject newTail = Instantiate(tailPrefab, transform.position, Quaternion.identity);
        TailBehavior newTailBehavior = newTail.GetComponent<TailBehavior>();
        newTailBehavior.SetIndex(tail.Count, this, tail);
        newTailBehavior.SetColor(headSpriteTransform.GetComponent<SpriteRenderer>().color);
        tail.Add(newTailBehavior);
        if (tail.Count > 1)
        {
            newTailBehavior.SetHead(tail[tail.IndexOf(newTailBehavior) - 1].transform);
            newTailBehavior.transform.position = new Vector2(transform.position.x,transform.position.y) - dir*i;

        }
        else
        {
            myTail = newTailBehavior;
            newTailBehavior.transform.position = new Vector2(transform.position.x,transform.position.y) - dir*i;
            newTailBehavior.SetHead(transform);

        }
        }
        transform.Translate(dir);

    }

    private void ContinousMotionControls() {

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

    private void SetByStepControls() {
        if (!isDied) {
            
			// Move in a new Direction?
			if (Input.GetKeyDown (KeyCode.D)) {
                if(playerSnake) {
				TryToTurn(Vector2.right);
                eulerRotationAngles = new Vector3(0,0,-90);}
                else {
                    AISnakeControls();
                }
                Move();
            }
			else if (Input.GetKeyDown (KeyCode.S)) {
                if(playerSnake) {
				TryToTurn(-Vector2.up);    // '-up' means 'down'
                eulerRotationAngles = new Vector3(0,0,-180);
                } else {
                    AISnakeControls();
                }
                Move();
            }
			else if (Input.GetKeyDown (KeyCode.A)) {
                if(playerSnake) {
				TryToTurn(-Vector2.right);// '-right' means 'left'
                eulerRotationAngles = new Vector3(0,0,90);
                } else {
                AISnakeControls();
                }
                Move();
                
            }
			else if (Input.GetKeyDown (KeyCode.W)) {
                if(playerSnake) {
				TryToTurn(Vector2.up);
                eulerRotationAngles = new Vector3(0,0,0);
                }else {
                AISnakeControls();
                }
                Move();
                
            
            } 

	
            } else {
                DieAsASnake(2);
            }
		}
    


 

    public virtual void AISnakeControls() {
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

    public void DieAsASnake(int i) {
        CutTailAt(myTail, true);
        Destroy(gameObject);
        if(playerSnake) {
            MyEventSystem.heroDeath(i);
        }

    }

    public void TryToTurn(Vector2 direction) {
        Collider2D checkForTailCollider = Physics2D.OverlapCircle(new Vector2(transform.position.x,transform.position.y)+direction,0.1f);
        if(checkForTailCollider!=null)  {

            if(checkForTailCollider.GetComponent<TailBehavior>()!=null) {
                if(checkForTailCollider.GetComponent<TailBehavior>()==myTail) {
                    // do not accept is point is on my tail (tracing backwards not allowed basically)
                } else{
                    dir = direction;
                    headSpriteTransform.up = dir;

                }
                
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
	} else {
        CutTailAt(myTail, true);
        Destroy(gameObject);
    }
    }

    void PassPositionToTail(Vector2 myOldPosition) {
        myTail.MoveToPosition(myOldPosition);
        

    }

	void OnTriggerEnter2D(Collider2D coll) {
        
        Vector3 collisionPosition = transform.position;
		// Food?
		if (coll.GetComponent<Food>()) {
            Debug.Log("Food!");
			// Get longer in next Move call
			ate = true;
            
			// Remove the Food
			Destroy(coll.gameObject);
		}
        
        else if(coll.GetComponent<Hole>()) {
            Destroy(coll.gameObject);
            if(tail.Count>0) {
            tail.Last().KillMyself(false);
            } else {
                isDied = true;
            }

            /*
            Destroy(coll.gameObject);
            if(tail.Count>1) {
            tail[tail.Count-2].myTail =null;    
            Destroy(tail[tail.Count-1].gameObject);
            tail.RemoveAt(tail.Count-1);
            
            } else {
                if(tail.Count==1) {
                    Destroy(myTail.gameObject);
                    myTail = null;
                    tail = new List<TailBehavior>();
                } else {
                    isDied = true;
                }
                
            }
            */
            
        } else if(coll.gameObject.GetComponent<TailBehavior>()!=null) {
            if(coll.gameObject.GetComponent<TailBehavior>().myParentSnake == this) {
			    //isDied = true;
                ate = true;
                coll.gameObject.GetComponent<TailBehavior>().KillMyself(true);
            } else {
                ate = true;
                coll.gameObject.GetComponent<TailBehavior>().KillMyself(true);
            }

		} else if(coll.gameObject.GetComponent<WallBehavior>()!=null) {
            transform.position = coll.gameObject.GetComponent<WallBehavior>().ReturnSpawnPosition(collisionPosition);
        } else if(coll.gameObject.GetComponent<SnakeBehavior>()!=null) {
            DieAsASnake(2);
        }
	}

    public void CutTailAt(TailBehavior cutTail, bool gotEaten) {
        int indexOfPart = tail.IndexOf(cutTail);
        Debug.Log("Cutting "+gameObject.name+" at "+indexOfPart);
        if(indexOfPart>0) {
            tail[indexOfPart].myTail = null;
            for(int i=indexOfPart;i<tail.Count;i++) {
                if(gotEaten) {
                tail[i].ReplaceMyselfWithFood();
                } else {
                tail[i].JustDisappear();
                }
            }   
            tail.RemoveRange(indexOfPart,tail.Count-indexOfPart);
        

        } else if(indexOfPart ==0) {
           for(int i=indexOfPart;i<tail.Count;i++) {
                if(gotEaten) {
                tail[i].ReplaceMyselfWithFood();
                } else {
                tail[i].JustDisappear();
                }
            }   
            tail.RemoveRange(indexOfPart,tail.Count-indexOfPart);
            if(gotEaten){
            myTail.ReplaceMyselfWithFood();
            } else {
            myTail.JustDisappear();
            }
            myTail =null;
            tail = new List<TailBehavior>();
        }
        }


       



        

       
    
}