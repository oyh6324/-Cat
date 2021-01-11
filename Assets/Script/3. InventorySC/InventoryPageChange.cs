using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.SymbolStore;
using UnityEngine;
using UnityEngine.UI;

public class InventoryPageChange : MonoBehaviour
{
    //아이템 디테일에서 사용
    public static bool[] isInvenBtClick; //0:헬멧 1:상의 2:하의
    //카테고리 버튼
    public Button[] categoryBt; //0:헬멧 1:상의 2:하의
    //카테고리 뒷배경
    public Sprite[] categorySprite;
    //아이템 이미지 배경
    public Image[] items; //1~9
    //아이템 이미지
    public Image[] itemImg; //1~9
    //스프라이트
    public Sprite[] itemSprite;
    //잠금 이미지
    public Image[] itemsLock; //1~9
    //아이템 이름 텍스트
    public Text[] itemNameTx; //1~9
    //알람 이미지
    public Image[] newAlarmImg;
    public Image detailImg;
    public Button detailBt;
    public Button rightBt;
    public Button leftBt;
    public Text statExTx;
    public Image categoryBackImg;

    public Animator catAnim; //고양이 애니메이터

    List<ItemData> itemDataList; //필요한 아이템 정보 저장 리스트

    private bool isPageOne; //페이지 넘겼는지 안넘겼는지
    private int allItemStr, allItemDef,allItemAgi; //스탯 창에 표시할 아이템 능력
    private decimal allItemCrip;
    private int listIndex = 0; //itemDataList의 크기
    private bool isSet; //세트 완료인지 아닌지
    void Awake()
    {
        isInvenBtClick = new bool[3];
        itemDataList = new List<ItemData>();
    }
    void OnEnable()
    {
        detailImg.gameObject.SetActive(false);
        isPageOne = true;
        allItemStr = DataManager.characterDatasList[0].itemstr;
        allItemDef = DataManager.characterDatasList[0].itemdef;
        allItemAgi = DataManager.characterDatasList[0].itemagi;
        allItemCrip = DataManager.characterDatasList[0].itemcrip;

        StatExReset();
        CategoryChange(0);
    }
    void Update()
    {
        for(int i=0; i<3; i++)
        {
            if (isInvenBtClick[i])
                categoryBt[i].transform.localPosition = Vector2.MoveTowards(new Vector2(categoryBt[i].transform.localPosition.x, categoryBt[i].transform.localPosition.y),
                                                                            new Vector2(categoryBt[i].transform.localPosition.x, 380), 15f);
            else
                categoryBt[i].transform.localPosition = Vector2.MoveTowards(new Vector2(categoryBt[i].transform.localPosition.x, categoryBt[i].transform.localPosition.y),
                                                                            new Vector2(categoryBt[i].transform.localPosition.x, 340), 15f);
        }
        if (PlayerPrefs.HasKey("아이템정보변경"))
        {
            itemDataList.Clear();
            allItemStr = DataManager.characterDatasList[0].itemstr;
            allItemDef = DataManager.characterDatasList[0].itemdef;
            allItemAgi = DataManager.characterDatasList[0].itemagi;
            allItemCrip = DataManager.characterDatasList[0].itemcrip;
            PageChange();
            StatExReset();
            PlayerPrefs.DeleteKey("아이템정보변경");
        }
    }
    public void CatBtClick()
    {
        StartCoroutine(CatClickAniWaiting());
    }
    IEnumerator CatClickAniWaiting()
    {
        catAnim.SetBool("isCatClick", true);
        yield return new WaitForSeconds(1.5f);
        catAnim.SetBool("isCatClick", false);
    }
    public void item0BtClick()
    {
        InstallItem(0);
    }
    public void item1BtClick()
    {
        InstallItem(1);
    }
    public void item2BtClick()
    {
        InstallItem(2);
    }
    public void item3BtClick()
    {
        InstallItem(3);
    }
    public void item4BtClick()
    {
        InstallItem(4);
    }
    public void item5BtClick()
    {
        InstallItem(5);
    }
    public void item6BtClick()
    {
        InstallItem(6);
    }
    public void item7BtClick()
    {
        InstallItem(7);
    }
    public void item8BtClick()
    {
        InstallItem(8);
    }
    public void rightBtClick()
    {
        isPageOne = false;
        PageChange();
        DetailBtOn();
    }
    public void leftBtClick()
    {
        isPageOne = true;
        PageChange();
        DetailBtOn();
    }
    public void helmetBtClick()
    {
        CategoryChange(0);
    }
    public void topBtClick()
    {
        CategoryChange(1);
    }
    public void bottomsBtClick()
    {
        CategoryChange(2);
    }
    public void detailBtClick()
    {
        detailImg.gameObject.SetActive(true);
    }
    void InstallItem(int itemnumber)
    {
        if (isPageOne == false) //2페이지 일 때
            itemnumber += 9;
        if (isInvenBtClick[0]) //헬멧 열었을 때
        {
            if (DataManager.characterDatasList[0].helmet == "") //헬멧 장착이 없을 때
            {
                DataManager.characterDatasList[0].helmet = itemDataList[itemnumber].name;
                allItemStr += itemDataList[itemnumber].str;
                allItemDef += itemDataList[itemnumber].def;
                allItemAgi += itemDataList[itemnumber].agi;
            }
            else if (DataManager.characterDatasList[0].helmet != "") //장착된 상태일 때
            {
                if (DataManager.characterDatasList[0].helmet.Equals(itemDataList[itemnumber].name)) //같은 헬멧 장착 해제
                {
                    DataManager.characterDatasList[0].helmet = "";
                    allItemStr -= itemDataList[itemnumber].str;
                    allItemDef -= itemDataList[itemnumber].def;
                    allItemAgi -= itemDataList[itemnumber].agi;
                }
                else //다른 헬멧을 장착할 때
                {
                    int preItemNumber = 0;
                    for (int i = 0; i < listIndex; i++)
                    {
                        if (DataManager.characterDatasList[0].helmet.Equals(itemDataList[i].name))
                            preItemNumber = i;
                    }
                    allItemStr -= itemDataList[preItemNumber].str; //원래 장착한 헬멧 스탯 제거
                    allItemDef -= itemDataList[preItemNumber].def;
                    allItemAgi -= itemDataList[preItemNumber].agi;

                    DataManager.characterDatasList[0].helmet = itemDataList[itemnumber].name;
                    allItemStr += itemDataList[itemnumber].str;
                    allItemDef += itemDataList[itemnumber].def;
                    allItemAgi += itemDataList[itemnumber].agi;
                }
            }
        }
        else if (isInvenBtClick[1]) //상의 열었을 때
        {
            if (DataManager.characterDatasList[0].top == "") // 장착이 없을 때
            {
                DataManager.characterDatasList[0].top = itemDataList[itemnumber].name;
                allItemStr += itemDataList[itemnumber].str;
                allItemDef += itemDataList[itemnumber].def;
                allItemAgi += itemDataList[itemnumber].agi;
            }
            else if (DataManager.characterDatasList[0].top != "") //장착된 상태일 때
            {
                if (DataManager.characterDatasList[0].top.Equals(itemDataList[itemnumber].name)) //같은 상의 장착 해제
                {
                    DataManager.characterDatasList[0].top = "";
                    allItemStr -= itemDataList[itemnumber].str;
                    allItemDef -= itemDataList[itemnumber].def;
                    allItemAgi -= itemDataList[itemnumber].agi;
                }
                else //다른 상의를 장착할 때
                {
                    int preItemNumber = 0;
                    for (int i = 0; i < listIndex; i++)
                    {
                        if (DataManager.characterDatasList[0].top.Equals(itemDataList[i].name))
                            preItemNumber = i;
                    }
                    allItemStr -= itemDataList[preItemNumber].str; //원래 장착한 상의 스탯 제거
                    allItemDef -= itemDataList[preItemNumber].def;
                    allItemAgi -= itemDataList[preItemNumber].agi;

                    DataManager.characterDatasList[0].top = itemDataList[itemnumber].name;
                    allItemStr += itemDataList[itemnumber].str;
                    allItemDef += itemDataList[itemnumber].def;
                    allItemAgi += itemDataList[itemnumber].agi;
                }

            }
            if (itemDataList[itemnumber].name == "우주복" || itemDataList[itemnumber].name == "천사 옷" || itemDataList[itemnumber].name == "악마 옷") //한벌옷을 입었을때
            {
                if (DataManager.characterDatasList[0].bottoms != "") //하의가 있다면
                {
                    for (int i = 0; i < DataManager.allClothesItemList.Count; i++)
                    {
                        if (DataManager.characterDatasList[0].bottoms.Equals(DataManager.allClothesItemList[i].name)) //하의 제거
                        {
                            allItemStr -= DataManager.allClothesItemList[i].str;
                            allItemDef -= DataManager.allClothesItemList[i].def;
                            allItemAgi -= DataManager.allClothesItemList[i].agi;
                        }
                    }
                    DataManager.characterDatasList[0].bottoms = "";
                }
            }
        }
        else if (isInvenBtClick[2]) //하의 열렸을 때
        {
            if (DataManager.characterDatasList[0].bottoms == "") // 장착이 없을 때
            {
                DataManager.characterDatasList[0].bottoms = itemDataList[itemnumber].name;
                allItemStr += itemDataList[itemnumber].str;
                allItemDef += itemDataList[itemnumber].def;
                allItemAgi += itemDataList[itemnumber].agi;
            }
            else if (DataManager.characterDatasList[0].bottoms != "") //장착된 상태일 때
            {
                if (DataManager.characterDatasList[0].bottoms.Equals(itemDataList[itemnumber].name)) //같은 하의 장착 해제
                {
                    DataManager.characterDatasList[0].bottoms = "";
                    allItemStr -= itemDataList[itemnumber].str;
                    allItemDef -= itemDataList[itemnumber].def;
                    allItemAgi -= itemDataList[itemnumber].agi;
                }
                else //다른 하의를 장착할 때
                {
                    int preItemNumber = 0;
                    for (int i = 0; i < listIndex; i++)
                    {
                        if (DataManager.characterDatasList[0].bottoms.Equals(itemDataList[i].name))
                            preItemNumber = i;
                    }
                    allItemStr -= itemDataList[preItemNumber].str; //원래 장착한 하의 스탯 제거
                    allItemDef -= itemDataList[preItemNumber].def;
                    allItemAgi -= itemDataList[preItemNumber].agi;

                    DataManager.characterDatasList[0].bottoms = itemDataList[itemnumber].name;
                    allItemStr += itemDataList[itemnumber].str;
                    allItemDef += itemDataList[itemnumber].def;
                    allItemAgi += itemDataList[itemnumber].agi;
                }
            }
            if (DataManager.characterDatasList[0].top == "우주복" || DataManager.characterDatasList[0].top == "천사 옷" || DataManager.characterDatasList[0].top == "악마 옷") //한벌옷 입었을때
            {
                int uniNumber = 0, angelNumber = 0, devilNumber = 0;
                for (int i = 0; i < DataManager.allClothesItemList.Count; i++)
                {
                    if (DataManager.allClothesItemList[i].name == "우주복")
                        uniNumber = i;
                    if (DataManager.allClothesItemList[i].name == "천사 옷")
                        angelNumber = i;
                    if (DataManager.allClothesItemList[i].name == "악마 옷")
                        devilNumber = i;
                }
                if (DataManager.characterDatasList[0].top == "우주복")
                {
                    allItemStr -= DataManager.allClothesItemList[uniNumber].str;
                    allItemDef -= DataManager.allClothesItemList[uniNumber].def;
                    allItemAgi -= DataManager.allClothesItemList[uniNumber].agi;
                }
                if (DataManager.characterDatasList[0].top == "천사 옷")
                {
                    allItemStr -= DataManager.allClothesItemList[angelNumber].str;
                    allItemDef -= DataManager.allClothesItemList[angelNumber].def;
                    allItemAgi -= DataManager.allClothesItemList[angelNumber].agi;
                }
                if (DataManager.characterDatasList[0].top == "악마 옷")
                {
                    allItemStr -= DataManager.allClothesItemList[devilNumber].str;
                    allItemDef -= DataManager.allClothesItemList[devilNumber].def;
                    allItemAgi -= DataManager.allClothesItemList[devilNumber].agi;
                }
                DataManager.characterDatasList[0].top = "";
            }
        }
        for(int i=0; i<DataManager.allClothesItemList.Count; i++) //새로운 아이템 알람 삭제
        {
            if (DataManager.allClothesItemList[i].name == itemDataList[itemnumber].name)
            {
                DataManager.allClothesItemList[i].isnew = false;
                itemDataList[itemnumber].isnew = false;
                if (isPageOne)
                    newAlarmImg[itemnumber].gameObject.SetActive(false);
                else
                    newAlarmImg[itemnumber - 9].gameObject.SetActive(false);
            }
        }
        SetCheck(itemnumber);
        StatExReset();
        DetailBtOn();
        SaveStatData();
    }
    void SetCheck(int itemnumber)
    {
        if (itemDataList[itemnumber].setname != "") //세트 아이템 착용 시
        {
            int setCount = itemDataList[itemnumber].setnumber;
            string installHelmetName = DataManager.characterDatasList[0].helmet;
            string installTopName = DataManager.characterDatasList[0].top;
            string installBottomsName = DataManager.characterDatasList[0].bottoms;

            for (int i = 0; i < DataManager.allClothesItemList.Count; i++)
            {
                if (DataManager.allClothesItemList[i].name.Equals(installHelmetName))
                {
                    if (DataManager.allClothesItemList[i].setname.Equals(itemDataList[itemnumber].setname))
                        setCount--;
                }
                if (DataManager.allClothesItemList[i].name.Equals(installTopName))
                {
                    if (DataManager.allClothesItemList[i].setname.Equals(itemDataList[itemnumber].setname))
                        setCount--;
                }
                if (DataManager.allClothesItemList[i].name.Equals(installBottomsName))
                {
                    if (DataManager.allClothesItemList[i].setname.Equals(itemDataList[itemnumber].setname))
                        setCount--;
                }
            }
            if (itemDataList[itemnumber].setname == "우주" && DataManager.characterDatasList[0].weapon == "레이저건")
                setCount--;
            if (itemDataList[itemnumber].setname == "인어" && DataManager.characterDatasList[0].weapon == "버블건")
                setCount--;
            if (itemDataList[itemnumber].setname == "천사"&& DataManager.characterDatasList[0].weapon == "천사링")
                setCount--;
            if (itemDataList[itemnumber].setname == "악마"&&DataManager.characterDatasList[0].weapon == "악마삼지창")
                setCount--;
            if (setCount == 0)
                isSet = true;
            else
                isSet = false;
        }
        if (isSet)
        {
            DataManager.characterDatasList[0].setstr = itemDataList[itemnumber].setstr;
            DataManager.characterDatasList[0].setdef = itemDataList[itemnumber].setdef;
            DataManager.characterDatasList[0].setagi = itemDataList[itemnumber].setagi;
            DataManager.characterDatasList[0].setcrip = itemDataList[itemnumber].setcrip;
            DataManager.characterDatasList[0].setname = itemDataList[itemnumber].setname;
        }
        else
        {
            DataManager.characterDatasList[0].setstr = 0;
            DataManager.characterDatasList[0].setdef = 0;
            DataManager.characterDatasList[0].setagi = 0;
            DataManager.characterDatasList[0].setcrip = 0;
            DataManager.characterDatasList[0].setname = "";
        }
    }
    void StatExReset()
    {
        statExTx.text = "레벨: " + DataManager.characterDatasList[0].level + "\n이름: " + DataManager.characterDatasList[0].name + "\nHP: " + DataManager.characterDatasList[0].hp +
            "\nSTR: " + DataManager.characterDatasList[0].str;
        if (allItemStr != 0)
            statExTx.text += " (+" + allItemStr + ")";
        statExTx.text += "\nDEF: " + DataManager.characterDatasList[0].def;
        if (allItemDef != 0)
            statExTx.text += " (+" + allItemDef + ")";
        statExTx.text += "\nAGI: " + DataManager.characterDatasList[0].agi;
        if (allItemAgi != 0)
            statExTx.text += " (+" + allItemAgi + ")";
        statExTx.text += "\n공격 속도: " + DataManager.characterDatasList[0].itemspeed + "\n크리티컬 확률: " + DataManager.characterDatasList[0].crip;
        if (allItemCrip != 0)
            statExTx.text += " (+" + allItemCrip + "%)";
        if (DataManager.characterDatasList[0].setname != "")
            statExTx.text += "\n" + DataManager.characterDatasList[0].setname + " 세트 적용!";
        if (DataManager.characterDatasList[0].setstr != 0)
            statExTx.text += "\nSTR +" + DataManager.characterDatasList[0].setstr;
        if (DataManager.characterDatasList[0].setdef != 0)
            statExTx.text += "\nDEF +" + DataManager.characterDatasList[0].setdef;
        if (DataManager.characterDatasList[0].setagi != 0)
            statExTx.text += "\nAGI +" + DataManager.characterDatasList[0].setagi;
        if (DataManager.characterDatasList[0].setcrip != 0)
            statExTx.text += "\n크리티컬 확률 +" + DataManager.characterDatasList[0].setcrip;
    }
    void PageChange()
    {
        if (isPageOne)
        {
            if (isInvenBtClick[0])
            {
                listIndex=InputItemData("헬멧");
            }
            if (isInvenBtClick[1])
            {
                listIndex=InputItemData("상의"); 
            }
            if (isInvenBtClick[2])
            {
                listIndex=InputItemData("하의");
            }
            for(int i=0; i<9; i++)
            {
                items[i].gameObject.SetActive(true);
                itemsLock[i].gameObject.SetActive(true);

                if (itemDataList[i].count !=0)
                {
                    itemsLock[i].gameObject.SetActive(false);
                    itemNameTx[i].text = itemDataList[i].name;
                    for (int j = 0; j < DataManager.allClothesItemList.Count; j++) //스프라이트 바꿔주기
                    {
                        if (DataManager.allClothesItemList[j].name.Equals(itemDataList[i].name))
                            itemImg[i].sprite = itemSprite[j];
                    }
                }
                else
                {
                    itemsLock[i].gameObject.SetActive(true);
                    itemNameTx[i].text = "";
                }
                if (itemDataList[i].isnew) //새로운 아이템 알람
                    newAlarmImg[i].gameObject.SetActive(true);
                else
                    newAlarmImg[i].gameObject.SetActive(false);
            }
            rightBt.gameObject.SetActive(true);
            leftBt.gameObject.SetActive(false);
        }
        else
        {
            if (isInvenBtClick[0])
            {
                listIndex = InputItemData("헬멧");
            }
            if (isInvenBtClick[1])
            {
                listIndex = InputItemData("상의");
            }
            if (isInvenBtClick[2])
            {
                listIndex = InputItemData("하의");
            }
            for (int i = 9; i < listIndex; i++)
            {
                items[i-9].gameObject.SetActive(true);
                itemsLock[i-9].gameObject.SetActive(true);

                if (itemDataList[i].count != 0)
                {
                    itemsLock[i - 9].gameObject.SetActive(false);
                    itemNameTx[i - 9].text = itemDataList[i].name;
                }
                else
                {
                    itemsLock[i - 9].gameObject.SetActive(true);
                    itemNameTx[i - 9].text = "";
                }
                if (itemDataList[i].isnew) //새로운 아이템 알람
                    newAlarmImg[i-9].gameObject.SetActive(true);
                else
                    newAlarmImg[i-9].gameObject.SetActive(false);
            }
            for(int i=listIndex-9; i<9; i++)
            {
                items[i].gameObject.SetActive(false);
                itemsLock[i].gameObject.SetActive(false);
            }
            rightBt.gameObject.SetActive(false);
            leftBt.gameObject.SetActive(true);
        }
    }
    void CategoryChange(int btnumber) //카테고리 바꿀 때
    {
        isPageOne = true; //바뀔 때마다 첫페이지 셋팅
        for(int i=0; i<3; i++)
        {
            if (i == btnumber)
                isInvenBtClick[i] = true;
            else
                isInvenBtClick[i] = false;
        }
        categoryBackImg.sprite = categorySprite[btnumber];
        PageChange();
        DetailBtOn();
    }
    int InputItemData(string type) //해당 카테고리 아이템만 저장하기
    {
        itemDataList.Clear();
        int count = 0;
        for(int i=0; i<DataManager.allClothesItemList.Count; i++)
        {
            if (DataManager.allClothesItemList[i].type.Equals(type))
            {
                itemDataList.Add(new ItemData(DataManager.allClothesItemList[i].name, DataManager.allClothesItemList[i].count,
                    DataManager.allClothesItemList[i].str, DataManager.allClothesItemList[i].def, DataManager.allClothesItemList[i].agi,
                    DataManager.allClothesItemList[i].setname, DataManager.allClothesItemList[i].setstr, DataManager.allClothesItemList[i].setdef,
                    DataManager.allClothesItemList[i].setagi, DataManager.allClothesItemList[i].setnumber, DataManager.allClothesItemList[i].setcrip,
                    DataManager.allClothesItemList[i].isnew));
                count++;
            }
        }
        return count;
    }
    void DetailBtOn()
    {
        int count = 0;
        if (isPageOne)
        {
            if (isInvenBtClick[0])
            {
                for (int i = 0; i < 9; i++)
                {
                    if (itemDataList[i].name.Equals(DataManager.characterDatasList[0].helmet))
                        count++;
                }
            }
            if (isInvenBtClick[1])
            {
                for (int i = 0; i < 9; i++)
                {
                    if (itemDataList[i].name.Equals(DataManager.characterDatasList[0].top))
                        count++;
                }
            }
            if (isInvenBtClick[2])
            {
                for (int i = 0; i < 9; i++)
                {
                    if (itemDataList[i].name.Equals(DataManager.characterDatasList[0].bottoms))
                        count++;
                }
            }
        }
        else
        {
            if (isInvenBtClick[0])
            {
                for (int i = 9; i < listIndex; i++)
                {
                    if (itemDataList[i].name.Equals(DataManager.characterDatasList[0].helmet))
                        count++;
                }
            }
            if (isInvenBtClick[1])
            {
                for (int i = 9; i < listIndex; i++)
                {
                    if (itemDataList[i].name.Equals(DataManager.characterDatasList[0].top))
                        count++;
                }
            }
            if (isInvenBtClick[2])
            {
                for (int i = 9; i < listIndex; i++)
                {
                    if (itemDataList[i].name.Equals(DataManager.characterDatasList[0].bottoms))
                        count++;
                }
            }
        }
        if (count == 1)
            detailBt.gameObject.SetActive(true);
        else
            detailBt.gameObject.SetActive(false);
    }
    void SaveStatData()
    {
        DataManager.characterDatasList[0].itemstr = allItemStr;
        DataManager.characterDatasList[0].itemdef = allItemDef;
        DataManager.characterDatasList[0].itemagi = allItemAgi;
        DataManager.characterDatasList[0].itemcrip = allItemCrip;

        DataManager.characterDatasList[0].allstr = DataManager.characterDatasList[0].itemstr + DataManager.characterDatasList[0].setstr + DataManager.characterDatasList[0].str;
        DataManager.characterDatasList[0].alldef = DataManager.characterDatasList[0].itemdef + DataManager.characterDatasList[0].setdef + DataManager.characterDatasList[0].def;
        DataManager.characterDatasList[0].allagi = DataManager.characterDatasList[0].itemagi + DataManager.characterDatasList[0].setagi + DataManager.characterDatasList[0].agi;
        DataManager.characterDatasList[0].allcrip = DataManager.characterDatasList[0].itemcrip + DataManager.characterDatasList[0].setcrip + DataManager.characterDatasList[0].crip;
    }
    public class ItemData
    {
        public ItemData(string _name, int _count,int _str, int _def, int _agi,string _setname, int _setstr, int _setdef, int _setagi,int _setnumber,decimal _setcrip,bool _isnew) 
        { 
            name = _name; count = _count; str = _str; def = _def; agi = _agi;
            setname = _setname; setstr = _setstr; setdef = _setdef; setagi = _setagi; setnumber = _setnumber; setcrip = _setcrip;
            isnew = _isnew;
        }
        public string name,setname;
        public int count,str,def,agi,setstr,setdef,setagi,setnumber;
        public decimal setcrip;
        public bool isnew;
    }
}
