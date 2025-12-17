using System;
using UnityEngine;

[AddComponentMenu("상호작용 오브젝트/문")]
public class SimpleDoor : MonoBehaviour
{
    [Serializable]
    class CustomeTransform
    {
        public Vector3 position;
        public Quaternion rotation;
    }

    [Header("오른쪽 위에 ...을 눌러서 간편하게 위치를 지정\n 열렸을 때 위치")]
    [SerializeField] private CustomeTransform openedPosition;
    [Header("닫혔을 때 위치")]
    [SerializeField] private CustomeTransform closedPosition;
    
    [ContextMenu("Set Closed Position: 닫혔을 때 위치에서 누르기")]
    public void SetClosedPosition()
    {
        closedPosition.rotation = transform.rotation;
        closedPosition.position = transform.position;
    }

    [ContextMenu("Set Opened Position: 열렸을 때 위치에서 누르기")]
    public void SetOpenedPosition()
    {
        openedPosition.rotation = transform.rotation;
        openedPosition.position = transform.position;
    }

    public void Switch()
    {
        if (transform.position == closedPosition.position && transform.rotation == closedPosition.rotation)
        {
            transform.position = openedPosition.position;
            transform.rotation = openedPosition.rotation;
        }
        else
        {
            transform.position = closedPosition.position;
            transform.rotation = closedPosition.rotation;
        }
    }
}
