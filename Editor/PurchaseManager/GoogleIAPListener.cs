using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Purchasing;

class GoogleIAPListener : IStoreListener
{
    private IStoreController controller;
    private IExtensionProvider extensions;
    public bool initialized { get; private set; }
    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        this.controller = controller;
        this.extensions = extensions;
        initialized = true;

        foreach (var product in controller.products.all)
        {
            Debug.Log(product.metadata.localizedTitle);
            Debug.Log(product.metadata.localizedDescription);
            Debug.Log(product.metadata.localizedPriceString);
        }
        Debug.Log("Google IAP Initialize Complete!");
    }
    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.LogError($"Google AIP Initialize Fail : error = {error}");
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        PurchaseManager.instance.OnPurchaseFailed("구매에 실패하였습니다.");
        Debug.LogError($"Purchase Failed : product = {product} reson = {failureReason}");
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
    {
        Debug.Log($"Process Purchases : {purchaseEvent.purchasedProduct}");
        PurchaseManager.instance.OnPurchaseComplete("구매가 완료되었습니다.");
        return PurchaseProcessingResult.Complete;
    }
    public GoogleIAPListener()
    {
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
        builder.AddProduct("com.owlgames.puzzleandrogue.purchasetest", ProductType.Consumable);
        UnityPurchasing.Initialize(this, builder);
        // Initialize Unity IAP...
    }
    public void OnPurchaseClicked(string productId)
    {
        controller.InitiatePurchase(productId);
    }
}