using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    [SerializeField] private float edgeThreshold = 10f; // pixels from edge
    [SerializeField] public Transform LockedOnRoom_; // pixels from edge

    [System.Flags]
    public enum ScreenEdge
    {
        None   = 0,
        Left   = 1 << 0,
        Right  = 1 << 1,
        Bottom = 1 << 2,
        Top    = 1 << 3,
    }

    [SerializeField] private Transform target;
    [SerializeField] private float maxY_;
    [SerializeField] private float maxX_;
    [SerializeField] private float smoothSpeed_ = 15;

    [SerializeField] private bool followPlayer_;


    private ScreenEdge edge_cursor_detect_;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            followPlayer_ = !followPlayer_; 
        }


        edge_cursor_detect_ = GetEdge();
      
    }
    public ScreenEdge GetEdge()
    {
        Vector3 mouse = Input.mousePosition;
        ScreenEdge result = ScreenEdge.None;

        if (mouse.x <= edgeThreshold)                   result |= ScreenEdge.Left;
        if (mouse.x >= Screen.width - edgeThreshold)    result |= ScreenEdge.Right;
        if (mouse.y <= edgeThreshold)                   result |= ScreenEdge.Bottom;
        if (mouse.y >= Screen.height - edgeThreshold)   result |= ScreenEdge.Top;

        return result;
    }

    void LateUpdate()
    {
        Vector3 pos = transform.position;

        if (followPlayer_)
        {
            float targetY = Mathf.Clamp(target.position.y, LockedOnRoom_.position.y-maxY_, LockedOnRoom_.position.y+maxY_);
            float targetX = Mathf.Clamp(target.position.x, LockedOnRoom_.position.x-maxX_, LockedOnRoom_.position.x+maxX_);
   
            pos.y = Mathf.Lerp(pos.y, targetY, Time.deltaTime * smoothSpeed_);
            pos.x = Mathf.Lerp(pos.x, targetX, Time.deltaTime * smoothSpeed_);
        }
        else if (edge_cursor_detect_ != ScreenEdge.None)
        {
            if (edge_cursor_detect_.HasFlag(ScreenEdge.Left))   pos.x -= smoothSpeed_ * Time.deltaTime;
            if (edge_cursor_detect_.HasFlag(ScreenEdge.Right))  pos.x += smoothSpeed_ * Time.deltaTime;
            if (edge_cursor_detect_.HasFlag(ScreenEdge.Bottom)) pos.y -= smoothSpeed_ * Time.deltaTime;
            if (edge_cursor_detect_.HasFlag(ScreenEdge.Top))    pos.y += smoothSpeed_ * Time.deltaTime;
      
      
            pos.x = Mathf.Clamp(pos.x, LockedOnRoom_.position.x-maxX_, LockedOnRoom_.position.x+maxX_);
            pos.y = Mathf.Clamp(pos.y, LockedOnRoom_.position.y-maxY_, LockedOnRoom_.position.y+maxY_);
        }
        else
        {
            pos.x += Input.GetAxisRaw("Horizontal") * smoothSpeed_ * Time.deltaTime;
            pos.y += Input.GetAxisRaw("Vertical") * smoothSpeed_ * Time.deltaTime;
      
            pos.y = Mathf.Clamp(pos.y, LockedOnRoom_.position.y-maxY_, LockedOnRoom_.position.y+maxY_);
            pos.x = Mathf.Clamp(pos.x, LockedOnRoom_.position.x-maxX_, LockedOnRoom_.position.x+maxX_);
        
        }

        transform.position = pos;

    }
}
