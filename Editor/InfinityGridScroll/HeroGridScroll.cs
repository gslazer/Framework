using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// 제네릭 클래스 InfinityGridScroll의 HeroInfo 타입한정 상속체
/// 유니티 오브젝트에서 이 클래스 또는 이 클래스의 상속체를 Compponent로 사용할 것
/// </summary>
public class HeroGridScroll : InfinityGridScroll<HeroInfo>
{
    public void SetData(HeroInfo[] arrGridItemData, int slotCount, Action<HeroSlot> setSlotDataCallBack)
    {
        SetData(arrGridItemData, PrefabManager.Instance.heroSlotPrefab, slotCount, (GridItemSlot<HeroInfo> gridSlot) => {
                setSlotDataCallBack((HeroSlot)gridSlot);
            });
    }

    public HeroInfo[] GetHeroInfoArr()
    {
        return GetDataArray();
    }

    public List<HeroSlot> GetSlotList()
    {
        return GetSlotList<HeroSlot>();
    }

    public HeroSlot GetSlot(int slotIndex)
    {
        return GetSlot<HeroSlot>(slotIndex);
    }
    public HeroInfo GetHeroInfo(int slotIndex)
    {
        return GetSlotData(slotIndex);
    }
}