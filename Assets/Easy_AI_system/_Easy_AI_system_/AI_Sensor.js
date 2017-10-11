//----------------------------------------------------------------------------------------------
// Script checks and saves tagged object collided with this one
// Allow to get tag of triggeredObject 
//----------------------------------------------------------------------------------------------

#pragma strict

var triggeredObject: GameObject = null; // Object collided with this one

//===================================================================================================
// Gets tag of triggeredObject 
function GetTriggeredObjectTag():String 
{
  
  if (triggeredObject) return triggeredObject.tag;
    else return "null";
     
}

//----------------------------------------------------------------------------------
// Remember object (if it tag != "Untagged") in collision
function OnTriggerStay(other : Collider)
{ 
  if (other.tag != "Untagged") triggeredObject = other.gameObject; 
}

// Clear stored object if collision is over
function OnTriggerExit(other : Collider)
{  
  triggeredObject = null;
}

//----------------------------------------------------------------------------------
// Debug visualization
function OnDrawGizmos () 
{
    // Draw a semitransparent green cube at the transforms position
    if (triggeredObject) Gizmos.color = Color (1,0,0,.5);
       else
          Gizmos.color = Color (0,1,0,.5);
          
      Gizmos.DrawCube (transform.position, GetComponent.<Collider>().bounds.size);
}
//----------------------------------------------------------------------------------
