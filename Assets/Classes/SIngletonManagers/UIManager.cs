using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public interface UIObject
{
}
public interface IPopupUI : UIObject, IEscape
{
}
public class PopupUI : IPopupUI
{
    public void OnCLick()
    {
    }

    public void OnEscape()
    {
    }
}

public interface IEscape
{
    public void OnEscape();
}

public class UIManager : Singleton<UIManager>
{
    Stack<IPopupUI> popupStack = new Stack<IPopupUI>();
    
    public void AddPopup(IPopupUI popup)
    {
        popupStack.Push(popup);
    }

    public void PopPopup()
    {
        var popup = popupStack.Pop();
        popup.OnEscape();
    }

    public override void Initialize()
    {
    }

}