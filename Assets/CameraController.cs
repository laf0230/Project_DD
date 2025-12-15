using Unity.Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    bool isMovable = true;
    CinemachineInputAxisController cinemachineInputAxis;

    private void Start()
    {
        cinemachineInputAxis = GetComponent<CinemachineInputAxisController>();
    }

    public void SetMovable(bool isMovable)
    {
        this.isMovable = isMovable;
        cinemachineInputAxis.enabled = this.isMovable;
    }

    public void SwitchMovable()
    {
        isMovable = !isMovable;
        cinemachineInputAxis.enabled = isMovable;
    }
}
