using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public static float lookSpeedH = 2f;
    public static float lookSpeedV = 2f;

    private float yaw;
    private float pitch;


    private void Start()
    {
        yaw = transform.eulerAngles.y;
        pitch = transform.eulerAngles.x;
    }

    void Update()
    {

        yaw += lookSpeedH * Input.GetAxis("Mouse X");
        pitch -= lookSpeedV * Input.GetAxis("Mouse Y");

        transform.eulerAngles = new Vector3(pitch, yaw, 0f);

    }
}
