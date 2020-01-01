using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    [SerializeField] float enemyMoveSpeed = 1f;


    // State 
     
    // cached component references 

    Rigidbody2D myRigidBody;
     

    // messages and methods 
    
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
         
    }
     
    // Update is called once per frame
    void Update()
    {
        Move();

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        transform.localScale = new Vector2(-Mathf.Sign(myRigidBody.velocity.x), 1f);
        // mathf.sign() return 1 if the value > 0 and -1 if the value < 0
        // onTriggerExit2D is called when the 2 collider collide and then exit
        // we use for the enemy 2 collider , we put one in front of him, so that when it touch the ground and then leave
        // it, the onTriggerExit2D is called and it flip the sprite coz it multiply the scale by -1
        // so even thought we have 2 collider and we did not specify one of them this method still works 
        // but note that one of the collider is ontrigger checked, the other is not, so the this method works on for the ontrigger checked

    }

    private void Move()
    {

        if (IsFacingRight())
        {
            myRigidBody.velocity = new Vector2(enemyMoveSpeed, 0f);
            // if enemy facing right move right
        }
        else
        {
            myRigidBody.velocity = new Vector2(-enemyMoveSpeed, 0f);
            // if enemy facing left move left 
        }
        
    }

     bool IsFacingRight()
    {
        return transform.localScale.x > 0;
        // this methode is a calculation of the enemy if it's facing right or left
        // it's better to calculate than to store it in a variable 
    }

    

}
