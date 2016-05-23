# RoomArrangement
This is the current implementation of Autoarchitect. For more info see autoarchitect.net , or email me at abdulrahman@autoarchitect.net .

I am currently working on restructuring the app (so programmers reading the code wouldnt feel like killing me) and adding a different/new geometric function. The main branch of the project (OldMaster) is locked for archiving purposes in the time of this writng (5/23/2016).

Things missing from the project, that should be implemented when the project is to continue (which I might do in my spare time):

* Adding a Criteria class, which includes certain design decision allowing me to tweak the behaviour via a text file or sth
* Refining the BldgProgram and Room classes to add "Qualities" to Rooms. Said Qualities (defined by an enum or a Quality class perhaps) define how the Room behaves. 
* Incorporating something about windows and sunlight direction in the Evaluation function. Based on the Qualities. Bedrooms need some part of their Eastern wall, for example, be exposed to sunlight.

Please keep in mind that the code in here is in no way finished, or for that matter, functional. It is a (paused) work in progress.
