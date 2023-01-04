namespace BattleBoats.Rendering
{
	/*
	 * Holds data about the things to draw and
	 * then draws them out in the correct order
	 * when told to do so.
	 */
	public class Renderer
	{
		/*
		 * Packet of data containing all the required
		 * information to draw an image to the screen.
		 */
		private struct RenderPass
		{
			public TextImage TextImage;
			public Coordinates Coords;
			public float Layer;
			public bool OverrideEverything;
		}

		// holds all of the parts we need to draw this frame
		private List<RenderPass> imageStack;

		private TextImage fbo;

		public Renderer()
		{
			imageStack = new List<RenderPass>();
			fbo = new TextImage(Program.WINDOW_WIDTH, Program.WINDOW_HEIGHT);
		}

		/*
		 * Adds an image to the render stack.
		 */
		public void PushImage(TextImage img, Coordinates coords, float layer, bool overrideEverything = false)
		{
			imageStack.Add(new RenderPass()
			{
				TextImage = img,
				Coords = coords,
				Layer = layer,
				OverrideEverything = overrideEverything
			});
		}

		/*
		 * Clears the screen
		 */
		public void Clear()
		{
			for (int y = 0; y < Program.WINDOW_HEIGHT; y++)
			{
				for (int x = 0; x < Program.WINDOW_WIDTH; x++)
				{
					fbo.Chars[y][x] = new ColouredChar(' ', ConsoleColor.White);
				}
			}
		}

		/*
		 * Writes out all render passes in the stack.
		 */
		public void Render()
		{
			imageStack = imageStack.OrderBy(x => x.Layer).ToList();

			foreach (var p in imageStack)
			{
				p.TextImage.DrawTo(fbo, p.Coords, p.OverrideEverything);
            }

            imageStack.Clear();

            fbo.Draw(Coordinates.ORIGIN, true);
		}
	}
}
