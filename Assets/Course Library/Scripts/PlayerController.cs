using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
   private float horizontalInput; 
   private float speed = 20f; 
   private float xRange = 20f; 
   public GameObject projectilePrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal"); 
        transform.Translate(Vector3.right * horizontalInput * Time.deltaTime * speed); 

        if (transform.position.x < -xRange) { 
            transform.position = new Vector3(-xRange, transform.position.y, transform.position.z); 
            } 

        else if(transform.position.x > xRange) { 
            transform.position = new Vector3(xRange, transform.position.y, transform.position.z); 
            } 

        if (Input.GetKeyDown(KeyCode.Space)) { 
            Instantiate(projectilePrefab, transform.position, projectilePrefab.transform.rotation); 
            }
    }
}
