public abstract class IPaint
{
	public abstract void paintDefaultBg(mGraphics g);

	public abstract void paintfillDefaultBg(mGraphics g);

	public abstract void repaintCircleBg();

	public abstract void paintSolidBg(mGraphics g);

	public abstract void paintDefaultPopup(mGraphics g, int x, int y, int w, int h);

	public abstract void paintWhitePopup(mGraphics g, int y, int x, int width, int height);

	public abstract void paintDefaultPopupH(mGraphics g, int h);

	public abstract void paintCmdBar(mGraphics g, Command left, Command center, Command right);

	public abstract void paintSelect(mGraphics g, int x, int y, int w, int h);

	public abstract void paintLogo(mGraphics g, int x, int y);

	public abstract void paintHotline(mGraphics g, string num);

	public abstract void paintInputTf(mGraphics g, bool iss, int x, int y, int w, int h, int xText, int yText, string text);

	public abstract void paintTabSoft(mGraphics g);

	public abstract void paintBackMenu(mGraphics g, int x, int y, int w, int h, bool iss);

	public abstract void paintMsgBG(mGraphics g, int x, int y, int w, int h, string title, string subTitle, string check);

	public abstract void paintDefaultScrLisst(mGraphics g, string title, string subTitle, string check);

	public abstract void paintCheck(mGraphics g, int x, int y, int index);

	public abstract void paintImgMsg(mGraphics g, int x, int y, int index);

	public abstract void paintTitleBoard(mGraphics g, int roomID);

	public abstract void paintCheckPass(mGraphics g, int x, int y, bool check, bool focus);

	public abstract void paintInputDlg(mGraphics g, int x, int y, int w, int h, string[] str);

	public abstract void paintIconMainMenu(mGraphics g, int x, int y, bool iss, bool issSe, int i, int wStr);

	public abstract void paintLineRoom(mGraphics g, int x, int y, int xTo, int yTo);

	public abstract void paintCellContaint(mGraphics g, int x, int y, int w, int h, bool iss);

	public abstract void paintScroll(mGraphics g, int x, int y, int h);

	public abstract int[] getColorMsg();

	public abstract void paintLogo(mGraphics g);

	public abstract void paintTextLogin(mGraphics g, bool issRes);

	public abstract void paintSellectBoard(mGraphics g, int x, int y, int w, int h);

	public abstract int issRegissterUsingWAP();

	public abstract string getCard();

	public abstract void paintSellectedShop(mGraphics g, int x, int y, int w, int h);

	public abstract string getUrlUpdateGame();

	public string getFAQLink()
	{
		return "http://wap.teamobi.com/faqs.php?provider=";
	}

	public abstract void doSelect(int focus);
}
