using BattleBoats.Game;

namespace BattleBoats.Saving
{
	/*
	 * Writes the game out to a save file.
	 */
	public class GameWriter
	{
		public void Write(string filename, SaveData data)
		{
			using BinaryWriter bw = new BinaryWriter(File.Open(filename, FileMode.Create));
			
			bw.Write(data.PlayerOneName);
			bw.Write(data.PlayerTwoName);
			bw.Write(data.IsPlayerTwoAI);
				
			bw.Write(data.PlayerOneBoats.Count);
			data.PlayerOneBoats.ForEach(x => WriteBoat(bw, x));
				
			bw.Write(data.PlayerTwoBoats.Count);
			data.PlayerTwoBoats.ForEach(x => WriteBoat(bw, x));
				
			bw.Write(data.PlayerOneMoves.Count);
			data.PlayerOneMoves.ForEach(x => WriteMove(bw, x));
				
			bw.Write(data.PlayerTwoMoves.Count);
			data.PlayerTwoMoves.ForEach(x => WriteMove(bw, x));
			
			bw.Write(data.LastMoveMessage);
			bw.Write(data.LastLastMoveMessage);
			bw.Write(data.MoveToNextPlayer);
			bw.Write(data.CurrentTurn);
			bw.Write(data.StartingTurn);
			bw.Write((int)data.CurrentState);
		}

		private void WriteBoat(BinaryWriter bw, Boat boat)
		{
			bw.Write(boat.Coords.X);
			bw.Write(boat.Coords.Y);
			bw.Write(boat.Parts.Count);
			boat.Parts.ForEach(x => WriteBoatPart(bw, x));
		}

		private void WriteBoatPart(BinaryWriter bw, BoatPart part)
		{
			bw.Write(part.Hit);
			bw.Write(part.LocalCoords.X);
			bw.Write(part.LocalCoords.Y);
		}

		private void WriteMove(BinaryWriter bw, HitMove move)
		{
			bw.Write(move.Coords.X);
			bw.Write(move.Coords.Y);
			bw.Write(move.SuccessfulHit);
		}
	}
}
