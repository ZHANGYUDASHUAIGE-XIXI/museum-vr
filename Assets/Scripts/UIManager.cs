using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public GameObject startMenuPanel;   //开始菜单面板
    public GameObject miniMap;          //小地图

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        miniMap.SetActive(false);
    }

    public void StartGame()
    {
        startMenuPanel.SetActive(false);
        GameManager.Instance.DisablePlayer(false);
        miniMap.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
