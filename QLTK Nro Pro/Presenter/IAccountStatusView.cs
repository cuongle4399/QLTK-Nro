namespace QLTK_Nro_Pro.Presenter
{
    public interface IAccountStatusView
    {
        void SetAccountOnline(string accountId, string characterName);
        void SetAccountOffline(string accountId);
        void ResetAllCells();
    }
}
