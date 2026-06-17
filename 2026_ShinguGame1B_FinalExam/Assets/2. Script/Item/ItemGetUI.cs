using TMPro;
using UnityEngine;
using DG.Tweening;

public class ItemGetUI : MonoBehaviour
{
    public static ItemGetUI Instance;

    public CanvasGroup canvasGroup;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descText;

    private void Awake()
    {
        Instance = this;
        canvasGroup.alpha = 0f;
    }

    public void Show(ItemDataSo itemData)
    {
        nameText.text = itemData.itemName;

        string desc = "";

        foreach (string line in itemData.dialogueLines)
        {
            desc += line + "\n";
        }

        descText.text = desc;

        canvasGroup.DOKill();

        Sequence seq = DOTween.Sequence();

        seq.Append(canvasGroup.DOFade(1f, 0.3f));
        seq.AppendInterval(0.4f);
        seq.Append(canvasGroup.DOFade(0f, 0.3f));
    }
}