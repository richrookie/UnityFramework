using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerEx
{
    public BaseScene CurrentScene { get { return GameObject.FindObjectOfType<BaseScene>(); } }
    public Animator _sceneTransitionAnimCtrl { private get; set; }
    private AsyncOperation _asyncLoadingOperation;

    private int _fadeInHash;
    private int _fadeOutHash;

    private bool _isLoadingScene;


    public void Init()
    {
        _fadeInHash = Animator.StringToHash("FadeIn");
        _fadeOutHash = Animator.StringToHash("FadeOut");
    }

    public async void LoadScene(Define.eScene type)
    {
        if (_isLoadingScene)
            return;

        _sceneTransitionAnimCtrl.Play(_fadeOutHash);

        await Task.Delay(500);

        if (!Application.isPlaying)
            return;

        Managers.Clear();

        RuntimeAnimatorController animatorController = Managers.Resource.Load<RuntimeAnimatorController>("SceneTransitionAnimator");
        _sceneTransitionAnimCtrl.runtimeAnimatorController = animatorController;

        _asyncLoadingOperation = SceneManager.LoadSceneAsync(GetSceneName(type), LoadSceneMode.Single);
        _asyncLoadingOperation.allowSceneActivation = false;

        while (!_asyncLoadingOperation.isDone)
        {
            if (_asyncLoadingOperation.progress >= .9f)
                break;

            await Task.Delay(1);
        }

        _asyncLoadingOperation.allowSceneActivation = true;
        _sceneTransitionAnimCtrl.Play(_fadeInHash);
        _isLoadingScene = false;
    }

    public async void FadeOut(Define.eScene type)
    {
        if (_isLoadingScene)
            return;

        _isLoadingScene = true;

        _sceneTransitionAnimCtrl.Play(_fadeOutHash);

        await Task.Delay(500);

        if (!Application.isPlaying)
            return;

        Managers.Clear();

        RuntimeAnimatorController animatorController = Managers.Resource.Load<RuntimeAnimatorController>("SceneTrasitionAnimator");
        _sceneTransitionAnimCtrl.runtimeAnimatorController = animatorController;
        SceneManager.LoadScene(GetSceneName(type), LoadSceneMode.Single);
        _isLoadingScene = false;
    }

    public void FadeIn()
    {
        _sceneTransitionAnimCtrl.Play(_fadeInHash);
    }

    public async void LoadSceneInstance(Define.eScene type)
    {
        if (_isLoadingScene)
            return;

        _isLoadingScene = true;

        _asyncLoadingOperation = SceneManager.LoadSceneAsync(GetSceneName(type), LoadSceneMode.Single);
        _asyncLoadingOperation.allowSceneActivation = false;

        while (!_asyncLoadingOperation.isDone)
        {
            if (_asyncLoadingOperation.progress >= .9f)
                break;

            await Task.Delay(1);

            _asyncLoadingOperation.allowSceneActivation = true;
            _isLoadingScene = false;
        }
    }

    private string GetSceneName(Define.eScene type)
    {
        string name = System.Enum.GetName(typeof(Define.eScene), type);

        return name;
    }

    public void Clear()
    {
        CurrentScene.Clear();
    }
}
