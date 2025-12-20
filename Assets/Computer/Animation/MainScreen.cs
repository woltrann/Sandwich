using UnityEngine;

public class MainScreen : MonoBehaviour
{
    public GameObject mainScreen;
    public GameObject loadingScreen;
    public TypewriterIntroText text;

    public void Hide()
    {
        mainScreen.SetActive(false);
        //loadingScreen.SetActive(true);
        text.ShowText("Yorucu bir mesaiden sonra stres atmalýk bir oyun oynasam iyi olur.");
    }
}
