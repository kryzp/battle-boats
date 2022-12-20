using BattleBoats.Rendering;

namespace BattleBoats.Game
{
	/*
	 * Represents the board, belonging to a
	 * player, representing their boat
	 * placing.
	 */
	public class Board
	{
		public const int SIZE = 8;
		public const int DRAW_WIDTH = 21;
		public const int DRAW_HEIGHT = 11;

		public List<Boat> Boats;
		public List<HitMove> TotalTriedMoves;

		public Board()
		{
			this.Boats = new List<Boat>();
			this.TotalTriedMoves = new List<HitMove>();
		}

		public void AddBoats(List<(Coordinates, Boat)> bb)
		{
			foreach (var b in bb)
			{
				b.Item2.Coords = b.Item1;
				this.Boats.Add(b.Item2);
			}
		}

		public void DispatchTriedMove(HitMove move)
		{
			TotalTriedMoves.Add(move);

			BoatPart? part = GetBoatPartAt(move.Coords);
			if (part != null)
			{
				part.State = BoatPart.Status.HIT;
				move.SuccessfulHit = true;
			}
		}
		
		public void Draw(Coordinates coords, bool hidden, string label)
		{
			TextImage boardOutput = new TextImage(DRAW_WIDTH, DRAW_HEIGHT);
			
			// draw box
			boardOutput.DrawBox(Coordinates.ORIGIN, new Coordinates(DRAW_WIDTH - 1, DRAW_HEIGHT - 1), ConsoleColor.DarkGray);
			
			// draw internals of the box (fairly messy and i think this could be
			// refactored / cleaned up by splitting it into multiple methods
			// but it works for now so...)
			for (int y = 0; y < SIZE; y++)
			{
				boardOutput.DrawChar(new Coordinates(2, y + 2), new ColouredChar((y + 1).ToString()[0], ConsoleColor.White));
				
				for (int x = 0; x < SIZE; x++)
				{
					boardOutput.DrawChar(new Coordinates((x + 2) * 2, 1), new ColouredChar((char)('A' + x), ConsoleColor.White));
					
					ColouredChar c = new ColouredChar();

					if (hidden)
					{
						c.Colour = ConsoleColor.DarkGray;
						c.Char = '#';

						if (Program.Random.Next(100) > 80)
							c.Char = '%';
					}
					else
					{
						c.Colour = ConsoleColor.DarkCyan;
						c.Char = '.';

						if (Program.Random.Next(100) > 80)
							c.Char = '~';

						if (Program.Random.Next(100) > 50)
							c.Colour = ConsoleColor.Cyan;

						BoatPart? alivePart = GetBoatPartAt(new Coordinates(x, y));

						if (alivePart != null)
						{
							c.Colour = ConsoleColor.Green;
							c.Char = '@';
						}
					}

					var brokenPart = GetBoatPartAt(new Coordinates(x, y));
					bool contains = TotalTriedMoves.Any(move => move.Coords == new Coordinates(x, y));

					if (contains)
					{
						if (brokenPart != null)
						{
							c.Char = 'H';
							c.Colour = ConsoleColor.Red;

							if (brokenPart.IsDestroyed())
							{
								c.Colour = ConsoleColor.DarkRed;
								c.Char = 'X';
							}
						}
						else
						{
							c.Char = 'M';
							c.Colour = ConsoleColor.DarkYellow;
						}
					}

					boardOutput.DrawChar(new Coordinates((x + 2) * 2, y + 2), c);
				}
			}
			
			// push out the image of the board to the renderer
			Program.Renderer.PushImage(boardOutput, coords, 1, true);
			
			// start label
			TextImage labelOutput = new TextImage()
				.DrawBox(Coordinates.ORIGIN, new Coordinates(DRAW_WIDTH - 1, 2), ConsoleColor.DarkGray)
				.DrawText(label, ConsoleColor.White, new Coordinates(2, 1))
				.DrawChar(new Coordinates(0, 2), new ColouredChar(Characters.SPLIT_RIGHT, ConsoleColor.DarkGray))
				.DrawChar(new Coordinates(DRAW_WIDTH - 1, 2), new ColouredChar(Characters.SPLIT_LEFT, ConsoleColor.DarkGray));

			// push out label to renderer
			Program.Renderer.PushImage(labelOutput, coords + new Coordinates(0, -2), 2, true);
		}

		private BoatPart? GetBoatPartAt(Coordinates coords)
		{
			foreach (var boat in Boats)
			{
				foreach (var part in boat.Parts)
				{
					Coordinates pCoord = boat.Coords + part.Coords;

					if (pCoord == coords)
						return part;
				}
			}

			return null;
		}
	}
}
