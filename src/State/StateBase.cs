namespace BattleBoats.State
{
	public abstract class StateBase
	{
		public abstract void Init();
		public abstract void Exit();
		public abstract void Update();
		public abstract void Draw();
	}
}
