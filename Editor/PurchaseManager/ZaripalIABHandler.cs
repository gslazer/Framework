using Zarinpal;

public class ZaripalIABHandler : IIAPHandler
{
    public void Init()
    {
        if (!ZarinpalIAB.Initialized)
            ZarinpalIAB.Init(showInvoice: true);
        
        ZarinpalIAB.OnPurchase += (PurchaseResult result) =>
        {

            if (result.Success && result.Status == "OK")
            {
                Debug.Log("ZarinpalIAB.OnPurchase() : result.Success!");
                Debug.Log($"result.Status : {result.Success}");
                Debug.Log($"result.Status : {result.Amount}");
                Debug.Log($"result.Status : {result.Date}");
                Debug.Log($"result.Status : {result.Description}");
                Debug.Log($"result.Status : {result.Provider}");
                Debug.Log($"result.Status : {result.RedirectUrl}");
                Debug.Log($"result.Status : {result.Status}");
                Debug.Log($"result.Status : {result.TransactionID}");
                Debug.Log(result);
                UnityEngine.Debug.unityLogger.Log("Zarinpal", result);
                PurchaseManager.instance.OnPurchaseSuceess("zarinpal purchase Success!!");
            }
            else if (!result.Success || result.Status == "NOK")
            {
                Debug.Log("ZarinpalIAB.OnPurchase() : result Failed!!");
                Debug.Log($"result.Status : {result.Status}");
                Debug.Log($"result.ErrorMessage : {result.ErrorMessage}");
                PurchaseManager.instance.OnPurchaseFailed($"zarinpal purchase failed! : {result.ErrorMessage}");
            }
        };
        
        ZarinpalIAB.OnQuery += (QuerySkuResult result) =>
        {
            Debug.Log("Zarinpal OnQuery callback");
            Debug.Log($"{result.Results}");
        };
        ZarinpalIAB.OnQueryError += (string queryError) =>
        {
            Debug.Log("Zarinpal OnQueryError callback");
            Debug.Log($"{queryError}");
        };
    }
    public ZaripalIABHandler()
    {
        Init();
    }
    public void OnPurchaseButtonClick(string productId)
    {
        if (!ZarinpalIAB.Initialized)
        {
            Debug.Log("zarinpal IAP is not initialized!");
            return;
        }

        Debug.Log("ZaripalIABHandler.OnPurchaseButtonClick() : Try Zaripal Purchase!");


        ZarinpalIAB.PurchaseBySKU(productId);
    }

    public bool CheckInit()
    {
        return ZarinpalIAB.Initialized;
    }
}