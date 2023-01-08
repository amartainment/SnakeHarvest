using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleCreator : MonoBehaviour
{
    public GameObject holePrefab;
    private bool timerRunning = false;
    public float holeCheckTimer = 2;
    public SpriteRenderer spriteForEditor;
    // Start is called before the first frame update
    void Start()
    {
        timerRunning = false;
        CheckAndReplenishHole();
       spriteForEditor.enabled = false;
    }

    private void OnDisable() {
        timerRunning = false;
        StopAllCoroutines();
    }

    private void OnEnable() {
        
        timerRunning = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(!timerRunning) {
            StartCoroutine(holeCheckNumerator());
        }
        
    }

    private void CheckAndReplenishHole() {
        if(transform.childCount==0) {
            Instantiate(holePrefab,transform.position,Quaternion.identity,transform);
        }
    }

    private IEnumerator holeCheckNumerator() {
        timerRunning = true;
        yield return new WaitForSeconds(holeCheckTimer);
        CheckAndReplenishHole();
        timerRunning = false;

    }


}
