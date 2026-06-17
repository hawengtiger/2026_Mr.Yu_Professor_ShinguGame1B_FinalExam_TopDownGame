using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class MainIntroUI : MonoBehaviour
{
    [Header("검은 패널")]
    public Image blackPanel;

    [Header("움직일 로고/이미지")]
    public RectTransform logoImage;

    [Header("이동 설정")]
    public float moveX = -960f;
    public float moveTime = 0.5f;

    [Header("씬")]
    public string gameSceneName = "GameScene";

    private bool isMoved;
    private bool isTweening;

    private void Start()
    {
        // 시작할 때 검은 화면
        blackPanel.gameObject.SetActive(true);

        Color c = blackPanel.color;
        c.a = 1f;
        blackPanel.color = c;

        // 천천히 페이드 아웃 → 로고 보임
        blackPanel
            .DOFade(0f, 1.5f)
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                blackPanel.gameObject.SetActive(false);
            });
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            MoveLogo();
        }
    }

    public void MoveLogo()
    {
        if (isMoved || isTweening || logoImage == null)
            return;

        isTweening = true;
        isMoved = true;

        SoundManager.Instance.PlaySFX("MainClick");

        logoImage
            .DOAnchorPosX(moveX, moveTime)
            .SetEase(Ease.InOutQuad)
            .OnComplete(() =>
            {
                isTweening = false;
            });
    }

    public void StartGame()
    {
        blackPanel.gameObject.SetActive(true);
        SoundManager.Instance.StopMusic();
        SoundManager.Instance.PlaySFX("Start");
        Color c = blackPanel.color;
        c.a = 0f;
        blackPanel.color = c;

        blackPanel
            .DOFade(1f, 5f)
            .SetEase(Ease.InQuad)
            .OnComplete(() =>
            {
                SceneManager.LoadScene(gameSceneName);
            });
    }

    public void MainGame()
    {
        SceneManager.LoadScene(gameSceneName);
        Time.timeScale = 1;
    }

    public void QuitGame()
    {
        blackPanel.gameObject.SetActive(true);

        Color c = blackPanel.color;
        c.a = 0f;
        blackPanel.color = c;

        blackPanel
            .DOFade(1f, 0.8f)
            .SetEase(Ease.InQuad)
            .OnComplete(() =>
            {
                #if UNITY_EDITOR
                                UnityEditor.EditorApplication.isPlaying = false;
                #else
                                Application.Quit();
                #endif
            });
    }
}