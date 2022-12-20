using BattleBoats.Rendering;

namespace BattleBoats.UI.Elements
{
	public class Button : UIElement
	{
		public Action OnClick;
		public string Text;
		public CentreMode CentreMode;
		
		public Button(Coordinates coords, string text, Action onClick)
			: base(coords)
		{
			this.OnClick = onClick;
			this.Text = text;
			this.CentreMode = CentreMode.LEFT;
		}
		
		public Button(CentreMode mode, Coordinates coords, string text, Action onClick)
			: base(coords)
		{
			this.OnClick = onClick;
			this.Text = text;
			this.CentreMode = mode;
		}
		
		public override void Update()
		{
			if (Input.IsPressed(ConsoleKey.Enter) && IsSelected)
				OnClick?.Invoke();
		}

		public override void Draw()
		{
			int xcoord = CentreMode switch
			{
				CentreMode.LEFT   => this.Coords.X,
				CentreMode.MIDDLE => this.Coords.X - (Text.Length / 2),
				CentreMode.RIGHT  => this.Coords.X - (Text.Length / 1)
			};

			ConsoleColor textCol = IsSelected ? ConsoleColor.White  : ConsoleColor.DarkGray;
			ConsoleColor bordCol = IsSelected ? ConsoleColor.Yellow : ConsoleColor.DarkGray;

			Program.Renderer.PushImage(new TextImage().DrawBox(Coordinates.ORIGIN, new Coordinates(Text.Length + 3, 2), bordCol), new Coordinates(xcoord - 1, Coords.Y - 1), 1, true);
			Program.Renderer.PushImage(new TextImage().DrawText(Text, textCol), new Coordinates(xcoord + 1, Coords.Y), 2, false);
		}
	}
}
