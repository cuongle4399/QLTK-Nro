using Assets.src.g;
using Xmap;

namespace Mod.CuongLe;

internal class NhapCodeLive
{
	public bool isEnable;

	public string code;

	public static NhapCodeLive _Instance;

	public bool isTrain;

	public bool isGoBack;

	private long lastActionTime;

	private int step;

	private const int Delay = 300;

	public static NhapCodeLive getInstance()
	{
		return _Instance ?? (_Instance = new NhapCodeLive());
	}

	public void update()
	{
		if (!isEnable || MainXmapCL.isXmaping || string.IsNullOrEmpty(code))
		{
			return;
		}
		long num = mSystem.currentTimeMillis();
		switch (step)
		{
		case 0:
			isGoBack = AutoTrainCL.isGoBack;
			isTrain = AutoTrainCL.isAutoTrain;
			AutoTrainCL.isGoBack = false;
			AutoTrainCL.isAutoTrain = false;
			if (TileMap.mapID != 5)
			{
				MainXmapCL.StartGoToMap(5);
				break;
			}
			ModProCL.teleNPC(39);
			lastActionTime = num;
			step = 1;
			break;
		case 1:
			if (num - lastActionTime >= 300)
			{
				NextMap.startComfirmNpc(39, "Nhập mã quà tặng");
				lastActionTime = num;
				step = 2;
			}
			break;
		case 2:
			if (num - lastActionTime >= 300)
			{
				TField tField = new TField();
				tField.setText(code);
				Service.gI().sendClientInput(new TField[1] { tField });
				ClientInput.gI().perform(1, null);
				AutoTrainCL.isGoBack = isGoBack;
				AutoTrainCL.isAutoTrain = isTrain;
				isEnable = false;
				step = 0;
			}
			break;
		}
	}
}
