using BattleBoats.Player;
using BattleBoats.Rendering;
using BattleBoats.Saving;
using BattleBoats.State;

namespace BattleBoats
{
	/*
	 * This game is designed to work with a console of dimensions:
	 *  ->  width / columns = 100
	 *  -> height / rows    = 32
	 *
	 * So you better make sure you're running those specs when running this game!
	 */
	public class Program
	{
		private static bool gameFinished = false;
		
		public static bool ProgramRunning = true;

		public const int WINDOW_WIDTH = 100;
		public const int WINDOW_HEIGHT = 32;

		public static StateBase? CurrentState;
		public static StateBase? NextState;

		public static Renderer Renderer;
		public static GameManager GameManager;
		public static SaveManager SaveManager;
		public static Random Random;

		/*
		 * Gather text player input, placed in the middle of the screen.
		 */
		public static string PopupPrompt(string prompt)
		{
			var coords = new Coordinates(
				(WINDOW_WIDTH / 2) - 2 - (prompt.Length / 2),
				(WINDOW_HEIGHT / 2) - 2
			);
			
			// create empty box + prompt
			Renderer.PushImage(new TextImage()
				.DrawBox(
					Coordinates.ORIGIN, 
					new Coordinates(prompt.Length + 3, 3),
					ConsoleColor.DarkGray
				).DrawText(
					prompt, ConsoleColor.Cyan,
					new Coordinates(2, 1)
				),
				coords, 10, true
			);
			
			// force a render
			Renderer.Render();
			
			Console.SetCursorPosition(coords.X + 2, coords.Y + 2);
			Console.Write("> ");
			return Console.ReadLine() ?? "";
		}
		
		/*
		 * Show Pop-Up Message to the user that can be discarded with any key
		 */
		public static void PopupMessage(string prompt, ConsoleColor col = ConsoleColor.Cyan)
		{
			var coords = new Coordinates(
				(WINDOW_WIDTH / 2) - 2 - (prompt.Length / 2),
				(WINDOW_HEIGHT / 2) - 2
			);
			
			// create empty box + prompt
			Renderer.PushImage(new TextImage()
				.DrawBox(
					new Coordinates(0, 0), new Coordinates(prompt.Length + 3, 2),
					ConsoleColor.DarkGray
				).DrawText(
					prompt, col, new Coordinates(2, 1)
				),
				coords, 10, true
			);
			
			// force a render
			Renderer.Render();
			
			// wait for key
			Console.ReadKey();
		}
		
		/*
		 * Special case of popup prompt.
		 */
		public static bool PopupAreYouSure()
		{ 
			return PopupPrompt("Are you sure? (Y/n)")
				.ToLower()
				.StartsWith("y");
		}
		
		/*
		 * Special case of popup prompt.
		 */
		public static void PopupError(string error)
		{ 
			PopupMessage(error, ConsoleColor.Red);
		}
		
		/*
		 * Sets the "gameFinished" flag to TRUE.
		 */
		public static void FinishGame()
		{
			gameFinished = true;
		}
		
		/*
		 * Entry point.
		 */
		public static void Main()
		{
			// General global variables for making the game function
			Renderer = new Renderer();
			GameManager = new GameManager();
			SaveManager = new SaveManager();
			Random = new Random();

			// the game starts in the main menu state
			NextState = new MainMenuState();

			// every time the state changes we need to refresh without stopping for input, so this is a quick solution to that
			bool firstframe = true;
			
			// init things
			SaveManager.Init();

			while (ProgramRunning)
			{
				// check if the game is finished
				if (gameFinished)
				{
					// archive
					SaveManager.Archive();
			
					// reset
					GameManager.Reset();
			
					// switch state
					NextState = new MainMenuState();
					
					// finished
					gameFinished = false;
				}

				// check if next state is not null (transition)
				if (NextState != null)
				{
					// exit out of current state (if it exists)
					CurrentState?.Exit();
					
					// current state becomes the requested new state
					CurrentState = NextState;
					
					// delete next state
					NextState = null;
					
					// initialize new state
					CurrentState.Init();

					// first frame of new state
					firstframe = true;
				}
				
				// get input from user (skip first frame to draw everything, since checking for input will pause the program)
				// also no need to wait for input if the current player is an ai
				if (!firstframe && GameManager.GetCurrentPlayer() is not AIPlayer)
					Input.PollNewKey();

				// update the current state
				CurrentState?.Update();
				
				// draw the scene
				if (GameManager.GetCurrentPlayer() is not AIPlayer) // we do not need to draw if it is the ai players turn
					CurrentState?.Draw();
				Renderer.Clear();
				Renderer.Render();
				
				// puts the cursor out of the way
				Console.SetCursorPosition(0, 0);
				
				// we have finished the first frame of the game
				firstframe = false;
			}

			// draw thank you message
			Renderer.Clear();
			PopupMessage("Thank you for playing my BattleBoats game, I hope you enjoyed!");
		}
	}
}
