using BattleBoats.Rendering;

namespace BattleBoats.UI.Elements
{
	public class InputBox : TextBox
	{
		private const string ALLOWED_CHARS = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@£$%^&*()#-=_+[]{};':\"\\/'|?.,<>`~ ";

		public string Label;
		
		public InputBox(Coordinates coords, (int, int) dimensions, string label)
			: base(coords, dimensions)
		{
			TextOffset = new Coordinates(0, 1);
			Label = label;
		}

		/*
		 * Handles all of the input for text.
		 */
		public override void Update()
		{
			base.Update();

			if (!IsSelected)
				return;

			string c = Input.GetCurrentKeyChar().ToString();

			if (Input.IsPressed(ConsoleKey.Backspace))
			{
				if (Text.Length > 0)
					Text = Text.Remove(Text.Length - 1);
			}
			else if (ALLOWED_CHARS.Contains(c))
			{
				Text += c;
			}
		}

		/*
		 * Draws the text box along with the cursor
		 */
		public override void Draw()
		{
			// draw border
			Program.Renderer.PushImage(
				new TextImage().DrawBox(
					new Coordinates(0, 0), new Coordinates(Dimensions.Item1 - 1, Dimensions.Item2 - 1),
					IsSelected ? ConsoleColor.Yellow : ConsoleColor.DarkGray
				),
				Coords, 
				1,
				true
			);
			
			// cursor, text, label & start indicator (this thing: '>')
			Program.Renderer.PushImage(
				new TextImage().DrawChar(
					new Coordinates(0, 1),
					new ColouredChar('>', IsSelected ? ConsoleColor.Gray : ConsoleColor.DarkGray)
				).DrawChar(
					new Coordinates(Text.Length + 2, 1),
					new ColouredChar(IsSelected ? '|' : ' ', IsSelected ? ConsoleColor.Gray : ConsoleColor.DarkGray)
				).DrawText(
					Label,
					IsSelected ? ConsoleColor.Gray : ConsoleColor.DarkGray,
					Coordinates.ORIGIN
				).DrawText(
					Text,
					IsSelected ? ConsoleColor.White : ConsoleColor.DarkGray,
					new Coordinates(2, 1)
				),
				Coords + new Coordinates(2, 1),
				10,
				false
			);
		}
	}
}
