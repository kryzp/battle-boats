namespace BattleBoats.UI
{
	/*
	 * Abstract class representing an item in the menu,
	 * such as a button or a textbox.
	 */
	public abstract class UIElement
	{
		public int ID; // unique identifier for ui element
		public int UpID; // unique identifier to ui element to be moved to if the up arrow key is pressed
		public int DownID; // unique identifier to ui element to be moved to if the down arrow key is pressed
		public int LeftID; // unique identifier to ui element to be moved to if the left arrow key is pressed
		public int RightID; // unique identifier to ui element to be moved to if the right arrow key is pressed

		// tells the menu item if it is currently being selected by the user, another way
		// of thinking about it is that it means its "active"
		public bool IsSelected => Parent.GetSelectedItemID() == ID;
		
		// where to draw the menu item
		public Coordinates Coords;
		
		// the menu that controls this menu item
		public Menu Parent;

		public UIElement(Coordinates coords)
		{
			this.Coords = coords;
			this.ID = -1;
			this.UpID = -1;
			this.DownID = -1;
			this.LeftID = -1;
			this.RightID = -1;
		}

		public virtual void Update()
		{
		}

		public virtual void Draw()
		{
		}
	}
}
