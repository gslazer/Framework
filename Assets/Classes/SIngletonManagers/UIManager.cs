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
    void OnCLick();
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

public class UIManager : Singleton<UIManager>
{
    Stack<IUILayer> layerStack = new Stack<IUILayer>();
    
    public void AddLayer(IUILayer iUILayer)
    {
        layerStack.Push(iUILayer);
    }

    public void PopPopup()
    {
        var popup = layerStack.Pop();
        popup.OnEscape();
    }

    public IUILayer GetTopUILayer()
    {
        if(layerStack.Count==0)
            return null;
        return layerStack.Peek();
    }

    public override void Initialize()
    {
    }

}