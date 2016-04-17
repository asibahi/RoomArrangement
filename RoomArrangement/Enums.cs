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
	public enum DesiredRoomOptions
	{
		LivingRoom,
		DiningRoom,
		Office,
		Library,
		DirtyKitchen,
		CleanKitchen,
		Reception,
		Pool,
		GameRoom,
		MaidRoom,
		DriverRoom
	}
}
