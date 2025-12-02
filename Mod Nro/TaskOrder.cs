public class TaskOrder
{
	public const sbyte TASK_DAY = 0;

	public const sbyte TASK_BOSS = 1;

	public int taskId;

	public int count;

	public short maxCount;

	public string name;

	public string description;

	public int killId;

	public int mapId;

	public TaskOrder(sbyte taskId, short count, short maxCount, string name, string description, sbyte killId, sbyte mapId)
	{
		this.count = count;
		this.maxCount = maxCount;
		this.taskId = taskId;
		this.name = name;
		this.description = description;
		this.killId = killId;
		this.mapId = mapId;
	}
}
