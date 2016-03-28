using System;
using System.Collections;
using System.Collections.Generic;
using static System.Math;

namespace RoomArrangement
{
	static class PushPull
	{
		public static void Run()
		{
			var hardCount = 0;
			var softCount = 0;
			var softLim = Database.Count * (Database.Count - 1);
			var loopCount = 0;
			var hardLim = 50000;

			// VERBAL ALGORITHM:
			// 
			// Go through every Adjacency Tuple. Pull the second room towards the first.

			foreach(Tuple<Room, Room> t in Database.Adjacencies)
			{
				// Experimental. Since Room is reference type I am going to edit
				// stuff within the Tuple instead of fishing out the ID.
				// Alternative approach is to use Room.ID to get the room out of
				// Database AND THEN move stuff.
			}
		}
	}
}