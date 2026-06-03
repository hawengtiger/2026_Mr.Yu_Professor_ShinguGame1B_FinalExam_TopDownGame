using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SettingScreenUI : MonoBehaviour
{

    [Header("설정 UI")]
    public Image settingScreen;        //할당할 설정 UI


    void Start()
    {
        settingScreen.gameObject.SetActive(false);     //시작할 때 설정 UI 꺼짐
        settingScreen.rectTransform.localScale = Vector3.one * 0.01f;  //whiteOverlay사이즈를 (0.01f, 0.01f, 0.01f)로 변환 (거의 0 크기로 축소);
    }

    /// <summary>
    /// === | 설정창 열림 | ===
    /// </summary>
    public void OpenSettingUI()
    {
        if (settingScreen == null)
        {
            Debug.LogWarning("설정UI창이 할당 안됐는데 개발자! 어케된거야!");
            return;      // 할당 안돼도 일단 빨간 버그 내지마!
        }

        settingScreen.gameObject.SetActive(true);           //창 열림

        settingScreen.rectTransform.DOScale(1, 0.2f);       // 스케일 늘림

    }

    /// <summary>
    /// === | 설정창 닫힘 | ===
    /// </summary>
    public void CloseSettingUI()
    {
        if (settingScreen == null)
        {
            Debug.LogWarning("설정UI창이 할당 안됐는데 개발자! 어케된거야!");
            return;      // 할당 안돼도 일단 빨간 버그 내지마!
        }

        settingScreen.rectTransform.DOScale(0.01f, 0.2f).OnComplete(() => settingScreen.gameObject.SetActive(false));            //스케일 줄이고 창 닫힘
    }
}
