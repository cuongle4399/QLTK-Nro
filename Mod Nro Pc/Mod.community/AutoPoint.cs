namespace Mod.community;

public class AutoPoint : IActionListener, IChatable
{
    private static AutoPoint _Instance;

    public static PotentialType typePotential;
    public static bool isAutoPoint;
    public static int damageToAuto;
    public static int hpToAuto;
    public static int mpToAuto;

    private static readonly string[] inputDamageAuto = new string[2] { "Nhập Sức Đánh Mà Bạn Muốn Auto", "Sức Đánh" };
    private static readonly string[] inputHPAuto = new string[2] { "Nhập HP Mà Bạn Muốn Auto", "HP" };
    private static readonly string[] inputMPAuto = new string[2] { "Nhập MP Mà Bạn Muốn Auto", "MP" };

    public enum PotentialType
    {
        HP = 0,
        MP = 1,
        Damage = 2,
        Defense = 3,
        Critical = 4
    }

    public static AutoPoint getInstance()
    {
        if (_Instance == null)
        {
            _Instance = new AutoPoint();
        }
        return _Instance;
    }

    public static void Update()
    {
        if (isAutoPoint)
        {
            DoIt();
        }
    }

    public void onChatFromMe(string text, string to)
    {
        if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(ChatTextField.gI().tfChat.getText()))
        {
            Service.gI().chat(text);
            ResetChatTextField();
            return;
        }

        string chatStr = ChatTextField.gI().strChat;

        if (chatStr.Equals(inputDamageAuto[0]))
        {
            HandleAutoTarget(ref damageToAuto, "Auto Cộng Sức Đánh Tới: ", "Sức Đánh Không Hợp Lệ, Vui Lòng Nhập Lại!");
        }
        else if (chatStr.Equals(inputHPAuto[0]))
        {
            HandleAutoTarget(ref hpToAuto, "Auto Cộng HP Tới: ", "HP Không Hợp Lệ, Vui Lòng Nhập Lại!");
        }
        else if (chatStr.Equals(inputMPAuto[0]))
        {
            HandleAutoTarget(ref mpToAuto, "Auto Cộng MP Tới: ", "MP Không Hợp Lệ, Vui Lòng Nhập Lại!");
        }
        else
        {
            Service.gI().chat(text);
        }

        ResetChatTextField();
    }

    private void HandleAutoTarget(ref int target, string successMsg, string errorMsg)
    {
        try
        {
            int value = int.Parse(ChatTextField.gI().tfChat.getText());

            if (ChatTextField.gI().strChat.Equals(inputHPAuto[0]))
            {
                if (value <= Char.myCharz().cHPGoc)
                {
                    GameScr.info1.addInfo("HP Phải Lớn Hơn HP Hiện Tại (" + NinjaUtil.getMoneys(Char.myCharz().cHPGoc) + ")");
                    return;
                }
            }
            else if (ChatTextField.gI().strChat.Equals(inputMPAuto[0]))
            {
                if (value <= Char.myCharz().cMPGoc)
                {
                    GameScr.info1.addInfo("MP Phải Lớn Hơn MP Hiện Tại (" + NinjaUtil.getMoneys(Char.myCharz().cMPGoc) + ")");
                    return;
                }
            }
            else if (ChatTextField.gI().strChat.Equals(inputDamageAuto[0]))
            {
                if (value <= Char.myCharz().cDamGoc)
                {
                    GameScr.info1.addInfo("Sức Đánh Phải Lớn Hơn Sức Đánh Hiện Tại (" + NinjaUtil.getMoneys(Char.myCharz().cDamGoc) + ")");
                    return;
                }
            }

            target = value;

            isAutoPoint = true;

            GameScr.info1.addInfo(successMsg + NinjaUtil.getMoneys(target) + "\nAuto [STATUS: ON]");
        }
        catch
        {
            GameScr.info1.addInfo(errorMsg);
        }
    }

    public void onCancelChat()
    {
    }

    public void perform(int idAction, object p)
    {
        switch (idAction)
        {
            case 3:
                isAutoPoint = !isAutoPoint;
                GameScr.info1.addInfo("Auto\n" + (isAutoPoint ? "[STATUS: ON]" : "[STATUS: OFF]"));
                break;
            case 4:
                OpenChatInput(inputDamageAuto);
                break;
            case 5:
                OpenChatInput(inputHPAuto);
                break;
            case 6:
                OpenChatInput(inputMPAuto);
                break;
            case 7:
                isAutoPoint = false;
                GameScr.info1.addInfo("Đã Dừng Auto Cộng Điểm\n[STATUS: OFF]");
                break;
        }
    }

    private void OpenChatInput(string[] inputConfig)
    {
        ChatTextField.gI().strChat = inputConfig[0];
        ChatTextField.gI().tfChat.name = inputConfig[1];
        ChatTextField.gI().startChat2(getInstance(), string.Empty);
    }

    private static void ResetChatTextField()
    {
        ChatTextField.gI().strChat = "Chat";
        ChatTextField.gI().tfChat.name = "chat";
        ChatTextField.gI().isShow = false;
    }

    public static void DoIt()
    {
        if (GameCanvas.gameTick % 20 != 0) return;

        if (Char.myCharz().cTiemNang < 100) return;

        bool damageCompleted = (damageToAuto == 0 || Char.myCharz().cDamGoc >= damageToAuto);
        bool hpCompleted = (hpToAuto == 0 || Char.myCharz().cHPGoc >= hpToAuto);
        bool mpCompleted = (mpToAuto == 0 || Char.myCharz().cMPGoc >= mpToAuto);

        if (damageCompleted && hpCompleted && mpCompleted)
        {
            isAutoPoint = false;
            GameScr.info1.addInfo("Auto Cộng Điểm Hoàn Thành!\n[STATUS: OFF]");
            return;
        }

        if (TryUpgradeStat(PotentialType.Damage, Char.myCharz().cDamGoc, damageToAuto)) return;
        if (TryUpgradeStat(PotentialType.HP, Char.myCharz().cHPGoc, hpToAuto)) return;
        TryUpgradeStat(PotentialType.MP, Char.myCharz().cMPGoc, mpToAuto);
    }

    private static bool TryUpgradeStat(PotentialType type, int currentValue, int targetValue)
    {
        if (currentValue >= targetValue) return false;

        int remaining = targetValue - currentValue;
        bool isHPMP = (type == PotentialType.HP || type == PotentialType.MP);

        if (isHPMP)
        {
            remaining /= 20;
        }

        UpgradeCost costs = CalculateCosts(type, currentValue);

        if (remaining >= 100 && Char.myCharz().cTiemNang >= costs.cost100)
        {
            Service.gI().upPotential((int)type, 100);
            return true;
        }

        if (remaining >= 10 && Char.myCharz().cTiemNang >= costs.cost10)
        {
            Service.gI().upPotential((int)type, 10);
            return true;
        }

        if (remaining >= 1 && Char.myCharz().cTiemNang >= costs.cost1)
        {
            Service.gI().upPotential((int)type, 1);
            return true;
        }

        return false;
    }

    private static UpgradeCost CalculateCosts(PotentialType type, int currentValue)
    {
        long cost1, cost10, cost100;

        switch (type)
        {
            case PotentialType.Damage:
                int expForOneAdd = Char.myCharz().expForOneAdd;
                cost1 = currentValue * expForOneAdd;
                cost10 = 10L * (2 * currentValue + 9) / 2 * expForOneAdd;
                cost100 = 100L * (2 * currentValue + 99) / 2 * expForOneAdd;
                break;

            case PotentialType.HP:
            case PotentialType.MP:
                cost1 = currentValue + 1000;
                cost10 = 10 * (2 * (currentValue + 1000) + 180) / 2;
                cost100 = 100 * (2 * (currentValue + 1000) + 1980) / 2;
                break;

            default:
                cost1 = cost10 = cost100 = 0;
                break;
        }

        return new UpgradeCost(cost1, cost10, cost100);
    }

    private struct UpgradeCost
    {
        public long cost1;
        public long cost10;
        public long cost100;

        public UpgradeCost(long cost1, long cost10, long cost100)
        {
            this.cost1 = cost1;
            this.cost10 = cost10;
            this.cost100 = cost100;
        }
    }
}