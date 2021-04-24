using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEditor.AI;
using UnityEngine.UI;


// This class controls player movement, rotation, stamina, and the effects of power-ups. 
public class Player : MonoBehaviour
{
    // Movement variables 
	float speed = 6.0f; 
	float jumpSpeed = 8.0f; 
	float gravity = 20.0f; 
	Vector3 moveDirection = Vector3.zero; 
	CharacterController controller; 

    // Rotation variables 
	public float speedH = 2.0f; 
    public float speedV = 3.0f; 
    private float yaw = 0.0f; 
    private float pitch = 0.0f; 

    // Power-Up variables 
    public int boostID; 
    public bool boost = false; 
    public double boostStartTime; 
    
    // Stamina variables 
    public float stamina = 1f;
    float staminaDepleteTime = 2f;
    float staminaRegenTime = 10f;

    // Stamina slider reference. 
    public Slider s; 
   

    // Start is called before the first frame update
    void Start()
    {
    	controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {

        // This section determines player movement 
        Vector3 moveValues = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));  
        
        if(controller.isGrounded) {
        	moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")); 
        	moveDirection = transform.TransformDirection(moveDirection); 
        	moveDirection *= speed; 
        	if(Input.GetKey("space")) {
        		moveDirection.y = jumpSpeed; 
        	}
           
            // This part controls stamina depletion and recovery. 
            if(Input.GetKey(KeyCode.LeftShift) && stamina > 0) {
                stamina -= Time.deltaTime / staminaDepleteTime;
                if (stamina > 0f) {
                    moveDirection = moveDirection * 1.5f; 
                }
            }
            else {
                stamina += Time.deltaTime / staminaRegenTime;
                 }
            }
            stamina = Mathf.Clamp01(stamina);
            setSlider(); 


        moveDirection.y -= gravity * Time.deltaTime; 
        // Sets player movement. 
        controller.Move(moveDirection * Time.deltaTime);



        // Controls the player rotation 
        yaw += speedH * Input.GetAxis("Mouse X"); 
        pitch -= speedV * Input.GetAxis("Mouse Y"); 
        transform.eulerAngles = new Vector3(pitch, yaw, 0.0f); 

        // If the boost has been enabled for 10 seconds or more it gets turned off. 
        if(boost == true && Time.time - boostStartTime > 10) { 
            boostOff(); 
        }

    }

    // Sets the UI slider's value. 
    public void setSlider() {
        s.value = stamina * 10;
    }

    // Turns powerup on according to the powerup ID. 
    public void boostOn() {
        boostStartTime = Time.time; 
        boost = true;
        
        // Blue = Speed Boost 
        if(boostID == 0 && speed == 8f) {
            speed = speed * 2; 
        }
        // Green = Jump Boost 
        else if(boostID == 1 && jumpSpeed == 8f) {
            jumpSpeed = jumpSpeed * 2; 
            gravity = gravity / 2; 
        }

        // Magenta = Invisible to enemies boost. 
        if(boostID == 2) {
             gameObject.GetComponent<GameController>().distPlayer = 0f;
        }
    }

    // Turns the powerup off according to the powerup ID
    void boostOff() {
       
        boost = false; 
       
        // Turn off sped boost (blue spheres) 
        if(boostID == 0) {
            speed = speed / 2; 
        }
            
        // Turn off jump boost (green spheres)
        else if (boostID == 1) {
            jumpSpeed = jumpSpeed / 2; 
            gravity = gravity * 2; 
        }

        // Turns off the invisible to enemy boost (magenta spheres)
        else if(boostID == 2) {
            gameObject.GetComponent<GameController>().distPlayer = 30.0f; 
        }
    }
}
    

