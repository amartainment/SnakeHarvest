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
    public TextMeshProUGUI currentTailLength;

    public TextMeshProUGUI textBoxDeathReason;

    public TextMeshProUGUI deathScoreBox;
    public TextMeshProUGUI textBoxGlobalHighScore;
    
    public  TextMeshProUGUI textBoxScore;

    private dreamloLeaderBoard myLeaderboard;
    public int requiredLowerLimit = 2;

    

    public int requiredLength = 0;

    public int requiredHigherLimit = 5;
    public float requiredLevelTime = 10;

    public float movementTick = 0.3f;

    public int score = 0;

    public bool stepByStepMode = false;
    public SnakeBehavior playerSnake;

    public Animator worldEndAnimator;

    public int globalHighScore = 0;
    public List<dreamloLeaderBoard.Score> scores;

    float levelTime;
    // Start is called before the first frame update
    void Start()
    {
        
        levelTime = requiredLevelTime;
        textBoxHarvestLength.SetText(requiredLowerLimit.ToString());
        currentTailLength.SetText((playerSnake.tail.Count +1).ToString());
        myLeaderboard = dreamloLeaderBoard.GetSceneDreamloLeaderboard();
        myLeaderboard.GetScores();
        
        
    }

    private void OnEnable() {
        MyEventSystem.levelComplete += incrementScore;
        MyEventSystem.heroDeath += ShowDeathScreen;
        

    }

    private void Awake() {
        requiredLowerLimit = Random.Range(1,requiredHigherLimit);
    }

    private void OnDisable() {
        MyEventSystem.levelComplete -= incrementScore;
        MyEventSystem.heroDeath -= ShowDeathScreen;
    }

    public int returnRandomTarget () {
        
        return requiredLowerLimit;
    }

    public void ShowDeathScreen(int i) {

        scores=  myLeaderboard.ToListHighToLow();
        if(scores!=null) {
        Debug.Log(scores[0].score);
        if(scores.Count>0) {
        globalHighScore = scores[0].score;
        
        }
        }

        switch(i) {
            case 1:
             deathScoreBox.transform.parent.gameObject.SetActive(true);
            deathScoreBox.SetText("You lasted for "+score.ToString()+" cycles");
            textBoxDeathReason.SetText("You were HARVESTED");
            textBoxGlobalHighScore.SetText(globalHighScore+" cycles");
            myLeaderboard.AddScore(SystemInfo.deviceUniqueIdentifier.ToString(),score);
            break;

            case 2:
             deathScoreBox.transform.parent.gameObject.SetActive(true);
            deathScoreBox.SetText("You lasted for "+score.ToString()+" cycles");
            textBoxDeathReason.SetText("You were EATEN");
            textBoxGlobalHighScore.SetText(globalHighScore+" cycles");
            myLeaderboard.AddScore(SystemInfo.deviceUniqueIdentifier.ToString(),score);
            break;

        }
       
    }

  
    


    // Update is called once per frame
    
    
    void Update()
    {
         if(playerSnake!=null) {
            currentTailLength.SetText((playerSnake.tail.Count +1).ToString());
        }

        if(!stepByStepMode) {
        levelTime-= Time.deltaTime;//*0.5f/movementTick;
        } else {
            checkingTimeForStepByStep();
        }


        textBoxTimer.SetText(levelTime.ToString("##"));

        if(levelTime<0) {
            TimerRanOut();
        }

        if(Input.GetKeyDown(KeyCode.Space)) {
            SceneManager.LoadScene(0);
            //TimerRanOut();
            
        }

        IncreaseSpeed();

       
       
    }
     public void checkingTimeForStepByStep() {
            if (Input.GetKeyDown (KeyCode.D)) {
                levelTime -=1;
            }
			else if (Input.GetKeyDown (KeyCode.S)) {
              levelTime -=1;
            }
			else if (Input.GetKeyDown (KeyCode.A)) {
                levelTime -=1;
                
            }
			else if (Input.GetKeyDown (KeyCode.W)) {
              levelTime -=1;
        }
     }

    private void TimerRanOut() {
        levelTime = requiredLevelTime;

        
        /*
        if(playerSnake.tail.Count != requiredLength) {
            
            SceneManager.LoadScene(0);
        }
        */

        MyEventSystem.worldEnd(1);
        worldEndAnimator.Play("HarvestFade");
        requiredLowerLimit = Random.Range(1,requiredHigherLimit);
        textBoxHarvestLength.SetText(requiredLowerLimit.ToString());
        if(MyEventSystem.ResetHeroEvent!=null) {
        MyEventSystem.ResetHeroEvent(1);
        }
        //textBoxHarvestLengthMax.SetText(requiredHigherLimit.ToString());
    }

    public void incrementScore(int i) {

        score++;
        textBoxScore.SetText(score.ToString());

    }

    public void IncreaseSpeed() {
        if(Input.GetKeyDown(KeyCode.LeftShift)) {
            movementTick = 0.1f;
            if(MyEventSystem.speedChange!=null) {
                MyEventSystem.speedChange(movementTick);
            }
        }

         if(Input.GetKeyUp(KeyCode.LeftShift)) {
            movementTick = 0.3f;
            if(MyEventSystem.speedChange!=null) {
                MyEventSystem.speedChange(movementTick);
            }
        }
        
    }


}
