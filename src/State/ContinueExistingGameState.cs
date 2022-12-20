using BattleBoats.Rendering;
using BattleBoats.UI;
using BattleBoats.UI.Elements;

namespace BattleBoats.State
{
	public class ContinueExistingGameState : StateBase
	{
		private const int MAX_BUTTONS_Y = 6;
		private const int BUTTON_Y_OFFSET_PER_LINE = 5;
		private const int BUTTON_X_OFFSET_PER_WRAP = 32;
		
		private Menu menu;
		private List<string> savefileNames;

		public override void Init()
		{
			menu = new Menu();
			savefileNames = GetSavefileNames();
			
			for (int i = 0; i < savefileNames.Count; i++)
			{
				int xOffset = (i / MAX_BUTTONS_Y) * BUTTON_X_OFFSET_PER_WRAP;
				int yOffset = (i % MAX_BUTTONS_Y) * BUTTON_Y_OFFSET_PER_LINE;
				
				menu.Add(new Button(
					new Coordinates(3 + xOffset, 2 + yOffset),
					savefileNames[i],
					LoadCurrentButton
				)
				{
					ID = i,
					UpID = (i <= 0) ? -1 : (i - 1),
					DownID = (i >= savefileNames.Count - 1) ? -1 : (i + 1)
				});
			}

			menu.SetSelectedElement(0);
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
				new TextImage().DrawLine(
					Coordinates.ORIGIN,
					new Coordinates(Program.WINDOW_WIDTH, 0),
					new ColouredChar(Characters.HORI_BOX, ConsoleColor.DarkGray)
				).DrawText(
					"Press Q to return to the main menu.",
					ConsoleColor.Cyan,
					new Coordinates(0, 1)
				),
				new Coordinates(0, Program.WINDOW_HEIGHT - 2),
				5,
				true
			);
		}

		/*
		 * Tries to quit to main menu, but asks the player again first.
		 */
		private void TryQuit()
		{
			if (!Program.PopupAreYouSure())
				return;
			
			Program.NextState = new MainMenuState();
		}
		
		/*
		 * Gets all the names of the savefiles
		 */
		private List<string> GetSavefileNames() => new DirectoryInfo("SAVES/").GetFiles("*.bbsave").Select(file => Path.GetFileNameWithoutExtension(file.Name)).ToList();
		
		/*
		 * Loads the save file at the current button
		 */
		private void LoadCurrentButton()
		{
			// get filename we have selected and load it
			string filename = savefileNames[menu.GetSelectedItemID()];
			Program.SaveManager.Load(filename);
			
			// transition to game playing state
			Program.NextState = new GamePlayingState();
		}
	}
}
