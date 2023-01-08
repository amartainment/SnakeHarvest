using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodDispenser : MonoBehaviour
{
    // Start is called before the first frame update
    public List<Vector2> spawnPositions;
    public SpriteRenderer spriteForEditor;
    public int spawnCount;
    public GameObject foodPrefab;

    public float foodCheckTimer;
    private bool timerRunnnig = false;
    void Start()
    {
        spriteForEditor.enabled = false;
        SpawnFoodAtPositions();
    }

    // Update is called once per frame
    void Update()
    {
        if(!timerRunnnig) {
            StartCoroutine(CheckingForFoodTimer());
        }
    }

    

    private void OnDisable() {
        
    }

    private void SpawnFoodAtPositions() {

        List<Vector2> tempPositionArray = new List<Vector2>();
        foreach(Vector2 spawmPos in spawnPositions) {
            tempPositionArray.Add(spawmPos);
        }

       for(int i=0;i<spawnCount;i++) {
        int randomIndex = Random.Range(0,tempPositionArray.Count-1);
        if(Physics2D.Raycast(tempPositionArray[randomIndex]+new Vector2(transform.position.x,transform.position.y), Vector2.zero)) {

        } else {
        Instantiate(foodPrefab,tempPositionArray[randomIndex] + new Vector2(transform.position.x,transform.position.y),Quaternion.identity,transform);
        tempPositionArray.RemoveAt(randomIndex);
        }

       }

    }

    public void CheckFoodReservesAndReplenish() {
        if(transform.childCount ==0) {
            SpawnFoodAtPositions();
        }
    }

    private IEnumerator CheckingForFoodTimer() {
        timerRunnnig = true;
        yield return new WaitForSeconds(foodCheckTimer);
        CheckFoodReservesAndReplenish();
        timerRunnnig = false;

    }
}
