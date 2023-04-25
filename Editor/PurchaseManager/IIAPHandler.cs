public interface IIAPHandler
{
    public bool CheckInit();
    public void Init();
    public void OnPurchaseButtonClick(string productId);
}