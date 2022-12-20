using BattleBoats.Game;
using BattleBoats.Player;

namespace BattleBoats
{
	/*
	 * "Master" game class. Controls the flow
	 * of events throughout the game.
	 */
	public class GameManager
	{
		public enum PlayingState
		{
			PLACING_FLEET,
			PLAYING
		}

		public bool MoveToNextPlayer;
		
		public string LastLastMoveMessage;
		public string LastMoveMessage;
		
		public PlayerBase PlayerOne;
		public PlayerBase PlayerTwo;

		public bool CurrentTurn; // true = player 1, false = player 2
		public PlayingState CurrentState;
		public string CurrentSaveFileName;

		public GameManager()
		{
			// reset our state
			Reset();
		}
		
		/*
		 * Updates all game logic.
		 */
		public void Update()
		{
			// do the placing fleet move
			if (CurrentState == PlayingState.PLACING_FLEET)
			{
				PlaceMove? m = null;

				if ((m = GetCurrentPlayer().UpdatePlaceBoats()) != null)
				{
					GetCurrentPlayer().Board.AddBoats(m.Boats);
					LastLastMoveMessage = LastMoveMessage;
					LastMoveMessage = GetCurrentPlayer().Name + " just placed their boats!";
				}
				else
					return;
			}
			
			// do the playing state move (hit)
			else if (CurrentState == PlayingState.PLAYING)
			{
				HitMove? m = null;

				if ((m = GetCurrentPlayer().UpdateHitEnemy()) != null)
				{
					GetOpposingPlayer().Board.DispatchTriedMove(m);
					LastLastMoveMessage = LastMoveMessage;
					LastMoveMessage = GetCurrentPlayer().Name + " just sent a hit to " + m.Coords.ToNumChar();
				}
				else
					return;
			}

			// check if both players have placed this fleets and switch state if so
			if (BothPlayersFleetsPlaced() && CurrentState == PlayingState.PLACING_FLEET)
				CurrentState = PlayingState.PLAYING;
			
			// check for le epic win!!
			if (CheckForWinCondition())
			{
				// display a congratulations message to the player who won
				Program.PopupMessage("Congratulations to " + GetCurrentPlayer().Name + " for winning the game!! (epic)");
				
				// game is finished!
				Program.FinishGame();
				return;
			}

			// if we move to next player, update the current turn
			if (MoveToNextPlayer)
			{
				CurrentTurn = !CurrentTurn;
				MoveToNextPlayer = false;
			}
		}
		
		/*
		 * Resets the state of the game manager.
		 */
		public void Reset()
		{
			PlayerOne = null;
			PlayerTwo = null;
			MoveToNextPlayer = false;
			LastLastMoveMessage = "";
			LastMoveMessage = "";
			CurrentTurn = true;
			CurrentState = PlayingState.PLACING_FLEET;
			CurrentSaveFileName = "";
		}
		
		/*
		 * Check for win condition.
		 */
		private bool CheckForWinCondition() => GetOpposingPlayer().Board.Boats.TrueForAll(x => x.IsDestroyed()) && CurrentState == PlayingState.PLAYING;
		
		/*
		 * Moves forward move.
		 */
		public void StartNextTurn() => MoveToNextPlayer = true;

		/*
		 * Returns the current player who's turn it is.
		 */
		public PlayerBase GetCurrentPlayer() => CurrentTurn ? PlayerOne : PlayerTwo;

		/*
		 * Returns the player who's being fought against during the current players turn.
		 */
		public PlayerBase GetOpposingPlayer() => CurrentTurn ? PlayerTwo : PlayerOne;

		/*
		 * Checks if both players have placed all of their fleets.
		 * Returns true if yes, and false if no.
		 */
		private bool BothPlayersFleetsPlaced()
		{
			int initCount = PlayerBase.GetInitialBoats().Count;
			return (PlayerOne.Board.Boats.Count == initCount) && (PlayerTwo.Board.Boats.Count == initCount);
		}
	}
}
