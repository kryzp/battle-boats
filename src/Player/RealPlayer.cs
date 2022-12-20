using BattleBoats.Game;

namespace BattleBoats.Player
{
	/*
	 * Represents an actual human playing the game.
	 */
	public class RealPlayer : PlayerBase
	{
		private List<Boat> boatsToPlace;
		
		public SelectionMarker SelectionMarker;
		
		public RealPlayer()
			: base()
		{
			boatsToPlace = GetInitialBoats();
			SelectionMarker = new SelectionMarker() { PreviewBoat = GetBoatBeingPlaced() };
		}
		
		public override PlaceMove? UpdatePlaceBoats()
		{
			base.UpdatePlaceBoats();

			// update selection marker
			SelectionMarker.Update();

			// check for rotation
			if (Input.IsPressed(ConsoleKey.Z))
				GetBoatBeingPlaced()?.Turn();

			// check if place key is pressed and if so place down boat and move to next boat
			if (Input.IsPressed(ConsoleKey.Spacebar) && boatsToPlace.Count > 0)
			{
				// check if boat is in invalid position
				if (SelectionMarker.IsPreviewBoatInInvalidPosition(Board))
				{
					Program.PopupError("Boat is in invalid position!");
					return null;
				}
				
				// make the new place move
				// real players only place one boat per move so it may seem a bit wasteful
				// to store a single move in a single list but its the most simple to
				// do it this way :/
				PlaceMove move = new PlaceMove()
				{
					Boats = new List<(Coordinates, Boat)>()
					{(
						SelectionMarker.GetPreviewBoatCoords(),
						GetBoatBeingPlaced()
					)}
				};

				// remove the boat from the front of the boats to place list
				boatsToPlace.RemoveAt(0);
				
				// update selection marker preview boat
				SelectionMarker.PreviewBoat = GetBoatBeingPlaced();
				
				// if we're finished placing boats, start new turn
				if (boatsToPlace.Count <= 0)
					Program.GameManager.StartNextTurn();

				return move;
			}

			return null;
		}

		public override HitMove? UpdateHitEnemy()
		{
			base.UpdateHitEnemy();
			
			if (Board.TotalTriedMoves.Count > 0)
			{
				if (Board.TotalTriedMoves.Any(x => x.SuccessfulHit))
					Console.Write("");
			}

			// update selection marker
			SelectionMarker.Update();
			
			// check if hit key is pressed and if so hit
			if (Input.IsPressed(ConsoleKey.Spacebar))
			{
				// start new turn
				Program.GameManager.StartNextTurn();
				
				// make sure other player (if real) doesn't see our board!!!!
				if (Program.GameManager.GetOpposingPlayer() is RealPlayer)
				{
					Program.Renderer.Clear();
					Program.PopupMessage("Press any key to move onto next players turn!");
				}
				
				// send the hit
				return new HitMove(SelectionMarker.GetHitCoords());
			}

			return null;
		}

		public override void Draw(Coordinates boardCoord, Coordinates enemyBoardCoord, bool myTurn)
		{
			base.Draw(boardCoord, enemyBoardCoord, myTurn);

			if (myTurn)
			{
				var smCoords = boardCoord;

				if (Program.GameManager.CurrentState == GameManager.PlayingState.PLAYING)
					smCoords = enemyBoardCoord;
				
				SelectionMarker.Draw(smCoords, Board);
			}
		}
		
		/*
		 * Returns the current boat we are placing
		 */
		private Boat GetBoatBeingPlaced()
		{
			if (boatsToPlace.Count <= 0)
				return null;
			
			return boatsToPlace[0];
		}
	}
}
