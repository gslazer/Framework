using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using DG.Tweening;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;
using Unity.Collections;

public class PopupBase : MonoBehaviour
{
    private System.Action<object> callbackAction = null;
    [SerializeField][ReadOnly] protected List<Button> buttonList = new List<Button>();

    public virtual void Show()
    {
        gameObject.SetActive(true);
        buttonList = gameObject.GetComponentsInChildren<Button>().ToList();

        if(buttonList == null) return;
        
        // 2개 이상의 버튼이 있으면 재정렬 해줌
        if(buttonList.Count > 1)
        {
            buttonList = buttonList.OrderByDescending(x => x.transform.position.y).ToList();
        }

        // 버튼이 1개라도 있으면
        if(buttonList.Count > 0)
        {
            // 리스트 첫번째 버튼에 포커싱 해줌
            //UIManager.Instance.SetFocusObject(buttonList[0].gameObject);
        }

        // todo gz : 인게임 포즈 기능 이전 필요
        // 인 게임 멈춰줌
        Debug.Log("<color=Magenta>PopupBase.Show() : </color>" + this.name);
        //UIManager.Instance.OnPauseGame();
    }
    
    public virtual void Refresh()
    {
        if(buttonList.Count > 0)
        {
            // 리스트 첫번째 버튼에 포커싱 해줌
            //UIManager.Instance.SetFocusObject(buttonList[0].gameObject);
        }
    }

    public void InvokeSubmitResult(object result)
    {
        callbackAction?.Invoke(result);
    }
    
    public virtual void Hide()
    {
        Debug.Log($"{gameObject.name} :: PopupBase.Hide()");
        //SoundPlayManager.Instance.PlayUISFX("button_move_sound");        
        gameObject.SetActive(false);
    }
    public List<Button> GetButtonList()
    {
        return buttonList;
    }

    public void SetButtonSubmitCalbacks(params UnityEngine.Events.UnityAction[] callbacks)
    {
        for(int i = 0; i< callbacks.Length; i++)
        {
            //buttonList[i].SetEventCallback(EventTriggerType.Submit, callbacks[i]);
        }
    }
    public void SetCallbackAction(System.Action<object> callbackAction)
    {
        this.callbackAction = callbackAction;
    }
}
