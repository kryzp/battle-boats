using BattleBoats.Game;

namespace BattleBoats.Saving
{
	/*
	 * Loads the game from a save file.
	 */
	public class GameReader
	{
		public SaveData Read(string filename)
		{
			using BinaryReader br = new BinaryReader(File.Open(filename, FileMode.Open));
			SaveData data = new SaveData();

			data.PlayerOneName = br.ReadString();
			data.PlayerTwoName = br.ReadString();
			data.IsPlayerTwoAI = br.ReadBoolean();

			data.PlayerOneBoats = new List<Boat>();
			int playerOneBoatCount = br.ReadInt32();
			for (int i = 0; i < playerOneBoatCount; i++)
				data.PlayerOneBoats.Add(ReadBoat(br));
			
			data.PlayerTwoBoats = new List<Boat>();
			int playerTwoBoatCount = br.ReadInt32();
			for (int i = 0; i < playerTwoBoatCount; i++)
				data.PlayerTwoBoats.Add(ReadBoat(br));

			data.PlayerOneMoves = new List<HitMove>();
			int playerOneMoveCount = br.ReadInt32();
			for (int i = 0; i < playerOneMoveCount; i++)
				data.PlayerOneMoves.Add(ReadMove(br));

			data.PlayerTwoMoves = new List<HitMove>();
			int playerTwoMoveCount = br.ReadInt32();
			for (int i = 0; i < playerTwoMoveCount; i++)
				data.PlayerTwoMoves.Add(ReadMove(br));

			data.LastMoveMessage = br.ReadString();
			data.LastLastMoveMessage = br.ReadString();
			data.MoveToNextPlayer = br.ReadBoolean();
			data.CurrentTurn = br.ReadBoolean();
			data.StartingTurn = br.ReadInt32();
			data.CurrentState = (GameManager.PlayingState)br.ReadInt32();

			return data;
		}

		private Boat ReadBoat(BinaryReader br)
		{
			int x = br.ReadInt32();
			int y = br.ReadInt32();

			var parts = new List<BoatPart>();
			int partCount = br.ReadInt32();
			for (int i = 0; i < partCount; i++)
				parts.Add(ReadBoatPart(br));

			return new Boat(parts)
			{
				Coords = new Coordinates(x, y)
			};
		}

		private BoatPart ReadBoatPart(BinaryReader br)
		{
			BoatPart.Status state = (BoatPart.Status)br.ReadInt32();
			int x = br.ReadInt32();
			int y = br.ReadInt32();

			return new BoatPart(new Coordinates(x, y))
			{
				State = state
			};
		}

		private HitMove ReadMove(BinaryReader br)
		{
			int x = br.ReadInt32();
			int y = br.ReadInt32();
			bool success = br.ReadBoolean();

			return new HitMove(new Coordinates(x, y))
			{
				SuccessfulHit = success
			};
		}
	}
}
