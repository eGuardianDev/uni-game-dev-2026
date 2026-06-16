
using UnityEngine;
using TMPro;

public class DamageTextFloat : MonoBehaviour
{
    [Header("Settings")]
    public float floatSpeed = 1.5f;
    public float fadeSpeed = 1.2f;
    public float lifetime = 0.8f;

    [SerializeField] private TMP_Text _tmp ;
    [SerializeField] private float _timer;

    void Awake()
    {
        _tmp = this.GetComponent<TMP_Text>();
        if (_tmp == null)
            _tmp = GetComponentInChildren<TMP_Text>(); // check children too
    }

    public void Init(int damage, Color color)
    {
        _tmp = this.GetComponent<TMP_Text>();
        if (_tmp == null)
            _tmp = GetComponentInChildren<TMP_Text>(); // check children too
  
        _tmp.text = damage.ToString();
        _tmp.color = color;
    }

    void Update()
    {
        _timer += Time.deltaTime;

        // Float up
        transform.position += Vector3.up * floatSpeed * Time.deltaTime;

        // Fade out
        Color c = _tmp.color;
        c.a = Mathf.Lerp(1f, 0f, _timer / lifetime);
        _tmp.color = c;

        if (_timer >= lifetime)
        {   
            Destroy(gameObject);
        }
    }
}
