using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using DG.Tweening;
using System.Linq;

// PopupManager 요약
// Dont Destroy상태로 모든 씬에서 하나만 존재한다.


public class PopupManager : MonoSingleton<PopupManager>
{   
//  Members : 
    [SerializeField] private GameObject popupCanvas;
    private GameObject PopupCanvasOBJ {
        get{
            if(popupCanvas == null || popupCanvas.activeSelf == false)
            {
                popupCanvas = GameObject.FindGameObjectsWithTag("PopupCanvas").Last();
            }
            return popupCanvas;
        }
    }
    
    //Prefab Asset Resource Dictionary
    private Dictionary<PopupEnum, PopupBase> prefabDict = null;

    //Active Popup Stack
    private Stack<PopupBase> popupStack= new Stack<PopupBase>();
    private PopupBase currentPopup {
        get => popupStack.Count > 0 ? popupStack.Peek() : null;
    } 

//  Unity Methods : 
    void Awake() 
    {
        if(Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Initialize();
    }

//  Private Methods : 

    public override void Initialize()
    {
        DontDestroyOnLoad(gameObject);
        
        // 팝업 프래팹 리소스 로드
        if(prefabDict == null)
        {
            prefabDict = new Dictionary<PopupEnum, PopupBase>();
            prefabDict.Append(LoadPopupPrefabsToDictionary<PopupBase>("Popup/Ingame"));
            prefabDict.Append(LoadPopupPrefabsToDictionary<PopupBase>("Popup/Main"));
            prefabDict.Append(LoadPopupPrefabsToDictionary<PopupBase>("Popup/ChapterSelect"));
        }
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
    {
        popupCanvas = scene.GetRootGameObjects().ToList().Find(obj => obj.tag == "PopupCanvas");
        popupStack.Clear();
        //popupCanvas.GetComponent<Canvas>().worldCamera = GameObject.FindGameObjectsWithTag("UI_Camera").Last();
    }

    private Dictionary<PopupEnum, T> LoadPopupPrefabsToDictionary<T>(string path) where T : Object
    {
        return Utils.LoadPrefabsToDictionary<PopupEnum, T>(path);
    }

    private PopupBase GetPopupPrefab(PopupEnum popupEnum)
    {
        if(!prefabDict.TryGetValue(popupEnum, out PopupBase popup))
        {
            Debug.LogError("PopupManager.GetPopupPrefab() : no resource match with key");
            Debug.LogError($"kay : {popupEnum.ToString()}");
            return null;
        }
        return popup;
    }

    private IEnumerator CoDelayAframeAndShow(PopupBase popupObject)
    {
        yield return new WaitForEndOfFrame();
        popupObject.Show();
    }

    public PopupBase ShowPopup(PopupEnum popupEnum)
    {
        return ShowPopup<PopupBase>(popupEnum);
    }
    public T ShowPopup<T>(PopupEnum popupEnum) where T : PopupBase
    {
        // Info GZ : ShowPopup()으로 팝업이 생성됨과 동시에 EventTrigger가 전달되는 상황을 막기 위해 한프레임 후에 생성한다.
        Debug.Log($"<color=Red>PopupManager.CoShowPopup() :</color> {popupEnum.ToString()}");

        T popupObject ;

        Debug.Log("PopupManager.ShowPopup() : From New Instantiate Object");
        var prefab = GetPopupPrefab(popupEnum) as T;
        popupObject= Instantiate(prefab, PopupCanvasOBJ.transform);  
        //popupPool.Add(popupEnum, popupObject);

        if(popupStack.Count == 0)
            //UIManager.Instance.KeepLastSelectedUI();

        popupObject.Show();
        popupStack.Push(popupObject);

        return popupObject;
    }

//  Public Methods : 
    public void CloseLastPopup()
    {
        var closedPopup = popupStack.Pop();
        
        Debug.Log($"PopupManager.CloseLastPopup() : {closedPopup.name}closed.");
        Debug.Log("PopupManager.PopupStack.Count is " + popupStack.Count);
        
        //closedPopup.Close();

        if(popupStack.Count==0)
        {
            //todo gz : 팝업 종료 플로우 개선할것
            // UIManager.Instance.FocusToLastSelectedObject();
            // UIManager.Instance.OnResumeGame();
            return;
        }
        
        popupStack.Peek().Refresh();
    }

    public GameObject GetCurrentPopup(PopupUI popup)
    {
        return currentPopup.gameObject;
    }
}
