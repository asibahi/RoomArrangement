using System;

namespace RoomArrangement
{
	enum RoomType {
		LivingRoom,
		Bedroom,
		Kitchen,
		Corridor
	}

	[Flags]
	enum CardinalDirections
	{
		None = 0,
		North = 1 << 0,
		East = 1 << 1,
		South = 1 << 2,
		West = 1 << 3
	}
}
