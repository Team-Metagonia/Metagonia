using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    public Light directionalLight;
    public Material[] daySkyboxes;
    public Material sunsetSkybox;
    public Material[] nightSkyboxes;
    public float dayDuration = 60f; // 하루의 길이 (초 단위)
    public float transitionDuration = 10f; // 전환 길이 (초 단위)

    private float time;
    private Material currentDaySkybox;
    private Material currentNightSkybox;
    private Material targetSkybox;
    private Material previousSkybox;
    private float blendFactor;
    private float transitionProgress;

    void Start()
    {
        // 초기 Skybox 설정
        SetRandomDaySkybox();
        SetRandomNightSkybox();
        RenderSettings.skybox = currentDaySkybox;
        currentDaySkybox.SetFloat("_Blend", 0f);
        sunsetSkybox.SetFloat("_Blend", 0f);
        currentNightSkybox.SetFloat("_Blend", 1f);
        targetSkybox = currentDaySkybox;
        previousSkybox = currentDaySkybox;
        transitionProgress = 0f;
    }

    void Update()
    {
        // 시간 업데이트
        time += Time.deltaTime / dayDuration;
        if (time > 1f)
        {
            time -= 1f;
        }

        // 라이트의 회전 업데이트
        directionalLight.transform.localRotation = Quaternion.Euler(new Vector3((time * 360f) - 90f, 170f, 0));

        // 라이트의 각도 계산
        float lightAngle = directionalLight.transform.eulerAngles.x;

        // 스카이박스 전환 및 블렌딩
        if ((lightAngle >= 350 || lightAngle <= 10) || (lightAngle >= 170 && lightAngle <= 190)) // 노을
        {
            if (RenderSettings.skybox != sunsetSkybox)
            {
                previousSkybox = RenderSettings.skybox;
                targetSkybox = sunsetSkybox;
                transitionProgress = 0f;
            }
            blendFactor = Mathf.Abs(lightAngle - 180) / 10f; // 블렌딩 계수
            sunsetSkybox.SetFloat("_Blend", blendFactor);
            RenderSettings.skybox = sunsetSkybox;

            // 조명 강도와 색상 조절
            directionalLight.intensity = Mathf.Lerp(0.2f, 1f, blendFactor);
            directionalLight.color = Color.Lerp(Color.red, Color.white, blendFactor);
        }
        else if (lightAngle > 10 && lightAngle < 170) // 낮
        {
            if (RenderSettings.skybox != currentDaySkybox)
            {
                SetRandomDaySkybox();
                previousSkybox = RenderSettings.skybox;
                targetSkybox = currentDaySkybox;
                transitionProgress = 0f;
            }
            blendFactor = Mathf.Abs(lightAngle - 90) / 80f; // 블렌딩 계수
            currentDaySkybox.SetFloat("_Blend", blendFactor);
            RenderSettings.skybox = currentDaySkybox;

            // 조명 강도와 색상 조절
            directionalLight.intensity = 1f;
            directionalLight.color = Color.white;
        }
        else if (lightAngle > 190 && lightAngle < 350) // 밤
        {
            if (RenderSettings.skybox != currentNightSkybox)
            {
                SetRandomNightSkybox();
                previousSkybox = RenderSettings.skybox;
                targetSkybox = currentNightSkybox;
                transitionProgress = 0f;
            }
            blendFactor = Mathf.Abs(lightAngle - 270) / 80f; // 블렌딩 계수
            currentNightSkybox.SetFloat("_Blend", blendFactor);
            RenderSettings.skybox = currentNightSkybox;

            // 조명 강도와 색상 조절
            directionalLight.intensity = 0.2f;
            directionalLight.color = Color.blue;
        }

        // Skybox 회전
        float rotationAngle = time * 360f;
        foreach (Material skybox in daySkyboxes)
        {
            skybox.SetFloat("_Rotation", rotationAngle);
        }
        sunsetSkybox.SetFloat("_Rotation", rotationAngle);
        foreach (Material skybox in nightSkyboxes)
        {
            skybox.SetFloat("_Rotation", rotationAngle);
        }

        // 블렌딩 업데이트
        transitionProgress += Time.deltaTime / transitionDuration;
        float blendFactorTransition = Mathf.Clamp01(transitionProgress);
        RenderSettings.skybox.Lerp(previousSkybox, targetSkybox, blendFactorTransition);
    }

    void SetRandomDaySkybox()
    {
        int randomIndex = Random.Range(0, daySkyboxes.Length);
        currentDaySkybox = daySkyboxes[randomIndex];
    }

    void SetRandomNightSkybox()
    {
        int randomIndex = Random.Range(0, nightSkyboxes.Length);
        currentNightSkybox = nightSkyboxes[randomIndex];
    }
}
