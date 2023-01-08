using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public interface IDialogueUI : UIObject
{
    public void SetData();
    public void ShowNext();
    public void SetHide();
}
public class DialogueUI : MonoBehaviour, IDialogueUI
{
    public void SetData()
    {
    }

    public void SetHide()
    {
    }

    public void ShowNext()
    {
    }
}