using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public interface IDialogueUI : IPopupUI
{
    public void SetData(DialobueData data);

    public void Show();
    public void ShowString(string str);
    public void Skip();
    public void Next();
    public void Hide();
}

public class DialogueUI : MonoBehaviour, IDialogueUI, IPointerClickHandler
{
    DialobueData data;
    [SerializeField] private TextMeshProUGUI dialogueString;

    public void OnClick()
    {
        Next();
    }
    public void Next()
    {
        var nextStr = data.GetNextString();
        ShowString(nextStr);
    }

    public void SetData(DialobueData data)
    {
        this.data = data;
    }
    public void Hide()
    {
        throw new NotImplementedException();
    }
    public void Show()
    {
        throw new NotImplementedException();
    }

    public void Skip()
    {
        throw new NotImplementedException();
    }

    public void ShowString(string str)
    {
        dialogueString.text = str;
    }

    public void OnEscape()
    {
        throw new NotImplementedException();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Next();
    }


    //to delete gz : 여기부터는 임시 씬 스크립트
    
    void Awake()
    {
        DialobueData data = new DialobueData();
        SetData(data);
        ShowString(data.GetCurrentString());
    }
}