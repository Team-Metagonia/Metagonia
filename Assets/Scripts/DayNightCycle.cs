using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    public Light directionalLight;
    public Material daySkybox;
    public Material nightSkybox;
    public float dayDuration = 60f; // 하루의 길이 (초 단위)

    private float time;

    void Start()
    {
        // 초기 Skybox 설정
        RenderSettings.skybox = daySkybox;
        daySkybox.SetFloat("_Blend", 0f);
        nightSkybox.SetFloat("_Blend", 1f);
    }

    void Update()
    {
        // 시간 업데이트
        time += Time.deltaTime / dayDuration;
        if (time > 1f) time -= 1f;

        // 라이트의 회전 업데이트
        directionalLight.transform.localRotation = Quaternion.Euler(new Vector3((time * 360f) - 90f, 170f, 0));

        // 라이트의 각도 계산
        float lightAngle = directionalLight.transform.eulerAngles.x;

        // Skybox 전환
        if (lightAngle > 0 && lightAngle < 180) // 낮
        {
            RenderSettings.skybox = daySkybox;
        }
        else // 밤
        {
            RenderSettings.skybox = nightSkybox;
        }

        // Skybox 블렌드 업데이트 (자연스러운 전환을 위해 사용)
        float blendFactor = Mathf.Clamp01((lightAngle - 90f) / 90f);
        daySkybox.SetFloat("_Blend", 1f - blendFactor);
        nightSkybox.SetFloat("_Blend", blendFactor);
    }
}