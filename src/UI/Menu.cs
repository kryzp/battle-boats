namespace BattleBoats.UI
{
	/*
	 * Base class all menus can derive from. Handles
	 * the updating of all menu items that it contains
	 * and allowing for arrow key movement between them.
	 */
	public class Menu
	{
		private UIElement? currentSelectedItem;
		private UIElement? nextSelectedItem;

		private readonly List<UIElement> elements;

		public Menu()
		{
			elements = new List<UIElement>();
			currentSelectedItem = null;
			nextSelectedItem = null;
		}
		
		/*
		 * Adds a UI Element to our managed elements.
		 */
		public void Add(UIElement elem)
		{
			elem.Parent = this;
			elements.Add(elem);
		}

		/*
		 * Updates each item and polls for input in the item that is being selected
		 * to allow transitions to other ui elements.
		 */
		public void Update()
		{
			foreach (var item in elements)
			{
				item.Update();

				if (item.IsSelected)
				{
					var k = Input.GetCurrentKey();

					if (k == ConsoleKey.UpArrow)
						SetSelectedElement(item.UpID);

					if (k == ConsoleKey.DownArrow)
						SetSelectedElement(item.DownID);

					if (k == ConsoleKey.LeftArrow)
						SetSelectedElement(item.LeftID);

					if (k == ConsoleKey.RightArrow)
						SetSelectedElement(item.RightID);
				}
			}
			
			if (nextSelectedItem != null)
			{
				currentSelectedItem = nextSelectedItem;
				nextSelectedItem = null;
			}
		}

		/*
		 * Draws all menu items in the scene.
		 */
		public void Draw()
		{
			elements.ForEach(x => x.Draw());
		}

		/*
		 * Returns the ID of the currently selected
		 * item.
		 */
		public int GetSelectedItemID() => currentSelectedItem?.ID ?? -1;
		
		/*
		 * Sets what item gets to be selected by the
		 * user via its ID.
		 */
		public void SetSelectedElement(int id) => nextSelectedItem = elements.FirstOrDefault(x => x.ID == id);
	}
}
