using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* This class generates the boundary walls surrounding the ground plane and 
 * creates an interior maze for the game to take place in. 
 * The maze algorithm I took from the first homework assignment and modified it a little 
 * bit to fit this project. 
 */ 
public class Maze : MonoBehaviour
{
     
    // Start is called before the first frame update
    public Maze(float boundX, float boundZ)
    {
        createBoundaryWall(new Vector3(200, 25, 2), 90);
        createBoundaryWall(new Vector3(-200, 25, 2), 90);
        createBoundaryWall(new Vector3(2, 25, 200), 0);
        createBoundaryWall(new Vector3(2, 25, -200), 0);
        recursiveDivision(-200f, -200f, boundX, boundZ, chooseDirection(boundX, boundZ));

    }

    // Creates the exterior walls. 
    void createBoundaryWall(Vector3 position, float rotate) {
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            float wallWidth = 0.5f; 
            float wallHeight = 50; 
            cube.transform.localScale = new Vector3(400, wallHeight, wallWidth);
            cube.transform.position = position;
            cube.transform.eulerAngles = new Vector3(
                        cube.transform.eulerAngles.x,
                        cube.transform.eulerAngles.y + rotate,
                        cube.transform.eulerAngles.z); 
            cube.GetComponent<Renderer>().material.SetColor("_Color", Color.grey);
    }

    // Creates a wall perpendicular to the longer side (x or z). 
    void recursiveDivision(float x, float y, float width, float height, bool d) {
        // End Recursion 
        if(width < 50 || height < 50) {
            return; 
        }
        System.Random r = new System.Random();

        // Chooses coordinates to create the wall from. 
        float x1 = x + (d ? width / 2: (float) r.Next(0, (int) width - 2));
        float y1 = y + (d ? (float) r.Next(0, (int) height - 2): height / 2f);  


        float length = d ? width : height; 

        // The space to be added in the wall
        float passage = (float) r.Next(0, (int) length) + 1; 


        float rotate = 0f; 

        // Rotate wall 90 degrees if the wall is horizontal.
        if(d) {
            rotate = 90f; 
        }

        // Creates the wall and passages through it. 
        createCube(x1, y1, rotate, 0.2f, passage);
        createCube(x1, y1, rotate, 0.2f, length - passage);


        /* Next section calls recursion with a newly divided area. 
         * Recursion ends when width or height == 50. 
         */ 

        // Left / down
        float nx = x; float ny = y; 
        
        float w = d ? width: x1-x + 1; 
        float h = d ? y1 - y + 1: height; 
        recursiveDivision(nx, ny, w, h, chooseDirection(w, h)); 

        // Right / up 
        nx = d ? x : x1+1; 
        ny = d ? y1+1 : y;    
        w = d ? width : x + width - x1 - 1; 
        h = d ? y + height - y1 - 1 : height; 
        recursiveDivision(nx, ny, w, h, chooseDirection(w, h)); 


    }

    /* Returns true if the height is larger or equal to the width.
     * True indicates that a horizontal wall should be created. 
     * False indicates that a vertical wall should be created.
     */ 
    bool chooseDirection(float x, float y) {
        if(x <= y) {
            return true; 
        }
        
        return false; 
    }


    // Creates one of the maze's walls. 
    GameObject createCube(float x, float z, float rotate, float xScale, float zScale) {
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            float wallWidth = Random.Range(1, 8); 
            float wallHeight = Random.Range(10, 20); 
            cube.transform.localScale = new Vector3(wallWidth, wallHeight, zScale);
           	cube.transform.position = new Vector3(x, wallHeight/2, z);
            cube.transform.eulerAngles = new Vector3(
                        cube.transform.eulerAngles.x,
                        cube.transform.eulerAngles.y + rotate,
                        cube.transform.eulerAngles.z); 

            cube.GetComponent<Renderer>().material.SetColor("_Color", Color.grey);
            return cube; 
        }


}

