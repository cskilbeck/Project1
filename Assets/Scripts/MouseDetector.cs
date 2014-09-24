using UnityEngine;

class MouseDetector : MonoBehaviour
{
    public iMouseEnabled parent;

    void Start()
    {
    }

    void Update()
    {
    }

    void OnMouseDown()
    {
        parent.OnMouseDown();
    }

    void OnMouseUp()
    {
        parent.OnMouseUp();
    }

    void OnMouseDrag()
    {
        parent.OnMouseDrag();
    }

}
