using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TailBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    public int myIndex =0;
    public SnakeBehavior myParentSnake;

    public bool heroSnakePart = false;

    public Transform myTail;
    public Transform myHead;
    private List<TailBehavior> myParentTailList;
    public GameObject foodParticle;

    bool partDestroyed = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetHead(Transform t) {
        myHead = t;
        if(t.GetComponent<TailBehavior>()!=null) {
            t.GetComponent<TailBehavior>().myTail = transform;
        }
    }

    public void SetIndex(int i, SnakeBehavior parent, List<TailBehavior> transformList) {
        myIndex = i;
        myParentSnake = parent;
        myParentTailList = transformList;
        Debug.Log("Parent set to"+parent.gameObject.name);
    }

    public void SetColor(Color c) {
        GetComponent<SpriteRenderer>().color =c;
  
    }

    public void KillMyself(bool gotEaten) {
        //old code
        /*
        if(!partDestroyed) {
        partDestroyed = true;
        if(myHead.GetComponent<TailBehavior>()!=null) {
            myHead.GetComponent<TailBehavior>().myTail = null;
        }
        Destroy(gameObject);
        for(int i=myParentTailList.IndexOf(this);i<myParentTailList.Count;i++) {
            myParentTailList[i].ReplaceMyselfWithFood();
        }
        myParentTailList.RemoveRange(myParentTailList.IndexOf(this),myParentTailList.Count);
        }
        */
        if(!partDestroyed) {
        partDestroyed = true;
        if(myParentSnake!=null) {
        myParentSnake.CutTailAt(this,gotEaten);
        }
        }
    }

    public void MoveToPosition(Vector2 headPosition) {
        if(myTail!=null){
        myTail.GetComponent<TailBehavior>().MoveToPosition(transform.position);
        }
        transform.position = headPosition;

    }

    public void ReplaceMyselfWithFood() {
        if(heroSnakePart) {
            GetComponent<Animator>().Play("DeflatePart");
        } else {
            GetComponent<Animator>().Play("DeflateEnemy");
        }
        GameObject food= Instantiate(foodParticle,transform.position,Quaternion.identity);
       // food.GetComponent<SpriteRenderer>().color = Color.red;
        

    }

    public void JustDisappear() {
        if(heroSnakePart) {
            GetComponent<Animator>().Play("DeflatePart");
        } else {
            GetComponent<Animator>().Play("DeflateEnemy");
        }
    }

    public void DestroyTheObject() {
        Destroy(gameObject);
    }
}
