using System.Collections;
using UnityEngine;
using TMPro;

[CreateAssetMenu(menuName = "UI/Congrats Config")]
public class CongratsConfigSO : ScriptableObject
{
    public int triggerPlanetIndex = 1;
    public GameObject popupPrefab;
    public ParticleSystem fireworksPrefab;
    public float fireworksDuration = 3f;
    [TextArea] public string messageFormat = "";
    private string GetPrefsKey(int planetIndex) => $"CongratsShown_{planetIndex}";


    public void ShowCongrats(MonoBehaviour host, Transform parent, string subject, int planetIndex)
    {
        string key = GetPrefsKey(planetIndex);
        if (PlayerPrefs.GetInt(key, 0) == 1) return;
        PlayerPrefs.SetInt(key, 1);
        PlayerPrefs.Save();

        var popup = Instantiate(popupPrefab, parent);
        var label = popup.GetComponentInChildren<TextMeshProUGUI>();
        label.text = /*string.Format(*/messageFormat /*+ subject, subject)*/;

        var fx = Instantiate(fireworksPrefab, parent);
        fx.Play();

        host.StartCoroutine(HideAfterDelay(host, popup, fx));
    }

    private IEnumerator HideAfterDelay(MonoBehaviour host, GameObject popup, ParticleSystem fx)
    {
        yield return new WaitForSeconds(fireworksDuration);
        fx.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        Destroy(fx.gameObject, fx.main.startLifetime.constantMax);
        Destroy(popup);
    }
}