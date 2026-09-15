using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public PlayerController playerController;

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
        DisablePlayer(true);
    }

    /// <summary>
    /// 禁用玩家
    /// </summary>
    /// <param name="disable"></param>
    public void DisablePlayer(bool disable)
    {
        playerController.isDisabled = disable;
    }
}
