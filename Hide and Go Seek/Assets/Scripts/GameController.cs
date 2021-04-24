using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI; 
using UnityEditor.AI;
using UnityEngine.AI;

public class GameController : MonoBehaviour
{
    // Variables to track and display how much time is left in the game. 
    public GameObject timerObject; 
    Text timerText; 
    int timerSeconds = 300; 

    // The navigation map that is used to spawn in enemies and power-ups  
    public NavMeshSurface surface; 

    // The maximum distance enemies can percieve the player. 
    public float distPlayer = 30; 

    List<Enemy> agentList = new List<Enemy>(); 
        
    // Used to spawn in more enemies (1 minute intervals) and power-ups (40 second intervals)
    double startTime;
    
    bool changeCheck = false; 


    // References to the texts and button that appear at the game's end. 
    GameObject start;
    GameObject gameEndText; 
    GameObject winText; 


    // Start is called before the first frame update
    void Start()
    {
        // Disables the end game UI components. 
        start = GameObject.Find("StartButton"); 
        start.gameObject.SetActive(false);
        gameEndText = GameObject.Find("GameOverText");
        gameEndText.gameObject.SetActive(false);
        winText = GameObject.Find("winText");
        winText.gameObject.SetActive(false);
                 
        // Create ground for game. 
        GameObject ground = GameObject.CreatePrimitive(PrimitiveType.Plane);
        int planeScale = 40; 
        ground.transform.localScale = new Vector3(planeScale, planeScale, planeScale); 


        // createWalls2((planeScale * 10) / 2); 
        Maze maze = new Maze(400, 400); 

        // Creates the navigation mesh surface for the agents and power-ups. 
        surface = GameObject.Find("navmesh").GetComponent<NavMeshSurface>();
        surface.BuildNavMesh();

        createEnemies(26); 
        createPowerUps(15);
    
        // Starts the game clock 
        startTime = Time.time; 
        timerObject = GameObject.Find("timerText"); 
        timerText = timerObject.GetComponent<Text>();
        InvokeRepeating("timer", 0f, 1f);

        
    }

    /* The main purpose of the update function here is to check if any of the enemies are within range to "see" the player.
     * If any of the enemies can see the player, they all obtain the player's position and head there. If the player gets a 
     * distance of 30f or more from each of the enemies then they are no longer locked on the player's position and navigate to a 
     * random point on the navigation mesh. 
     *
     * The update function here also adds a new set of power-ups if 40 seconds have passed.
     * Each new set of power-ups is set to appear at 40 second intervals (as the old ones dissapear). 
     */
    void Update()
    {
        if(timerSeconds <= 0) {
            Destroy(this.GetComponent<Player>()); 
           // Destroy(GameObject.Find("timerText"));
            winText.gameObject.SetActive(true);
            start.gameObject.SetActive(true);
        }


        // Spawns power-ups. 
        if((Time.time - startTime) % 40 < 0.09 && Time.time - startTime >= 40) {
            createPowerUps(7);
        }

        bool lockedOnPlayer = false; 

        // Check if any enemies are within range of the player.
        for(int i = 0; i < agentList.Count; i ++) {
            if(Vector3.Distance(gameObject.transform.position, agentList[i].transform.position) <= distPlayer) {
                lockedOnPlayer = true; 
            }
        }

        // Label "locked on the player" true or false for all enemies. (This value will be true if the above loop has any enemies within 30f). 
        for(int i = 0; i < agentList.Count; i ++) {
            agentList[i].locked = lockedOnPlayer; 
            agentList[i].setTarget(); 

        }

        /* There is an occasional error where the player falls through the plane at the start of the game. 
         * This code adds the start new game option to the screen if the player's y value gets too low. 
         * This way they can restart if the error occurs. 
         */ 
        if(transform.position.y <= -15) {
            start.gameObject.SetActive(true);
            gameEndText.gameObject.SetActive(false);
        }
    }




    void OnCollisionEnter(Collision other)
    {
        GameObject player = GameObject.Find("Main Camera"); 

        // If a enemy is hit the game is over and a start new game button appears. 
        if(other.gameObject.name == "Enemy") {
            Destroy(this.GetComponent<Player>()); 
            timerText.gameObject.SetActive(false);
            gameEndText.gameObject.SetActive(true);
            start.gameObject.SetActive(true);
        }

        // If a power-up is hit then 
        if(other.gameObject.name == "PowerUp") {
            player.GetComponent<Player>().boostID = other.gameObject.GetComponent<PowerUp>().ID; 
            Destroy(other.gameObject);
            player.GetComponent<Player>().boostOn();
        }
               
    }


    // timer that controls the game clock. 
    void timer() {
        if(timerSeconds <= 0) {
            // Stop game
        }
        int minutes = timerSeconds / 60; 
        int seconds = timerSeconds % 60; 
        if(seconds < 10) {
             timerText.text = minutes.ToString() + ":0" + seconds.ToString();
        }
        else {
            timerText.text = minutes.ToString() + ":" + seconds.ToString();
        }
        timerSeconds --; 
    }

    // This function calculates and returns a valid position on the navigation mesh.  
    public Vector3 RandomNavPoint(Vector3 origin, float distance, int layermask) {
            Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * distance;
           
            randomDirection += origin;
           
            NavMeshHit navHit;
           
            NavMesh.SamplePosition (randomDirection, out navHit, distance, layermask);
           
            return navHit.position;
        }

    // Creates "n" enemies at random points on the navigation mesh
    void createEnemies(int n) {
        for(int i = 0; i < n; i ++) {
            GameObject enemy = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            enemy.AddComponent<Enemy>();
            enemy.name = "Enemy"; 
            enemy.transform.position = RandomNavPoint(enemy.transform.position, 400, -1);
            agentList.Add(enemy.GetComponent<Enemy>());
        }
    }

    // Creates "n" powerups at random points on the navigation mesh
    void createPowerUps(int n) {
        for(int i = 0; i < n; i ++) {
            GameObject powerup = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            powerup.AddComponent<PowerUp>(); 
            powerup.name = "PowerUp"; 
            powerup.transform.position = RandomNavPoint(powerup.transform.position, 400, -1);
            float y = powerup.transform.position.y; 
            powerup.transform.position += new Vector3(0f, (y/1000000) + 2, 0f);
        }
    }

    public void RestartGame() {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name); // loads current scene
    }
}
