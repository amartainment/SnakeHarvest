using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeCreator : MonoBehaviour
{
    public GameObject SnakePrefab;
    public SpriteRenderer SpriteForEditor;

    public bool createdSnakes = false;

    public bool replenishSnakes = false;

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
        if(transform.childCount==0) {
            Instantiate(SnakePrefab, transform.position,Quaternion.identity,transform);
            replenishSnakes = true;
        }

    }

    public void CheckChildrenAndMakeSnakes() {
        if(transform.childCount==0) {
            Instantiate(SnakePrefab, transform.position,Quaternion.identity,transform);
            replenishSnakes = true;
        }

    }

    public void StopReplenishing(int i) {
        replenishSnakes = false;
    }
}
