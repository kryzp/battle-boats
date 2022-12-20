using BattleBoats.Game;

namespace BattleBoats.Player
{
	/*
	 * Represents an abstract "player" that could be either
	 * the computer or an actual human.
	 */
	public abstract class PlayerBase
	{
		// arbitrarily chosen magic number that nicely works with the ui
		// ...
		// it works, ok?
		public const int MAX_PLAYER_NAME_LENGTH = 17;
		
		public readonly Board Board;
		public string Name;

		public PlayerBase()
		{
			this.Board = new Board();
			this.Name = "";
		}

		/*
		 * Move functions, called when the game manager is either
		 * placing the boats or when the game is actually being carried
		 * out (aka: the players are hitting each other)
		 */
		public virtual PlaceMove? UpdatePlaceBoats() => null;
		public virtual HitMove? UpdateHitEnemy() => null;
		
		/*
		 * Draws out the player info.
		 * Basically just the board.
		 */
		public virtual void Draw(Coordinates boardCoord, Coordinates enemyBoardCoord, bool myTurn)
		{
			Board.Draw(
				boardCoord,
				!myTurn,
				Name
			);
		}

		/*
		 * Returns a list of the initial boats
		 * each player gets.
		 */
		public static List<Boat> GetInitialBoats()
		{
			return new List<Boat>()
			{
				// destroyer
				new Boat(new List<BoatPart>()
				{
					new BoatPart(new Coordinates(0, 0)),
				}),
				
				// destroyer
				new Boat(new List<BoatPart>()
				{
					new BoatPart(new Coordinates(0, 0)),
				}),

				// submarine
				new Boat(new List<BoatPart>()
				{
					new BoatPart(new Coordinates(0, 0)),
					new BoatPart(new Coordinates(1, 0)),
				}),
				
				// submarine
				new Boat(new List<BoatPart>()
				{
					new BoatPart(new Coordinates(0, 0)),
					new BoatPart(new Coordinates(1, 0)),
				}),
				
				// carrier
				new Boat(new List<BoatPart>()
				{
					new BoatPart(new Coordinates(0, 0)),
					new BoatPart(new Coordinates(1, 0)),
					new BoatPart(new Coordinates(2, 0)),
				})
			};
		}
	}
}
