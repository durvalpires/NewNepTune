using UnityEngine;
using UnityEngine.UI;

public class GradientBackground : MonoBehaviour
{
    public Image gradientImage;
    public Color leftColor = Color.red;
    public Color rightColor = Color.blue;
    public float fillSpeed = 0.1f;
    private float fillAmount = 0f;

    void Start()
    {
        if (gradientImage != null && gradientImage.material != null)
        {
            gradientImage.material.SetColor("_LeftColor", leftColor);
            gradientImage.material.SetColor("_RightColor", rightColor);
            gradientImage.material.SetFloat("_FillAmount", fillAmount);
        }
    }

    void Update()
    {
        if (gradientImage != null && gradientImage.material != null)
        {
            fillAmount += fillSpeed * Time.deltaTime;
            fillAmount = Mathf.Clamp01(fillAmount);
            gradientImage.material.SetFloat("_FillAmount", fillAmount);
        }
    }
}