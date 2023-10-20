# PathDirecting
'Reverse' path finding - modular for insertion into future projects
Different approach to path finding. Works in reverse - you set a target space to move to,
and the cells radiate out from there directing any agents on them to flow toward the target. 
This will naturally flow around obstacles (either by marking the cells unwalkable, or by cells 
being absent in some spaces.

Works best for 'hoard' style games, where all agents should move toward target(s) along the 
shortest routes.
