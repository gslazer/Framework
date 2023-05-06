using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupUI : MonoBehaviour, IPopupUI
{
    bool IsClosable = true;

    public void Close()
    {
        Destroy(gameObject);
    }

    public virtual void OnEscape()
    {
        if(IsClosable)
            Close();
    }

    public void Show()
    {
    }

    public virtual void SetData()
    {

    }
}
