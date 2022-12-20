using BattleBoats.Rendering;
using BattleBoats.UI;
using BattleBoats.UI.Elements;

namespace BattleBoats.State
{
	/*
	 * First state that the player sees. Allows the player to select all of the different
	 * options, such as starting a new game, continuing an existing game, reading all instructions
	 * or just quitting the program.
	 */
	public class MainMenuState : StateBase
	{
		private readonly Menu menu;
		private TextBox infoBox;
		private TextImage logoImage;
		
		private List<TextImage> shipAnimation;
		private int shipAnimationIdx = 0;

		private TextImage inputInfoImage;
		
		public MainMenuState()
		{
			menu = new Menu();
		}
		
		public override void Init()
		{
			menu.Add(new Button(CentreMode.LEFT, new Coordinates((Program.WINDOW_WIDTH / 2) - 11, 15), "Start New Game", () => Program.NextState = new StartNewGameState())
			{
				ID = 1,
				DownID = 2,
				UpID = 4
			});
			
			menu.Add(new Button(CentreMode.LEFT, new Coordinates((Program.WINDOW_WIDTH / 2) - 11, 19), "Continue Existing Game", () => Program.NextState = new ContinueExistingGameState())
			{
				ID = 2,
				UpID = 1,
				DownID = 3
			});
			
			menu.Add(new Button(CentreMode.LEFT, new Coordinates((Program.WINDOW_WIDTH / 2) - 11, 23), "Read Instructions", () => Program.NextState = new ReadInstructionsState())
			{
				ID = 3,
				UpID = 2,
				DownID = 4
			});
			
			menu.Add(new Button(CentreMode.LEFT, new Coordinates((Program.WINDOW_WIDTH / 2) - 11, 27), "Quit Game", () => Program.ProgramRunning = false)
			{
				ID = 4,
				UpID = 3,
				DownID = 1
			});

			infoBox = new TextBox(new Coordinates(6, 14), (25, 15))
			{
				ID = 5
			};

			menu.Add(infoBox);
			
			// select the "Start New Game" button initially
			menu.SetSelectedElement(1);

			// logo
			{
				var lines = new List<string>()
				{
					@" _______          __    __   __",
					@"|   _    \_____ _/  |__/  |_|  |   __",
					@"|  |_)   /\__  \\   __/    _/  | / __ \",
					@"|  |_)   \(  __ \|  |  |  | |  |_\  ___/",
					@"|________/_____ /|__|  |__| |____/\____/",
					@"   _______                  __",
					@"  |   _    \  ___  ____   _/  |_  _____",
					@"  |  |_)   / / _ \ \__  \\    __/  ___/",
					@"  |  |_)   \( (_) )(  __ \|  |  \___ \",
					@"  |________/ \___/ \____/ \__| /_____/"
				};

				logoImage = new TextImage(lines, ConsoleColor.DarkCyan)
					.SetColourForLinesInRange(0, 5, ConsoleColor.Cyan);
			}

			// ship animation
			{
				var shipAnim0 = new List<string>()
				{
					@"              _",
					@"           __/_|",
					@"          [_____]",
					@"    \__   __/__|  _,",
					@"  *//__\_[]_o_o_|(_)_*",
					@"  \ \               /",
					@"^^^~~^``^^~^^``^~~~^^~^",
					@"`^^~~~^^~~~~`^~^~^^^```"
				};
				
				var shipAnim1 = new List<string>()
				{
					@"    *BOOM*",
					@"              _",
					@"           __/_|",
					@"          [_____]",
					@"    \__   __/__|  _,",
					@"  *//__\_[]_o_o_|(_)_* ",
					@"`^^~~~^^~^~~`^^^~~~^```",
					@"^^^~~~^^^^^~^^``^~~^^~^"
				};

				shipAnimation = new List<TextImage>()
				{
					new TextImage(shipAnim0, ConsoleColor.Gray).SetColourForLinesInRange(6, 7, ConsoleColor.Blue),
					new TextImage(shipAnim1, ConsoleColor.Gray).SetColourForLinesInRange(0, 0, ConsoleColor.Red).SetColourForLinesInRange(6, 7, ConsoleColor.Blue)
				};
			}
			
			// input info
			{
				var lines = new List<string>()
				{
					@"       +---+",
					@"       | ^ |",
					@"   +---+---+---+",
					@"   | < | v | > |",
					@"   +---+---+---+",
					@" Use the arrow keys",
					@"to navigate the menu!",
					@"",
					@"      +----+",
					@"      | v- |",
					@"      +-+  |",
					@"        |  |",
					@"        +--+",
					@"",
					@" And press enter to",
					@"confirm your choice!"
				};

				inputInfoImage = new TextImage(lines, ConsoleColor.Magenta);
			}
			
			// update info box
			UpdateInfoBox();
		}

		public override void Exit()
		{
		}

		public override void Update()
		{
			// update menu and box
			menu.Update();
			UpdateInfoBox();
		}
		
		public override void Draw()
		{
			// draw the menu
			menu.Draw();

			// draw images
			Program.Renderer.PushImage(logoImage, new Coordinates((Program.WINDOW_WIDTH / 2) - 23, 0), 0, true);
			Program.Renderer.PushImage(shipAnimation[shipAnimationIdx], new Coordinates(2, 4), 0, false);
			Program.Renderer.PushImage(inputInfoImage, new Coordinates(Program.WINDOW_WIDTH - 25, 2), 0, false);
			
			// push border
			Program.Renderer.PushImage(
				new TextImage().DrawLine(
					Coordinates.ORIGIN,
					new Coordinates(0, 17),
					new ColouredChar(Characters.VERT_BOX, ConsoleColor.DarkGray)
				),
				new Coordinates(34, 13),
				0,
				true
			);

			// move forward one frame
			shipAnimationIdx = (shipAnimationIdx + 1) % shipAnimation.Count;
		}

		/*
		 * Updates the info box with the relevant text.
		 */
		private void UpdateInfoBox()
		{
			// select proper info text based on the selected item id
			infoBox.Text = menu.GetSelectedItemID() switch
			{
				1 => "This will allow you\nto start a new game,\nbe it against another\nplayer, or an AI!",
				2 => "Selecting this will\nlet you choose from\nan array of your\nalready existing save\nfiles,and continue\nwhere you left\noff!",
				3 => "Selecting this will\ntake you to the\nmanual page for how\nto play Battle\nBoats (tm)!",
				4 => "Selecting this will\nquit the game,\nthank you for\nplaying!",
				_ => infoBox.Text
			};
		}
	}
}
