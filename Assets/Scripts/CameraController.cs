using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;
    public float mouseSensitivity = 2f;
    float cameraVerticalRotation = 0f;

    private void Update()
    {
        //get mouse input
        float inputX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float inputY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        //translates inputY to a rotation and clamps the value between -90 and 90 deg
        cameraVerticalRotation -= inputY;
        cameraVerticalRotation = Mathf.Clamp(cameraVerticalRotation, -90f, 90f);

        //moves the camera up and down around the transform X axis
        transform.localEulerAngles = Vector3.right * cameraVerticalRotation;

        //rotates camera left and right around the transform Y axis
        player.Rotate(Vector3.up * inputX);
    }
}
