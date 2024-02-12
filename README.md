## 📖 Description
- <b>프로젝트명</b> : Unity Framework<br>
- <b>목 적</b> : 개발자들의 작업 효율을 위한 프레임워크 제공<br>
- <b>구 조</b> :<br>
### 1. Managers
  - Managers.cs : Manager들을 관리하는 Monobehaviour (모든 매니져는 해당 스크립트로 접근)
      ![image](https://github.com/richrookie/UnityFramework/assets/83854046/a5fd02df-0f48-482c-8ffe-defff1952775)
  - DataManager.cs : 인게임 Data 관리용도
  - Poolable.cs : 풀링 하고싶은 오브젝트 프리팹에 넣으면 풀링 되어 사용됨
  ![image](https://github.com/richrookie/UnityFramework/assets/83854046/6b6c88f3-d750-4dbc-b053-da01f25f4f7f)

  - PoolManager.cs :  풀링을 위해 사용하는 매니저 (Stack으로 구현)
    ![image](https://github.com/richrookie/UnityFramework/assets/83854046/6a984478-765c-4052-a367-f590f1a75711)

  - ResourceManager.cs : 리소스(Resourecs) 관리용도의 매니저 (기본적으로 Resources.Load 방식을 활용함)
    - Load()
    - Instantiate()
    - Destroy()
  - SceneManagerEx.cs : 씬 변경 연출용도
  - SoundManager.cs : 사운드 관리 용도의 매니저\
    ![image](https://github.com/richrookie/UnityFramework/assets/83854046/b7904dd8-817a-4108-a4a5-3ff0bd0e7b08)

  - UIManager.cs : UI 관리 용도의 매니저

### 2. UI_Base
Bind 오브젝트
- GetObject
- Getimage
- GetButton
- GetText
- GetTextMesh
  
함수를 통해 하위 오브젝트 컴포너트에 접근. (관리의 복잡함을 해소해줌)

### 3. Utils
- Util.cs : 쓸모있는 함수들 미리 정의 (MonoBehaviour 상속)
  - GetOrAddComponent<T>(GameObject go)
  - WaitForSeconds WaitGet(float waitSec) : 코루틴 사용 시 WaitForSeconds를 Dictionary에 미리 등록
  - StringToEnum<T>(string name) : string을 Enum으로 바꾸기
  - ParticleStart(GameObject particle) : 파티클 동작하기
  - ParticleStop(GameObject particle) : 파티클 멈추기
  - FindChild(GameObject go, string name = null, bool recursive = false) : 자식에 해당 name이 존재하는지 여부 체크
- Define.cs : 미리 정의해 놓는 여러가지 상수
- Extension.cs :
  - GetOrAddComponent<T>(this GameObject go) : 일단 GetComponent 없으면 AddComponent
  - BindEvent(this GameObject go) : Image 에 이벤트 달기
  - Shuffle<T>(this IList list, int seed = -1) : 리스트 내 값들 섞기
  - UnitSplit(int num) : 숫자를 천단위마다 ','로 나눔
  - SetTextUnit(string[] unit) : 돈 단위 변환  
  - TryParseInt(string value) : string to int/float 변환
  - Sum(this IEnumerable<int/float/double> array) : 배열의 합
  - Average(this IEnumerable<int/float/double> array) : 배열의 평균값
  - SetPositionX(this Transform tr, float x) : Transform의 X만 바꾸기
  - SetAlpha(this Graphic graphic, float alpha) : Color의 Alpha 만 바꾸기
  - PlayAllParticle(this ParticleSystem particle) : 파티클 자식들까지 모두 실행
  - StopAllParticle(this ParticleSystem particle) : 파티클 자식들까지 모두 정지

### 4. 폴더구성
- 00Scenes : 씬 모음집
- 01Scripts : 스크립 모음집
- 02Prefabs : 프래팹 모음집
- 03Materials : 머테리얼 모음집
- 04Textures : 텍스쳐 모음집
- 05Animations : 애니메이션 모음집
- 06Sounds : Bgm, Effect 사운드 모음집
- Plugins : 미리 포함된 에셋들 모음집

02Prefabs, 03Materials, 04Textures 폴더 내부에 Resources 폴더가 존재한다면,
Managers.Resource.Load<Sprite>("Name") 혹은 Managers.Resource.Instantiate("Name") 같이 로드할 수 있음.
