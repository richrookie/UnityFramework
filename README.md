## 📖 Description
- <b>프로젝트명</b> : Unity Framework<br>
- <b>목 적</b> : 개발자들의 작업 효율을 위한 프레임워크 제공<br>
- <b>구 조</b> :<br>
  <b>1. Managers</b>
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


