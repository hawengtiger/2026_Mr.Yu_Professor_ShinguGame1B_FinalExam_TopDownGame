using UnityEngine;
using UnityEngine.SceneManagement;

public class GoGame : MonoBehaviour
{
    public string scene;

    private void Update()
    {

    }

    public void GotoGame(string map)
    {
        SceneManager.LoadScene(map);
    }

    public void ExitGame()
    {
        Debug.Log("게임 종료됨");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // 에디터에서 종료
#else
        Application.Quit(); // 빌드된 게임에서 종료
#endif
    }
}
