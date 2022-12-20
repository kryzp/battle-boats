namespace BattleBoats
{
	/*
	 * Utility class for getting the input from the user.
	 * Since the command line pauses each time I gather input, and we can assume
	 * only one button will realistically be pressed each frame, I just get the
	 * input state of a single key and use that throughout the program. That way
	 * multiple classes can "poll" the input without stopping the program.
	 */
	public static class Input
	{
		private static ConsoleKeyInfo pressedKey;
		
		/*
		 * Called once per frame to get the next key pressed.
		 * It's fine that it pauses (in fact its *good* because
		 * it prevents a glitchy-looking screen) because the game
		 * will only update if theres input from the user, because
		 * why would it need to otherwise?
		 */
		public static void PollNewKey()
		{
			pressedKey = Console.ReadKey(true);
		}
		
		/*
		 * Function that returns if the polled key
		 * equals the current key being pressed by the user
		 */
		public static bool IsPressed(ConsoleKey key)
		{
			return pressedKey.Key == key;
		}
		
		/*
		 * Gives the current key that is being pressed
		 */
		public static ConsoleKey GetCurrentKey()
		{
			return pressedKey.Key;
		}
		
		/*
		 * Gives the current key character that is being pressed
		 */
		public static char GetCurrentKeyChar()
		{
			return pressedKey.KeyChar;
		}
	}
}
