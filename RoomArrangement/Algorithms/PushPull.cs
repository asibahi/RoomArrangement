using System;
using static System.Math;

namespace RoomArrangement
{
	static class PushPull
	{
		public static void Run(House house)
		{
			var hardCount = 0;
			var resolvedCount = 0;
			var totalRoomPairs = house.Count * (house.Count - 1);
			var hardLim = 50000;

			do
			{
				// VERBAL ALGORITHM:
				// Go through every Adjacency Tuple. Pull the second room towards the first.
				foreach(Tuple<Room, Room> tuple in house.Adjacencies)
				{
					// Experimental. Since Room is reference type I am going to edit
					// stuff within the Tuple instead of fishing out the ID.
					var r1 = tuple.Item1;
					var r2 = tuple.Item2;

					Pull(r1, r2);
				}

				// VERBAL ALGORITHM
				// Go Through the rooms in sequence and move them away from each other.
				for(int i = 0; i < house.Count; i++)
				{
					var ri = house[i];

					for(int j = i + 1; j < house.Count; j++)
					{
						var rj = house[j];
						resolvedCount = PushOrAddCount(house, resolvedCount, ri, rj);
					}
				}

				if(++hardCount >= hardLim)
					break;
			}
			while(resolvedCount <= totalRoomPairs);
		}

		private static void Pull(Room r1, Room r2)
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
			{
				if(xDim > yDim)
				{
					yAlign = 1;
				}
				else if(yDim > xDim)
				{
					xAlign = 1;
				}
				else
				{
					if(random % 2 == 0)
						yAlign = 1;
					else
						xAlign = 1;
				}
			}
			#endregion

			if(xDim != 0 && yDim != 0)
			{
				var xTV = (xDir * xAlign * 1) + (xDir * xDim * xOn);
				var yTV = (yDir * yAlign * 1) + (yDir * yDim * yOn);

				var tV = new Vector(xTV, yTV);

				r2.Move(tV);
			}
		}

		private static int PushOrAddCount(House house, int resolvedCount, Room ri, Room rj)
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

			if(xCnt2 > xCnt1)
				xDir = -1;

			if(yCnt2 > yCnt1)
				yDim = -1;

			if(xDim < yDim || (xDim == yDim && random % 2 == 0))
				xOn = 1;
			else
				yOn = 1;
			#endregion
			if(xDim > 0 && yDim > 0)
			{
				resolvedCount = 0;

				var tVx = xDir * xDim * xOn;
				var tVy = yDir * yDim * yOn;

				#region Out of Bounds variables
				// Positions of ri and rj after moving. To check if they're outside boundary
				// ri new positions after change
				var riXMax = ri.Anchor.X + ri.Space.XDim + tVx / 2;
				var riXMin = ri.Anchor.X + tVx / 2;
				var riYMax = ri.Anchor.Y + ri.Space.YDim + tVy / 2;
				var riYMin = ri.Anchor.Y + tVy / 2;
				var riStillIn = (riXMax <= house.Boundary.XDim)
						&& (riXMin >= 0)
						&& (riYMax <= house.Boundary.YDim)
						&& (riYMax >= 0);

				// rj new positions after change
				var rjXMax = rj.Anchor.X + rj.Space.XDim - tVx / 2;
				var rjXMin = rj.Anchor.X - tVx / 2;
				var rjYMax = rj.Anchor.Y + rj.Space.YDim - tVy / 2;
				var rjYMin = rj.Anchor.Y - tVy / 2;
				var rjStillIn = (rjXMax <= house.Boundary.XDim)
						&& (rjXMin >= 0)
						&& (rjYMax <= house.Boundary.YDim)
						&& (rjYMin >= 0);
				#endregion

				var tV = new Vector(tVx, tVy);

				// Checking for boundary. Should be improved by having the room go sideways.
				// Needs to be tested
				if(riStillIn)
					ri.Move(tV / 2);

				if(rjStillIn)
					rj.Move(-tV / 2);

			}
			else
			{
				resolvedCount++;
			}

			return resolvedCount;
		}
	}
}