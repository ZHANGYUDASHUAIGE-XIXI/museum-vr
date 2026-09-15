using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 6f;        //玩家移动速度
    public float mouseSensitvity = 100f;//鼠标灵敏度
    private float rotationX = 0;
    private float rotationY = 0;
    public bool isDisabled = false;     //判断角色是否被禁用

    private CharacterController characterController;
    public GameObject playerCamera;  //玩家相机

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (isDisabled)
        {
            return;
        }

        PlayerMove();
        if (Input.GetMouseButton(0))
        {
            PlayerRotate();
        }
    }

    /// <summary>
    /// 控制玩家移动
    /// </summary>
    public void PlayerMove()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 dir = transform.right * horizontal + transform.forward * vertical;
        dir.Normalize();

        characterController.Move(dir * moveSpeed * Time.deltaTime);
    }

    /// <summary>
    /// 控制玩家旋转
    /// </summary>
    public void PlayerRotate()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        rotationX -= mouseY * mouseSensitvity * Time.deltaTime;
        rotationY += mouseX * mouseSensitvity * Time.deltaTime;

        rotationX = Mathf.Clamp(rotationX, -90f, 90f);

        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        transform.localRotation = Quaternion.Euler(0, rotationY, 0);
    }
}
