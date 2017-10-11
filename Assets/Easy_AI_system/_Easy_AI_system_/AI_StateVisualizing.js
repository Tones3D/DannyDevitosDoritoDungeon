//---------------------------------------------------------------------------------------------
// Simple help script allowing to visualize current AI state
//---------------------------------------------------------------------------------------------

#pragma strict

var AIController: AI_Controller;   // Link to main AI script to tune
var text: TextMesh;				   // Link to 3DText object

//-------------------------------------------------------------------------------
function Update () 
{
  transform.LookAt(Camera.main.transform);
  text.text = AIController.currentAction.actionType.ToString();
  
}
//-------------------------------------------------------------------------------