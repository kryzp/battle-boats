using BattleBoats.Game;

namespace BattleBoats.Player
{
	/*
	 * Beware.
	 * Skynet (tm) has come. And its powered by anger, electricity and Random.Next().
	 * The apocalypse is coming. Hide your family.
	 */
	public class AIPlayer : PlayerBase
	{
		private List<Coordinates> alreadyBombed = new List<Coordinates>();
		
		public AIPlayer()
			: base()
		{
		}

		/*
		 * AI just does it all at once, unlike puny human players.
		 */
		public override PlaceMove? UpdatePlaceBoats()
		{
			PlaceMove placeMove = new PlaceMove();
			placeMove.Boats = new List<(Coordinates, Boat)>();

			// go through each boat in initial boats and put them there
			foreach (var boat in GetInitialBoats())
			{
				Coordinates boatCoord = Coordinates.ORIGIN;

				do
				{
					boatCoord = new Coordinates(
						Program.Random.Next(0, 8),
						Program.Random.Next(0, 8)
					);
				}
				while (boat.IsPositionInvalid(boatCoord, Board) || boat.OverlapsWithBoats(boatCoord, placeMove.GetBoatsWithCoords()));
				
				placeMove.Boats.Add((boatCoord, boat));
			}
			
			// move to next turn
			Program.GameManager.StartNextTurn();

			return placeMove;
		}

		/*
		 * Literally just randomly picks a spot on the board to hit, which
		 * it hasn't already bombed previously.
		 */
		public override HitMove? UpdateHitEnemy()
		{
			Program.GameManager.StartNextTurn();
			
			Coordinates coord = Coordinates.ORIGIN;
			do
			{
				coord = new Coordinates(
					Program.Random.Next(0, 8),
					Program.Random.Next(0, 8)
				);
			}
			while (alreadyBombed.Contains(coord));

			alreadyBombed.Add(coord);
			return new HitMove(coord);
		}
	}
}
