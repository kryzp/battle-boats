namespace BattleBoats.Saving
{
	/*
	 * Responsible for "archiving" the game
	 * into a human-readable format which allows
	 * players to keep a log of all of the games
	 * that they have played.
	 */
	public class GameArchiver
	{
		public void Archive(string filename)
		{
			using StreamWriter sw = new StreamWriter(filename);

			var now = DateTime.Now;
			
			sw.WriteLine("DATE: " + now.ToString("dd/mm/yyyy"));
			sw.WriteLine("TIME: " + now.ToString("hh:mm:ss tt"));
			sw.WriteLine("PLAYERS:");
			sw.WriteLine("- " + Program.GameManager.PlayerOne.Name);
			sw.WriteLine("- " + Program.GameManager.PlayerTwo.Name);
			sw.WriteLine("WINNER: " + Program.GameManager.GetCurrentPlayer().Name);
			sw.WriteLine("- BOATS LEFT: " + Program.GameManager.GetCurrentPlayer().Board.Boats.FindAll(x => !x.IsDestroyed()).Count);

			int workingPartsLeft = Program.GameManager.GetCurrentPlayer().Board.Boats.Sum(b => b.Parts.Count(part => !part.IsDestroyed()));
			sw.WriteLine("- WORKING PARTS LEFT: " + workingPartsLeft.ToString());
		}
	}
}
