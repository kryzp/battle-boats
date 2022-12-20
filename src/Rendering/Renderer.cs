namespace BattleBoats.Rendering
{
	public class Renderer
	{
		private struct RenderPass
		{
			public TextImage TextImage;
			public Coordinates Coords;
			public float Layer;
			public bool OverrideEverything;
		}

		private List<RenderPass> imageStack;

		public Renderer()
		{
			imageStack = new List<RenderPass>();
		}

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

		public void Clear()
		{
			for (int y = 0; y < Program.WINDOW_HEIGHT; y++)
			{
				for (int x = 0; x < Program.WINDOW_WIDTH; x++)
				{
					DrawUtils.Write(x, y, ' ');
				}
			}
		}

		public void Render()
		{
			imageStack = imageStack.OrderBy(x => x.Layer).ToList();

			foreach (var p in imageStack)
			{
				p.TextImage.Draw(p.Coords, p.OverrideEverything);
			}
			
			imageStack.Clear();
		}
	}
}
