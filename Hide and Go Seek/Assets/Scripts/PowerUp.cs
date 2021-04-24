using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    GameObject player; 

    // Tracks how long the powerup has existed. 
	double startTime; 
	double curTime; 
	
    /* ID of the powerup. 
     * 0 = blue powerup (speed bonus)
     * 1 = green powerup (jump bonus)
     * 2 = magenta powerup (makes player invisible to enemies)
     */ 
    public int ID; 

    // Start is called before the first frame update
    void Start()
    {

        player = GameObject.Find("Main Camera");

        // Picks a random ID. 
    	ID = Random.Range(0, 3); 

    	startTime = Time.time;
        addProperties(); 
      //  setStartPosistion(Random.Range(-200, 200), Random.Range(-200, 200)); 
        setLightAndColor();
    }

    // Update is called once per frame
    void Update()
    {   
        // If the powerup has existed for 40 seconds, it is destroyed. 
        curTime = Time.time - startTime; 
        if(curTime >= 40) {
        	Object.Destroy(this.gameObject);
        }


    }

    void setStartPosistion(float x, float z) {
        this.gameObject.transform.position = new Vector3(x, 2f, z);
    }

    void addProperties() {
        this.gameObject.AddComponent<Rigidbody>(); 
        this.gameObject.GetComponent<Rigidbody>().useGravity = false;
        this.gameObject.AddComponent<Light>(); 
        this.gameObject.GetComponent<Light>().intensity = 5; 
        this.gameObject.GetComponent<Light>().type = LightType.Point;
    }

    void setLightAndColor() {
        if(ID == 0) {
    		 this.gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.blue);
    		 this.gameObject.GetComponent<Light>().color = Color.blue; 
    	}
    	else if(ID == 1) {
    		this.gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.green);
    		this.gameObject.GetComponent<Light>().color = Color.green; 
    	}

    	else if(ID == 2) {
    		this.gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.magenta);
    		this.gameObject.GetComponent<Light>().color = Color.magenta; 
    	}
    }

}
