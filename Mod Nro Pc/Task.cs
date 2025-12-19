public class Task
{
	public int index;

	public int max;

	public short[] counts;

	public short taskId;

	public string[] names;

	public string[] details;

	public string[] subNames;

	public string[] contentInfo;

	public short count;

	public Task(short taskId, sbyte index, string name, string detail, string[] subNames, short[] counts, short count, string[] contentInfo)
	{
		this.taskId = taskId;
		this.index = index;
		names = mFont.tahoma_7b_green2.splitFontArray(name, Panel.WIDTH_PANEL - 20);
		details = mFont.tahoma_7.splitFontArray(detail, Panel.WIDTH_PANEL - 20);
		this.subNames = subNames;
		this.counts = counts;
		this.count = count;
		this.contentInfo = contentInfo;
	}
}
