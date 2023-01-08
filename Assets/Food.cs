using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    // Start is called before the first frame update
    public float lifeTime = 4;
    public bool disappearing  = true;
    void Start()
    {
        
    }

    private void OnEnable() {
        MyEventSystem.levelComplete += DestroyMyself;
    }

    private void OnDisable() {
        MyEventSystem.levelComplete -= DestroyMyself;
        
    }

    // Update is called once per frame
    void Update()
    {
        if(disappearing) {
        
        if(lifeTime>0) {
            lifeTime -= Time.deltaTime;
        } else {
            Destroy(gameObject);
        }
        
        }
        
        
    }

    void DestroyMyself(int i) {
        Destroy(gameObject);
    }
}
