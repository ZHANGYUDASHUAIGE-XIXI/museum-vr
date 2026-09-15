using UnityEngine;
using UnityEngine.UI;

public class NavListController : MonoBehaviour
{
    [Header("===== 绑定导航管理器 =====")]
    public NavigationManager navigationManager;
    public Button[] relicButtons;        // 10个文物按钮

    private bool isListVisible = false;

    void Start()
    {
        // 只绑定文物按钮，取消按钮现在由NavigationManager统一管理
        for (int i = 0; i < relicButtons.Length; i++)
        {
            int index = i;
            relicButtons[i].onClick.AddListener(() => navigationManager.StartNavigation(index));
        }
    }

    void Update()
    {
        // Tab逻辑移到NavigationManager，本脚本不再处理Tab
    }

    /// <summary>外部调用：切换列表显隐（Tab触发）</summary>
    public void ToggleListVisibility()
    {
        isListVisible = !isListVisible;
        gameObject.SetActive(isListVisible);
    }

    /// <summary>外部调用：强制隐藏面板（点击文物时调用）</summary>
    public void HideNavList()
    {
        isListVisible = false;
        gameObject.SetActive(false);
    }
}