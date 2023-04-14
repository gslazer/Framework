using System;
using Unity.Services.Core;
using Unity.Services.Core.Environments;

class GoogleIAPHandler : IIAPHandler
{
    
    private GoogleIAPListener listener = null;
    private string unityEnvironment = "UnityEnvironment";

    public async void Init()
    {
        //gz : Unity Game Service가 Initialize 상태여야 IAP를 Initialize할 수 있다.
        try
        {
            var options = new InitializationOptions()
                .SetEnvironmentName(unityEnvironment);
            await UnityServices.InitializeAsync(options);
        }
        catch (Exception exception)
        {
            Debug.LogError($"Fail to Initialize Unity Gaming Service : {exception.Message}");
            return;
        }

        Debug.Log("Unity Gameing Service Initialize Complete");
        if (listener == null)
            listener = new GoogleIAPListener();
    }

    public void OnPurchaseButtonClick(string productId)
    {
        if(listener == null || !listener.initialized)
        {
            Debug.LogError("Google IAP has not Initialized!");
            return;
        }
        listener.OnPurchaseClicked(productId);
    }

    public bool CheckInit()
    {
        return listener == null? false : listener.initialized;
    }

    public GoogleIAPHandler()
    {
        Init();
    }
}