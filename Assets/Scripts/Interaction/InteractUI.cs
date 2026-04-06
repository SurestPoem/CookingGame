using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InteractUI : MonoBehaviour
{
    [SerializeField] private GameObject containerObject;
    [SerializeField] private TextMeshProUGUI interactText;

    public void Show(string text)
    {
        containerObject.SetActive(true);
        interactText.text = text;
    }

    public void Hide()
    {
        containerObject.SetActive(false);
    }
}
