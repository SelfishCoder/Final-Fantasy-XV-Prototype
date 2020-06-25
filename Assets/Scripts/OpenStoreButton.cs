using UnityEngine;

[DisallowMultipleComponent]
public class OpenStoreButton : MonoBehaviour
{
    public void OpenStore()
    {
        Application.OpenURL("https://play.google.com/store/apps/details?id=com.epicactiononline.ffxv.ane&hl=tr");
    }
}