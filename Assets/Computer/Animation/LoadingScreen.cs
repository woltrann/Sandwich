using UnityEngine;

public class LoadingScreen : MonoBehaviour
{
    public GameObject loadingScreen;

    public void Hide()
    {
        loadingScreen.SetActive(false);
    }
}
