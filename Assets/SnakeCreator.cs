using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeCreator : MonoBehaviour
{
    public GameObject SnakePrefab;
    public SpriteRenderer SpriteForEditor;

    public bool createdSnakes = false;

    public bool replenishSnakes = false;

    public List<GameObject> mySnakes;

    // Start is called before the first frame update
    private void OnEnable() {
        MyEventSystem.levelComplete += ReInitialize;
        MyEventSystem.worldEnd += StopReplenishing;
    }

    private void OnDisable() {
        MyEventSystem.levelComplete -= ReInitialize;
        MyEventSystem.worldEnd -= StopReplenishing;
    }

    void ReInitialize(int i) {
        Invoke("CheckChildrenAndMakeSnakesFirst",2);
    }
    void Start()
    {
        if(mySnakes==null) {
            mySnakes = new List<GameObject>();
        }
        SpriteForEditor.enabled = false;
        Invoke("CheckChildrenAndMakeSnakesFirst",2);
        
    }

    // Update is called once per frame
    void Update()
    {
        if(replenishSnakes) {
            CheckChildrenAndMakeSnakes();
        }
    }



    public void CheckChildrenAndMakeSnakesFirst() {
        if(mySnakes.Count==0) {
            GameObject newSnake = Instantiate(SnakePrefab,transform.position,Quaternion.identity);
            mySnakes.Add(newSnake);

            replenishSnakes = true;
        }

    }

    public void CheckChildrenAndMakeSnakes() {
        if(mySnakes.Count==0) {
            GameObject newSnake = Instantiate(SnakePrefab,transform.position,Quaternion.identity);
            mySnakes.Add(newSnake);
            replenishSnakes = true;
        }

    }

    public void StopReplenishing(int i) {
        replenishSnakes = false;
    }
}
