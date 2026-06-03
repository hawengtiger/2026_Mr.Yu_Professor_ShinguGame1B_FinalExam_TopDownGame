using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

/// <summary>
/// === | 설정UI창 갱신 | ===
/// </summary>
public class ScreenUI : MonoBehaviour
{
    /// <summary>
    /// | public 변수 | =====================
    /// </summary>


    [Header("종료 UI")]
    public Image quitScreen;            //할당할 종료 UI

    [Header("시작 UI")]
    public Image whiteOverlay, blackOverlay;       //화면전환을 위한 오버레이.

    [Header("넘어갈 다음씬")]
    public string nextScene;

    private void Start()
    {
        Reset();        //Reset함수 항목 참고
    }

    /// <summary>
    /// === | 시작 버튼 클릭 시 | ===
    /// </summary>
    public void StartGame()
    {
        if (whiteOverlay == null)
        {
            Debug.LogWarning("whiteOverlay가 할당 안됐는데 개발자! 어케된거야!");
            return;      // 할당 안돼도 일단 빨간 버그 내지마!
        }
        else if (blackOverlay == null)
        {
            Debug.LogWarning("blackOverlay가 할당 안됐는데 개발자! 어케된거야!");
            return;      // 할당 안돼도 일단 빨간 버그 내지마!
        }
        
        WhiteOverlay();
    }

    /// <summary>
    /// === | 하얀 스크린 | ===
    /// </summary>
    public void WhiteOverlay()
    {
        Sequence start = DOTween.Sequence();

        whiteOverlay.gameObject.SetActive(true);
        
        start.Append(whiteOverlay.rectTransform.DOScaleX(1f, 0.2f)); // X값을 1로 늘림

        start.Append(whiteOverlay.rectTransform.DOScaleY(1f, 0.2f).OnComplete(() => BlackOverlay()));   // Y값을 1로 늘리고 BlackOverlay함수 호출
    }

    /// <summary>
    /// === | 검은 스크린 | ===
    /// </summary>
    public void BlackOverlay()
    {
        blackOverlay.gameObject.SetActive(true);

        blackOverlay.color = new Color(0, 0, 0, 0);     // 투명하게 사용

            Debug.Log("nextScene : " + nextScene);
        
            blackOverlay.DOFade(1, 0.5f).OnComplete(() => LoadingSceneController.LoadScene(nextScene));       // 페이드 인 후 nextScene 씬 전환.
    }







    /// <summary>
    /// === | 종료창 열림 | ===
    /// </summary>
    public void OpenQuitUI()
    {
        if (quitScreen == null)
        {
            Debug.LogWarning("설정UI창이 할당 안됐는데 개발자! 어케된거야!");
            return;      // 할당 안돼도 일단 빨간 버그 내지마!
        }

        quitScreen.gameObject.SetActive(true);          //창 열림

        quitScreen.rectTransform.DOScale(1, 0.2f);       // 스케일 늘림
    }

    /// <summary>
    /// === | 종료창 닫힘 | ===
    /// </summary>
    public void CloseQuitUI()
    {
        if (quitScreen == null)
        {
            Debug.LogWarning("설정UI창이 할당 안됐는데 개발자! 어케된거야!");
            return;      // 할당 안돼도 일단 빨간 버그 내지마!
        }

        quitScreen.rectTransform.DOScale(0.01f, 0.2f).OnComplete(() => quitScreen.gameObject.SetActive(false));            //스케일 줄이고 창 닫힘
    }

    /// <summary>
    /// === | 게임 종료 | ===
    /// </summary>
    public void GameQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // 에디터에서 종료
#else
        Application.Quit(); // 빌드된 게임에서 종료
#endif
    }

    /// <summary>
    /// === | 초기 세팅 | ===
    /// </summary>
    private void Reset()
    {
        whiteOverlay.gameObject.SetActive(false);
        blackOverlay.gameObject.SetActive(false);
        quitScreen.gameObject.SetActive(false);         //시작할 때 종료 UI 꺼짐

        whiteOverlay.rectTransform.localScale = Vector3.one * 0.01f;  //whiteOverlay사이즈를 (0.01f, 0.01f, 0.01f)로 변환 (거의 0 크기로 축소);
        quitScreen.rectTransform.localScale = Vector3.one * 0.01f;  //whiteOverlay사이즈를 (0.01f, 0.01f, 0.01f)로 변환 (거의 0 크기로 축소);
    }
}
