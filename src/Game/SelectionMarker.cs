using BattleBoats.Rendering;

namespace BattleBoats.Game
{
	/*
	 * Shows the current selected coordinate by a player.
	 */
	public class SelectionMarker
	{
		private Coordinates markerCoords;
		private TextImage sprite;

		// if not null, will draw a "preview" boat above it
		public Boat? PreviewBoat = null;

		public SelectionMarker()
		{
			markerCoords = Coordinates.ORIGIN;

			sprite = new TextImage(3, 3);
			sprite.DrawChar(new Coordinates(1, 0), new ColouredChar('v', ConsoleColor.Yellow));
			sprite.DrawChar(new Coordinates(0, 1), new ColouredChar('>', ConsoleColor.Yellow));
			sprite.DrawChar(new Coordinates(2, 1), new ColouredChar('<', ConsoleColor.Yellow));
			sprite.DrawChar(new Coordinates(1, 2), new ColouredChar('^', ConsoleColor.Yellow));
		}
		
		/*
		 * Updates the selection markers position by checking for input.
		 */
		public void Update()
		{
			var k = Input.GetCurrentKey();

			if (k == ConsoleKey.UpArrow)
				markerCoords.Y--;

			if (k == ConsoleKey.DownArrow)
				markerCoords.Y++;

			if (k == ConsoleKey.LeftArrow)
				markerCoords.X--;

			if (k == ConsoleKey.RightArrow)
				markerCoords.X++;
			
			markerCoords.X = Math.Clamp(markerCoords.X, 0, Board.SIZE - 1);
			markerCoords.Y = Math.Clamp(markerCoords.Y, 0, Board.SIZE - 1);
		}
		
		/*
		 * Draws the selection marker (and preview boat, if not null) to the screen.
		 */
		public void Draw(Coordinates boardCoord, Board board)
		{
			Program.Renderer.PushImage(sprite, GetDrawCoords(boardCoord) - Coordinates.UNIT, 5);
			
			if (PreviewBoat != null)
			{
				TextImage boatImg = new TextImage();
				
				foreach (BoatPart part in PreviewBoat.Parts)
				{
					boatImg.DrawChar(
						new Coordinates(part.Coords.X * 2, part.Coords.Y),
						new ColouredChar('@', ConsoleColor.DarkGreen)
					);
				}

				if (IsPreviewBoatInInvalidPosition(board))
					boatImg.SetColour(ConsoleColor.Red);
				
				// draw boat
				Program.Renderer.PushImage(boatImg,  GetDrawCoords(boardCoord), 6);
				
				// draw label / name
				{
					Program.Renderer.PushImage(
						new TextImage().DrawText(GetPreviewBoatName(), ConsoleColor.Cyan),
						GetDrawCoords(boardCoord) + new Coordinates(-GetPreviewBoatName().Length / 2, -2),
						10,
						true
					);
				}
			}
		}

		/*
		 * Checks if the current preview boat is in an invalid position on the board.
		 */
		public bool IsPreviewBoatInInvalidPosition(Board board) => PreviewBoat != null && PreviewBoat.IsPositionInvalid(markerCoords, board);
		
		/*
		 * Returns the coordinate to hit.
		 */
		public Coordinates GetHitCoords() => markerCoords;

		/*
		 * Gets the coordinates on where to place the boat / where the preview boat is relative to the board.
		 */
		public Coordinates GetPreviewBoatCoords() => (PreviewBoat != null) ? PreviewBoat.Coords + markerCoords : Coordinates.ORIGIN;

		/*
		 * Gets the coordinates where to draw the marker, relative to the board.
		 */
		private Coordinates GetDrawCoords(Coordinates boardCoord)
		{
			return new Coordinates(
				(markerCoords.X * 2) + 3,
				(markerCoords.Y * 1) + 1
			) + Coordinates.UNIT + boardCoord;
		}

		/*
		 * Gets the name of the boat based on how many parts it has.
		 */
		private string GetPreviewBoatName()
		{
			return PreviewBoat?.Parts.Count switch
			{
				1 => "[Destroyer]",
				2 => "[Submarine]",
				3 => "[Carrier]",
				_ => ""
			};
		}
	}
}
