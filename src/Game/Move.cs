namespace BattleBoats.Game
{
	/*
	 * Represents a move where a player tries to hit
	 * a boat on the enemy players board.
	 */
	public class HitMove
	{
		public Coordinates Coords { get; set; }
		public bool SuccessfulHit { get; set; }

		public HitMove(Coordinates coords)
		{
			this.Coords = coords;
			this.SuccessfulHit = false;
		}
	}

	/*
	 * Represents a move when a player tries
	 * to place down a boat.
	 */
	public class PlaceMove
	{
		public List<(Coordinates, Boat)> Boats { get; set; }

		public PlaceMove()
		{
			this.Boats = new List<(Coordinates, Boat)>();
		}

		public List<Boat> GetBoatsWithCoords()
		{
			List<Boat> result = new List<Boat>();

			foreach (var bpair in Boats)
			{
				result.Add(new Boat(bpair.Item2.Parts)
				{
					Coords = bpair.Item2.Coords + bpair.Item1
				});
			}
			
			return result;
		}
	}
}
