using UnityEngine;

/* This class is not currently enabled in the game. It contains different ways of movement for the player, 
 * such as vertical movement with scrolling. I just use it in order to get a aerial view of the game 
 * while it runs which helps me troubleshoot. 
 */ 

public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    public float panSpeed = 50f; 
    public float scrollSpeed = 4;  
    private Vector3 dragOrigin;

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.position; 

        if(Input.GetKey("w")) {
            pos.z += panSpeed * Time.deltaTime; 
        }

        if(Input.GetKey("s")) {
            pos.z -= panSpeed * Time.deltaTime;
        }

        if(Input.GetKey("a")) {
            pos.x -= panSpeed * Time.deltaTime;
        }

        if(Input.GetKey("d")) {
            pos.x += panSpeed * Time.deltaTime;
        }

        float scroll1 = Input.GetAxis("Mouse X");   
        pos.z += scroll1 * scrollSpeed * Time.deltaTime; 
    

       float scroll2 = Input.GetAxis("Mouse ScrollWheel"); 
       pos.y += scroll2 * scrollSpeed * Time.deltaTime;  


        transform.position = pos;  
    }
}

