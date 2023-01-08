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

    public int maximumSnakes = 0;

    public List<Transform> SnakeCreatorTransforms;
    public List<Transform> HoleCreatorTransforms;
    public List<Transform> FoodCreatorTransforms;

    public bool levelStarted = false;

    public GameObject instructionScreen;
    

    float levelTime;
    // Start is called before the first frame update
    void Start()
    {
        
        levelTime = requiredLevelTime;
        textBoxHarvestLength.SetText(requiredLowerLimit.ToString());
        currentTailLength.SetText((playerSnake.tail.Count +1).ToString());
        myLeaderboard = dreamloLeaderBoard.GetSceneDreamloLeaderboard();
        myLeaderboard.GetScores();
        SetLimitsBasedOnScore(score);
        
        
    }

    private void OnEnable() {
        MyEventSystem.levelComplete += incrementScore;
        MyEventSystem.heroDeath += ShowDeathScreen;
        

    }

    private void Awake() {
        requiredLowerLimit = Random.Range(3,requiredHigherLimit);
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
            textBoxDeathReason.SetText("You were HARVESTED for being "+(playerSnake.tail.Count+1).ToString()+" parts long instead of "+requiredLowerLimit.ToString());
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

        if(levelStarted) {

        if(!stepByStepMode) {
        levelTime-= Time.deltaTime;//*0.5f/movementTick;
        } else {
            checkingTimeForStepByStep();
        }
        }


        textBoxTimer.SetText(levelTime.ToString("##"));

        if(levelTime<0) {
            TimerRanOut();
        }

        if(Input.GetKeyDown(KeyCode.Space)) {
           // SceneManager.LoadScene(0);
            //TimerRanOut();
            
        }

        IncreaseSpeed();
        CheckForKeypressAndStartLevel();
        ShowInstructions();

       
       
    }

    void ShowInstructions() {
        if(levelStarted) {
            instructionScreen.SetActive(false);
        } else {
            instructionScreen.SetActive(true);
        }
    }

    void CheckForKeypressAndStartLevel() {
        if(Input.GetKeyDown(KeyCode.Space)) {
            levelStarted = true;
        }
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
        if(MyEventSystem.worldEnd!=null) {
        MyEventSystem.worldEnd(1);
        }
        
        worldEndAnimator.Play("HarvestFade");
       // requiredLowerLimit = Random.Range(1,requiredHigherLimit);
        // textBoxHarvestLength.SetText(requiredLowerLimit.ToString());
      
        //textBoxHarvestLengthMax.SetText(requiredHigherLimit.ToString());
    }

    public void SetANewLimitForWave() {
        Debug.Log("set new limits yall");
        requiredLowerLimit = Random.Range(3,requiredHigherLimit);
        textBoxHarvestLength.SetText(requiredLowerLimit.ToString());
         if(MyEventSystem.ResetHeroEvent!=null) {
                MyEventSystem.ResetHeroEvent(1);
        }

        
    }

    public void incrementScore(int i) {

        score++;
        SetLimitsBasedOnScore(score);
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


    public void SetLimitsBasedOnScore(int gameScore) {
        ;
        // decide level difficulty based on current score
        switch(gameScore) {
            case int s when (s<=5):
                maximumSnakes = 1;
                requiredHigherLimit = 7;
                
                // Percentage chance going from 5 to 25%
                if(Random.Range(0.00f,1.00f)<0.05*score) {
                    SetActiveSnakeGenerators(1);
                }

                SetANewLimitForWave();
                SetupHolesAndFood();

                
                break;
            case int s when (s>5 && s<=10):
                maximumSnakes = 2;
                requiredHigherLimit = 5;
                if(Random.Range(0.00f,1.00f)>(0.5+0.05*(score-5))){

                    SetActiveSnakeGenerators(2);

                } else {
                    SetActiveSnakeGenerators(2);
                }

                SetANewLimitForWave();
                SetupHolesAndFood();
                

                break;
            case int s when (s>10 && s<=15) :
                maximumSnakes = 3;
                requiredHigherLimit = 7;
                 if(Random.Range(0.00f,1.00f)>(0.5+0.05*(score-10))){

                    SetActiveSnakeGenerators(3);

                } else {
                    SetActiveSnakeGenerators(2);
                }
                SetANewLimitForWave();
                SetupHolesAndFood();
                break;
            case int s when (s>15):
                maximumSnakes = 5;
                requiredHigherLimit = 8;
                 if(Random.Range(0.00f,1.00f)>(0.5+0.05*(score-10))){

                    SetActiveSnakeGenerators(Random.Range(4,5));

                } else {
                    SetActiveSnakeGenerators(3);
                }
                SetANewLimitForWave();
                SetupHolesAndFood();
                break;
        }

        
        
    }

    public void SetActiveSnakeGenerators(int count) {

        
        ResetAllGenerators();
        List<Transform> temporarySnakeGenTransforms = new List<Transform>();
        for(int i=0;i<SnakeCreatorTransforms.Count;i++) {
            temporarySnakeGenTransforms.Add(SnakeCreatorTransforms[i]);
        }
        for(int i=0;i<count;i++) {
            Debug.Log("tried to turn on some snakes");
            int randomIndex = Random.Range(0, temporarySnakeGenTransforms.Count);
            temporarySnakeGenTransforms[randomIndex].gameObject.SetActive(true);
            //temporaryTransform[randomIndex].GetComponent<SnakeCreator>().replenishSnakes = true;
            temporarySnakeGenTransforms.Remove(temporarySnakeGenTransforms[randomIndex]);
        }



        


    }

    public void SetupHolesAndFood() {

        ResetHolesAndFood();
        List<Transform> temporaryFoodGenTransforms = new List<Transform>();
        for(int i=0;i<FoodCreatorTransforms.Count;i++) {
            temporaryFoodGenTransforms.Add(FoodCreatorTransforms[i]);
        }
        //magic number food generators = 3
        for(int i=0;i<3;i++) {
            Debug.Log("tried to turn on some snakes");
            int randomIndex = Random.Range(0, temporaryFoodGenTransforms.Count);
            temporaryFoodGenTransforms[randomIndex].gameObject.SetActive(true);
            //temporaryTransform[randomIndex].GetComponent<SnakeCreator>().replenishSnakes = true;
            temporaryFoodGenTransforms.Remove(temporaryFoodGenTransforms[randomIndex]);
        }

        List<Transform> temporaryHoleGeneratorTransforms = new List<Transform>();
        for(int i=0;i<HoleCreatorTransforms.Count;i++) {
            temporaryHoleGeneratorTransforms.Add(HoleCreatorTransforms[i]);
        }
        //magic number hole generators = 4
        for(int i=0;i<4;i++) {
            Debug.Log("tried to turn on some snakes");
            int randomIndex = Random.Range(0, temporaryHoleGeneratorTransforms.Count);
            temporaryHoleGeneratorTransforms[randomIndex].gameObject.SetActive(true);
            //temporaryTransform[randomIndex].GetComponent<SnakeCreator>().replenishSnakes = true;
            temporaryHoleGeneratorTransforms.Remove(temporaryHoleGeneratorTransforms[randomIndex]);
        }
    }

    public void ResetHolesAndFood() {

        foreach(Transform t in FoodCreatorTransforms) {
            t.gameObject.SetActive(false);
        }

        foreach(Transform t in HoleCreatorTransforms) {
            t.gameObject.SetActive(false);
        }

    }

    public void ResetAllGenerators() {
        foreach(Transform t in SnakeCreatorTransforms) {
            t.gameObject.SetActive(false);
        }

   
    }


}
