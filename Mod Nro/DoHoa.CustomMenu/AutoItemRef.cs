using System.Threading;

namespace DoHoa.CustomMenu;

public class AutoItemRef
{
	public string Name;

	public int IdCon;

	public int Id;

	public volatile bool AutoFlag;

	public Thread Thread;
}
