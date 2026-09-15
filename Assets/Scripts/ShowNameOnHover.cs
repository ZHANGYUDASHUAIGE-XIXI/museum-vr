using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.UI;

public class ShowNameOnHover : MonoBehaviour
{
    private bool mouseEnter = false;//判断鼠标是否进入文物
    private bool hasShown = false;  //判断文物详情是否显示
    private bool isChinese = true;  //判断当前是否播放中文音频
    private bool isFirstPlay = true;//判断当前是否为第一次播放音频

    public GameObject nameImage;    //名字UI
    public GameObject relicImage;   //文物图片UI
    public GameObject relicText;    //文物介绍文本UI
    public AudioClip chineseClip;   //中文音频
    public AudioClip englishClip;   //英文音频
    public GameObject playPauseBtn;     //播放暂停按钮
    public GameObject restartBtn;       //重头播放按钮
    public GameObject toggleLanguageBtn;//切换语言按钮

    private void Start()
    {
        nameImage.SetActive(false);
        relicImage.SetActive(false);
        relicText.SetActive(false);
        playPauseBtn.SetActive(false);
        restartBtn.SetActive(false);
        toggleLanguageBtn.SetActive(false);
    }

    private void Update()
    {
        if (mouseEnter && !hasShown)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                ShowRelicDetails(true);
                GameManager.Instance.DisablePlayer(true);
            }
        }

        if (hasShown)
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                ShowRelicDetails(false);
                GameManager.Instance.DisablePlayer(false);
                AudioManager.Instance.audioSource.Stop();
                AudioManager.Instance.audioSource.clip = null;
                isFirstPlay = true;
            }
        }
    }

    /// <summary>
    /// 检测鼠标进入
    /// </summary>
    private void OnMouseEnter()
    {
        nameImage.SetActive(true);
        mouseEnter = true;

        Debug.Log("检测鼠标进入");
    }

    /// <summary>
    /// 检测鼠标退出
    /// </summary>
    private void OnMouseExit()
    {
        nameImage.SetActive(false);
        mouseEnter = false;

        Debug.Log("检测鼠标退出");
    }

    /// <summary>
    /// 显示文物详情信息
    /// </summary>
    /// <param name="enter"></param>
    public void ShowRelicDetails(bool show)
    {
        relicImage.SetActive(show);
        relicText.SetActive(show);
        ShowBtn(show);
        hasShown = show;

        Debug.Log("显示文物详情信息");
    }

    /// <summary>
    /// 显示按钮
    /// </summary>
    public void ShowBtn(bool show)
    {
        playPauseBtn.SetActive(show);
        restartBtn.SetActive(show);
        toggleLanguageBtn.SetActive(show);
    }

    /// <summary>
    /// 播放或暂停音频
    /// </summary>
    public void PlayOrPauseAudio()
    {
        if (isFirstPlay)
        {
            AudioManager.Instance.audioSource.clip = chineseClip;
            isFirstPlay = false;
        }

        if (AudioManager.Instance.audioSource.isPlaying)
        {
            AudioManager.Instance.audioSource.Pause();
        }
        else
        {
            AudioManager.Instance.audioSource.Play();
        }
    }

    /// <summary>
    /// 从头播放音频
    /// </summary>
    public void RestartAudio()
    {
        AudioManager.Instance.audioSource.Stop();
        AudioManager.Instance.audioSource.Play();
    }

    /// <summary>
    /// 切换语言
    /// </summary>
    public void ToggleLanguage()
    {
        isChinese = !isChinese;

        if (isChinese)
        {
            AudioManager.Instance.audioSource.clip = chineseClip;
        }
        else
        {
            AudioManager.Instance.audioSource.clip = englishClip;
        }

        AudioManager.Instance.audioSource.Pause();
    }
}
