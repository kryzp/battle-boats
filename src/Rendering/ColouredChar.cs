namespace BattleBoats.Rendering
{
	public class ColouredChar
	{
		public ConsoleColor Colour { get; set; }
		public char Char { get; set; }

		public ColouredChar()
		{
			this.Char = ' ';
			this.Colour = ConsoleColor.White;
		}
		
		public ColouredChar(char c, ConsoleColor col)
		{
			this.Char = c;
			this.Colour = col;
		}

		public void Write(int x, int y)
		{
			DrawUtils.Write(x, y, Char, Colour);
		}
	}
}
