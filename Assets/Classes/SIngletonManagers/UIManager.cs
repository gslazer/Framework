using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public interface IUILayer : IEscape
{
    void Show();
    void Close();
}
public interface IPopupUI : IUILayer
{
}
public interface IClickableUI
{
    void OnClick();
}
public class PopupUI : IPopupUI
{
    public void Close()
    {
    }

    public void OnEscape()
    {
    }

    public void Show()
    {
    }
}

public interface IEscape
{
    public void OnEscape();
}

public class UIManager : Singleton<UIManager>, IEscape
{
    Stack<IUILayer> uiLayers = new Stack<IUILayer>();

    public void PushUI(IUILayer iUILayer)
    {
        uiLayers.Push(iUILayer);
    }

    public void PopUI()
    {
        var popup = uiLayers.Pop();
        popup.OnEscape();
    }

    public IUILayer PeekUI()
    {
        if(uiLayers.Count==0)
            return null;
        return uiLayers.Peek();
    }

    public override void Initialize()
    {
    }

    public void OnEscape()
    {
        if (uiLayers.Count == 0)
            return;
        uiLayers.Peek().OnEscape();
    }
}