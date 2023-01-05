namespace BattleBoats.Rendering
{
	public static class DrawUtils
	{
		private static ConsoleColor lastFg;
		private static ConsoleColor lastBg;

		public static void Write(int x, int y, char c)
		{
			if (x < 0 || x >= Program.WINDOW_WIDTH || y < 0 || y >= Program.WINDOW_HEIGHT)
				return;
			
			Console.SetCursorPosition(x, y);
			Console.Write(c.ToString());
		}
		
		public static void Write(int x, int y, char c, ConsoleColor fg, ConsoleColor bg = ConsoleColor.Black)
		{
			if (c != ' ')
			{
				if (fg != lastFg)
				{
					Console.ForegroundColor = fg;
					lastFg = fg;
				}

				if (bg != lastBg)
				{
					Console.BackgroundColor = bg;
					lastBg = bg;
				}
			}

			Write(x, y, c);
		}
	}
}
