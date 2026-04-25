using UnityEngine;

public class CursorPositioning : MonoBehaviour
{
    public static CursorPositioning Instance { get; private set; }

    [SerializeField] private Sprite cursorSpriteDefault;
    [SerializeField] private Sprite cursorSpriteSelect;
    [SerializeField] private Sprite cursorSpriteAttack;
    [SerializeField] private int Scale_Cursor = 4;

    [SerializeField] private Vector2 clickPosition = Vector2.zero;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private LayerMask selectableLayer;
    [SerializeField] private Camera _cam;
    
    [SerializeField] private Transform player_move_to_;

    [SerializeField] private PlayerScript player_script_;

    public bool activate_global_cursor_detection = true;
    private static Texture2D ExtractTexture(Sprite sprite, int scale = 4)
    {
        Rect r = sprite.textureRect;
        Color[] pixels = sprite.texture.GetPixels(
            (int)r.x, (int)r.y,
            (int)r.width, (int)r.height
        );

        int srcW = (int)r.width;
        int srcH = (int)r.height;
        int dstW = srcW * scale;
        int dstH = srcH * scale;

        Color[] scaled = new Color[dstW * dstH];
        for (int y = 0; y < dstH; y++)
            for (int x = 0; x < dstW; x++)
                scaled[y * dstW + x] = pixels[(y / scale) * srcW + (x / scale)];

        // Explicit RGBA32, no mip chain (mipmapCount = false)
        var tex = new Texture2D(dstW, dstH, TextureFormat.RGBA32, false);
        tex.filterMode = FilterMode.Point;
        tex.alphaIsTransparency = true;   // satisfies the cursor requirement
        tex.SetPixels(scaled);
        tex.Apply();
        return tex;
    }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    Texture2D tex_default_;
    Texture2D tex_select_;
    Texture2D tex_attack_;
    void Start()
    {
        tex_default_ = ExtractTexture(cursorSpriteDefault, Scale_Cursor);
        tex_select_ = ExtractTexture(cursorSpriteSelect, Scale_Cursor);
        tex_attack_ = ExtractTexture(cursorSpriteAttack, Scale_Cursor);
        
        Cursor.SetCursor(ExtractTexture(cursorSpriteDefault, Scale_Cursor),
                                             clickPosition, CursorMode.Auto);
    }

    public void SetToMode(ModeOfCursor modeOfCursor)
    {
        Texture2D tex;
        switch (modeOfCursor)
        {
            case ModeOfCursor.Default:
                tex = tex_default_;
                break;
            case ModeOfCursor.Select:
                tex = tex_select_;
                break;
            case ModeOfCursor.Attack:
                tex = tex_attack_;
                break;
            default:
                tex = tex_default_;
                break;
        }
        Cursor.SetCursor(tex, clickPosition, CursorMode.Auto);
    }


    [SerializeField] private GameObject hovered_object_ = null;
    void Update()
    {
        Vector2 worldPos = _cam.ScreenToWorldPoint(Input.mousePosition);
       
        if(!activate_global_cursor_detection) return; 

        RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero, 0f);
        if (hit.collider != null && ((1 << hit.collider.gameObject.layer) & enemyLayer) != 0)
        {
            SetToMode(ModeOfCursor.Attack);
        }
        else if(hit.collider != null && ((1 << hit.collider.gameObject.layer) & selectableLayer) != 0)
        {
            SetToMode(ModeOfCursor.Select);
        }
        else
        {
            SetToMode(ModeOfCursor.Default);
        }

        if(hit.collider != null)
        {
            hovered_object_ = hit.collider.gameObject;
        }else hovered_object_ = null;

        if (Input.GetMouseButton(1))
        {
            player_move_to_.position = worldPos;

            if (hovered_object_.CompareTag("Enemy"))
                player_script_.enemy_ = hovered_object_.GetComponent<Enemy>();
            else player_script_.enemy_ = null;
        }

    }
}

public enum ModeOfCursor
{
    Default,
    Select,
    Attack,
}