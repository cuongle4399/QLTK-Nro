public class EffectChar
{
	public static EffectTemplate[] effTemplates;

	public static sbyte EFF_ME;

	public static sbyte EFF_FRIEND = 1;

	public int timeStart;

	public int timeLenght;

	public short param;

	public EffectTemplate template;

	public EffectChar(short templateId, int timeStart, int timeLenght, short param)
	{
		template = effTemplates[templateId];
		this.timeStart = timeStart;
		this.timeLenght = timeLenght / 1000;
		this.param = param;
	}
}
