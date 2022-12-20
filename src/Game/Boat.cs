namespace BattleBoats.Game
{
	/*
	 * Part of the boat.
	 * Just a single cell.
	 */
	public class BoatPart
	{
		public enum Status
		{
			OK,
			HIT
		}

		public Status State;
		public Coordinates Coords;
		public Boat Parent;

		public BoatPart(Coordinates coords)
		{
			this.State = Status.OK;
			this.Coords = coords;
		}

		public bool IsDestroyed() => this.Parent.IsDestroyed();
	}
	
	/*
	 * Collection of boat parts pretty much.
	 */
	public class Boat
	{
		public List<BoatPart> Parts { get; private set; }
		public Coordinates Coords;
		
		public Boat(List<BoatPart> parts)
		{
			Parts = parts;
			Parts.ForEach(p => p.Parent = this);
		}

		/*
		 * Checks if each part was hit.
		 * If so, the boat is entirely destroyed.
		 */
		public bool IsDestroyed() => Parts.All(part => part.State == BoatPart.Status.HIT);

		/*
		 * "Turns" (or "flips") the boat by flipping each parts' X and Y coordinates.
		 */
		public void Turn() => Parts.ForEach(p => (p.Coords.X, p.Coords.Y) = (p.Coords.Y, p.Coords.X));

		/*
		 * Checks if the boat is in an invalid position.
		 */
		public bool IsPositionInvalid(Coordinates coord, Board board)
		{
			int i = 0;
			
			foreach (BoatPart part in Parts)
			{
				Coordinates partCoords = new Coordinates(part.Coords.X, part.Coords.Y) + coord;

				if (partCoords.X < 0 || partCoords.X >= Board.SIZE ||
				    partCoords.Y < 0 || partCoords.Y >= Board.SIZE)
				{
					return true;
				}
			}

			if (OverlapsWithBoats(coord, board.Boats))
				return true;
			
			return false;
		}
		
		/*
		 * Checks for overlap with boats.
		 */
		public bool OverlapsWithBoats(Coordinates coord, List<Boat> boats)
		{
			foreach (BoatPart part in Parts)
			{
				foreach (Boat otherBoat in boats)
				{
					foreach (BoatPart otherPart in otherBoat.Parts)
					{
						if ((otherBoat.Coords + otherPart.Coords) == (coord + part.Coords))
							return true;
					}
				}
			}

			return false;
		}
	}
}
