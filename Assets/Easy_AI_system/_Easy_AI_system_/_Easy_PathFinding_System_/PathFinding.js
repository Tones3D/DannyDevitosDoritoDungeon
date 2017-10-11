//-----------------------------------------------------------------------------------------------
// Main script of this Path finding system. 
// Calculates(find) path automatically or according to specified rules
// Generates array of waypoints (around obstacles) until target will be reached      
//-----------------------------------------------------------------------------------------------

#pragma strict


var target: Transform;						// Target/final point to build path to
var waypoints = Array();			        // Array of generated waypoints
var maxComplexity: int = 15;				// Max number of waypoins in path
var maxLookingDistance: float = 50.0;		// Max distance of raycasts
var offsetFromObstacles: float = 1;			// Set additional offset between waypoints and  obstacles
var autoUpdateTime: float;					// Delay to next path recalculation. Works  automatically if updateOnTargetMove & manualUpdateOnly = false;
var updateOnTargetMove: boolean = true;		// Update only if new target position different  from previous one
var manualUpdateOnly: boolean = false;		// Allow only manual updates  by calling "FindPath" function
var useZAxisAsHeight: boolean = false;      // By default path calculates in XZ plane, set it to true to use XY plane
var ignoreTargetHeight: boolean = true;     // Ignore target Y (or Z) offset from this object 
var color: Color = Color(0,1,0, 0.5);       // Debug path-visualization color
var yOffset: float = 0.1;			  		// Custom offset from ground

var noPathFound: boolean = false;


// Usefull internal variables, please don't change them blindly
private var recalculatePathTime: float;
private var oldTargetPosition: Vector3;
private var wasRecalculated: boolean;


//================================================================================================
// Find path on object creation
function Start () 
{
  
  FindPath ();
}

//----------------------------------------------------------------------------------
// Makes path finding automatically, according to specified rules
function Update () 
{
  if(!manualUpdateOnly)
    {
      // Update only if new target position different from previous one
       if (updateOnTargetMove)
         {
	      if (target.position != oldTargetPosition)
	         // and only if autoUpdateTime passed
			  if (Time.time > recalculatePathTime)
			    {
			      FindPath ();
			      recalculatePathTime = Time.time + autoUpdateTime;
			      oldTargetPosition = target.position;
		        }
	     } 
	    else
	     // Update by timer
	      if (Time.time > recalculatePathTime)
		   {
			 FindPath ();
			 recalculatePathTime = Time.time + autoUpdateTime;
		   }  
    }
}

//----------------------------------------------------------------------------------
// Recalculate path. Main function that finds path and generate waypoints
// Please also use this function to manually initiate FindPath procedure

function FindPath () 
{ 
    // Usefull internal variables, please don't change them blindly
     var hit : RaycastHit;
     var ray : Ray;
     var pos : Vector3;
     var dir : Vector3;
     var raycastedPoint  : Vector3; 
     var lookingDistance : float;
     var targetPosition : Vector3;
 
    // Reset path
     waypoints.clear();
     waypoints.Add(transform.position);   
              
	 lookingDistance = maxLookingDistance;
	 targetPosition = target.position;
	 
	 // Ignore Targets height and assign vertical axis
	 if (ignoreTargetHeight)
		if (useZAxisAsHeight)
			 targetPosition.z = transform.position.z;
		   else
		       targetPosition.y = transform.position.y;
              
           
    // Set start point and direction            
     pos = waypoints[waypoints.length-1]; 
	 dir = targetPosition - pos;
	                            
	 noPathFound = false;                                    
	                                            
  // Main loop. Generate waypoints around obstacles until target be  reached (or waypoints quantity become bigger than maxComplexity)                                                                    
     while(waypoints[waypoints.length-1] != targetPosition  && (waypoints.length < maxComplexity) ) 
      { 
       
	       ray = Ray (pos, dir);
	      
	      // Raycast from last finded waypoint to choosed direction (straight to target or around current obstacle)
	 	  if (Physics.Raycast (ray, hit, lookingDistance)) 
		    {
		    
		       // If there is  obstacle - create new  waypoint in front of it
		        raycastedPoint = ray.GetPoint(hit.distance-offsetFromObstacles);
		        waypoints.Add(raycastedPoint);
				
				
				// Calculate normal around obstacle (taking(or not) into account vertical coordinates)
			    if (useZAxisAsHeight)
				   { 
					  dir.x = hit.normal.x*Mathf.Cos(1.570796) - hit.normal.y*Mathf.Sin(1.570796); 
					  dir.y = hit.normal.x*Mathf.Sin(1.570796) + hit.normal.y*Mathf.Cos(1.570796); 
					  dir.z = 0;
						      
					  if(hit.collider.bounds.size.x > hit.collider.bounds.size.y)
				          	lookingDistance = hit.collider.bounds.size.x*(1+offsetFromObstacles);
				          else
				          	lookingDistance = hit.collider.bounds.size.y*(1+offsetFromObstacles);
			        }
			       else
			        {
				      dir.x = hit.normal.x*Mathf.Cos(1.570796) - hit.normal.z*Mathf.Sin(1.570796); 
					  dir.z = hit.normal.x*Mathf.Sin(1.570796) + hit.normal.z*Mathf.Cos(1.570796); 
					  dir.y = 0;
				  
				     // Choose looking distance lenght equal to max obstacle bounds extents
					  if(hit.collider.bounds.size.x > hit.collider.bounds.size.z)
				          	lookingDistance = hit.collider.bounds.size.x*(1+offsetFromObstacles);
				           else
				          	lookingDistance = hit.collider.bounds.size.z*(1+offsetFromObstacles);
			        }        
			          	
			          	
			          	
			      if (lookingDistance>maxLookingDistance)  lookingDistance = maxLookingDistance;              
					                  
			      pos = waypoints[waypoints.length-1];  

 				  // Set direction if there are more obstacles. Choose side to move
                  if (Physics.Raycast (pos, targetPosition - pos, hit, lookingDistance)) 
                     {
					  if (Vector3.Distance(targetPosition, pos+dir*lookingDistance) > Vector3.Distance(targetPosition, pos-dir*lookingDistance))
  					     dir = (pos-dir*lookingDistance)-raycastedPoint;
  					 }
  					else // If there is no other obstacles - set direction straight to target
  					  dir = targetPosition - pos;
    					   
			   }
		      else
		        {
		         if(Vector3.Distance(pos, targetPosition) < maxLookingDistance)
		          {
			         // If there is no colliders - set direction straight to target and create new waypoint.
			          if (dir == (targetPosition - pos))  
						      raycastedPoint = targetPosition;
				            else
				              raycastedPoint = ray.GetPoint(lookingDistance-offsetFromObstacles);
				         
			           lookingDistance = maxLookingDistance;
			           waypoints.Add(raycastedPoint);
	  
			           pos = waypoints[waypoints.length-1]; 
			           dir = targetPosition - pos; 
		           }
		           else
		            {
		             noPathFound = true;
		             break;
		            }
	            }
  		 
	   }    
  
   if (waypoints.length >= maxComplexity) noPathFound = true;
}

//----------------------------------------------------------------------------------
// Draw debug visualization
function OnDrawGizmos() 
{
  Gizmos.color = color;
  
  if (waypoints.length > 0)
    for (var i=0; i<(waypoints.length-1); i++)
         {
           Gizmos.DrawWireSphere(waypoints[i], offsetFromObstacles/2);
           Gizmos.DrawWireSphere(waypoints[i+1], offsetFromObstacles/2);
           
           Gizmos.DrawLine (waypoints[i], waypoints[i+1]);
         }
}
 
//----------------------------------------------------------------------------------