# RoomArrangement
This is the current implementation of Autoarchitect. For more info see autoarchitect.net , or email me at abdulrahman@autoarchitect.net .

I am currently working on restructuring the app (so programmers reading the code wouldnt feel like killing me) and adding a different/new geometric function. The "live" branch is Boundary at the time of this writing (4/7/2016). Other stuff I have in mind to add as I am working on it:

* Adding a Criteria class, which includes certain design decision allowing me to tweak the behaviour via a text file or sth
* Refining the BldgProgram and Room classes to add "Qualities" to Rooms. Said Qualities (defined by an enum or a Quality class perhaps) define how the Room behaves. 
* Incorporating something about windows and sunlight direction in the Evaluation function. Based on the Qualities. Bedrooms need some part of their Eastern wall, for example, be exposed to sunlight.

Please keep in mind that the code in here is in no way finished, or for that matter, functional. It is a work in progress.
