using BattleBoats.Player;
using BattleBoats.UI;
using BattleBoats.UI.Elements;

namespace BattleBoats.State
{
	public class StartNewGameState : StateBase
	{
		private Menu menu;

		private InputBox saveFileNameInputBox;
		private InputBox playerOneNameInputBox;
		private InputBox playerTwoNameInputBox;
		private InputBox playerTwoIsAIInputBox;
		
		public override void Init()
		{
			menu = new Menu();
			
			saveFileNameInputBox = new InputBox(new Coordinates(3, 1), (50, 4), "Save File Name")
			{
				ID = 3,
				DownID = 4
			};
			menu.Add(saveFileNameInputBox);
			
			playerOneNameInputBox = new InputBox(new Coordinates(3, 6), (50, 4), "Player 1 Name")
			{
				ID = 4,
				UpID = 3,
				DownID = 5
			};
			menu.Add(playerOneNameInputBox);
			
			playerTwoNameInputBox = new InputBox(new Coordinates(3, 11), (50, 4), "Player 2 Name")
			{
				ID = 5,
				UpID = 4,
				DownID = 6
			};
			menu.Add(playerTwoNameInputBox);
			
			playerTwoIsAIInputBox = new InputBox(new Coordinates(3, 16), (50, 4), "Is Player 2 an AI? (Y/n)")
			{
				ID = 6,
				UpID = 5,
				DownID = 1
			};
			menu.Add(playerTwoIsAIInputBox);
			
			// submit button
			menu.Add(new Button(CentreMode.LEFT, new Coordinates(4, Program.WINDOW_HEIGHT - 3), "Submit", SubmitAllData)
			{
				ID = 1,
				UpID = 6,
				RightID = 2
			});
			
			// quit button
			menu.Add(new Button(CentreMode.RIGHT, new Coordinates(Program.WINDOW_WIDTH - 6, Program.WINDOW_HEIGHT - 3), "Quit", TryQuit)
			{
				ID = 2,
				UpID = 6,
				LeftID = 1
			});

			// select the saveFileNameInputBox to be the first selected element
			menu.SetSelectedElement(3);
		}

		public override void Exit()
		{
		}

		public override void Update()
		{
			menu.Update();
		}

		public override void Draw()
		{
			menu.Draw();
		}

		/*
		 * Submits all data in the text boxes to the game manager, and
		 * starts a new game.
		 */
		private void SubmitAllData()
		{
			// check for incorrect data
			if (ErrorCheckData())
				return;

			// set game manager data
			{
				Program.GameManager.CurrentSaveFileName = saveFileNameInputBox.Text;

				Program.GameManager.PlayerOne = new RealPlayer();
				
				if (playerTwoIsAIInputBox.Text.ToLower().StartsWith("y"))
					Program.GameManager.PlayerTwo = new AIPlayer();
				else
					Program.GameManager.PlayerTwo = new RealPlayer();

				Program.GameManager.PlayerOne.Name = playerOneNameInputBox.Text;
				Program.GameManager.PlayerTwo.Name = playerTwoNameInputBox.Text;
			}

			// transition to game playing state
			Program.NextState = new GamePlayingState();
		}

		/*
		 * Checks for errors in the data submitted by the player.
		 * Returns an true if an error is found, else it returns false.
		 */
		private bool ErrorCheckData()
		{
			if (saveFileNameInputBox.Text.Contains(" "))
			{
				Program.PopupError("You cannot have spaces in the save file name!");
				return true;
			}
			
			if (playerOneNameInputBox.Text == playerTwoNameInputBox.Text)
			{
				Program.PopupError("You cannot have two players with the same name!");
				return true;
			}

			if (playerOneNameInputBox.Text.Length <= 0 || playerTwoNameInputBox.Text.Length <= 0)
			{
				Program.PopupError("You cannot have a player with a completely empty name!");
				return true;
			}

			if (playerOneNameInputBox.Text.Length > PlayerBase.MAX_PLAYER_NAME_LENGTH ||
			    playerTwoNameInputBox.Text.Length > PlayerBase.MAX_PLAYER_NAME_LENGTH)
			{
				Program.PopupError($"You cannot have a player with name longer than {PlayerBase.MAX_PLAYER_NAME_LENGTH} characters!");
				return true;
			}

			if (playerTwoIsAIInputBox.Text.ToLower() != "y" && playerTwoIsAIInputBox.Text.ToLower() != "n")
			{
				Program.PopupError("You must answer [Y]es/[n]o for whether you want the second player to be an AI!");
				return true;
			}
			
			// todo: check if save file is duplicate
			
			return false;
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
	}
}
