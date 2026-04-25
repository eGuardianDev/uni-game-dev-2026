using UnityEngine;
using UnityEngine.EventSystems;

public class CursorChange : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private ModeOfCursor modeOfCursor;

    public void OnPointerEnter(PointerEventData eventData)
    {
        CursorPositioning.Instance.activate_global_cursor_detection = false;
        CursorPositioning.Instance.SetToMode(modeOfCursor);
    }

    public void OnPointerExit(PointerEventData  eventData)
    {
        CursorPositioning.Instance.activate_global_cursor_detection = true;
        CursorPositioning.Instance.SetToMode(ModeOfCursor.Default);
    }

    // private void OnMouseEnter()
    // {
    //     CursorPositioning.Instance.SetToMode(modeOfCursor);
    // }

    // private void OnMouseExit()
    // {
    //     CursorPositioning.Instance.SetToMode(ModeOfCursor.Default);
    // }

}
