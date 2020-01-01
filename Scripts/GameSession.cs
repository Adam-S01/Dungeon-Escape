using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
 


public class GameSession : MonoBehaviour
{

    [SerializeField] int playerLives    ;// this var must be entered manually
    [SerializeField] int playerScore    ;// this var must be entered manually
    [SerializeField] TextMeshProUGUI playerScoreText; // this is a TMP hooked to the TMP in Unity, the value of this TMP is 
                                                      // specified by this script to equal the playerScore var above at start and in the game
                                                      // and since the TMP in Unity is hooked to this TMP, the text in Unity will change accordingly
    [SerializeField] TextMeshProUGUI playerLivesText;// this is a TMP hooked to the TMP in Unity, the value of this TMP is 
                                                     // specified by this script to equal the playerScore var above at start and in the game
                                                     // and since the TMP in Unity is hooked to this TMP, the text in Unity will change accordingly


    void Awake()//the awake method is compiled before the start method, we need this for the singleton
    {
        SetUpSingleton(); // this is a method we create to use the singleton concept
    }

    private void SetUpSingleton()
    {
         if (FindObjectsOfType(GetType()).Length > 1)
        
        {  
            // the singleton concept is that if we have 2 or more gameObject, that are the same  
            // in two different scenes, we destroy the new one before it get compiled and we keep 
            // the one from the previous scene.
            // FindObjectsOfType() find all the objects that are of a cretain name 
            // here we use GetType() which is a method that return the name of the current class 

            Destroy(this);// so when we find the new gameObject with the same name we destroy it
        }
        else
        {
            DontDestroyOnLoad(this);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        playerScoreText.text = playerScore.ToString();
        playerLivesText.text = playerLives.ToString();
         

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlayerHasDied()
    {
      
        if (playerLives > 1)
        {
          TakeLife();
            
        }
        else
        {
            ResetGameSession();
        }


    }



    private void TakeLife()
    {

         
        playerLives--;

        playerLivesText.text = playerLives.ToString();
        // updating the TMP text

        
        var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
        //this line is to load the same scene when the player dies and still have lives 

    }

    public void ResetGameSession()
    {
        SceneManager.LoadScene(0);// when we don't have enough lives we go back to scene(0)

        Destroy(gameObject);// and we go to scene 0 we need to destroy the GameSessio to reset the game
        



    }


    public void AddToScore(int score) // a public method to be called from outside with a parameter
                                      // to give the value of the scroe to be added , this method will be called 
                                      // whenever we need to add score, kill enemy or take a coin 
    {
        playerScore += score; // we add the score 
        playerScoreText.text = playerScore.ToString(); // update the TMP score 

    }

   


}
