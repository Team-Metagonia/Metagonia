using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SliderHandler : MonoBehaviour, IBeginDragHandler, IEndDragHandler
{
    public Slider slider;
    public SunMoonController sunMoonController;

    void Start()
    {
        slider.onValueChanged.AddListener(OnSliderValueChanged);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // 드래그 시작 시 커서 설정
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // 드래그 종료 시 커서 설정
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void OnSliderValueChanged(float value)
    {
        sunMoonController.SetDayNightTransition(value);
    }
}