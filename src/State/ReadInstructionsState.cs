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
				Text = "Welcome to BattleBoats (tm), the greatest game ever made according to Albert Einstein, Stephen\nHawking and some other guy we asked on the street.\n\nBattle boats is a turn based strategy game where players eliminate their opponents fleet of\nboats by ‘firing’ at a location on a grid in an attempt to sink them. The first player to\nsink all of their opponents’ battle boats is declared the winner.\n\nEach player has two eight by eight grids. One grid is used for their own battle boats\nand the other is used to record any hits or misses placed on their opponents. At the\nbeginning of the game, players decide where they wish to place their fleet of five\nbattle boats. During game play, players take it in turns to fire at a location on their\nopponent’s board. They do this by stating the coordinates for their target. If a player\nhits their opponent's boat then this is recorded as a hit. If they miss then this is\nrecorded as a miss. The game ends when a player's fleet of boats have been sunk.\nThe winner is the player with boats remaining at the end of the game.\n\nWhen playing, the player who's current turn it is will be displayed above, in addition\nto the previous move the last player made, for reference. You can select where to\nshoot your shot on the (unknown) enemy's board when it is your turn by using the\narrow keys and pressing spacebar to confirm your shot, after which you will recieve\neither confirmation that you struck a valid hit, or be met with failure.\n\nBut first! How do you start a game? Well, if you're completely new, just press the start\nnew game button in the main menu, but if you're wanting to continue a game you already\nstarted, you can press the resume existing game button and select your correponding\nsave file. After each game, your wins/losses are archived in the /ARCHIVE/ folder where\nyou can find the games main executable, while your actual save-files can be found in /SAVES/.\n\nHappy battling!"
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
