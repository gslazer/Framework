using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using Zarinpal;

/// <summary>
/// 결재 기능이 필요한 곳에서 각 결재 플랫폼의 사양에 관계 없이 작업할 수 있도록 
/// 공통된 메소드로 각 결재 플랫폼의 Initialize와 기능에 접근하기 위한 매니저.
/// 
/// GoogleIAP, Zarinal 등 ConfigData의 TargetMarket에 따라 선택적으로 결재 플랫폼 API를 Init 하고,
/// 각 구매 페이지에서 사용할수있는 유틸리티 메소드를 제공한다.
/// </summary>
public class PurchaseManager : MonoBehaviour
{
    public static PurchaseManager instance = null;
    private IIAPHandler IAPHandler = null;

    public void OnPurchaseComplete(string succeedMsg)
    {
        //GameAssistant.Instance.CreateMessagePopup("succeedMsg");
    }
    public void OnPurchaseSuceess(string succeedMsg)
    {
        OnPurchaseComplete(succeedMsg);
    }
    public void OnPurchaseFailed(string failMsg)
    {
        OnPurchaseComplete(failMsg);
        //Debug.Log($"Purchase Failed {product.transactionID}, {reason}");
    }
    IEnumerator CoConfigPurchaseData()
    {
        /*while (NetManager.Instance == null || NetManager.Instance.configData == null)
            yield return 0;*/
        TargetMarket purchaseSDK = TargetMarket.GOOGLE;// NetManager.Instance.configData.purchaseSDK;
        if (purchaseSDK == TargetMarket.NONE)
        {
            //Debug.LogError($"[{NetManager.Instance.configData.purchaseSDK}] IAP Handler Can not Initialized!");
            yield return 0;
        }

        switch (purchaseSDK)
        {
            case TargetMarket.ZARINPAL:
                {
                    ZaripalIABHandler zarinpalIABhandler = new ZaripalIABHandler(); //오타가 아니다. Zarinpal은 IAB이다.
                    IAPHandler = zarinpalIABhandler;
                }
                break;
            case TargetMarket.GOOGLE:
                {
                    GoogleIAPHandler googleIAPHandler = new GoogleIAPHandler();
                    IAPHandler = googleIAPHandler;
                }
                break;
            default:
                break;
        }
        yield return CoCheckInit();
    }
    IEnumerator CoCheckInit()
    {
        while (!IAPHandler.CheckInit())
            yield return 0;
        //Debug.Log($"[{NetManager.Instance.configData.purchaseSDK}] IAP Handler Initialized!");
    }
    public void OnPurchaseButtonClick(string productId)
    {
        if (IAPHandler == null)
        {
            //Debug.LogError($"PurchaseManager : [{NetManager.Instance.configData.purchaseSDK}] IAPHandler is null! ");
            return;
        }
        if (!IAPHandler.CheckInit())
        {
            //Debug.LogError($"PurchaseManager : [{NetManager.Instance.configData.purchaseSDK}] IAPHandler is not Initialized yet! ");
            return;
        }
        IAPHandler.OnPurchaseButtonClick(productId);
    }
    void Start()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
#if UNITY_EDITOR
            //Debug.Log($"[{NetManager.Instance.configData.purchaseSDK}] IAP Handler not Initialized On Unity Editor.");
#elif UNITY_ANDROID
            StartCoroutine(CoConfigPurchaseData());
#endif
        }
    }
}
