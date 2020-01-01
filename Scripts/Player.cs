using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{
    // config

    [SerializeField] float moveSpeed = 6f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] float climbLadderSpeed = 6f;
    [SerializeField] Vector2 dyingPunch = new Vector2(-5, 25);

    // State 

    bool isAlive = true;
    bool touchPortal = false;// when we touch the portal we change this to implement some effects
    bool playerHasHorizontalSpeed;
    bool playerHasVerticalSpeed;
    float gravityScaleAtStart;// we don't serialized this coz it's already in the inspector and serialized
                              // this var is to make the gravity on the ladder zero, and restore it after leaving the ladderm 


    float timePortalIsTouched;// this var used to mark the time when the playe hit the portal ( we use this time to create a timer)
    float rotationOfPlayerWhenTouchingPortal = -10f;

    // cached component references 

    Rigidbody2D myRigidBody;
    Animator myAnimator;
    CapsuleCollider2D myBodyCollider;
    BoxCollider2D myFeetCollider;
    ExitPortal exitPortal; // will use it to got the portal x and y 
    GameSession gameSession;
     
    // messages and methods 
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myBodyCollider = GetComponent<CapsuleCollider2D>();
        myFeetCollider = GetComponent<BoxCollider2D>();
        gravityScaleAtStart = myRigidBody.gravityScale; // we take the value of the gravity scale at the be

        touchPortal = false;

        exitPortal = FindObjectOfType<ExitPortal>();
        gameSession = FindObjectOfType<GameSession>();

        myBodyCollider.enabled = true;
        isAlive = true;





    }


    void Update()
    {


        if (isAlive && !touchPortal)
        {
            Run();
            ClimbLadder();
            Jump();
        }

        if (touchPortal)// when the touch portal is true we stop run and jump and climb and trigger the effect
        {

            ScalePlayer();
            RotatePlayer();
        }

        playerHasHorizontalSpeed = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;
        // mathf.Abs return the absolute value, mathf.Epsilon is basically zero 0 
        // so if there's a velocity > 0 this boolean will be true and if there's no velocity the bool will be false

        playerHasVerticalSpeed = Mathf.Abs(myRigidBody.velocity.y) > Mathf.Epsilon;

        FlipSprite();


    }

    private void ScalePlayer()
    {
        /*if (rotateVar > 0f)
        {
            rotateVar *= 0.9999f;
        }
        else
        {
            rotateVar = 0f;
        }*/
        //rotateVar = 0.1f;

        //this.transform.localScale = new Vector3((Mathf.Cos(rotateVar * 3.14159f) * 1f), (Mathf.Cos(rotateVar * 3.14159f) * 1f), 0f);

        // print(transform.localScale);
        //print((Mathf.Cos(rotateVar * 3.14159f) * 1f));
        //print(this.transform.localScale);
        // cos( Variable(StartPortal)*3.14159 )*1
        // 0.2*TimeDelta()
        // sin( Variable(Fade)*3.14159 )*255


        float timeSinceTouchPortal = Time.time - timePortalIsTouched;
        // Time.time return the time since the game start
        // this is like creating a timer, when we touch the portal we create a variable that hold the time at that moment
        float newScale = Mathf.Lerp(1f, 0f, timeSinceTouchPortal);
        transform.localScale = new Vector3(newScale, newScale, 1);
        // this will change the scale of the player between 1 and 0 
        // lerp function is a linear interpolation between the two number (1 and 0 in this example) 


        float newPostionX = Mathf.Lerp(this.transform.position.x, exitPortal.transform.position.x, timeSinceTouchPortal / 2);
        float newPostionY = Mathf.Lerp(this.transform.position.y, exitPortal.transform.position.y, timeSinceTouchPortal / 2);
        transform.position = new Vector3(newPostionX, newPostionY, 0);
        // this wil change the position of the player to the position of the portal slowly 
    }
    private void RotatePlayer()
    {
        this.transform.Rotate(0, 0, rotationOfPlayerWhenTouchingPortal);
        // this will rotate the player when touching the portal 

    }
    private void ClimbLadder()
    {
        // same as the Run() method concept 

        // we check if the player is touching the layer "Ladder" ( we assign the ladder gameObject to a layer called "Ladder" in unity)
        if (myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ladder")))
        {

            var controlThrow = Input.GetAxis("Vertical") * climbLadderSpeed;
            Vector2 climbVelocity = new Vector2(myRigidBody.velocity.x, controlThrow);

            myRigidBody.velocity = climbVelocity;
            myAnimator.SetBool("Climbing", true);
            // changing the "Climbing" parameter in unity to change the animation 

            myRigidBody.gravityScale = 0f;
            // when we're on the ladder we make the gravity zero to not slide on the ladder 
        }
        else
        {
            myRigidBody.gravityScale = gravityScaleAtStart;
            // after leaving the ladder we make the gravityScale the same as the start value
            myAnimator.SetBool("Climbing", false);
            // this line make sure that when we don't touch the ladder we quite the climbing animation 
        }

    }
      
    private void Run()
    {

        /*
          // this approach work on the position of the player 

        var deltaX = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed;
        // var deltaY = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed;

        // Input.GetAxis(""Horizontal") give back a float value between -1 and 1, if the user press right on the keyboard it
        // will return a positive value and if he press left it'll return a negative value, 
        // this method is automatically implemented by the left right up and down or the a s d w buttons that return values 
        // since we are using this method in the update , then it's called each frame , and we don't want to move the player 
        // based on the frame/sec coz it varies from a computer to another , so we need to move independently from the frames 
        // that why we use Time.deltaTime
        // Time.deltaTime is a value calculated by unity , this value return the duration of frame 
        // we multiply it by get axis value, so if the frame per seconds = 10 , then the duration of frame = 1/10 sec 
        // it the f/s =  100 the the duration of frame = 1/100 , so by multiplying we got the same result as a movement 
        // moveSpeed is a variable to control the speed of the mouvment 

        var newXpos = transform.position.x + deltaX;
        //var newYpos = transform.position.y + deltaY;

        transform.position = new Vector2(newXpos, this.transform.position.y);
        */


        // this approach work on the velocity of the player 

        var controlThrow = Input.GetAxis("Horizontal") * moveSpeed;

        // Input.GetAxis(""Horizontal") give back a float value between -1 and 1, if the user press right on the keyboard it
        // will return a positive value and if he press left it'll return a negative value, 
        // this method is automatically implemented by the left right up and down or the a s d w buttons that return values 
        // moveSpeed is a variable to control the speed of the mouvment 

        Vector2 runVelocity = new Vector2(controlThrow, myRigidBody.velocity.y);

        myRigidBody.velocity = runVelocity;
        // changing the velocity 


        myAnimator.SetBool("Running", playerHasHorizontalSpeed);
        // to change the parameter in the Animator component in unity we use the method SetBool("name of parameter", true or false ) :
        // here the running animation state is depend on if the player has horizontal speed or not 
    }
    
    private void Jump()
    {
        // basically to jump we add vertical velocity 
        // the jump in this game is a one click action, so we wont use input.getAxis("Vertical")
        // we'll use the input.GetButtonDown(), which trigger only once 

        if (Input.GetButtonDown("Jump"))
        {
            if (myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
            // we check if the player Collider2D is touching the Ground Layer ( which need to be added in unity to the foreground)
            // so it there's a collision (true) , we add velocity to jump else no 
            {
                Vector2 jumpVelocity = new Vector2(0, jumpSpeed);
                myRigidBody.velocity += jumpVelocity;

            }
        }

    }

    private void FlipSprite()
    {
        // this method is to flip the player when walking left and right 
        // to flip a sprite we need to change the x scale in the inspector from 1 to -1 


        if (playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(myRigidBody.velocity.x), 1f);
            // transform.localScale change the scale value, so we change the x scale from 1 to -1 depending on the velocity sign
        }
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {


        PlayerDieByEnemy();
        // when the player touch something as a trigger we call the die method 

        if (myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Portal")))
        {
            touchPortal = true;
            myRigidBody.gravityScale = 0;
            myRigidBody.velocity = new Vector2(0, 0);

            timePortalIsTouched = Time.time;


        }

    }

    private void PlayerDieByEnemy()
    {
        if (myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemy", "Hazards")))
        {
            // if the player collide with a layer Enemy we change the isAlive to false and change the animation to the die state 
            // we add two layers here, so whenever we collide with one of those layers the code below got compiled

            isAlive = false;
            myAnimator.SetBool("Dieying", !isAlive);

            Vector2 jumpVelocity = dyingPunch; // this is to make the player fly in the air when he dies 
            myRigidBody.velocity += jumpVelocity;
            myBodyCollider.enabled = false;// when touch an enemy we disable the collider to not touch more than once

            gameSession.PlayerHasDied();

        }
    }


}


