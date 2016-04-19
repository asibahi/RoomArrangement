using System;

namespace RoomArrangement
{
	[Flags]
	public enum CardinalDirections
	{
		None = 0,
		North = 1 << 0,
		East = 1 << 1,
		South = 1 << 2,
		West = 1 << 3
	}

	// For input purposes. Does not need to match the room types. IF I am ever
	// opening this library I should find a better solution than matching a list
	// of radio-buttons.
	[Flags]
	public enum InputRooms
	{
		None		= 0,
		LivingRoom	= 1 << 0,
		DiningRoom	= 1 << 1,
		Office		= 1 << 2,
		Library		= 1 << 3,
		DirtyKitchen	= 1 << 4,
		CleanKitchen	= 1 << 5,
		Reception	= 1 << 6,
		Pool		= 1 << 7,
		GameRoom	= 1 << 8,
		MaidRoom	= 1 << 9,
		DriverRoom	= 1 << 10
	}
}
