using UnityEngine;

public class SceneChange : MonoBehaviour
{
    public void Scene(string scene)
    {
        LoadingSceneController.LoadScene(scene);
    }
}
