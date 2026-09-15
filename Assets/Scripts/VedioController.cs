using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VedioController : MonoBehaviour
{
    private VideoPlayer videoPlayer;

    public Button playOrPauseBtn;  //播放暂停按钮
    public Button restartBtn;      //重头播放按钮

    private void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();

        videoPlayer.Pause();
    }

    /// <summary>
    /// 播放或暂停
    /// </summary>
    public void PlayOrPause()
    {
        if (videoPlayer.isPlaying)
        {
            videoPlayer.Pause();
        }
        else
        {
            videoPlayer.Play();
        }
    }

    /// <summary>
    /// 重头播放
    /// </summary>
    public void Restart()
    {
        videoPlayer.Stop();
        videoPlayer.Play();
    }
}
