using StarterAssets;
using Unity.Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    bool isMovable = true;
    CinemachineInputAxisController cinemachineInputAxis;
    StarterAssetsInputs pointerMoveable;

    private void Start()
    {
        cinemachineInputAxis = GetComponent<CinemachineInputAxisController>();
        pointerMoveable = GetComponent<StarterAssetsInputs>();
    }

    public void SetMovable(bool isMovable)
    {
        this.isMovable = isMovable;
        if (cinemachineInputAxis != null)
            cinemachineInputAxis.enabled = this.isMovable;
        if (pointerMoveable != null)
            pointerMoveable.cursorInputForLook = this.isMovable;
    }

    public void SetMouseLock(bool isLock)
    {
        if(pointerMoveable != null)
        {
            pointerMoveable.CursorLocked = isLock;
            return;
        }

        Debug.Log("[CameraController] pointerMovable is null");
    }

    public void SwitchMovable()
    {
        isMovable = !isMovable;
        cinemachineInputAxis.enabled = isMovable;
    }
}
