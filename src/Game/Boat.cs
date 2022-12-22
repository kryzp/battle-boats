namespace BattleBoats.Game
{
	/*
	 * Part of the boat.
	 * Just a single cell.
	 */
	public class BoatPart
	{
		public bool Hit;
		public Coordinates LocalCoords;
		public Boat Parent;

		public BoatPart(Coordinates localCoords)
		{
			this.Hit = false;
			this.LocalCoords = localCoords;
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
		public bool IsDestroyed() => Parts.TrueForAll(part => part.Hit);

		/*
		 * "Turns" (or "flips") the boat by flipping each parts' X and Y coordinates.
		 */
		public void Turn() => Parts.ForEach(p => (p.LocalCoords.X, p.LocalCoords.Y) = (p.LocalCoords.Y, p.LocalCoords.X));

		/*
		 * Checks if the boat is in an invalid position.
		 */
		public bool IsPositionInvalid(Coordinates coord, Board board)
		{
			int i = 0;
			
			foreach (BoatPart part in Parts)
			{
				Coordinates partCoords = new Coordinates(part.LocalCoords.X, part.LocalCoords.Y) + coord;

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
						if ((otherBoat.Coords + otherPart.LocalCoords) == (coord + part.LocalCoords))
							return true;
					}
				}
			}

			return false;
		}
	}
}
