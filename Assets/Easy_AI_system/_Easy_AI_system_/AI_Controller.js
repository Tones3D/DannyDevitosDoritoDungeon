//---------------------------------------------------------------------------------------------
// Main script that control/manage all AI states and requests related actions (and animations)
// Script also conected to PathFinding system to handle navigation activity
//---------------------------------------------------------------------------------------------

#pragma strict

// List of predefined actions. Please change it if needed
 public enum AI_ActionTypes 
 { 
   None,
   Idle,
   Seek,
   Move,
   Attack,
   RunAway,
   Patrol,
   Heal,
   Die,
   Follow
 }
 

// Basic and universal parameters for  actions
class AI_Action
{
 var Caption: String;							// Just a caption for more comfortable navigation
 var actionType: AI_ActionTypes;				// Action type. Function with this name will be called to perform this action
 var actionPriority: int;						// Priority. Only actions with higher priority can interupt actions with lower
 var actionDuration: float;						// During this time  action can't be interupted by action with lower priority. 
 var animationClip: AnimationClip;				// Play this animation during the action
 var loopAnimation: boolean = false;			// Specifies should be animation looped during action Duration or not
 var customValue: float;						// Just some value that you can use in functions to setup/tune some parameters of action
}


var viewTrigger: AI_Sensor;				  // Script attached to object used as view-field of AI. It's collider size useful for ranged attack distances too
var pathFollower: PathFollowing;          // Script to process PathFinding and PathFollowing
var animatedObject: GameObject;		      // Object that contains visual player appearance and animations
var enemyTag: String;					  // Tag of object, that regarded as enemy
var followAlly: GameObject;				  // If specified - AI will follow this object/character
var enableRangedAttack: boolean = true;   // Enable range attack. Only mele is allowed if  false
var enablePatrol: boolean = true;         // If true - AI will patrol territory instead of simple Seeking
var life: float = 100;					  // Initial/max amount of life
var actions: AI_Action[];                 // List of all possible AI actions



// Important internal value. Please don't change it blindly
var currentAction: AI_Action; 	// Current requested action - this is internal variable, but it should be public
private var meleeAttackDistance: float = 2;
private var rangedAttackDistance: float = 20;  
private var lifeCriticalPecent: float = 0.3;
private var currentLife: float;
private var timer: float;
private var enemy: GameObject;
private var runAwayObject: GameObject;


//===================================================================================================
// Assign gameCursor to playerDirector.gameCursor - just to use the same GameCursor script
function Start () 
{
 // Calculate distances for ranged and  melee attacks. 
 // Based on viewTrigger collider size and waypointActivationDistance specified in pathFollower
  rangedAttackDistance = GetMaxLookingDistance(viewTrigger.gameObject.GetComponent.<Collider>());
  meleeAttackDistance = pathFollower.waypointActivationDistance*1.2;
  currentLife = life;
 
 // Create empty object, that will be used as path target for RunAway and Patrol activities
  runAwayObject = new GameObject();
  runAwayObject.name = gameObject.name+"_Runaway_Target";
  runAwayObject.transform.position = transform.position;
 // runAwayObject.transform.parent = transform;
  
  enemy = runAwayObject;
 
  // Set Idle as default action 
  ManageAction(GetActionByID(AI_ActionTypes.Idle));
  
}

//----------------------------------------------------------------------------------
// Allows to enable/disable pathFinding and PathFollowing scripts and  specify ne target for them
function EnablePathFollowing ( isEnabled: boolean, pathTarget: GameObject)
{
   if (pathTarget) pathFollower.pathFindingScript.target = pathTarget.transform;
   
   pathFollower.enabled = isEnabled;
   pathFollower.pathFindingScript.enabled = isEnabled;
   
}

//----------------------------------------------------------------------------------
// Returns max looking distance for this AI. Based on max viewTrigger collider size
function GetMaxLookingDistance (collider: Collider): float
{
  var maxDistance: float;
  
  if (collider.bounds.size.x < collider.bounds.size.z) maxDistance = collider.bounds.size.z;
  if (maxDistance < collider.bounds.size.y) maxDistance = collider.bounds.size.y;
  
  maxDistance -= maxDistance/2 - Vector3.Distance(transform.position, collider.bounds.center);

  return maxDistance;
  
}

//----------------------------------------------------------------------------------
// Gets action number in actions list by ID
function GetActionByID (actionID: AI_ActionTypes): AI_Action
{ 
  for (var i=0; i<actions.Length; i++)
	 if (actions[i].actionType == actionID) return actions[i];
}

//----------------------------------------------------------------------------------
// Perform specific actions requested from PlayerController script according to rules specified for this action (like activationDistance, Priority, etc)
function ManageAction (action: AI_Action) 
{

 // if requested the same  action as  previously
  if (action == currentAction)
    {
      // Process action-specific timer
	  if (timer>0) 
		   {
		     timer-=Time.deltaTime;
		     
		     // Loop animation if it's continues and it's required
		     if(currentAction.animationClip)
		       if(!animatedObject.GetComponent.<Animation>().isPlaying)
		          if (currentAction.loopAnimation) animatedObject.GetComponent.<Animation>().CrossFade(currentAction.animationClip.name);
		             else timer=0;
		   }
		   
		   
       // Call specific function to process action
	   SendMessage (action.actionType.ToString());  
    }
  else  
    // Try to switch to new action, according to it priority and current  action timer
    if (action.actionPriority > currentAction.actionPriority || timer <=0) 
     {
		// Switch animation to assigned to the action by default
	    if (animatedObject.GetComponent.<Animation>() && action.animationClip) animatedObject.GetComponent.<Animation>().CrossFade(action.animationClip.name);
        		//  else Debug.Log ("WARNING: There aren't animations attached to " + animatedObject.name);   
  
	    currentAction = action;
	    timer = currentAction.actionDuration;
       
       
        // Call specific function to process action
	   SendMessage (action.actionType.ToString());  
     }
   
   
    
     
}

//----------------------------------------------------------------------------------
// Function to check and  perform basic states(actions)
function Update () 
{
   var requestedAction: AI_ActionTypes = currentAction.actionType;

    // Activate chasing if AI doesn't enemy in viewTrigger
    if ((enemy==runAwayObject) && (viewTrigger.GetTriggeredObjectTag() == enemyTag))  
	    {
	      enemy = viewTrigger.triggeredObject;
	      EnablePathFollowing(true, enemy);
	    }
   
   // If AI have enemy in view
	if (enemy!=runAwayObject)    
	  // Runaway if life is in critical state
	   if (currentLife < life*lifeCriticalPecent)  requestedAction = AI_ActionTypes.RunAway; 
	     else
		    // If enemy exists - chase or attack it
		     if(enemy)
		        {
			       // If enemy  on attack distance  -  Attack it
			        var distance: float = Vector3.Distance(enemy.transform.position, transform.position);
			        if ((enableRangedAttack && distance<=rangedAttackDistance) || distance <= meleeAttackDistance)
			           {
			            EnablePathFollowing(false, null);
			            requestedAction = AI_ActionTypes.Attack;
			           }
			          else // Chase
			            {
			             EnablePathFollowing(true, null);
			             requestedAction = AI_ActionTypes.Move;
			            }
		         }
		       //if enemy lost - reset
		       else
		        {
		          enemy = runAwayObject;
		          pathFollower.pathFindingScript.target = runAwayObject.transform;
		          EnablePathFollowing(false, null);
		          ManageAction(GetActionByID(AI_ActionTypes.Idle));
		        }

   // Die if life <= 0
   if (currentLife <=0) requestedAction = AI_ActionTypes.Die;
 
   // Request action
   ManageAction(GetActionByID(requestedAction));
   
}

//===================================================================================================
// There are TEMPLATE functions, please update them or create new if needed
// Action-specific function should be called according to requested action
// Their names should be exactly the same as entries in ActionTypes.
//===================================================================================================
// Perform action - Idle 
function Idle () 
{ 
 // if followAlly is specified then follow it, else Seek or Patrol
  if (followAlly) ManageAction(GetActionByID(AI_ActionTypes.Follow));
     else
       if(enablePatrol) ManageAction(GetActionByID(AI_ActionTypes.Patrol));
        else ManageAction(GetActionByID(AI_ActionTypes.Seek));
}

//----------------------------------------------------------------------------------
// Perform action - Patrol 
function Patrol () 
{ 
 // Get random target in specified(in customValue) radius
  if (!pathFollower.enabled)
   {
 	 runAwayObject.transform.position = transform.position + Random.onUnitSphere * currentAction.customValue;
	 runAwayObject.transform.position.y = transform.position.y;
	 EnablePathFollowing(true, runAwayObject);
	 yield WaitForEndOfFrame;
   }
  else
  // If current target is reached -  go to Idle  state
   if(Vector3.Distance(runAwayObject.transform.position, transform.position) < meleeAttackDistance)
     {
       EnablePathFollowing(false, null);
       enemy = runAwayObject;
       ManageAction(GetActionByID(AI_ActionTypes.Idle));
     }
  
  
  
   // If enemy was spotted and on attack distance -  go to Attack
   if (enemy!=runAwayObject) 
    {
	  var distance: float = Vector3.Distance(enemy.transform.position, transform.position);
	  if ((enableRangedAttack && distance<=rangedAttackDistance) || distance <= meleeAttackDistance)
	      {
	        EnablePathFollowing(false, null);
	        ManageAction(GetActionByID(AI_ActionTypes.Attack));
	      }
    }
}

//----------------------------------------------------------------------------------
// Perform action - Follow specified object
function Follow () 
{ 
  EnablePathFollowing(true, followAlly);
  ManageAction(GetActionByID(AI_ActionTypes.Move));
  
}

//----------------------------------------------------------------------------------
// Perform action - Seek 
function Seek () 
{ 
  transform.RotateAround(Vector3.up, Time.deltaTime/currentAction.customValue);
  
}

//----------------------------------------------------------------------------------
// Perform action - Move 
function Move ()
{  
  // If noPathFound  -  go to Idle  state
  if (pathFollower.pathFindingScript.noPathFound) 
     {
      enemy = runAwayObject;
	  pathFollower.pathFindingScript.target = runAwayObject.transform;
	  ManageAction(GetActionByID(AI_ActionTypes.Patrol));
	 // EnablePathFollowing(false, null);
     // ManageAction(GetActionByID(AI_ActionTypes.Idle));
     }
}

//----------------------------------------------------------------------------------
// Perform action - Attack enemy
function Attack ()
{ 
  // If no enemy have been spotted
  if (enemy==runAwayObject) 
     {
    
      // If followAlly is specified then follow it
      if (followAlly) ManageAction(GetActionByID(AI_ActionTypes.Follow));
         else
     		{
     		  pathFollower.pathFindingScript.target = runAwayObject.transform;
      		  ManageAction(GetActionByID(AI_ActionTypes.Idle));
      		}
     }
     else
	   { 
	    var offsettedPosition: Vector3 = transform.position;
        offsettedPosition.y += pathFollower.pathFindingScript.yOffset;
         
	    // If enemy  on attack distance and no obstacles betwen it and AI -  Attack it
		if(!Physics.Raycast (offsettedPosition, enemy.transform.position-offsettedPosition, rangedAttackDistance))
		 { 
		    transform.LookAt(enemy.transform);
		    
		    // Decrease enemy life every actionDuration seconds for customValue amount
		    if (timer<=0)  
		     {
		       enemy.BroadcastMessage("ApplyDamage", currentAction.customValue, SendMessageOptions.DontRequireReceiver);
		       timer = currentAction.actionDuration;
		     }
		         
		  }
		   // If enemy is far then max attack distance - chase it
		   else 
		       ManageAction(GetActionByID(AI_ActionTypes.Move));
	}
 
}

//----------------------------------------------------------------------------------
// Perform action - RunAway from enemy
function RunAway ()
{ 
 // Get random target in specified(in customValue) radius
  if(Vector3.Distance(enemy.transform.position, transform.position) < meleeAttackDistance || !pathFollower.enabled)
    {
	   runAwayObject.transform.position = transform.position + Random.onUnitSphere * rangedAttackDistance*currentAction.customValue;
	   runAwayObject.transform.position.y = transform.position.y;
	   EnablePathFollowing(true, runAwayObject);
	 }

   yield WaitForEndOfFrame;
   
   
    // If current target is reached -  go to Heal  
   if(Vector3.Distance(runAwayObject.transform.position, transform.position) < meleeAttackDistance)
     {
     // EnablePathFollowing(false, null);
      enemy = runAwayObject;
      ManageAction(GetActionByID(AI_ActionTypes.Heal));
     }
}

//----------------------------------------------------------------------------------
// Perform action - Heal 
function Heal ()
{ 
  // Increase life every actionDuration seconds in customValue amount
   if (timer<=0.1)  
	{
		 currentLife += currentAction.customValue;
		 timer = currentAction.actionDuration;
		   
		   // If fully healed - go to Idle  state
		 if (currentLife >= life) 
			   {
			     currentLife = life;
			     enemy = runAwayObject;
			     EnablePathFollowing(false, null);
			     timer = -1;
			     ManageAction(GetActionByID(AI_ActionTypes.Idle));
			   }
	 }

}

//----------------------------------------------------------------------------------
// Perform action - Die 
function Die ()
{ 
 // Disable and destroy AI and all related scripts/objects
   EnablePathFollowing(false, null);
   gameObject.tag = "Untagged";
   Destroy(runAwayObject);
   Destroy(gameObject, currentAction.customValue);
    
}

//----------------------------------------------------------------------------------
// Decrease AI life
function ApplyDamage (amount: float)
{
  currentLife -= amount;
}

//----------------------------------------------------------------------------------
// Set lifeCriticalPecent
function SetLifeCriticalPercent (amount: float)
{
  lifeCriticalPecent = amount;
}

//----------------------------------------------------------------------------------
