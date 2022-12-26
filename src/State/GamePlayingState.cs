using BattleBoats.Game;
using BattleBoats.Rendering;

namespace BattleBoats.State
{
	public class GamePlayingState : StateBase
	{
		public override void Init()
		{
		}

		public override void Exit()
		{
		}

		public override void Update()
		{
			var mgr = Program.GameManager;
			
			// check for quit
			if (Input.IsPressed(ConsoleKey.Q))
			{
				if (Program.PopupAreYouSure())
				{
					Program.SaveManager.Save();
					Program.NextState = new MainMenuState();
					return;
				}
			}
			
			// update the game
			mgr.Update();
		}

		public override void Draw()
		{
			DrawHeader();
			DrawPlayers();
			DrawBorders();
			DrawQuitInfo();
		}

		/*
		 * Draws the info header on the top.
		 */
		private void DrawHeader()
		{
			TextImage header = new TextImage();
			
			// draw who's turn it is
			header.DrawText($"It is currently {Program.GameManager.GetCurrentPlayer().Name}'s turn", ConsoleColor.White, new Coordinates(2, 1));
			
			if (Program.GameManager.CurrentState == GameManager.PlayingState.PLACING_FLEET)
				header.DrawText($"You can press C to clear all your placed boats.", ConsoleColor.Cyan, new Coordinates(2, 2));
			
			// draw last moves
			header.DrawText("1ST LAST MOVE: " + Program.GameManager.LastMoveMessage, ConsoleColor.White, new Coordinates(Program.WINDOW_WIDTH * 1/2 + 3, 1));
			header.DrawText("2ND LAST MOVE: " + Program.GameManager.LastLastMoveMessage, ConsoleColor.White, new Coordinates(Program.WINDOW_WIDTH * 1/2 + 3, 2));
			
			Program.Renderer.PushImage(header, Coordinates.ORIGIN, 2, true);
		}
		
		/*
		 * Draws all of the dividers / borders.
		 */
		private void DrawBorders()
		{
			// draw actual borders
			{
				TextImage borderImage = new TextImage().DrawLine(
					new Coordinates(0, 4),
					new Coordinates(Program.WINDOW_WIDTH, 4),
					new ColouredChar(Characters.HORI_BOX, ConsoleColor.DarkGray)
				).DrawLine(
					new Coordinates(Program.WINDOW_WIDTH * 1/2, 0),
					new Coordinates(Program.WINDOW_WIDTH * 1/2, 4),
					new ColouredChar(Characters.VERT_BOX, ConsoleColor.DarkGray)
				).DrawLine(
					new Coordinates(Program.WINDOW_WIDTH * 1/2, 4),
					new Coordinates(Program.WINDOW_WIDTH * 1/2, Program.WINDOW_HEIGHT - 2),
					new ColouredChar(Characters.VERT_BOX, ConsoleColor.DarkGray)
				).DrawChar(
					new Coordinates(Program.WINDOW_WIDTH * 1/2, 4),
					new ColouredChar(Characters.SPLIT_DOWN, ConsoleColor.DarkGray)
				).DrawChar(
					new Coordinates(Program.WINDOW_WIDTH * 1/2, 4),
					new ColouredChar(Characters.SPLIT_CENTRE, ConsoleColor.DarkGray)
				).DrawChar(
					new Coordinates(Program.WINDOW_WIDTH * 1/2, Program.WINDOW_HEIGHT - 2),
					new ColouredChar(Characters.SPLIT_UP, ConsoleColor.DarkGray)
				);

				Program.Renderer.PushImage(borderImage, Coordinates.ORIGIN, 6);
			}
		}
		
		/*
		 * Draws the quit info at the bottom of the screen.
		 */
		private void DrawQuitInfo()
		{
			Program.Renderer.PushImage(
				new TextImage().DrawLine(
					Coordinates.ORIGIN,
					new Coordinates(Program.WINDOW_WIDTH, 0),
					new ColouredChar(Characters.HORI_BOX, ConsoleColor.DarkGray)
				).DrawText(
					"Press Q to save and quit.",
					ConsoleColor.Cyan,
					new Coordinates(0, 1)
				),
				new Coordinates(0, Program.WINDOW_HEIGHT - 2),
				4
			);
		}

		/*
		 * Draws the players boards.
		 */
		private void DrawPlayers()
		{
			// general board coordinates
			Coordinates baseBoardCoord = new Coordinates((Program.WINDOW_WIDTH / 2) - (Board.DRAW_WIDTH / 2), (Program.WINDOW_HEIGHT / 2) - (Board.DRAW_HEIGHT / 2) + 4 - 2);
			Coordinates leftBoardCoord = baseBoardCoord - new Coordinates(Program.WINDOW_WIDTH / 4, 0);
			Coordinates rightBoardCoord = baseBoardCoord + new Coordinates(Program.WINDOW_WIDTH / 4, 0);
			
			// draw players
			Program.GameManager.PlayerOne.Draw(
				leftBoardCoord,
				rightBoardCoord,
				Program.GameManager.CurrentTurn == true
			);
			
			Program.GameManager.PlayerTwo.Draw(
				rightBoardCoord,
				leftBoardCoord,
				Program.GameManager.CurrentTurn == false
			);
		}
	}
}
