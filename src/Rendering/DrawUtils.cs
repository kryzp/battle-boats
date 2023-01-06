namespace BattleBoats.Rendering
{
	public static class DrawUtils
	{
		public static void Write(int x, int y, char c)
		{
			if (x < 0 || x >= Program.WINDOW_WIDTH || y < 0 || y >= Program.WINDOW_HEIGHT)
				return;
			
			Console.SetCursorPosition(x, y);
			Console.Write(c.ToString());
		}
		
		public static void Write(int x, int y, char c, ConsoleColor col)
		{
			Console.ForegroundColor = col;
			Write(x, y, c);
		}
	}
}
