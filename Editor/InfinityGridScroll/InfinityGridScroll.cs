using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public interface IGridItemSlot<TSlotData>
{
    public void InitLayout();
    public void SetData(TSlotData data, int slotIndex);
    public void SetDefault(bool bDisEnable = true);
}

public abstract class GridItemSlot<TSlotData> : MonoBehaviour, IGridItemSlot<TSlotData>
{
    public abstract void InitLayout();
    public abstract void SetData(TSlotData data, int slotIndex);
    public abstract void SetDefault(bool bDisEnable = true);
}

/// <summary>
/// gz : TSlotData와 IGridItemSlot에 대응해 사용가능한 범용 무한 스크롤 UI
/// </summary>
/// <typeparam name="TSlotData"></typeparam>
public class InfinityGridScroll<TSlotData> : MonoBehaviour
{
    //[SerializeField] RectTransform rectParent;
    [Title("Infinity Grid Scroll : Essential")]
    [SerializeField] private ScrollRect scrollRect; //유니티 ScrollRect 오브젝트

    [Title("Infinity Grid Scroll : Layout Rect Transforms")]
    [SerializeField] private RectTransform upperRect; //그리드 상단에 배치할 공간
    [SerializeField] private RectTransform downerRect; //그리드 아래에 배치할 공간
    [SerializeField] private GridLayoutGroup gridLayout; //그리드 레이아웃 UI

    [Title("Infinity Grid Scroll : Slot and Datas")]
    [ShowInInspector][ReadOnly] private Dictionary<int,GridItemSlot<TSlotData>> slotDict = new Dictionary<int, GridItemSlot<TSlotData>>(); //그리드에 유지하는 슬롯 오브젝트
    private GridItemSlot<TSlotData> gridItemSlot; //그리드에 생성할 슬롯 프리팹
    private RectTransform contentRect; //빈공간이 포함된 스크롤뷰 전체 콘텐트

    [ShowInInspector][ReadOnly] private TSlotData[] arrGridItemData; //전체 데이터 Array
    private Action<GridItemSlot<TSlotData>> setSlotDataCallBack = null; //TSlotData.SetData() 이후에 슬롯의 후처리가 필요한 경우 등록할 콜백

    private int visibleCount; //실제 슬롯을 생성하기위한, 화면에 보이는 슬롯의 수
    private int headIndex; //최상단 오브젝트의 index
    private int widthCount; // 그리드에 실제 배치되는 슬롯의 가로 갯수
    private int heightCount;// 그리드에 실제 배치되는 슬롯의 세로 갯수

    private int dataCount = 0; // 표시해야할 전체 데이터 카운트 = arrGridItemData.length // 쓸모 적을시 삭제
    private int slotCount = 0; //빈 슬롯을 포함한 전체 슬롯 카운트

    
    private void Awake()
    {
        scrollRect.onValueChanged.AddListener(OnScrollChange);
    }
    protected void SetData(TSlotData[] arrGridItemData, GridItemSlot<TSlotData> slotPrefab, int slotCount, Action<GridItemSlot<TSlotData>> setSlotDataCallBack)
    {
        this.setSlotDataCallBack = setSlotDataCallBack;
        SetData(arrGridItemData, slotPrefab, slotCount);
    }
    protected void SetData(TSlotData[] arrGridItemData, GridItemSlot<TSlotData> slotPrefab, int slotCount)
    {
        this.arrGridItemData = arrGridItemData;
        this.gridItemSlot = slotPrefab;
        this.slotCount = slotCount;
        var visibleRect = scrollRect.GetComponent<RectTransform>();
        var slotRect = gridItemSlot.GetComponent<RectTransform>();
        contentRect = scrollRect.content;


        headIndex = 0;
        widthCount = (visibleRect.rect.width / slotRect.rect.width).FloorToInt();
        var cellSpace = gridLayout.spacing;
        var gridRect = gridLayout.GetComponent<RectTransform>();
        gridRect.sizeDelta = new Vector2(gridLayout.cellSize.x * widthCount + cellSpace.x * widthCount + gridLayout.padding.left + gridLayout.padding.right, gridRect.sizeDelta.y);

        heightCount = (visibleRect.rect.height / slotRect.rect.height).FloorToInt() + 1; //한줄 더 생성해야한다.
        visibleCount = (widthCount * heightCount).Min(dataCount);

        if (visibleCount > slotCount)
            visibleCount = slotCount;

        ResizeBlankRectHeight();

        List<GridItemSlot<TSlotData>> slotRecyclePool = new List<GridItemSlot<TSlotData>>();
        if (slotDict.Count > 0)
        {
            slotRecyclePool = slotDict.Values.ToList();
            slotDict.Clear();
        }
        for (int index = 0; index < visibleCount; index++)
        {
            if (slotRecyclePool.Count > 0)
            {
                slotDict.Add(index, slotRecyclePool[0]);
                slotRecyclePool.RemoveAt(0);
            }
            else
                slotDict.Add(index, Instantiate(slotPrefab, gridLayout.transform));
            slotDict[index].transform.SetAsLastSibling();
            SetSlotData(index);
        }
        if (slotRecyclePool.Count > 0)
            slotRecyclePool.ForEach(slot => Destroy(slot.gameObject));

        LayoutRebuilder.ForceRebuildLayoutImmediate(gridRect);
    }

    private void OnScrollChange(Vector2 vec)
    {
        Refresh();
    }

    private void MoveDictionaryData(int fromKey, int toKey)
    {
        if (!slotDict.ContainsKey(fromKey))
        {
            Debug.LogWarning($"dictionary dont contain key : {fromKey}");
            return;
        }
        var tempSlot = slotDict[fromKey];
        slotDict.Remove(fromKey);
        slotDict.Add(toKey, tempSlot);
        SetSlotData(toKey);
    }

    private void SetSlotData(int index)
    {
        if (!slotDict.ContainsKey(index))
            return;
        if(slotCount<=index)
        {
            slotDict[index].gameObject.SetActive(false);
            return;
        }
        if (!slotDict[index].isActiveAndEnabled)
        {
            slotDict[index].gameObject.SetActive(true);
        }
        if (arrGridItemData.Length <= index)
        {
            slotDict[index].SetDefault();
            return;
        }
        slotDict[index].SetData(arrGridItemData[index], index);
        if (setSlotDataCallBack != null)
            setSlotDataCallBack(slotDict[index]);
    }

    private void ResizeBlankRectHeight()
    {
        int upperCount = headIndex / widthCount;
        var cellSizeY = gridLayout.cellSize.y;
        var cellSpaceY = gridLayout.spacing.y;
        float upperHeight = upperCount * (cellSizeY + cellSpaceY);
        int downerCount = (arrGridItemData.Length / widthCount - upperCount - heightCount + 1);
        float downerHeight = downerCount > 0 
            ? downerCount * (cellSizeY + cellSpaceY) 
            : 0f;
        upperRect.sizeDelta = new Vector2(upperRect.sizeDelta.x,  upperHeight);
        downerRect.sizeDelta = new Vector2(downerRect.sizeDelta.x, downerHeight);
    }

    private int getCurrentIndex()
    {
        int maxIndex = ((slotCount / widthCount) - heightCount + 1).Min(1) * widthCount;
        int index = (int)(contentRect.anchoredPosition.y / (gridLayout.cellSize.y+ gridLayout.spacing.y)) * widthCount;
        if (index < 0)
            index = 0;
        if (index > maxIndex)
            index = maxIndex;
        return index;
    }

    //Protected Utility Methods
    //제네릭을 지정하는 상속체에서 Wrapping하여 사용할것
    protected List<T> GetSlotList<T>() where T : GridItemSlot<TSlotData>
    {
        var dicToList =  slotDict.Values.ToList();
        var listT = new List<T>();
        dicToList.ForEach(item => listT.Add((T)item));
        return listT;
    }

    protected T GetSlot<T>(int index) where T: GridItemSlot<TSlotData>
    {
        if (!slotDict.ContainsKey(index))
            return null;
        return slotDict[index] as T;
    }
    protected TSlotData[] GetDataArray()
    {
        return arrGridItemData;
    }
    protected List<TSlotData> GetDataList()
    {
        return arrGridItemData.ToList();
    }
    protected TSlotData GetSlotData(int slotIndex)
    {
        if (arrGridItemData.Length <= slotIndex)
            return default(TSlotData);
        return arrGridItemData[slotIndex];
    }


    //Public Utility Methods
    //Wrapping 필요없고, 외부에서 접근할 필요가 많은 함수들
    public void Refresh()
    {
        //1. ScrollRect의 anchoredPosition로부터 현재 표시해야하는 newHeadIndex 가져온다.
        //2. newHeadIndex 이전의 슬롯은 가장 뒤로 이동하고, newHeadIndex + visibleCount 보다 뒤의 슬롯은 가장 앞으로 이동시킨다.
        //3. 슬롯의 새 index에 맞는 데이터를 세트한다.
        //4. 상하단의 Blank Rect들의 Height를 최신화한다.

        var newHeadIndex = getCurrentIndex();
        if (headIndex == newHeadIndex)
        {
            return;
        }
        if (headIndex < newHeadIndex)
        {
            for (int oldIndex = headIndex; oldIndex < newHeadIndex; oldIndex++)
            {
                int newIndex = oldIndex + visibleCount;
                MoveDictionaryData(oldIndex, newIndex);
                slotDict[newIndex].transform.SetAsLastSibling();
            }
        }
        else if (headIndex > newHeadIndex)
        {
            for (int newIndex = headIndex - 1; newIndex >= newHeadIndex; newIndex--)
            {
                int oldIndex = newIndex + visibleCount;
                MoveDictionaryData(oldIndex, newIndex);
                slotDict[newIndex].transform.SetAsFirstSibling();
            }
        }
        headIndex = newHeadIndex;
        ResizeBlankRectHeight();
    }
    public void ResetScrollValue()
    {
        scrollRect.verticalNormalizedPosition = 1f;
    }
    public int GetDataCount()
    {
        if (arrGridItemData == null)
            return 0;
        return arrGridItemData.Length;
    }
}
