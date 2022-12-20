using BattleBoats.Rendering;

namespace BattleBoats.UI.Elements
{
	/*
	 * Generic text box that has set dimensions and
	 * fits the text inside of it. To make things simpler
	 * since I don't need dynamic textboxes of any kind
	 * I just represent a newline with a \n in the string I give it.
	 */
	public class TextBox : UIElement
	{
		public string Text;
		public (int, int) Dimensions;
		public Coordinates TextOffset = Coordinates.ORIGIN;
		
		public TextBox(Coordinates coords, (int, int) dimensions)
			: base(coords)
		{
			this.Dimensions = dimensions;
			this.Text = "";
		}

		public override void Update()
		{
		}

		/*
		 * Draws the text box by first creating the border
		 * then writing the text into it.
		 */
		public override void Draw()
		{
			var textLines = this.Text.Split("\n");
			
			// draw border
			Program.Renderer.PushImage(
				new TextImage().DrawBox(
					Coordinates.ORIGIN, new Coordinates(Dimensions.Item1 - 1, Dimensions.Item2 - 1),
					ConsoleColor.DarkGray
				),
				Coords, 
				1,
				true
			);
			
			// draw text
			for (int i = 0; i < textLines.Length; i++)
			{
				string line = textLines[i];
				Program.Renderer.PushImage(
					new TextImage().DrawText(
						line, ConsoleColor.White
					),
					new Coordinates(Coords.X + TextOffset.X + 2, Coords.Y + TextOffset.Y + i + 1),
					2,
					true
				);
			}
		}
	}
}
