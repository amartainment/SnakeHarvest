using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI textBoxTimer;
    public TextMeshProUGUI textBoxHarvestLength;
    public float requiredLength = 5;
    public float requiredLevelTime = 10;
    public SnakeBehavior playerSnake;

    float levelTime;
    // Start is called before the first frame update
    void Start()
    {
        requiredLength = Random.Range(2,5);
        levelTime = requiredLevelTime;
        textBoxHarvestLength.SetText(requiredLength.ToString());
        
    }

    // Update is called once per frame
    void Update()
    {
        levelTime-= Time.deltaTime;
        textBoxTimer.SetText(levelTime.ToString("##"));

        if(levelTime<0) {
            TimerRanOut();
        }

        if(Input.GetKeyDown(KeyCode.Space)) {
            TimerRanOut();
            
        }
    }

    private void TimerRanOut() {
        levelTime = requiredLevelTime;
        if(playerSnake.tail.Count != requiredLength) {
            SceneManager.LoadScene(0);
        }
    }
}
