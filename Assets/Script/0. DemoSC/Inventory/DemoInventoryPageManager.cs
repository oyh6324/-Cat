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

        allItemStr = DemoDataManager.Instance.characterDatasList[0].itemstr;
        allItemDef = DemoDataManager.Instance.characterDatasList[0].itemdef;
        allItemAgi = DemoDataManager.Instance.characterDatasList[0].itemagi;
        allItemCrip = DemoDataManager.Instance.characterDatasList[0].itemcrip;

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
            allItemStr = DemoDataManager.Instance.characterDatasList[0].itemstr;
            allItemDef = DemoDataManager.Instance.characterDatasList[0].itemdef;
            allItemAgi = DemoDataManager.Instance.characterDatasList[0].itemagi;
            allItemCrip = DemoDataManager.Instance.characterDatasList[0].itemcrip;
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
            if (DemoDataManager.Instance.characterDatasList[0].helmet == "") //헬멧 장착이 없을 때
            {
                DemoDataManager.Instance.characterDatasList[0].helmet = itemDataList[itemnumber].name;
                allItemStr += itemDataList[itemnumber].str;
                allItemDef += itemDataList[itemnumber].def;
                allItemAgi += itemDataList[itemnumber].agi;
            }
            else if (DemoDataManager.Instance.characterDatasList[0].helmet != "") //장착된 상태일 때
            {
                if (DemoDataManager.Instance.characterDatasList[0].helmet.Equals(itemDataList[itemnumber].name)) //같은 헬멧 장착 해제
                {
                    DemoDataManager.Instance.characterDatasList[0].helmet = "";
                    allItemStr -= itemDataList[itemnumber].str;
                    allItemDef -= itemDataList[itemnumber].def;
                    allItemAgi -= itemDataList[itemnumber].agi;
                }
                else //다른 헬멧을 장착할 때
                {
                    int preItemNumber = 0;
                    for (int i = 0; i < listIndex; i++)
                    {
                        if (DemoDataManager.Instance.characterDatasList[0].helmet.Equals(itemDataList[i].name))
                            preItemNumber = i;
                    }
                    allItemStr -= itemDataList[preItemNumber].str; //원래 장착한 헬멧 스탯 제거
                    allItemDef -= itemDataList[preItemNumber].def;
                    allItemAgi -= itemDataList[preItemNumber].agi;

                    DemoDataManager.Instance.characterDatasList[0].helmet = itemDataList[itemnumber].name;
                    allItemStr += itemDataList[itemnumber].str;
                    allItemDef += itemDataList[itemnumber].def;
                    allItemAgi += itemDataList[itemnumber].agi;
                }
            }
        }
        else if (isInvenBtClick[1]) //상의 열었을 때
        {
            if (DemoDataManager.Instance.characterDatasList[0].top == "") // 장착이 없을 때
            {
                DemoDataManager.Instance.characterDatasList[0].top = itemDataList[itemnumber].name;
                allItemStr += itemDataList[itemnumber].str;
                allItemDef += itemDataList[itemnumber].def;
                allItemAgi += itemDataList[itemnumber].agi;
            }
            else if (DemoDataManager.Instance.characterDatasList[0].top != "") //장착된 상태일 때
            {
                if (DemoDataManager.Instance.characterDatasList[0].top.Equals(itemDataList[itemnumber].name)) //같은 상의 장착 해제
                {
                    DemoDataManager.Instance.characterDatasList[0].top = "";
                    allItemStr -= itemDataList[itemnumber].str;
                    allItemDef -= itemDataList[itemnumber].def;
                    allItemAgi -= itemDataList[itemnumber].agi;
                }
                else //다른 상의를 장착할 때
                {
                    int preItemNumber = 0;
                    for (int i = 0; i < listIndex; i++)
                    {
                        if (DemoDataManager.Instance.characterDatasList[0].top.Equals(itemDataList[i].name))
                            preItemNumber = i;
                    }
                    allItemStr -= itemDataList[preItemNumber].str; //원래 장착한 상의 스탯 제거
                    allItemDef -= itemDataList[preItemNumber].def;
                    allItemAgi -= itemDataList[preItemNumber].agi;

                    DemoDataManager.Instance.characterDatasList[0].top = itemDataList[itemnumber].name;
                    allItemStr += itemDataList[itemnumber].str;
                    allItemDef += itemDataList[itemnumber].def;
                    allItemAgi += itemDataList[itemnumber].agi;
                }

            }
            if (itemDataList[itemnumber].name == "우주복") //한벌옷을 입었을때
            {
                if (DemoDataManager.Instance.characterDatasList[0].bottoms != "") //하의가 있다면
                {
                    for (int i = 0; i < DemoDataManager.Instance.allClothesItemList.Count; i++)
                    {
                        if (DemoDataManager.Instance.characterDatasList[0].bottoms.Equals(DemoDataManager.Instance.allClothesItemList[i].name)) //하의 제거
                        {
                            allItemStr -= DemoDataManager.Instance.allClothesItemList[i].str;
                            allItemDef -= DemoDataManager.Instance.allClothesItemList[i].def;
                            allItemAgi -= DemoDataManager.Instance.allClothesItemList[i].agi;
                        }
                    }
                    DemoDataManager.Instance.characterDatasList[0].bottoms = "";
                }
            }
        }
        else if (isInvenBtClick[2]) //하의 열렸을 때
        {
            if (DemoDataManager.Instance.characterDatasList[0].bottoms == "") // 장착이 없을 때
            {
                DemoDataManager.Instance.characterDatasList[0].bottoms = itemDataList[itemnumber].name;
                allItemStr += itemDataList[itemnumber].str;
                allItemDef += itemDataList[itemnumber].def;
                allItemAgi += itemDataList[itemnumber].agi;
            }
            else if (DemoDataManager.Instance.characterDatasList[0].bottoms != "") //장착된 상태일 때
            {
                if (DemoDataManager.Instance.characterDatasList[0].bottoms.Equals(itemDataList[itemnumber].name)) //같은 하의 장착 해제
                {
                    DemoDataManager.Instance.characterDatasList[0].bottoms = "";
                    allItemStr -= itemDataList[itemnumber].str;
                    allItemDef -= itemDataList[itemnumber].def;
                    allItemAgi -= itemDataList[itemnumber].agi;
                }
                else //다른 하의를 장착할 때
                {
                    int preItemNumber = 0;
                    for (int i = 0; i < listIndex; i++)
                    {
                        if (DemoDataManager.Instance.characterDatasList[0].bottoms.Equals(itemDataList[i].name))
                            preItemNumber = i;
                    }
                    allItemStr -= itemDataList[preItemNumber].str; //원래 장착한 하의 스탯 제거
                    allItemDef -= itemDataList[preItemNumber].def;
                    allItemAgi -= itemDataList[preItemNumber].agi;

                    DemoDataManager.Instance.characterDatasList[0].bottoms = itemDataList[itemnumber].name;
                    allItemStr += itemDataList[itemnumber].str;
                    allItemDef += itemDataList[itemnumber].def;
                    allItemAgi += itemDataList[itemnumber].agi;
                }
            }
            if (DemoDataManager.Instance.characterDatasList[0].top == "우주복") //한벌옷 입었을때
            {
                int uniNumber = 0;
                for (int i = 0; i < DemoDataManager.Instance.allClothesItemList.Count; i++)
                {
                    if (DemoDataManager.Instance.allClothesItemList[i].name == "우주복")
                        uniNumber = i;
                }
                if (DemoDataManager.Instance.characterDatasList[0].top == "우주복")
                {
                    allItemStr -= DemoDataManager.Instance.allClothesItemList[uniNumber].str;
                    allItemDef -= DemoDataManager.Instance.allClothesItemList[uniNumber].def;
                    allItemAgi -= DemoDataManager.Instance.allClothesItemList[uniNumber].agi;
                }
                DemoDataManager.Instance.characterDatasList[0].top = ""; //상의 제거
            }
        }
        for (int i = 0; i < DemoDataManager.Instance.allClothesItemList.Count; i++) //새로운 아이템 알람 삭제
        {
            if (DemoDataManager.Instance.allClothesItemList[i].name == itemDataList[itemnumber].name)
            {
                DemoDataManager.Instance.allClothesItemList[i].isnew = false;
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
            string installHelmetName = DemoDataManager.Instance.characterDatasList[0].helmet;
            string installTopName = DemoDataManager.Instance.characterDatasList[0].top;
            string installBottomsName = DemoDataManager.Instance.characterDatasList[0].bottoms;

            for (int i = 0; i < DemoDataManager.Instance.allClothesItemList.Count; i++)
            {
                if (DemoDataManager.Instance.allClothesItemList[i].name.Equals(installHelmetName))
                {
                    if (DemoDataManager.Instance.allClothesItemList[i].setname.Equals(itemDataList[itemnumber].setname))
                        setCount--;
                }
                if (DemoDataManager.Instance.allClothesItemList[i].name.Equals(installTopName))
                {
                    if (DemoDataManager.Instance.allClothesItemList[i].setname.Equals(itemDataList[itemnumber].setname))
                        setCount--;
                }
                if (DemoDataManager.Instance.allClothesItemList[i].name.Equals(installBottomsName))
                {
                    if (DemoDataManager.Instance.allClothesItemList[i].setname.Equals(itemDataList[itemnumber].setname))
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
            DemoDataManager.Instance.characterDatasList[0].setstr = itemDataList[itemnumber].setstr;
            DemoDataManager.Instance.characterDatasList[0].setdef = itemDataList[itemnumber].setdef;
            DemoDataManager.Instance.characterDatasList[0].setagi = itemDataList[itemnumber].setagi;
            DemoDataManager.Instance.characterDatasList[0].setcrip = itemDataList[itemnumber].setcrip;
            DemoDataManager.Instance.characterDatasList[0].setname = itemDataList[itemnumber].setname;
        }
        else
        {
            DemoDataManager.Instance.characterDatasList[0].setstr = 0;
            DemoDataManager.Instance.characterDatasList[0].setdef = 0;
            DemoDataManager.Instance.characterDatasList[0].setagi = 0;
            DemoDataManager.Instance.characterDatasList[0].setcrip = 0;
            DemoDataManager.Instance.characterDatasList[0].setname = "";
        }
    }
    void StatExReset()
    {
        statExTx.text = "레벨: " + DemoDataManager.Instance.characterDatasList[0].level + "\n이름: " + DemoDataManager.Instance.characterDatasList[0].name + "\nHP: " + DemoDataManager.Instance.characterDatasList[0].hp +
            "\nSTR: " + DemoDataManager.Instance.characterDatasList[0].str;
        if (allItemStr != 0)
            statExTx.text += " (+" + (allItemStr + DemoDataManager.Instance.characterDatasList[0].setstr) + ")";
        statExTx.text += "\nDEF: " + DemoDataManager.Instance.characterDatasList[0].def;
        if (allItemDef != 0)
            statExTx.text += " (+" + (allItemDef + DemoDataManager.Instance.characterDatasList[0].setdef) + ")";
        statExTx.text += "\nAGI: " + DemoDataManager.Instance.characterDatasList[0].agi;
        if (allItemAgi != 0)
            statExTx.text += " (+" + (allItemAgi + DemoDataManager.Instance.characterDatasList[0].setagi) + ")";
        statExTx.text += "\n공격 속도: " + DemoDataManager.Instance.characterDatasList[0].itemspeed + "\n크리티컬 확률: " + DemoDataManager.Instance.characterDatasList[0].crip;
        if (allItemCrip != 0)
            statExTx.text += " (+" + allItemCrip + "%)";
        if (DemoDataManager.Instance.characterDatasList[0].setname != "")
            statExTx.text += "\n" + DemoDataManager.Instance.characterDatasList[0].setname + " 세트 적용!";
        if (DemoDataManager.Instance.characterDatasList[0].setstr != 0)
            statExTx.text += "\nSTR +" + DemoDataManager.Instance.characterDatasList[0].setstr;
        if (DemoDataManager.Instance.characterDatasList[0].setdef != 0)
            statExTx.text += "\nDEF +" + DemoDataManager.Instance.characterDatasList[0].setdef;
        if (DemoDataManager.Instance.characterDatasList[0].setagi != 0)
            statExTx.text += "\nAGI +" + DemoDataManager.Instance.characterDatasList[0].setagi;
        if(DemoDataManager.Instance.characterDatasList[0].setcrip!=0)
            statExTx.text+="\n크리티컬 확률 +" + DemoDataManager.Instance.characterDatasList[0].setcrip;
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
                for(int j=0; j<DemoDataManager.Instance.allClothesItemList.Count; j++) //스프라이트 바꿔주기
                {
                    if (DemoDataManager.Instance.allClothesItemList[j].name.Equals(itemDataList[i].name))
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
                if (itemDataList[i].name.Equals(DemoDataManager.Instance.characterDatasList[0].helmet))
                    count++;
            }
        }
        if (isInvenBtClick[1])
        {
            for (int i = 0; i < listIndex; i++)
            {
                if (itemDataList[i].name.Equals(DemoDataManager.Instance.characterDatasList[0].top))
                    count++;
            }
        }
        if (isInvenBtClick[2])
        {
            for (int i = 0; i < listIndex; i++)
            {
                if (itemDataList[i].name.Equals(DemoDataManager.Instance.characterDatasList[0].bottoms))
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
        DemoDataManager.Instance.characterDatasList[0].itemstr = allItemStr;
        DemoDataManager.Instance.characterDatasList[0].itemdef = allItemDef;
        DemoDataManager.Instance.characterDatasList[0].itemagi = allItemAgi;
        DemoDataManager.Instance.characterDatasList[0].itemcrip = allItemCrip;

        DemoDataManager.Instance.characterDatasList[0].allstr = DemoDataManager.Instance.characterDatasList[0].itemstr + DemoDataManager.Instance.characterDatasList[0].setstr + DemoDataManager.Instance.characterDatasList[0].str;
        DemoDataManager.Instance.characterDatasList[0].alldef = DemoDataManager.Instance.characterDatasList[0].itemdef + DemoDataManager.Instance.characterDatasList[0].setdef + DemoDataManager.Instance.characterDatasList[0].def;
        DemoDataManager.Instance.characterDatasList[0].allagi = DemoDataManager.Instance.characterDatasList[0].itemagi + DemoDataManager.Instance.characterDatasList[0].setagi + DemoDataManager.Instance.characterDatasList[0].agi;
        DemoDataManager.Instance.characterDatasList[0].allcrip = DemoDataManager.Instance.characterDatasList[0].itemcrip + DemoDataManager.Instance.characterDatasList[0].setcrip + DemoDataManager.Instance.characterDatasList[0].crip;
    }
    int InputItemData(string type) //필요한 데이터만 저장하기
    {
        itemDataList.Clear();
        int count = 0;
        for (int i = 0; i < DemoDataManager.Instance.allClothesItemList.Count; i++)
        {
            if (DemoDataManager.Instance.allClothesItemList[i].type.Equals(type))
            {
                itemDataList.Add(new ItemData(DemoDataManager.Instance.allClothesItemList[i].name, DemoDataManager.Instance.allClothesItemList[i].count,
                    DemoDataManager.Instance.allClothesItemList[i].str, DemoDataManager.Instance.allClothesItemList[i].def, DemoDataManager.Instance.allClothesItemList[i].agi,
                    DemoDataManager.Instance.allClothesItemList[i].setname, DemoDataManager.Instance.allClothesItemList[i].setstr, DemoDataManager.Instance.allClothesItemList[i].setdef,
                    DemoDataManager.Instance.allClothesItemList[i].setagi, DemoDataManager.Instance.allClothesItemList[i].setnumber, DemoDataManager.Instance.allClothesItemList[i].setcrip,
                    DemoDataManager.Instance.allClothesItemList[i].isnew));
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
