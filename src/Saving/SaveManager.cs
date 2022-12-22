using BattleBoats.Player;

namespace BattleBoats.Saving
{
	/*
	 * Manages saving, loading and archiving games.
	 */
	public class SaveManager
	{
		private GameWriter writer;
		private GameReader loader;
		private GameArchiver archiver;

		public SaveManager()
		{
			writer = new GameWriter();
			loader = new GameReader();
			archiver = new GameArchiver();
		}

		/*
		 * Initialises general stuff such as the save/archive directory.
		 */
		public void Init()
		{
			if (!Directory.Exists("SAVES"))
				Directory.CreateDirectory("SAVES");
			
			if (!Directory.Exists("ARCHIVES"))
				Directory.CreateDirectory("ARCHIVES");
		}
		
		/*
		 * Checks if given save file already exists.
		 */
		public bool DoesSaveFileExist(string saveFileName)
		{
			var files = Directory.GetFiles("SAVES/");
			
			foreach (var file in files)
			{
				if (file == "SAVES/" + saveFileName + ".bbsave")
					return true;
			}

			return false;
		}
		
		/*
		 * Archives the game.
		 */
		public void Archive()
		{
			// tell the archiver to archive the game
			archiver.Archive("ARCHIVES/" + Program.GameManager.CurrentSaveFileName + ".txt");
			
			// delete the save file since it is now archived
			if (File.Exists("SAVES/" + Program.GameManager.CurrentSaveFileName + ".bbsave")) 
				File.Delete("SAVES/" + Program.GameManager.CurrentSaveFileName + ".bbsave");
			
			// display info to user
			Program.PopupMessage("Game archived to: /ARCHIVES/" + Program.GameManager.CurrentSaveFileName + ".txt");
		}

		/*
		 * Writes all game data out to file.
		 */
		public void Save()
		{
			// initialize all data
			SaveData data = new SaveData()
			{
				// general
				LastMoveMessage = Program.GameManager.LastMoveMessage,
				LastLastMoveMessage = Program.GameManager.LastLastMoveMessage,
				MoveToNextPlayer = Program.GameManager.MoveToNextPlayer,
				CurrentTurn = Program.GameManager.CurrentTurn,
				CurrentState = Program.GameManager.CurrentState,

				// player one
				PlayerOneName = Program.GameManager.PlayerOne.Name,
				PlayerOneBoats = Program.GameManager.PlayerOne.Board.Boats,
				PlayerOneMoves = Program.GameManager.PlayerTwo.Board.TotalTriedMoves,
				
				// player two
				PlayerTwoName = Program.GameManager.PlayerTwo.Name,
				PlayerTwoBoats = Program.GameManager.PlayerTwo.Board.Boats,
				PlayerTwoMoves = Program.GameManager.PlayerOne.Board.TotalTriedMoves,
				IsPlayerTwoAI = Program.GameManager.PlayerTwo is AIPlayer
			};

			// write out
			writer.Write("SAVES/" + Program.GameManager.CurrentSaveFileName + ".bbsave", data);
			
			// display info to user
			Program.PopupMessage("Game saved to: /SAVES/" + Program.GameManager.CurrentSaveFileName + ".bbsave");
		}

		/*
		 * Loads all game data from file into GameManager.
		 */
		public void Load(string filename)
		{
			// get data
			SaveData data = loader.Read("SAVES/" + filename + ".bbsave");
			
			// send data to game manager
			{
				// general
				Program.GameManager.CurrentSaveFileName = filename;
				Program.GameManager.LastMoveMessage = data.LastMoveMessage;
				Program.GameManager.LastLastMoveMessage = data.LastLastMoveMessage;
				Program.GameManager.MoveToNextPlayer = data.MoveToNextPlayer;
				Program.GameManager.CurrentTurn = data.CurrentTurn;
				Program.GameManager.CurrentState = data.CurrentState;

				// player one
				Program.GameManager.PlayerOne = new RealPlayer();
				Program.GameManager.PlayerOne.Name = data.PlayerOneName;
				Program.GameManager.PlayerOne.Board.Boats = data.PlayerOneBoats;
				Program.GameManager.PlayerOne.Board.TotalTriedMoves = data.PlayerTwoMoves;

				// player two
				Program.GameManager.PlayerTwo = data.IsPlayerTwoAI ? new AIPlayer() : new RealPlayer();
				Program.GameManager.PlayerTwo.Name = data.PlayerTwoName;
				Program.GameManager.PlayerTwo.Board.Boats = data.PlayerTwoBoats;
				Program.GameManager.PlayerTwo.Board.TotalTriedMoves = data.PlayerOneMoves;
				
				// turn off selection markers if currently playing
				if (Program.GameManager.CurrentState == GameManager.PlayingState.PLAYING)
				{
					((RealPlayer)Program.GameManager.PlayerOne).SelectionMarker.PreviewBoat = null;
					
					if (!data.IsPlayerTwoAI)
						((RealPlayer)Program.GameManager.PlayerTwo).SelectionMarker.PreviewBoat = null;
				}
			}
			
			// display info to user
			Program.PopupMessage("Loaded game: /SAVES/" + Program.GameManager.CurrentSaveFileName + ".bbsave");
		}
	}
}
