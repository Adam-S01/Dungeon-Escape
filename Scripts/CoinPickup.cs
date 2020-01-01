using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickup : MonoBehaviour
{

    [SerializeField] AudioClip coinPickupSFX;
    [SerializeField] float coinPickUpSFXVolume = 1f;
    [SerializeField] int coinScoreValue = 5;



    CapsuleCollider2D playerCapsulCollider;


    private void Start()
    {
       
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        playerCapsulCollider = FindObjectOfType<Player>().GetComponent<CapsuleCollider2D>();
        if (collision == playerCapsulCollider)
        // since the player have 2 colliders, when touching the coin sometimes the code compiled 2 times 
        // so i need to specify which collider need to be touch the coin for the code to be compiled
        {
           

            AudioSource.PlayClipAtPoint(coinPickupSFX, Camera.main.transform.position, coinPickUpSFXVolume);
            // PlayClipAtPoint() will play the sound even if the gameobject is destroyed 
            // and will take 3 parameter, the audio clip, the position and the volume 
            // the audio clip must be serialized and hooked, the position here we choose the camera position 
            // because when a game object is far from the camera the sound will be lower 
            // the volume is a float


            FindObjectOfType<GameSession>().AddToScore(coinScoreValue);
            // when a collision happen with a coin we need to add the coinScoreValue to the score in the GameSession class

            // this line is needed to prevent bugs coz detroy() method is excuted last so we need to deactivate

            Destroy(gameObject);
            // this will destroy this class 
            // in unity we make the layer of this class colliding with the player layer only in the collision matrix
            // so we don't need to specify which layer here this class is colliding with
        }


    }



}


