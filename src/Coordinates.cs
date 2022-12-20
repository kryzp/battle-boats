namespace BattleBoats
{
	public struct Coordinates
	{
		public static readonly Coordinates ORIGIN = new Coordinates(0, 0);
		public static readonly Coordinates UNIT = new Coordinates(1, 1);
		
		public int X;
		public int Y;

		public Coordinates(int x, int y)
		{
			this.X = x;
			this.Y = y;
		}

		public string ToNumChar()
		{
			return $"{(char)('A' + X)}, {Y+1}";
		}
		
		public static Coordinates operator + (Coordinates lhs, Coordinates rhs)
		{
			return new Coordinates(lhs.X + rhs.X, lhs.Y + rhs.Y);
		}
		
		public static Coordinates operator - (Coordinates lhs, Coordinates rhs)
		{
			return new Coordinates(lhs.X - rhs.X, lhs.Y - rhs.Y);
		}
		
		public static Coordinates operator * (Coordinates lhs, int scalar)
		{
			return new Coordinates(lhs.X * scalar, lhs.Y * scalar);
		}

		public static bool operator == (Coordinates lhs, Coordinates rhs)
		{
			return (lhs.X == rhs.X) && (lhs.Y == rhs.Y);
		}

		public static bool operator != (Coordinates lhs, Coordinates rhs)
		{
			return !(lhs == rhs);
		}
	}
}
