using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// === | 스플래시 화면 (일반적으로 애플리케이션 실행 시 페이지의 컨텐츠가 로딩되기까지 일시적으로 보여주는 화면을 말한다.)| ===
/// </summary>
public class Splash : MonoBehaviour
{
    [System.Serializable]
    public struct SplashObject                      //SplashObject이라는 객체를 하나 생성
    {
        public GameObject objectToShow;      // 보여줄 오브젝트
        public float fadeInDuration;                // 페이드인 시간
        public float holdDuration;                  // 유지 시간
        public float fadeOutDuration;            // 페이드아웃 시간
    }

    /// <summary>
    /// | public 변수 | =====================
    /// </summary>

    [Header("스플래시 오브젝트")]
    public List<SplashObject> splashObjects = new List<SplashObject>();         // (new 객체 리스트 선언.) 스플래시 오브젝트 리스트

    [Header("다음씬")]
    public string nextSceneName;        // 로드할 다음 씬 이름




    /// <summary>
    /// | private 변수 | =====================
    ///</summary>

    private AsyncOperation sceneLoadOperation;      //비동기 로딩 상태를 저장하는 변수    (진행률, 완료 여부 다 여기 있음)

    private void Start()
    {
        StartCoroutine(PlaySplashSequence());           //스플래시 씬 실행
    }

    /// <summary>
    /// === | 다음 씬 비동기 설정 | ===
    /// </summary>
    private IEnumerator PlaySplashSequence()
    {
        // 다음 씬 비동기 로드 시작
        sceneLoadOperation = SceneManager.LoadSceneAsync(nextSceneName);    // nextSceneName씬을 현재 씬 뒤에서 로딩 시작
        sceneLoadOperation.allowSceneActivation = false;                                         //로딩 다 되어도 씬 전환 금지

        // 스플래시 시퀀스 실행
        foreach (var splash in splashObjects)       //리스트에 있는 것들을 하나씩 순서대로 실행
        {
            if (splashObjects == null)
            {
                Debug.LogWarning("splashObject가 없어서 자동씬전환 됨.");
                yield break; //splashObjects가 없으면 52번줄로 이동.
            }


            yield return StartCoroutine(ShowSplash(splash));
        }

        // 90%까지 로딩 기다림
        yield return new WaitUntil(() => sceneLoadOperation.progress >= 0.9f);

        // 이제 씬 전환 허용
        sceneLoadOperation.allowSceneActivation = true;
    }

    /// <summary>
    /// === | 스플레쉬 화면 실행 | ===
    /// </summary>
    private IEnumerator ShowSplash(SplashObject splash)
    {
        // CanvasGroup 추가 (없으면 생성)
        CanvasGroup canvasGroup = splash.objectToShow.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = splash.objectToShow.AddComponent<CanvasGroup>();
        }

        // 초기 설정
        canvasGroup.alpha = 0f;
        splash.objectToShow.SetActive(true);

        // 페이드 인
        yield return canvasGroup.DOFade(1f, splash.fadeInDuration).WaitForCompletion();

        // 유지 시간
        yield return new WaitForSeconds(splash.holdDuration);

        // 페이드 아웃
        yield return canvasGroup.DOFade(0f, splash.fadeOutDuration).WaitForCompletion();

        // 오브젝트 비활성화
        splash.objectToShow.SetActive(false);
    }
}