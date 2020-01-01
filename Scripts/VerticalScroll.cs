using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalScroll : MonoBehaviour
{

    [Tooltip("Game units per second")]
    // this will add a tip to the bellow serialized field in unity 
    [SerializeField] float verticalScrollSpeed = 0 ;
    [SerializeField] float horizontalScrollSpeed = 0 ;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector2(horizontalScrollSpeed * Time.deltaTime, verticalScrollSpeed * Time.deltaTime));
        // normal vertical translation for the water
    }
}
