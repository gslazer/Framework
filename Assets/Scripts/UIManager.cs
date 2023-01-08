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


public interface IEscape
{
    public void OnEscape();
}

public class UIManager : Singleton<UIManager>
{
    Stack<PopupUI> popupStack = new Stack<PopupUI>();
    
    public void AddPopup(PopupUI popup)
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