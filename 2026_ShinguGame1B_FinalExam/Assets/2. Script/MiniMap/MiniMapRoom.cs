using UnityEngine;
using UnityEngine.UI;

public class MiniMapRoom : MonoBehaviour
{
    public Image image;

    public void SetColor(Color color)
    {
        image.color = color;
    }

    public void SetVisible(bool visible)
    {
        gameObject.SetActive(visible);
    }
}