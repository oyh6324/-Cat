# Cat

사용 설명서

## 코드

### Demo 버전 (현재 사용 중인 코드)
모든 Demo 버전 Script는 `Script-0. Demo` 경로에 있음. 
졸작은 Demo 버전으로 낸다. 후에 정식 버전이 나올지는 불투명.
Demo가 붙지 않은 Script는 정식에도 사용할 수 있음.
그리고 왜 다 끝에 Manager를 붙였지? ㅎㅎ 흥

#### 기본
- DemoDataManager: 데이터를 저장하고 불러오는 용도. 무슨 데이터 파일이 있는지 알 수 있음.
- DemoTopManager: 상단바를 관리. 뒤로 가기 버튼과 각 게임 머니, 옵션이 작동함.
- DemoCatManager: 고양이 애니메이션과 입고 있는 옷을 보여줌.
- FreeTicketReset: 상점의 멸치 상자 무료이용권을 매일 자정마다 넣어줌.

#### 시작 화면
- StartGame: 게임 시작 후 이름 입력하는 곳까지 구현.
- FadeIO: 페이드 인/아웃 구현.

#### 튜토리얼
- Tutorial: 튜토리얼 구현.

#### 로비
- Moveplace: 로비에서 각 버튼의 구역으로 이동시켜 줌. 로비의 고양이 스탯을 보여줌.

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
Script 파일의 1번부터 6번 해당. 현재 사용하고 있지 않음.
업데이트 안한 지 오래됐음. 삭제하고 데모 버전을 정식 버전으로 업그레이드 시켜서 새로 짜는 게 나을 듯.

## 리소스 파일
처음으로 플레이할 때 유저의 저장 공간에 넣을 데이터가 들어 있음.

- AchievementDataText: 업적 데이터
- DemoClothesItemDataText: 의상 데이터
- DemoWeaponItemDataText: 무기 데이터
- MonsterCollectioniDataText: 도감을 위한 몬스터 데이터

## 이미지 파일

- 0 Ui: Ui와 관련된 모든 이미지
- 1 BG: 모든 Background 이미지
- 2 Weapon: 모든 무기 이미지
- 3 Top: 모든 상의 이미지
- 4 Bottom: 모든 하의 이미지
- 5 Helmet: 모든 헬멧 이미지
- 6 Cat: 모든 고양이 이미지 (대기 모션, 워킹 모션, 공격 모션, 데드 모션, 피격 모션)
- 7 Monster: 모든 몬스터 이미지 (1~15 스테이지까지)

## 애니메이션 파일
- 0 Ui: Ui에 필요한 애니메이션들
- 1 Cat: 고양이 애니메이션
- 2 Monster: 몬스터 애니메이션. 스테이지별로 분류되어 있음.
- 3 Top: 상의 애니메이션
- 4 Bottom: 하의 애니메이션
- 5 Helmet: 헬멧 애니메이션

## 폰트 파일
- 레코체: 강조 효과.
- 유토이미지고딕: 기본 폰트.

## 브랜치
각자의 개인 공간. 한 파일을 가지고 있지만 각자의 브랜치에는 요청하지 않는 이상 영향을 끼칠 수 없다!

### 자기 브랜치 생성하는 법
- 초대를 받고 자기 컴퓨터에 Cat 레퍼지토리를 clone 한다.
- 해당 디렉토리를 열어서 git bash를 킨다.
- `git branch 브랜치 이름`을 입력한다. (ex. git branch younghyun)
- 그러면 `이름`의 브랜치가 생성된다. (우리는 우리 영어 이름 쓰자!)
- 계속해서 `git checkout 브랜치 이름`을 입력하면 해당 브랜치로 스위치된다.
- 이제 거기서 작업하면 됨! main 브랜치 쓰면 안됨!

### main
각자 작업 공간(개인 브랜치)에서 다 된 것만 넣는 곳. 반드시 완성본만 넣어야 한다. 만약 기존 코드나 파일(clone 했을 때 초기 환경)을 건드는 게 있다면 병합 요청을 넣어도 된다.

### 각자의 브랜치
평소에 작업하는 곳. 끝낸 작업이 있다면 main에 병합 요청을 넣어야 한다. 그리고 main이 업데이트 됐다면 자기 브랜치로 합병하는 것을 잊지 말기!
