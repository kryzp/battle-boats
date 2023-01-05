using BattleBoats.Rendering;
using BattleBoats.UI;
using BattleBoats.UI.Elements;

namespace BattleBoats.State
{
	public class ReadInstructionsState : StateBase
	{
		private readonly Menu menu;

		public ReadInstructionsState()
		{
			menu = new Menu();

			menu.Add(new TextBox(new Coordinates(0, 0), (Program.WINDOW_WIDTH, Program.WINDOW_HEIGHT - 1))
			{
				Text = "Welcome to BattleBoats (tm), the greatest game ever made according to Albert Einstein, Stephen\n" +
				       "Hawking and some other guy we asked on the street.\n" +
				       "\n" +
				       "Battle boats is a turn based strategy game where players eliminate their opponents fleet of\n" +
				       "boats by ‘firing’ at a location on a grid in an attempt to sink them. The first player to\n" +
				       "sink all of their opponents’ battle boats is declared the winner.\n" +
				       "\n" +
				       "Each player has two eight by eight grids. One grid is used for their own battle boats\n" +
				       "and the other is used to record any hits or misses placed on their opponents. At the\n" +
				       "beginning of the game, players decide where they wish to place their fleet of five\n" +
				       "battle boats. During game play, players take it in turns to fire at a location on their\n" +
				       "opponent’s board. They do this by stating the coordinates for their target. If a player\n" +
				       "hits their opponent's boat then this is recorded as a hit. If they miss then this is\n" +
				       "recorded as a miss. The game ends when a player's fleet of boats have been sunk.\n" +
				       "The winner is the player with boats remaining at the end of the game.\n" +
				       "\n" +
				       "When playing, the player who's current turn it is will be displayed above, in addition\n" +
				       "to the previous move the last player made, for reference. You can select where to\n" +
				       "shoot your shot on the (unknown) enemy's board when it is your turn by using the\n" +
				       "arrow keys and pressing spacebar to confirm your shot, after which you will receive\n" +
				       "either confirmation that you struck a valid hit, or be met with failure.\n" +
				       "\n" +
				       "But first! How do you start a game? Well, if you're completely new, just press the start\n" +
				       "new game button in the main menu, but if you're wanting to continue a game you already\n" +
				       "started, you can press the resume existing game button and select your corresponding\n" +
				       "save file. After each game, your wins/losses are archived in the /ARCHIVE/ folder where\n" +
				       "you can find the games main executable, while your actual save-files can be found in /SAVES/.\n" +
				       "\n" +
				       "Happy battling!"
			});
		}
		
		public override void Init()
		{
		}

		public override void Exit()
		{
		}

		public override void Update()
		{
			// check for quit
			if (Input.IsPressed(ConsoleKey.Q))
			{
				Program.NextState = new MainMenuState();
				return;
			}
			
			// update menu
			menu.Update();
		}

		public override void Draw()
		{
			// draw menu
			menu.Draw();
			
			// draw info text
			Program.Renderer.PushImage(
				new TextImage().DrawText(
					"Press Q to return to the main menu.",
					ConsoleColor.Cyan
				),
				new Coordinates(0, Program.WINDOW_HEIGHT - 1),
				5,
				true
			);
		}
	}
}
