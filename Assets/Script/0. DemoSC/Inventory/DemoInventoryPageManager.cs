using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DemoInventoryPageManager : MonoBehaviour
{
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
    //오디오
    public AudioSource bgmAS;
    public AudioSource soundEffectAS;
    public AudioClip buttonClickClip;
    public AudioClip inventoryBgmClip;

    List<ItemData> itemDataList; //필요한 아이템 정보 저장 리스트

    private int allItemStr, allItemDef, allItemAgi; //스탯 창에 표시할 아이템 능력
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
        bgmAS.clip = inventoryBgmClip;
        bgmAS.Play();
        detailImg.gameObject.SetActive(false);

        allItemStr = DemoDataManager.characterDatasList[0].itemstr;
        allItemDef = DemoDataManager.characterDatasList[0].itemdef;
        allItemAgi = DemoDataManager.characterDatasList[0].itemagi;
        allItemCrip = DemoDataManager.characterDatasList[0].itemcrip;

        StatExReset();
        CategoryChange(0);
    }
    void Update()
    {
        for (int i = 0; i < 3; i++)
        {
            if (isInvenBtClick[i])
                categoryBt[i].transform.localPosition = Vector2.MoveTowards(new Vector2(categoryBt[i].transform.localPosition.x, categoryBt[i].transform.localPosition.y),
                                                                            new Vector2(categoryBt[i].transform.localPosition.x, 340), 15f);
            else
                categoryBt[i].transform.localPosition = Vector2.MoveTowards(new Vector2(categoryBt[i].transform.localPosition.x, categoryBt[i].transform.localPosition.y),
                                                                            new Vector2(categoryBt[i].transform.localPosition.x, 300), 15f);
        }
        if (PlayerPrefs.HasKey("아이템정보변경"))
        {
            itemDataList.Clear();
            allItemStr = DemoDataManager.characterDatasList[0].itemstr;
            allItemDef = DemoDataManager.characterDatasList[0].itemdef;
            allItemAgi = DemoDataManager.characterDatasList[0].itemagi;
            allItemCrip = DemoDataManager.characterDatasList[0].itemcrip;
            PageChange();
            StatExReset();
            PlayerPrefs.DeleteKey("아이템정보변경");
        }
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
    public void helmetBtClick()
    {
        soundEffectAS.clip = buttonClickClip;
        soundEffectAS.Play();
        CategoryChange(0);
    }
    public void topBtClick()
    {
        soundEffectAS.clip = buttonClickClip;
        soundEffectAS.Play();
        CategoryChange(1);
    }
    public void bottomsBtClick()
    {
        soundEffectAS.clip = buttonClickClip;
        soundEffectAS.Play();
        CategoryChange(2);
    }
    public void detailBtClick()
    {
        soundEffectAS.clip = buttonClickClip;
        soundEffectAS.Play();
        detailImg.gameObject.SetActive(true);
    }
    void InstallItem(int itemnumber)
    {
        soundEffectAS.clip = buttonClickClip;
        soundEffectAS.Play();
        if (isInvenBtClick[0]) //헬멧 열었을 때
        {
            if (DemoDataManager.characterDatasList[0].helmet == "") //헬멧 장착이 없을 때
            {
                DemoDataManager.characterDatasList[0].helmet = itemDataList[itemnumber].name;
                allItemStr += itemDataList[itemnumber].str;
                allItemDef += itemDataList[itemnumber].def;
                allItemAgi += itemDataList[itemnumber].agi;
            }
            else if (DemoDataManager.characterDatasList[0].helmet != "") //장착된 상태일 때
            {
                if (DemoDataManager.characterDatasList[0].helmet.Equals(itemDataList[itemnumber].name)) //같은 헬멧 장착 해제
                {
                    DemoDataManager.characterDatasList[0].helmet = "";
                    allItemStr -= itemDataList[itemnumber].str;
                    allItemDef -= itemDataList[itemnumber].def;
                    allItemAgi -= itemDataList[itemnumber].agi;
                }
                else //다른 헬멧을 장착할 때
                {
                    int preItemNumber = 0;
                    for (int i = 0; i < listIndex; i++)
                    {
                        if (DemoDataManager.characterDatasList[0].helmet.Equals(itemDataList[i].name))
                            preItemNumber = i;
                    }
                    allItemStr -= itemDataList[preItemNumber].str; //원래 장착한 헬멧 스탯 제거
                    allItemDef -= itemDataList[preItemNumber].def;
                    allItemAgi -= itemDataList[preItemNumber].agi;

                    DemoDataManager.characterDatasList[0].helmet = itemDataList[itemnumber].name;
                    allItemStr += itemDataList[itemnumber].str;
                    allItemDef += itemDataList[itemnumber].def;
                    allItemAgi += itemDataList[itemnumber].agi;
                }
            }
        }
        else if (isInvenBtClick[1]) //상의 열었을 때
        {
            if (DemoDataManager.characterDatasList[0].top == "") // 장착이 없을 때
            {
                DemoDataManager.characterDatasList[0].top = itemDataList[itemnumber].name;
                allItemStr += itemDataList[itemnumber].str;
                allItemDef += itemDataList[itemnumber].def;
                allItemAgi += itemDataList[itemnumber].agi;
            }
            else if (DemoDataManager.characterDatasList[0].top != "") //장착된 상태일 때
            {
                if (DemoDataManager.characterDatasList[0].top.Equals(itemDataList[itemnumber].name)) //같은 상의 장착 해제
                {
                    DemoDataManager.characterDatasList[0].top = "";
                    allItemStr -= itemDataList[itemnumber].str;
                    allItemDef -= itemDataList[itemnumber].def;
                    allItemAgi -= itemDataList[itemnumber].agi;
                }
                else //다른 상의를 장착할 때
                {
                    int preItemNumber = 0;
                    for (int i = 0; i < listIndex; i++)
                    {
                        if (DemoDataManager.characterDatasList[0].top.Equals(itemDataList[i].name))
                            preItemNumber = i;
                    }
                    allItemStr -= itemDataList[preItemNumber].str; //원래 장착한 상의 스탯 제거
                    allItemDef -= itemDataList[preItemNumber].def;
                    allItemAgi -= itemDataList[preItemNumber].agi;

                    DemoDataManager.characterDatasList[0].top = itemDataList[itemnumber].name;
                    allItemStr += itemDataList[itemnumber].str;
                    allItemDef += itemDataList[itemnumber].def;
                    allItemAgi += itemDataList[itemnumber].agi;
                }

            }
            if (itemDataList[itemnumber].name == "우주복") //한벌옷을 입었을때
            {
                if (DemoDataManager.characterDatasList[0].bottoms != "") //하의가 있다면
                {
                    for (int i = 0; i < DemoDataManager.allClothesItemList.Count; i++)
                    {
                        if (DemoDataManager.characterDatasList[0].bottoms.Equals(DemoDataManager.allClothesItemList[i].name)) //하의 제거
                        {
                            allItemStr -= DemoDataManager.allClothesItemList[i].str;
                            allItemDef -= DemoDataManager.allClothesItemList[i].def;
                            allItemAgi -= DemoDataManager.allClothesItemList[i].agi;
                        }
                    }
                    DemoDataManager.characterDatasList[0].bottoms = "";
                }
            }
        }
        else if (isInvenBtClick[2]) //하의 열렸을 때
        {
            if (DemoDataManager.characterDatasList[0].bottoms == "") // 장착이 없을 때
            {
                DemoDataManager.characterDatasList[0].bottoms = itemDataList[itemnumber].name;
                allItemStr += itemDataList[itemnumber].str;
                allItemDef += itemDataList[itemnumber].def;
                allItemAgi += itemDataList[itemnumber].agi;
            }
            else if (DemoDataManager.characterDatasList[0].bottoms != "") //장착된 상태일 때
            {
                if (DemoDataManager.characterDatasList[0].bottoms.Equals(itemDataList[itemnumber].name)) //같은 하의 장착 해제
                {
                    DemoDataManager.characterDatasList[0].bottoms = "";
                    allItemStr -= itemDataList[itemnumber].str;
                    allItemDef -= itemDataList[itemnumber].def;
                    allItemAgi -= itemDataList[itemnumber].agi;
                }
                else //다른 하의를 장착할 때
                {
                    int preItemNumber = 0;
                    for (int i = 0; i < listIndex; i++)
                    {
                        if (DemoDataManager.characterDatasList[0].bottoms.Equals(itemDataList[i].name))
                            preItemNumber = i;
                    }
                    allItemStr -= itemDataList[preItemNumber].str; //원래 장착한 하의 스탯 제거
                    allItemDef -= itemDataList[preItemNumber].def;
                    allItemAgi -= itemDataList[preItemNumber].agi;

                    DemoDataManager.characterDatasList[0].bottoms = itemDataList[itemnumber].name;
                    allItemStr += itemDataList[itemnumber].str;
                    allItemDef += itemDataList[itemnumber].def;
                    allItemAgi += itemDataList[itemnumber].agi;
                }
            }
            if (DemoDataManager.characterDatasList[0].top == "우주복") //한벌옷 입었을때
            {
                int uniNumber = 0;
                for (int i = 0; i < DemoDataManager.allClothesItemList.Count; i++)
                {
                    if (DemoDataManager.allClothesItemList[i].name == "우주복")
                        uniNumber = i;
                }
                if (DemoDataManager.characterDatasList[0].top == "우주복")
                {
                    allItemStr -= DemoDataManager.allClothesItemList[uniNumber].str;
                    allItemDef -= DemoDataManager.allClothesItemList[uniNumber].def;
                    allItemAgi -= DemoDataManager.allClothesItemList[uniNumber].agi;
                }
                DemoDataManager.characterDatasList[0].top = ""; //상의 제거
            }
        }
        for (int i = 0; i < DemoDataManager.allClothesItemList.Count; i++) //새로운 아이템 알람 삭제
        {
            if (DemoDataManager.allClothesItemList[i].name == itemDataList[itemnumber].name)
            {
                DemoDataManager.allClothesItemList[i].isnew = false;
                itemDataList[itemnumber].isnew = false;
                newAlarmImg[itemnumber].gameObject.SetActive(false);
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
            string installHelmetName = DemoDataManager.characterDatasList[0].helmet;
            string installTopName = DemoDataManager.characterDatasList[0].top;
            string installBottomsName = DemoDataManager.characterDatasList[0].bottoms;

            for (int i = 0; i < DemoDataManager.allClothesItemList.Count; i++)
            {
                if (DemoDataManager.allClothesItemList[i].name.Equals(installHelmetName))
                {
                    if (DemoDataManager.allClothesItemList[i].setname.Equals(itemDataList[itemnumber].setname))
                        setCount--;
                }
                if (DemoDataManager.allClothesItemList[i].name.Equals(installTopName))
                {
                    if (DemoDataManager.allClothesItemList[i].setname.Equals(itemDataList[itemnumber].setname))
                        setCount--;
                }
                if (DemoDataManager.allClothesItemList[i].name.Equals(installBottomsName))
                {
                    if (DemoDataManager.allClothesItemList[i].setname.Equals(itemDataList[itemnumber].setname))
                        setCount--;
                }
            }
            if (setCount == 0)
                isSet = true;
            else
                isSet = false;
        }
        if (isSet)
        {
            DemoDataManager.characterDatasList[0].setstr = itemDataList[itemnumber].setstr;
            DemoDataManager.characterDatasList[0].setdef = itemDataList[itemnumber].setdef;
            DemoDataManager.characterDatasList[0].setagi = itemDataList[itemnumber].setagi;
            DemoDataManager.characterDatasList[0].setcrip = itemDataList[itemnumber].setcrip;
            DemoDataManager.characterDatasList[0].setname = itemDataList[itemnumber].setname;
        }
        else
        {
            DemoDataManager.characterDatasList[0].setstr = 0;
            DemoDataManager.characterDatasList[0].setdef = 0;
            DemoDataManager.characterDatasList[0].setagi = 0;
            DemoDataManager.characterDatasList[0].setcrip = 0;
            DemoDataManager.characterDatasList[0].setname = "";
        }
    }
    void StatExReset()
    {
        statExTx.text = "레벨: " + DemoDataManager.characterDatasList[0].level + "\n이름: " + DemoDataManager.characterDatasList[0].name + "\nHP: " + DemoDataManager.characterDatasList[0].hp +
            "\nSTR: " + DemoDataManager.characterDatasList[0].str;
        if (allItemStr != 0)
            statExTx.text += " (+" + allItemStr + ")";
        statExTx.text += "\nDEF: " + DemoDataManager.characterDatasList[0].def;
        if (allItemDef != 0)
            statExTx.text += " (+" + allItemDef + ")";
        statExTx.text += "\nAGI: " + DemoDataManager.characterDatasList[0].agi;
        if (allItemAgi != 0)
            statExTx.text += " (+" + allItemAgi + ")";
        statExTx.text += "\n공격 속도: " + DemoDataManager.characterDatasList[0].itemspeed + "\n크리티컬 확률: " + DemoDataManager.characterDatasList[0].crip;
        if (allItemCrip != 0)
            statExTx.text += " (+" + allItemCrip + "%)";
        if (DemoDataManager.characterDatasList[0].setname != "")
            statExTx.text += "\n" + DemoDataManager.characterDatasList[0].setname + " 세트 적용!";
        if (DemoDataManager.characterDatasList[0].setstr != 0)
            statExTx.text += "\nSTR +" + DemoDataManager.characterDatasList[0].setstr;
        if (DemoDataManager.characterDatasList[0].setdef != 0)
            statExTx.text += "\nDEF +" + DemoDataManager.characterDatasList[0].setdef;
        if (DemoDataManager.characterDatasList[0].setagi != 0)
            statExTx.text += "\nAGI +" + DemoDataManager.characterDatasList[0].setagi;
        if(DemoDataManager.characterDatasList[0].setcrip!=0)
            statExTx.text+="\n크리티컬 확률 +" + DemoDataManager.characterDatasList[0].setcrip;
    }
    void CategoryChange(int btnumber) //카테고리 바꿀 때
    {
        for (int i = 0; i < 3; i++)
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
    void PageChange()
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
        for (int i = 0; i < listIndex; i++)
        {
            items[i].gameObject.SetActive(true);
            itemsLock[i].gameObject.SetActive(true);

            if (itemDataList[i].count != 0) //아이템 보일 때
            {
                itemsLock[i].gameObject.SetActive(false);
                itemNameTx[i].text = itemDataList[i].name;
                for(int j=0; j<DemoDataManager.allClothesItemList.Count; j++) //스프라이트 바꿔주기
                {
                    if (DemoDataManager.allClothesItemList[j].name.Equals(itemDataList[i].name))
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
        for(int i=listIndex; i<9; i++) //자리가 빈 아이템 창 제거
        {
            items[i].gameObject.SetActive(false);
            itemsLock[i].gameObject.SetActive(false);
        }
        rightBt.gameObject.SetActive(false);
        leftBt.gameObject.SetActive(false);
    }
    void DetailBtOn() //살펴보기 버튼 관리
    {
        int count = 0;
        if (isInvenBtClick[0])
        {
            for (int i = 0; i < listIndex; i++)
            {
                if (itemDataList[i].name.Equals(DemoDataManager.characterDatasList[0].helmet))
                    count++;
            }
        }
        if (isInvenBtClick[1])
        {
            for (int i = 0; i < listIndex; i++)
            {
                if (itemDataList[i].name.Equals(DemoDataManager.characterDatasList[0].top))
                    count++;
            }
        }
        if (isInvenBtClick[2])
        {
            for (int i = 0; i < listIndex; i++)
            {
                if (itemDataList[i].name.Equals(DemoDataManager.characterDatasList[0].bottoms))
                    count++;
            }
        }
        if (count == 1)
            detailBt.gameObject.SetActive(true);
        else
            detailBt.gameObject.SetActive(false);
    }
    void SaveStatData() //능력치 데이터 저장
    {
        DemoDataManager.characterDatasList[0].itemstr = allItemStr;
        DemoDataManager.characterDatasList[0].itemdef = allItemDef;
        DemoDataManager.characterDatasList[0].itemagi = allItemAgi;
        DemoDataManager.characterDatasList[0].itemcrip = allItemCrip;

        DemoDataManager.characterDatasList[0].allstr = DemoDataManager.characterDatasList[0].itemstr + DemoDataManager.characterDatasList[0].setstr + DemoDataManager.characterDatasList[0].str;
        DemoDataManager.characterDatasList[0].alldef = DemoDataManager.characterDatasList[0].itemdef + DemoDataManager.characterDatasList[0].setdef + DemoDataManager.characterDatasList[0].def;
        DemoDataManager.characterDatasList[0].allagi = DemoDataManager.characterDatasList[0].itemagi + DemoDataManager.characterDatasList[0].setagi + DemoDataManager.characterDatasList[0].agi;
        DemoDataManager.characterDatasList[0].allcrip = DemoDataManager.characterDatasList[0].itemcrip + DemoDataManager.characterDatasList[0].setcrip + DemoDataManager.characterDatasList[0].crip;
    }
    int InputItemData(string type) //필요한 데이터만 저장하기
    {
        itemDataList.Clear();
        int count = 0;
        for (int i = 0; i < DemoDataManager.allClothesItemList.Count; i++)
        {
            if (DemoDataManager.allClothesItemList[i].type.Equals(type))
            {
                itemDataList.Add(new ItemData(DemoDataManager.allClothesItemList[i].name, DemoDataManager.allClothesItemList[i].count,
                    DemoDataManager.allClothesItemList[i].str, DemoDataManager.allClothesItemList[i].def, DemoDataManager.allClothesItemList[i].agi,
                    DemoDataManager.allClothesItemList[i].setname, DemoDataManager.allClothesItemList[i].setstr, DemoDataManager.allClothesItemList[i].setdef,
                    DemoDataManager.allClothesItemList[i].setagi, DemoDataManager.allClothesItemList[i].setnumber, DemoDataManager.allClothesItemList[i].setcrip,
                    DemoDataManager.allClothesItemList[i].isnew));
                count++;
            }
        }
        return count;
    }
    public class ItemData
    {
        public ItemData(string _name, int _count, int _str, int _def, int _agi, string _setname, int _setstr, int _setdef, int _setagi, int _setnumber, decimal _setcrip, bool _isnew)
        {
            name = _name; count = _count; str = _str; def = _def; agi = _agi;
            setname = _setname; setstr = _setstr; setdef = _setdef; setagi = _setagi; setnumber = _setnumber; setcrip = _setcrip;
            isnew = _isnew;
        }
        public string name, setname;
        public int count, str, def, agi, setstr, setdef, setagi, setnumber;
        public decimal setcrip;
        public bool isnew;
    }
}
