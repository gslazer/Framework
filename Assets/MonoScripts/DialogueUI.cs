using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public interface IDialogueUI : IPopupUI
{
    public void SetData(DialogueData data);

    public void Show();
    public void ShowString(string str);
    public void Skip();
    public void Next();
    public void Hide();
}

public class DialogueUI : MonoBehaviour, IDialogueUI, IPointerClickHandler
{
    DialogueData data;
    [SerializeField] private TextMeshProUGUI dialogueString;

    public void OnClick()
    {
        Next();
    }
    public void Next()
    {
        var nextStr = data.GetNextString();
        if (nextStr == null)
            Close();
        ShowString(nextStr);
    }

    public void SetData(DialogueData data)
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
    void Start()
    {
        DialogueData data = DataManager.Instance.GetDialogueData("SampleDialogueData");
        if (data == null)
            Close();
        SetData(data);
        ShowString(data?.GetCurrentString());
    }

    public void Close()
    {
        Destroy(gameObject);
    }
}