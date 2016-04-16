using System;
using static System.Math;

namespace RoomArrangement
{
	static class PushPull
	{
		public static void RunPushPull(this House house)
		{
			var hardCount = 0;
			var resolvedCount = 0;
			var totalRoomPairs = house.Count * (house.Count - 1);
			var hardLim = 50000;

			do
			{
				// Go through every Adjacency Tuple. Pull the second room towards the first.
				foreach(Tuple<Room, Room> tuple in house.Adjacencies)
					Pull(tuple.Item1, tuple.Item2);

				// Go Through the rooms in sequence and move them away from each other.
				for(int i = 0; i < house.Count; i++)
					for(int j = i + 1; j < house.Count; j++)
						resolvedCount = PushOrAddCount(resolvedCount, house[i], house[j], house.Boundary);
		
				if(++hardCount >= hardLim)
					break;
			}
			while(resolvedCount <= totalRoomPairs);
		}

		static void Pull(Room r1, Room r2)
		{
			#region Switching Switches
			double xRec1, yRec1, xCnt1, yCnt1;
			double xRec2, yRec2, xCnt2, yCnt2;

			r1.Read(out xRec1, out yRec1, out xCnt1, out yCnt1);
			r2.Read(out xRec2, out yRec2, out xCnt2, out yCnt2);

			var xDim = Abs(xCnt1 - xCnt2) - ((xRec1 / 2) + (xRec2 / 2));
			var yDim = Abs(yCnt1 - yCnt2) - ((yRec1 / 2) + (yRec2 / 2));
			var xOn = 0;
			var yOn = 0;
			var xDir = 1;
			var yDir = 1;
			var xAlign = 0; // Do I need
			var yAlign = 0; // these?
			var random = new Random().Next();

			if(xCnt2 > xCnt1)
				xDir = -1;

			if(yCnt2 > yCnt1)
				yDim = -1;

			if(xDim > 0)
				xOn = 1;

			if(yDim > 0)
				yOn = 1;

			if(xDim > 0 && yDim > 0)
				if(xDim > yDim)
					yAlign = 1;
				else if(xDim < yDim)
					xAlign = 1;
				else if(random % 2 == 0)
					yAlign = 1;
				else
					xAlign = 1;
			#endregion

			if(xDim != 0 && yDim != 0)
			{
				var xTV = (xDir * xAlign * 1) + (xDir * xDim * xOn);
				var yTV = (yDir * yAlign * 1) + (yDir * yDim * yOn);

				var tV = new Vector(xTV, yTV);

				r2.Move(tV);
			}
		}

		static int PushOrAddCount(int resolvedCount, Room ri, Room rj, Rectangle boundary)
		{
			#region Switching Switches
			double xRec1, yRec1, xCnt1, yCnt1;
			double xRec2, yRec2, xCnt2, yCnt2;

			ri.Read(out xRec1, out yRec1, out xCnt1, out yCnt1);
			rj.Read(out xRec2, out yRec2, out xCnt2, out yCnt2);

			var xDim = ((xRec1 / 2) + (xRec2 / 2)) - Abs(xCnt1 - xCnt2);
			var yDim = ((yRec1 / 2) + (yRec2 / 2)) - Abs(yCnt1 - yCnt2);
			var xOn = 0;
			var yOn = 0;
			var xDir = 1;
			var yDir = 1;
			var random = new Random().Next();

			if(xCnt1 < xCnt2)
				xDir = -1;

			if(yCnt1 < yCnt2)
				yDim = -1;

			if(xDim < yDim || (xDim == yDim && random % 2 == 0))
				xOn = 1;
			else
				yOn = 1;
			#endregion

			if(xDim > 0 && yDim > 0)
			{
				resolvedCount = 0;

				var tV = new Vector(xDir * xDim * xOn, yDir * yDim * yOn);

				bool riStillIn = IsStillIn(ri, tV, boundary);
				bool rjStillIn = IsStillIn(rj, tV, boundary);

				// Checking for boundary. Should be improved by having the room go sideways.
				// Needs to be tested
				if(riStillIn)
					ri.Move(tV / 2);
				if(rjStillIn)
					rj.Move(-tV / 2);
			}
			else
				resolvedCount++;

			return resolvedCount;
		}

		static bool IsStillIn(Room room, Vector tV, Rectangle boundary)
		{
			// Positions of room after moving. To check if it's outside boundary
			var xMax = room.Anchor.X + room.Space.XDim + tV.X / 2;
			var xMin = room.Anchor.X + tV.X / 2;
			var yMax = room.Anchor.Y + room.Space.YDim + tV.Y / 2;
			var yMin = room.Anchor.Y + tV.Y / 2;
			return (xMax <= boundary.XDim)
				&& (xMin >= 0)
				&& (yMax <= boundary.YDim)
				&& (yMin >= 0);
		}
	}
}