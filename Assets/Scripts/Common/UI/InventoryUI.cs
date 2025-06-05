using Gpm.Ui;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


// 인벤토리 정렬 타입 열거형 정의
public enum InventorySortType
{
    // 아이템 등급 기준 정렬
    ItemGrade,
    // 아이템 타입 기준 정렬
    ItemType,
}
// 인벤토리 UI 클래스 - BaseUI를 상속받음
public class InventoryUI : BaseUI
{
    // 무기 슬롯 컴포넌트 참조
    public EquippedItemSlot WeaponSlot;
    // 방패 슬롯 컴포넌트 참조
    public EquippedItemSlot ShieldSlot;
    // 가슴 방어구 슬롯 컴포넌트 참조
    public EquippedItemSlot ChestArmorSlot;
    // 부츠 슬롯 컴포넌트 참조
    public EquippedItemSlot BootsSlot;
    // 장갑 슬롯 컴포넌트 참조
    public EquippedItemSlot GlovesSlot;
    // 액세서리 슬롯 컴포넌트 참조
    public EquippedItemSlot AccessorySlot;

    // 인벤토리 스크롤 리스트 컴포넌트 참조
    public InfiniteScroll InventoryScrollList;
    // 정렬 버튼의 텍스트 컴포넌트 참조
    public TextMeshProUGUI SortBtnTxt;

    // 현재 인벤토리 정렬 타입 (기본값: 아이템 등급)
    private InventorySortType m_InventorySortType = InventorySortType.ItemGrade;

    // UI 정보를 설정하는 메서드 오버라이드
    public override void SetInfo(BaseUIData uiData)
    {
        // 부모 클래스의 SetInfo 메서드 호출
        base.SetInfo(uiData);

        // 장착된 아이템들 설정
        SetEquippedItems();
        // 인벤토리 데이터 설정
        SetInventory();
        // 인벤토리 정렬 실행
        SortInventory();
    }

    // 장착된 아이템들을 설정하는 메서드
    private void SetEquippedItems()
    {
        // 사용자 인벤토리 데이터 가져오기
        var userInventoryData = UserDataManager.Instance.GetUserData<UserInventoryData>();
        // 사용자 인벤토리 데이터가 없는 경우 에러 처리
        if(userInventoryData == null)
        {
            // 에러 로그 출력
            Logger.LogError("UserInventoryData does not exist.");
            // 메서드 종료
            return;
        }

        // 장착된 무기 데이터가 있는 경우
        if(userInventoryData.EquippedWeaponData != null)
        {
            // 무기 슬롯에 아이템 설정
            WeaponSlot.SetItem(userInventoryData.EquippedWeaponData);
        }
        else
        {
            // 무기 슬롯 초기화
            WeaponSlot.ClearItem();
        }

        // 장착된 방패 데이터가 있는 경우
        if (userInventoryData.EquippedShieldData != null)
        {
            // 방패 슬롯에 아이템 설정
            ShieldSlot.SetItem(userInventoryData.EquippedShieldData);
        }
        else
        {
            // 방패 슬롯 초기화
            ShieldSlot.ClearItem();
        }

        // 장착된 가슴 방어구 데이터가 있는 경우
        if (userInventoryData.EquippedChestArmorData != null)
        {
            // 가슴 방어구 슬롯에 아이템 설정
            ChestArmorSlot.SetItem(userInventoryData.EquippedChestArmorData);
        }
        else
        {
            // 가슴 방어구 슬롯 초기화
            ChestArmorSlot.ClearItem();
        }

        // 장착된 부츠 데이터가 있는 경우
        if (userInventoryData.EquippedBootsData != null)
        {
            // 부츠 슬롯에 아이템 설정
            BootsSlot.SetItem(userInventoryData.EquippedBootsData);
        }
        else
        {
            // 부츠 슬롯 초기화
            BootsSlot.ClearItem();
        }

        // 장착된 장갑 데이터가 있는 경우
        if (userInventoryData.EquippedGlovesData != null)
        {
            // 장갑 슬롯에 아이템 설정
            GlovesSlot.SetItem(userInventoryData.EquippedGlovesData);
        }
        else
        {
            // 장갑 슬롯 초기화
            GlovesSlot.ClearItem();
        }

        // 장착된 액세서리 데이터가 있는 경우
        if (userInventoryData.EquippedAccessoryData != null)
        {
            // 액세서리 슬롯에 아이템 설정
            AccessorySlot.SetItem(userInventoryData.EquippedAccessoryData);
        }
        else
        {
            // 액세서리 슬롯 초기화
            AccessorySlot.ClearItem();
        }
    }

    // 인벤토리 데이터를 설정하는 메서드
    private void SetInventory()
    {
        // 인벤토리 스크롤 리스트 초기화
        InventoryScrollList.Clear();

        // 사용자 인벤토리 데이터 가져오기
        var userInventoryData = UserDataManager.Instance.GetUserData<UserInventoryData>();
        // 사용자 인벤토리 데이터가 존재하는 경우
        if(userInventoryData != null)
        {
            // 인벤토리 아이템 리스트를 순회
            foreach (var itemData in userInventoryData.InventoryItemDataList)
            {
                // 새로운 인벤토리 아이템 슬롯 데이터 생성
                var itemSlotData = new InventoryItemSlotData();
                // 아이템 시리얼 번호 설정
                itemSlotData.SerialNumber = itemData.SerialNumber;
                // 아이템 ID 설정
                itemSlotData.ItemId = itemData.ItemId;
                // 스크롤 리스트에 아이템 데이터 삽입
                InventoryScrollList.InsertData(itemSlotData);
            }
        }
    }

    // 인벤토리를 정렬하는 메서드
    private void SortInventory()
    {
        // 현재 정렬 타입에 따른 분기 처리
        switch (m_InventorySortType)
        {
            // 아이템 등급 기준 정렬인 경우
            case InventorySortType.ItemGrade:
                // 정렬 버튼 텍스트를 "GRADE"로 설정
                SortBtnTxt.text = "GRADE";

                // 데이터 리스트를 아이템 등급 기준으로 정렬
                InventoryScrollList.SortDataList((a, b) =>
                {
                    // 데이터 A를 InventoryItemSlotData로 캐스팅
                    var itemA = a.data as InventoryItemSlotData;
                    // 데이터 B를 InventoryItemSlotData로 캐스팅
                    var itemB = b.data as InventoryItemSlotData;

                    // 아이템 등급으로 정렬 (높은 등급이 앞으로)
                    int compareResult = ((itemB.ItemId / 1000) % 10).CompareTo((itemA.ItemId / 1000) % 10);

                    // 같은 아이템 등급인 경우, 아이템 타입으로 정렬
                    if (compareResult == 0)
                    {
                        // 아이템 A의 ID를 문자열로 변환
                        var itemAIdStr = itemA.ItemId.ToString();
                        // 아이템 A의 비교값 생성 (11001 -> 1001)
                        var itemAComp = itemAIdStr.Substring(0, 1) + itemAIdStr.Substring(2,3);

                        // 아이템 B의 ID를 문자열로 변환
                        var itemBIdStr = itemB.ItemId.ToString();
                        // 아이템 B의 비교값 생성 (11001 -> 1001)
                        var itemBComp = itemBIdStr.Substring(0, 1) + itemBIdStr.Substring(2, 3);

                        // 아이템 타입으로 비교
                        compareResult = itemAComp.CompareTo(itemBComp);
                    }

                    // 비교 결과 반환
                    return compareResult;
                });
                break;
            // 아이템 타입 기준 정렬인 경우
            case InventorySortType.ItemType:
                // 정렬 버튼 텍스트를 "TYPE"으로 설정
                SortBtnTxt.text = "TYPE";
                // 데이터 리스트를 아이템 타입 기준으로 정렬
                InventoryScrollList.SortDataList((a, b) =>
                {
                    // 데이터 A를 InventoryItemSlotData로 캐스팅
                    var itemA = a.data as InventoryItemSlotData;
                    // 데이터 B를 InventoryItemSlotData로 캐스팅
                    var itemB = b.data as InventoryItemSlotData;

                    // 아이템 A의 ID를 문자열로 변환
                    var itemAIdStr = itemA.ItemId.ToString();
                    // 아이템 A의 비교값 생성 (11001 -> 1001)
                    var itemAComp = itemAIdStr.Substring(0, 1) + itemAIdStr.Substring(2, 3);

                    // 아이템 B의 ID를 문자열로 변환
                    var itemBIdStr = itemB.ItemId.ToString();
                    // 아이템 B의 비교값 생성 (11001 -> 1001)
                    var itemBComp = itemBIdStr.Substring(0, 1) + itemBIdStr.Substring(2, 3);

                    // 아이템 타입으로 비교
                    int compareResult = itemAComp.CompareTo(itemBComp);

                    // 같은 아이템 타입인 경우, 아이템 등급으로 정렬
                    if(compareResult == 0)
                    {
                        // 아이템 등급으로 정렬 (높은 등급이 앞으로)
                        compareResult = ((itemB.ItemId / 1000) % 10).CompareTo((itemA.ItemId / 1000) % 10);
                    }

                    // 비교 결과 반환
                    return compareResult;
                });
                break;
            // 기본 케이스 (처리 없음)
            default:
                break;
        }
    }

    // 정렬 버튼 클릭 시 호출되는 메서드
    public void OnClickSortBtn()
    {
        // 현재 정렬 타입에 따른 분기 처리
        switch (m_InventorySortType)
        {
            // 현재 아이템 등급 정렬인 경우
            case InventorySortType.ItemGrade:
                // 아이템 타입 정렬로 변경
                m_InventorySortType = InventorySortType.ItemType;
                break;
            // 현재 아이템 타입 정렬인 경우
            case InventorySortType.ItemType:
                // 아이템 등급 정렬로 변경
                m_InventorySortType= InventorySortType.ItemGrade;
                break;
            // 기본 케이스 (처리 없음)
            default:
                break;
        }

        // 변경된 정렬 타입으로 인벤토리 재정렬
        SortInventory();
    }
}