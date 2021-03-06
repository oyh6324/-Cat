# Cat

사용 설명서

## 코드

### Demo 버전
모든 Demo 버전 Script는 `Script-0. Demo` 경로에 있음. 
졸작은 Demo 버전으로 낸다. 후에 정식 버전이 나올지는 불투명.
Demo가 붙지 않은 Script는 정식에도 사용할 수 있음.
그리고 왜 다 끝에 Manager를 붙였지? ㅎㅎ 이해할 수 없는 과거의 나.

#### 기본
- DemoDataManager: 데이터를 저장하고 불러오는 용도. 무슨 데이터 파일이 있는지 알 수 있음.
- DemoTopManager: 상단바를 관리. 뒤로 가기 버튼과 각 게임 머니, 옵션이 작동함.
- DemoCatManager: 고양이 애니메이션과 입고 있는 옷, 스탯을 보여줌.
- FreeTicketReset: 상점의 멸치 상자 무료이용권을 매일 자정마다 넣어줌.

#### 로비
- Moveplace: 로비에서 각 버튼의 구역으로 이동시켜 줌.

#### 업적
- DemoAchievementManager: 모든 업적 작동에 대해 관리.
- InstanceMove: 업적의 보상 instance의 모션.

#### 도감
- DemoCollectionManager: 모든 도감 작동에 대해 관리.

#### 인벤토리
- DemoInventoryPageManager: 아이템 데이터를 불러와 인벤토리 넣고 보여줌. 의상 장착 관리.
- DemoInventroyDetailManager: 터치한 아이템을 자세히 볼 수 있음. 아이템 업그레이드도 관리.

#### 무기고
- DemoWeaponManager: 무기고 작동에 대해 관리. 무기 장착 관리.

#### 상점
- DemoShopPageManager: 상점의 구매 가능한 물건을 보여줌.
- DemoShopBuyManager: 상점의 물건을 구매할 때 작동.
- DemoRandomBoxManager: 멸치, 진주 상자 사용 시 뽑기 작동.

#### 스테이지 선택 화면(미완)
- StageSelect: 스테이지를 고르고 열쇠를 사용하는 것을 관리. 스테이지에 들어갈 수 있도록 해야 함.

### 정식 버전
Script 파일의 1번부터 6번 해당.
업데이트 안한 지 오래됐음. 삭제하고 데모 버전을 정식 버전으로 업그레이드 시켜서 새로 짜는 게 나을 듯.

## 리소스 파일
처음으로 플레이할 때 유저의 저장 공간에 넣을 데이터가 들어 있음.

## 이미지 파일

- 0. Ui: Ui와 관련된 모든 이미지
- 1. BG: 모든 Background 이미지

## 폰트 파일
레코체와 유토이미지고딕 사용 중.
