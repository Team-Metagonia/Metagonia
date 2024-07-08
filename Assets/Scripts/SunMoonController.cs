using UnityEngine;

public class SunMoonController : MonoBehaviour
{
    public Light sunLight;
    public Light moonLight;
    public float duration = 10.0f; // 낮과 밤 전환 시간

    private float currentTime = 0f;

    void Update()
    {
        float t = currentTime / duration;
        sunLight.intensity = Mathf.Lerp(1, 0, t);
        moonLight.intensity = Mathf.Lerp(0, 1, t);
    }

    public void SetDayNightTransition(float value)
    {
        currentTime = value * duration;
    }
}