# CS483-Final-Project

  Hide and Seek Game
-----------------------
      
Gameplay and controls: 
  1. The player is controlled using the awsd keys, and mouse. 
  They can also jump with the space bar and sprint for a short time with the left shift key. 
       
  2. Each game lasts 5 minutes, to win the game the player must avoid coming in contact with the enemy agents (black spheres with red lights). 
  The player can get power ups by jumping into and colliding with the power up spheres. 

  3. Three power ups spawn in the game in the form of floating spheres. The blue sphere increases the player's speed, the green sphere increases the player's         jump height, and the purple sphere makes the player invisible to the agents. Each power up lasts for 10 seconds. Players can have multiple powers ups               activated at one time as long as they are not the same power. Power ups can be tricking to trigger collisions with so be careful trying to gain one while           being chased by the agents. 

  4. The enemy agents move to random points on the map. When they reach their destinations they are assigned a new one. If any of the enemies come within a           specified range of the player, they are all alerted of the player's position and the player must try to avoid them while getting to a safe range. Once the           player is at a safe range the enemies lose the position and go back to randomly exploring the map. 

  5. When enemies are not tracking the player, they have no lights activated, meaning extra caution must be taken not to stumble upon them. If they are               tracking the player their lights are turned to red. 
    
  6. At the end of the game, a "start game" button will appear in the left. If clicked a new game starts. 



Known Issues: 

 1. On rare occasions the player phases through the floor at the start of the game. The "start game" button is enabled if they do. This can be used to restart        the game. 

2. The navigation mesh that the enemies and power ups are spawned on sometimes misinterprets the space on top of and between the wider walls. It maps them as       navigatable space for the mesh leading some agents and power ups to be spawned on top of or between a wall.  
       
