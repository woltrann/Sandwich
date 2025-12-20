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
        text.StartGame();
    }
}
