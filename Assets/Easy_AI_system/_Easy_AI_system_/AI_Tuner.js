//----------------------------------------------------------------------------------------------
// Simple help script allowing to tune AI parameters in attached AI_Controller easier
// This script is suitable  for  current(template) realisation of AI_Controller
// If you will change AI_Controller - please update this  script accordingly
//----------------------------------------------------------------------------------------------


#pragma strict

var updateOnStartOnly: boolean = true;      // Allow to specify should parameters be updated OnStartOnly or  every frame
var AIController: AI_Controller;            // Link to main AI script to tune

var life: float = 100;					    // Initial/max amount of life
var lifeCriticalPercent: float = 0.3;	    // How less life is critical (AI will try to runaway for  healing)
var allyToFollow: GameObject;			    // If specified - AI will follow this object/character
var idleDuration: float = 1;			    // How long should Idle-state be
var movementSpeed: float = 3;			    // Speed of movement
var seekingSpeed: float = 5; 			    // For Seeking-state - how fast AI will look around
var enablePatrol: boolean = true;			// If true - AI will patrol territory instead of simple Seeking
var patrolDistance: float = 5; 				// Max distance of  movement in one patrolling step
var attackObjectsWithTag: String = "enemy"; // Tag of object, that regarded as enemy
var enableRangedAttack: boolean = true;		// Enable range attack. Only mele is allowed if false
var attackDamage: float = 1;				// Damage per one attack
var attackSpeed: float = 0.3;				// How often will AI try to hit enemy
var runawayRangeMultiplier: float = 2;		// Multiplier that allows to increase max distance oto run away from enemy 
var healingSpeed: float = 1;				// How fast will AI use healing ability if damaged
var healingLifeAmount: float = 5;			// How much healt should be restored at once
var delayBeforeDead: float = 1;				// How much time   AI should  wait before destroying itself (useful for death animation)


//===================================================================================================
// OnStart update
function Start () 
{
  if (!AIController) AIController = gameObject.GetComponent(AI_Controller);
  
  if (AIController)
  {
	  AIController.life = life;
	  AIController.SetLifeCriticalPercent(lifeCriticalPercent);
	  AIController.followAlly = allyToFollow;
	  AIController.GetActionByID(AI_ActionTypes.Idle).actionDuration = idleDuration;
	  AIController.GetActionByID(AI_ActionTypes.Move).customValue = movementSpeed;
	  AIController.GetActionByID(AI_ActionTypes.Seek).customValue = seekingSpeed;
	  AIController.enemyTag = attackObjectsWithTag;
	  AIController.enableRangedAttack = enableRangedAttack;
	  AIController.GetActionByID(AI_ActionTypes.Attack).customValue = attackDamage;
	  AIController.GetActionByID(AI_ActionTypes.Attack).actionDuration = attackSpeed;
	  AIController.GetActionByID(AI_ActionTypes.RunAway).customValue = runawayRangeMultiplier;
	  AIController.GetActionByID(AI_ActionTypes.Heal).actionDuration = healingSpeed;
	  AIController.GetActionByID(AI_ActionTypes.Heal).customValue = healingLifeAmount;
	  AIController.GetActionByID(AI_ActionTypes.Die).customValue = delayBeforeDead;
	  AIController.enablePatrol = enablePatrol;
	  AIController.GetActionByID(AI_ActionTypes.Patrol).customValue = patrolDistance;
  }
   else
     Debug.Log("Can't find AIController for " + gameObject.name);
      
}

//----------------------------------------------------------------------------------
// Update every frame
function Update () 
{
  if (!updateOnStartOnly) 
   if (AIController)
   {
	  AIController.SetLifeCriticalPercent(lifeCriticalPercent);
	  AIController.followAlly = allyToFollow;
	  AIController.GetActionByID(AI_ActionTypes.Idle).actionDuration = idleDuration;
	  AIController.GetActionByID(AI_ActionTypes.Move).customValue = movementSpeed;
	  AIController.GetActionByID(AI_ActionTypes.Seek).customValue = seekingSpeed;
	  AIController.enemyTag = attackObjectsWithTag;
	  AIController.enableRangedAttack = enableRangedAttack;
	  AIController.GetActionByID(AI_ActionTypes.Attack).customValue = attackDamage;
	  AIController.GetActionByID(AI_ActionTypes.Attack).actionDuration = attackSpeed;
	  AIController.GetActionByID(AI_ActionTypes.RunAway).customValue = runawayRangeMultiplier;
	  AIController.GetActionByID(AI_ActionTypes.Heal).actionDuration = healingSpeed;
	  AIController.GetActionByID(AI_ActionTypes.Heal).customValue = healingLifeAmount;
	  AIController.GetActionByID(AI_ActionTypes.Die).customValue = delayBeforeDead;
	  AIController.enablePatrol = enablePatrol;
	  AIController.GetActionByID(AI_ActionTypes.Patrol).customValue = patrolDistance;
   }
   else
     Debug.Log("Can't find AIController for " + gameObject.name);
}
//----------------------------------------------------------------------------------