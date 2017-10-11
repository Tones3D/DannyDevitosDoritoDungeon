OVERVIEW

This system allows you to easily implement quite complex AI (artificial intelligence) behavior for any enemies or allies in your game. Path finding functionality included (allows avoiding even dynamic objects/obstacles)!

The system is extremely easy to setup and tune - just in several clicks you’ll able to implement it anywhere.
It really fast and performance safe (you can adjust it to get better accuracy or even higher performance).
The system doesn’t require any specific actions or updates for your objects and scenes.

List of predefined actions (you can easily change/add them if needed):   Idle,   Seek,   Follow,   Move,   Attack,   RunAway,   Patrol,   Heal,   Die

This system works on all platforms supported by Unity3D.






HOW TO USE

To use this system – you should just:

1st option (auto):

1.	Add(drag and drop) all prefabs from “_Easy_PathFinding_System_”  to Hierarchy window
2.	Add child object (with animation) to this object and assign it to AnimatedObject property of “AI_Controller” component.
3.	Tune parameters in “AI_Tuner” component.



2nd option (manual):
1.	Create game object (preferable with Rigidbody and Collider) and assign PathFinding, PathFollowing  and AI_Controller components (scripts) to it 
2.	Create child game object (with Rigidbody and Collider) and assign AI_Sensor component to it 
3.	Assign this object(with AI_Sensor) to viewTrigger property of AI_Controller component
4.	Assign PathFinding script to pathFindingScript property of PathFollowing component
5.	Assign PathFollowing script to pathFollower property of AI_Controller component
6.	Add child object (with animation) to this object and assign it to AnimatedObject property of “AI_Controller” component.
7.	Add/setup needed actions
8.	Write enemyTag and/or specify  FollowAlly properties in “AI_Controller” component.






FOR MORE INFO - please check Manual.pdf (".\_Easy_AI_system_\Manual.pdf")
