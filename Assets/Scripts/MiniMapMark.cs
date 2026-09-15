using UnityEngine;
using UnityEngine.UI;

public class MiniMapMark : MonoBehaviour
{
    [Header("拖拽赋值")]
    public Camera miniMapCam;    // 小地图相机
    public Transform playerTrans; // 你的第一人称主体(主相机/角色)

    private RectTransform mapContentRect; // 小地图UI区域

    void Start()
    {
        // 获取父物体RectTransform
        mapContentRect = GetComponentInParent<RectTransform>();
    }

    void Update()
    {
        if (miniMapCam == null || playerTrans == null || mapContentRect == null)
            return;

        // 1. 将玩家3D世界坐标 → 小地图相机视口坐标(0~1范围)
        Vector3 viewPos = miniMapCam.WorldToViewportPoint(playerTrans.position);

        // 2. 视口坐标 → UI像素坐标（适配小地图区域）
        Vector2 uiPos = new Vector2(
            viewPos.x * mapContentRect.rect.width - mapContentRect.rect.width / 2f,
            viewPos.y * mapContentRect.rect.height - mapContentRect.rect.height / 2f
        );

        // 3. 更新标记位置
        GetComponent<RectTransform>().anchoredPosition = uiPos;

        // 4. 同步玩家旋转（箭头朝向）
        transform.rotation = Quaternion.Euler(0, 0, -playerTrans.eulerAngles.y);

        // 5. 小地图相机始终在角色上方
        Vector3 targetPos = playerTrans.position;
        targetPos.y = miniMapCam.transform.position.y;
        miniMapCam.transform.position = targetPos;
    }
}
