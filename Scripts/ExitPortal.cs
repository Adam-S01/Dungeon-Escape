using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // we need to use this to change from scene to scene


public class ExitPortal : MonoBehaviour
{
         
    [SerializeField] float delayToLoadScene = 5f;
    //[SerializeField] float levelExitSlowMotion = 0.25f;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        this.transform.Rotate(0, 0, -1);
        // this to rotate the portal aroun the z axis 

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        StartCoroutine(WaitThenLoad());
        // when something collide trigger with this class ( the portal exit ) we call the StartCoroutine()
        // coroutine is a method() that we use to compile a code after a certain condition met 

    }



    public IEnumerator WaitThenLoad()
    {
        //Time.timeScale = levelExitSlowMotion;// when we touch the portal we make a slowmotion 
        yield return new WaitForSecondsRealtime(delayToLoadScene);//the yield instruction or condition(here it's waiting for some seconds)
                                                                  // note that WaitForSecondsRealtime are real time seconds indpendant from
                                                                  // the slowmotion 
                                                                  //Time.timeScale = 1f;// return the speed of the game to it's normal pace 




        LoadNextScene();

    }

    public void LoadStartMenu()
    {
        SceneManager.LoadScene(0);// start menu normaly will be scene 0 
        // FindObjectOfType<GameSession>().ResetGame();
        // this will call a methode that we use to reset the game after losing or game over
    }

    public void LoadNextScene()
    {

        if (SceneManager.GetActiveScene().name == "End Scene")
        {
            // SceneManager.LoadScene(0);
            Application.Quit();
        }
        else
        {
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            // this return an int of the active scene in play 

            SceneManager.LoadScene(currentSceneIndex + 1);
            // then we load the scene after it 
        }
    }


    public void LoadGame()
    {

        // this function we use it in the laser defender tutorial to prevent some errors, we hook it to the button that
        // start the game, basically we delete the game session right after loading the game to reset it and get the score 
        // back to 0 and such 

        //SceneManager.LoadScene("Game");
        //// this will load the scene with the name "Game", it's better to not load scene 
        //// by their names coz we could change the name in unity and it will not update here 

        if (FindObjectOfType<GameSession>()) // this is a check to see if GameSession is existed or not
        {
            FindObjectOfType<GameSession>().ResetGameSession();
            // this to reset the game and the score before the game start, by destroying GameSession class 
            // the first time the game launch GameSession is not yet created so that's why we put the if 
            // it's so that we don't get an error 
        }
    }

    public void QuitGame()
    {

        Application.Quit();

    }
}










