using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TailBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    public int myIndex =0;
    public SnakeBehavior myParentSnake;
    public List<Transform> myParentTailList;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetIndex(int i, SnakeBehavior parent, List<Transform> transformList) {
        myIndex = i;
        myParentSnake = parent;
        myParentTailList = transformList;
    }

    public void SetColor(Color c) {
        GetComponent<SpriteRenderer>().color =c;
    }

    public void KillMyself() {
        Destroy(gameObject);
        myParentTailList.RemoveAt(myParentTailList.Count-1);
    }
}
