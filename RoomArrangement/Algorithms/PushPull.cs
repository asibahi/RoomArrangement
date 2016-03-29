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

			foreach(Tuple<Room, Room> tuple in Database.Adjacencies)
			{
				// Experimental. Since Room is reference type I am going to edit
				// stuff within the Tuple instead of fishing out the ID.

				
				var r1 = tuple.Item1;
				var r2 = tuple.Item2;

				#region Switching Switches
				double xRec1, yRec1, xCnt1, yCnt1;
				double xRec2, yRec2, xCnt2, yCnt2;

				Database.ReadRoom(r1.Index, out xRec1, out yRec1, out xCnt1, out yCnt1);
				Database.ReadRoom(r2.Index, out xRec2, out yRec2, out xCnt2, out yCnt2);

				var xDim = Abs(xCnt1 - xCnt2) - ((xRec1 / 2) + (xRec2 / 2));
				var yDim = Abs(yCnt1 - yCnt2) - ((yRec1 / 2) + (yRec2 / 2));
				var xOn = 0;
				var yOn = 0;
				var xDir = 1;
				var yDir = 1;
				// var xAlign = 0;
				// var yAlign = 0; // Do I need these two?

				var random = new Random().Next();

				if(xCnt2 > xCnt1)
					xDir = -1;

				if(yCnt2 > yCnt1)
					yDim = -1;

				if(xDim > 0)
					xOn = 1;

				if(yDim > 0)
					yOn = 1;

				//if(xDim > 0 && yDim > 0)
				//{
				//	if(xDim > yDim)
				//		yAlign = 1;
				//	else if(yDim > xDim)
				//		xAlign = 1;
				//	else
				//	{
				//		if(random.Next(0, 100) % 2 == 0)
				//			yAlign = 1;
				//		else
				//			xAlign = 1;
				//	}
				//} 
				#endregion

				var t = 0;
				if (xDim != 0 && yDim != 0)
				{

				}



			}
		}
	}
}