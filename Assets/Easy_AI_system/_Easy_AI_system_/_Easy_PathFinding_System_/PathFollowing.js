//----------------------------------------------------------------------------------
// The example script to follow path. 
// It manages waypointed path from pathFindingScript and move object along it.
//----------------------------------------------------------------------------------


#pragma strict

 
var pathFindingScript: PathFinding;				// Path holder/generator script
var damping = 3.0;								// Smooth facing/movement value
var movementSpeed: float = 5.0;					// Speed of object movement along the path
var waypointActivationDistance: float = 1.0;	// How far should object be to waypoint for its activation and choosing new
var stuckDistance: float = 2;                   // Max distance of move per regenTimeout that supposed to indicate stuking
var stuckTimeout: float = 2;                    // How fast should path be regenerated if player stucks

// Usefull internal variables, please don't change them blindly
private var currentWaypoint: int = 0;
private var targetPosition : Vector3;
var inMove: boolean = false;

private var oldPosition : Vector3;
private var timeToRegen: float;
//=============================================================================================================
// Setup initial data according to specified parameters
function Start () 
{
  	// Make the rigid body not change rotation
   	if (GetComponent.<Rigidbody>()) GetComponent.<Rigidbody>().freezeRotation = true;
}

//----------------------------------------------------------------------------------
//Main loop
function Update () 
{
  // Check if object is near target point
  if(Vector3.Distance(transform.position, pathFindingScript.target.position) < waypointActivationDistance) 
   {
    if (inMove) 
       { 
         pathFindingScript.FindPath();
    	 inMove = false;
       }
   }
    else
    {
	  
	   // Try to get next waypoint. If it is missed in some reason - set currentWaypoint to 1
		  try
		     targetPosition = pathFindingScript.waypoints[currentWaypoint]; 
			catch(err)
			    currentWaypoint = 1;

  	   // Activate waypoint when object is closer than waypointActivationDistance
	   if(Vector3.Distance(transform.position, targetPosition) < waypointActivationDistance) 
	    {
	      if (currentWaypoint > pathFindingScript.waypoints.length-1) currentWaypoint = 1;
	      currentWaypoint ++;
	    }
	      
		  
		  // Look at and dampen the rotation
			var rotation = Quaternion.LookRotation(targetPosition - transform.position);
			
			 transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * damping);
			 transform.Translate(Vector3.forward*movementSpeed*Time.deltaTime);
	
	    inMove = true;
	    
	    
	   if (Time.time > timeToRegen)
	    {
	      if (Vector3.Distance(transform.position, oldPosition) < stuckDistance) 
	       {
	          pathFindingScript.FindPath();
	          currentWaypoint = 1;
	       }
	      
	      oldPosition = transform.position; 
	      timeToRegen = Time.time + stuckTimeout;
	    }
	}
}

//----------------------------------------------------------------------------------
// Return true if object is moving now
function isMoving (): boolean 
{
	return inMove;
}

//----------------------------------------------------------------------------------