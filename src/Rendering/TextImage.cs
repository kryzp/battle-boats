namespace BattleBoats.Rendering
{
	public class TextImage
	{
		public List<List<ColouredChar>> Chars { get; set; }

		public int Height => Chars.Count;
		public int Width => Chars.Select(c => c.Count).Prepend(0).Max();

		public TextImage()
		{
			this.Chars = new List<List<ColouredChar>>();
		}
		
		public TextImage(int w, int h)
		{
			this.Chars = new List<List<ColouredChar>>();
			
			for (int i = 0; i < h; i++)
			{
				this.Chars.Add(new List<ColouredChar>());
				
				for (int j = 0; j < w; j++)
					this.Chars[i].Add(new ColouredChar());
			}
		}
		
		public TextImage(List<string> lines, ConsoleColor col)
		{
			this.Chars = new List<List<ColouredChar>>();
			
			for (int i = 0; i < lines.Count; i++)
			{
				Chars.Add(new List<ColouredChar>(lines[i].Length));
				
				foreach (char c in lines[i])
					Chars[i].Add(new ColouredChar(c, col));
			}
		}
		
		public TextImage DrawText(string text, ConsoleColor col)
		{
			return DrawText(text, col, Coordinates.ORIGIN);
		}
		
		public TextImage DrawText(string text, ConsoleColor col, Coordinates coords)
		{
			TryResizeUp(coords.X + text.Length, coords.Y + 1);

			for (int i = 0; i < text.Length; i++)
			{
				this.Chars[coords.Y][coords.X + i] = new ColouredChar(text[i], col);
			}

			return this;
		}
		
		public TextImage DrawLine(Coordinates p1, Coordinates p2, ColouredChar c)
		{
			TryResizeUp(Math.Max(p1.X, p2.X) + 1, Math.Max(p1.Y, p2.Y) + 1);
			
			if (p2.X - p1.X == 0)
			{
				for (int t = 0; t < p2.Y - p1.Y; t++)
				{
					DrawChar(
						new Coordinates(
							p1.X,
							p1.Y + t
						),
						c
					);
				}
			}
			else
			{
				float m = (float)(p2.Y - p1.Y) / (float)(p2.X - p1.X);
				
				for (int t = 0; t < p2.X - p1.X; t++)
				{
					DrawChar(
						new Coordinates(
							p1.X + t,
							(int)(p1.Y + (m * (t - p1.X)))
						),
						c
					);
				}
			}

			return this;
		}

		public TextImage DrawChar(Coordinates coords, ColouredChar c)
		{
			TryResizeUp(coords.X + 1, coords.Y + 1);
			
			this.Chars[coords.Y][coords.X] = c;

			return this;
		}

		public TextImage DrawBox(Coordinates p1, Coordinates p2, ConsoleColor stroke)
		{
			DrawLine(p1, new Coordinates(p1.X, p2.Y), new ColouredChar(Characters.VERT_BOX, stroke));
			DrawLine(new Coordinates(p2.X, p1.Y), p2, new ColouredChar(Characters.VERT_BOX, stroke));
			DrawLine(p1, new Coordinates(p2.X, p1.Y), new ColouredChar(Characters.HORI_BOX, stroke));
			DrawLine(new Coordinates(p1.X, p2.Y), p2, new ColouredChar(Characters.HORI_BOX, stroke));

			DrawChar(p1, new ColouredChar(Characters.TL_BOX, stroke));
			DrawChar(new Coordinates(p2.X, p1.Y), new ColouredChar(Characters.TR_BOX, stroke));
			DrawChar(new Coordinates(p1.X, p2.Y), new ColouredChar(Characters.BL_BOX, stroke));
			DrawChar(p2, new ColouredChar(Characters.BR_BOX, stroke));

			return this;
		}
		
		public TextImage SetColourForLinesInRange(int mn, int mx, ConsoleColor col)
		{
			for (int i = 0; i < mx - mn + 1; i++)
			{
				foreach (var c in Chars[i + mn])
					c.Colour = col;
			}

			return this;
		}

		public TextImage SetColour(ConsoleColor col)
		{
			foreach (var line in Chars)
			{
				foreach (var cc in line)
					cc.Colour = col;
			}
			
			return this;
		}
		
		public void Draw(Coordinates coords, bool overrideEverything = false)
		{
			for (int y = 0; y < Chars.Count; y++)
			{
				for (int x = 0; x < Chars[y].Count; x++)
				{
					var cc = Chars[y][x];

					if (cc.Char != ' ' || overrideEverything)
						cc.Write(x + coords.X, y + coords.Y);
				}
			}
		}

		public void DrawTo(TextImage other, Coordinates coords, bool overrideEverything = false)
		{
            for (int y = 0; y < Chars.Count; y++)
            {
                for (int x = 0; x < Chars[y].Count; x++)
                {
                    var cc = Chars[y][x];

                    int px = x + coords.X;
                    int py = y + coords.Y;

                    bool pxInRange = px >= 0 && px < other.Chars[y].Count;
                    bool pyInRange = py >= 0 && py < other.Chars.Count;

					if ((cc.Char != ' ' || overrideEverything) && pxInRange && pyInRange)
						other.Chars[py][px] = cc;
                }
            }
        }

		private void TryResizeUp(int w, int h)
		{
			int initialHeight = Height;
			
			if (h > initialHeight)
			{
				for (int i = 0; i < h - initialHeight; i++)
					Chars.Add(new List<ColouredChar>());
			}
			
			foreach (var c in Chars)
			{
				int initialWidth = c.Count;
				
				for (int i = 0; i < Math.Max(w, Width) - initialWidth; i++)
					c.Add(new ColouredChar());
			}
		}
	}
}
