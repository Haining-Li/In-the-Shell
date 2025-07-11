using UnityEngine;
using UnityEngine.UI;

public class UICrosshairController : MonoBehaviour
{
    [SerializeField] private RectTransform crosshairRect;
    [SerializeField] private Canvas parentCanvas;
    [SerializeField] private float offsetX = 0f;
    [SerializeField] private float offsetY = 0f;

    private void Start()
    {
        Cursor.visible = false;

        // 如果未手动赋值，尝试自动获取
        if (crosshairRect == null) crosshairRect = GetComponent<RectTransform>();
        if (parentCanvas == null) parentCanvas = GetComponentInParent<Canvas>();
    }

    private void Update()
    {
        Vector2 mousePos = Input.mousePosition;

        // 转换鼠标位置到Canvas空间
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            parentCanvas.transform as RectTransform,
            mousePos,
            parentCanvas.worldCamera,
            out localPoint);

        // 应用位置和偏移
        crosshairRect.localPosition = new Vector3(
            localPoint.x + offsetX,
            localPoint.y + offsetY,
            0f);
    }

    // 可选：添加显示/隐藏准星的方法
    public void ShowCrosshair(bool show)
    {
        gameObject.SetActive(show);
        Cursor.visible = !show;
    }

}
