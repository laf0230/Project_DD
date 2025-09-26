using UnityEngine;
using UnityEngine.UIElements;

public class ResizablePanel : MouseManipulator
{
    private VisualElement targetPanel;
    private VisualElement handle;
    private bool dragging = false;
    private float startY;
    private float startHeight;

    public ResizablePanel(VisualElement panel, VisualElement handle)
    {
        targetPanel = panel;
        this.handle = handle;

        // 이벤트 등록
        this.handle.RegisterCallback<MouseDownEvent>(OnMouseDown);
        this.handle.RegisterCallback<MouseMoveEvent>(OnMouseMove);
        this.handle.RegisterCallback<MouseUpEvent>(OnMouseUp);
    }

    protected override void RegisterCallbacksOnTarget()
    {
        throw new System.NotImplementedException();
    }

    protected override void UnregisterCallbacksFromTarget()
    {
        throw new System.NotImplementedException();
    }

    private void OnMouseDown(MouseDownEvent evt)
    {
        if (evt.button == 0)
        {
            dragging = true;
            startY = evt.mousePosition.y;
            startHeight = targetPanel.resolvedStyle.height;
            handle.CaptureMouse();
            evt.StopPropagation();
        }
    }

    private void OnMouseMove(MouseMoveEvent evt)
    {
        if (!dragging) return;
        float delta = evt.mousePosition.y - startY;
        float newHeight = Mathf.Max(50, startHeight + delta); // 최소 높이 50
        targetPanel.style.height = newHeight;
    }

    private void OnMouseUp(MouseUpEvent evt)
    {
        if (dragging && evt.button == 0)
        {
            dragging = false;
            handle.ReleaseMouse();
        }
    }
}
