using UnityEngine;
using UnityEngine.UI;

public class NavigationManager : MonoBehaviour
{
    [Header("===== 导航核心配置 =====")]
    public Transform playerTrans;
    //直线渲染器
    public LineRenderer navLineRenderer;
    public float lineYOffset = 0.05f;
    public float arriveJudgeDistance = 5f; // 到达目标判定距离，可自行调整

    
    [System.Serializable]
    public class RelicPathData
    {
        public Transform targetRelic;       // 目标文物
        public Transform[] pathPoints;     // 沿途路径点（沿走廊摆放，避开墙体）
    }
    public RelicPathData[] relicPathDatas; // 10个文物的路径数据，和按钮顺序一一对应

    [Header("===== UI绑定 =====")]
    public NavListController navListController;
    public Button globalCancelNavBtn; // 独立的取消导航按钮（已经移出列表面板）

    private Transform currentTarget;
    private bool isNavigating = false;
    private RelicPathData currentPathData;

    void Start()
    {
        navLineRenderer.positionCount = 0;
        navLineRenderer.enabled = false;
        navLineRenderer.material = new Material(Shader.Find("Unlit/Color"));
        navLineRenderer.material.color = Color.green;
        navLineRenderer.startWidth = 0.5f;
        navLineRenderer.endWidth = 0.1f;

        // 绑定取消按钮点击事件
        globalCancelNavBtn.onClick.AddListener(CancelNavigation);
        globalCancelNavBtn.gameObject.SetActive(false); // 初始隐藏取消按钮
    }

    void Update()
    {
        // Tab切换列表显示（只控制文物列表，不碰取消按钮）
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            navListController.ToggleListVisibility();
        }

        if (isNavigating && currentTarget != null)
        {
            UpdateNavLine();
            // 距离达标自动结束导航
            float distance = Vector3.Distance(playerTrans.position, currentTarget.position);
            if (distance <= arriveJudgeDistance)
            {
                CancelNavigation();
            }
        }
    }

    /// <summary>点击文物按钮启动导航</summary>
    public void StartNavigation(int relicIndex)
    {
        if (relicIndex < 0 || relicIndex >= relicPathDatas.Length) return;

        currentPathData = relicPathDatas[relicIndex];
        currentTarget = currentPathData.targetRelic;
        isNavigating = true;

        // 【需求1：点击文物 → 隐藏导航列表面板】
        navListController.HideNavList();
        // 【需求2：开启取消导航按钮】
        globalCancelNavBtn.gameObject.SetActive(true);

        // 路线点数 = 起点(玩家)+所有路径点+终点(文物)
        int totalPointCount = 1 + currentPathData.pathPoints.Length + 1;
        navLineRenderer.enabled = true;
        navLineRenderer.positionCount = totalPointCount;
        UpdateNavLine();
    }

    /// <summary>手动取消导航</summary>
    public void CancelNavigation()
    {
        currentTarget = null;
        currentPathData = null;
        isNavigating = false;
        navLineRenderer.enabled = false;
        navLineRenderer.positionCount = 0;
        // 导航结束 → 隐藏取消按钮
        globalCancelNavBtn.gameObject.SetActive(false);
    }

    /// <summary>按路径点逐段生成导航线，不再穿墙直线</summary>
    private void UpdateNavLine()
    {
        // 0号点位：玩家位置
        Vector3 playerPos = playerTrans.position;
        playerPos.y = lineYOffset;
        navLineRenderer.SetPosition(0, playerPos);

        // 中间点位：预设路径点
        for (int i = 0; i < currentPathData.pathPoints.Length; i++)
        {
            Vector3 pointPos = currentPathData.pathPoints[i].position;
            pointPos.y = lineYOffset;
            navLineRenderer.SetPosition(i + 1, pointPos);
        }

        // 最后点位：目标文物
        Vector3 targetPos = currentTarget.position;
        targetPos.y = lineYOffset;
        navLineRenderer.SetPosition(currentPathData.pathPoints.Length + 1, targetPos);
    }
}