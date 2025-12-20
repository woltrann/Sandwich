using System.Collections;
using TMPro;
using UnityEngine;

public class TypewriterIntroText: MonoBehaviour
{
    public static TypewriterIntroText Instance;

    [Header("Referans")]
    [SerializeField] private TextMeshProUGUI textUI;
    [SerializeField] private GameObject textPanel;

    [Header("Ayarlar")]
    [SerializeField] private float letterDelay = 0.05f;
    [SerializeField] private float closeDelay = 3f;

    private Coroutine currentCoroutine;

    private void Awake()
    {
        Instance = this;
        //gameObject.SetActive(false);
    }


    public void ShowText(string textToWrite)
    {
        if (currentCoroutine != null)
            StopCoroutine(currentCoroutine);
        textPanel.SetActive(true);
        //gameObject.SetActive(true);
        textUI.text = "";

        currentCoroutine = StartCoroutine(TypeText(textToWrite));
    }

    private IEnumerator TypeText(string text)
    {
        foreach (char c in text)
        {
            textUI.text += c;
            yield return new WaitForSeconds(letterDelay);
        }

        yield return new WaitForSeconds(closeDelay);
        //gameObject.SetActive(false);
        textPanel.SetActive(false);
    }
}
