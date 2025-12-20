using System.Collections;
using TMPro;
using UnityEngine;

public class TypewriterIntroText : MonoBehaviour
{
    [Header("Referans")]
    [SerializeField] private TextMeshProUGUI textUI;
    [SerializeField] private GameObject TextPanel;

    [Header("Yazý Ayarlarý")]
    [TextArea(3, 10)]
    [SerializeField] private string fullText;
    [SerializeField] private float letterDelay = 0.05f;

    [Header("Kapanma")]
    [SerializeField] private float closeDelay = 3f;

    public void StartGame()
    {
        textUI.text = "";
        StartCoroutine(TypeText());
    }

    private IEnumerator TypeText()
    {
        foreach (char c in fullText)
        {
            textUI.text += c;
            yield return new WaitForSeconds(letterDelay);
        }

        // Tüm yazý yazýldý
        yield return new WaitForSeconds(closeDelay);

        gameObject.SetActive(false);
        TextPanel.SetActive(false);
    }
}
