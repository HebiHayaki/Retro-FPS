using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI buttonText;
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    public void Setup(string text, System.Action onClickAction)
    {
        buttonText.text = text;
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => onClickAction());
    }
}
