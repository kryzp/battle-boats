using BattleBoats.Game;

namespace BattleBoats.Saving
{
	/*
	 * Contains all the data that you can find in a save-file.
	 */
	public class SaveData
	{
		public string PlayerOneName;
		public List<Boat> PlayerOneBoats;
		public List<HitMove> PlayerOneMoves;
		
		public string PlayerTwoName;
		public List<Boat> PlayerTwoBoats;
		public List<HitMove> PlayerTwoMoves;
		
		public bool IsPlayerTwoAI;

		public string LastMoveMessage;
		public string LastLastMoveMessage;
		public bool MoveToNextPlayer;
		public bool CurrentTurn;
		public int StartingTurn;

		public GameManager.PlayingState CurrentState;
	}
}
