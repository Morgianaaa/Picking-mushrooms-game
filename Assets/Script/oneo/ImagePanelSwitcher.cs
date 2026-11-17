using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ImagePanelSwitcher : MonoBehaviour, IPointerClickHandler
{
    public Sprite[] images;
    public Image displayImage;
    private int currentIndex = 0;

    private void Start()
    {
        RefreshImage();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        RectTransform rectTransform = displayImage.rectTransform;
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, eventData.position, eventData.pressEventCamera, out localPoint);

        // 判断点击位置
        if (localPoint.x < 0)
            SwitchLeft();
        else
            SwitchRight();
    }

    private void SwitchLeft()
    {
        currentIndex = Mathf.Clamp(currentIndex - 1, 0, images.Length - 1);
        RefreshImage();
    }

    private void SwitchRight()
    {
        currentIndex = Mathf.Clamp(currentIndex + 1, 0, images.Length - 1);
        RefreshImage();
    }

    private void RefreshImage()
    {
        if (images.Length == 0) return;
        displayImage.sprite = images[currentIndex];
    }
}