namespace BattleBoats.Rendering
{
	public class LoadingBar
	{
		private DateTime time1 = DateTime.Now;
		private DateTime time2 = DateTime.Now;

		private int counter = 0;
		private float time = 0f;
		
		private float progress = 0f;
		public float Progress => progress;

		public LoadingBar()
		{
			this.progress = 0f;
		}

		public void Run(Coordinates coords, int length, Func<float, float> progressFn, int delay)
		{
			if (counter < delay)
			{
				counter++;
				return;
			}

			// I use datetime.Now as tick speed varies from computer to
			// computer meaning that if we dont take into account actual
			// time differences the loading bar can take ages for people on
			// slow computers and really fast on quick computers.
			// since the loading bar is just an aesthetic thing I make
			// sure it runs in ~approx. the same speed for everyone.
			time2 = DateTime.Now;
			time += (time2.Ticks - time1.Ticks) / 5000f;
			time1 = time2;

			counter = 0;
			progress = progressFn(time / 1024f);
			Draw(coords, length);
		}
		
		private void Draw(Coordinates coords, int length)
		{
			TextImage output = new TextImage();
			
			output.DrawChar(Coordinates.ORIGIN, new ColouredChar('[', ConsoleColor.White));
			output.DrawChar(new Coordinates(length+1, 0), new ColouredChar(']', ConsoleColor.White));

			for (int i = 0; i < length; i++)
			{
				if ((i * 100 / length) < Progress)
					output.DrawChar(new Coordinates(i+1, 0), new ColouredChar('#', ConsoleColor.Yellow));
				else
					output.DrawChar(new Coordinates(i+1, 0), new ColouredChar(' ', ConsoleColor.Gray));
			}

			Program.Renderer.PushImage(output, coords, 4, true);
		}
	}
}
