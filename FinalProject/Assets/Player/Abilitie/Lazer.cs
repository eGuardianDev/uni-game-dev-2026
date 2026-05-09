using UnityEngine;
using System.Collections;
public class Lazer : Ability
{
    [Header("Laser Settings")]
    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private LayerMask hitLayers;

    [Header("References")]
    [SerializeField] private LineRenderer lineRenderer;

    private Camera cam;
    private Coroutine disableCoroutine;

    void Start()
    {
        lineRenderer.enabled = false;
    }


    protected override bool isCastable()
    {
        return true; // or add mana check etc.
    }

    protected override bool OnCast()
    {
        if (player_script_ == null || lineRenderer == null) return false;

        cam = Camera.main;

        Vector3 mouseWorld = cam.ScreenToWorldPoint(Input.mousePosition);
        mouseWorld.z = 0f;

        Vector2 origin = player_script_.transform.position;
        Vector2 direction = ((Vector2)mouseWorld - origin).normalized;

        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, origin);

        RaycastHit2D[] hits = Physics2D.RaycastAll(origin, direction, Range, hitLayers);

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.TryGetComponent<Enemy>(out var enemy))
                enemy.GetDamage(Damage);
        }

        // Set laser end point to last hit or max range
        lineRenderer.SetPosition(1, hits.Length > 0 
            ? hits[hits.Length - 1].point 
            : origin + direction * Range);

        // Restart fade timer
        if (disableCoroutine != null) StopCoroutine(disableCoroutine);
        disableCoroutine = StartCoroutine(FadeAndDisable());

        return true;
    }

    private IEnumerator FadeAndDisable()
    {
        yield return new WaitForSeconds(1f);

        float elapsed = 0f;
        float startWidth = lineRenderer.startWidth;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float t = 1f - (elapsed / fadeDuration);
            lineRenderer.startWidth = startWidth * t;
            lineRenderer.endWidth = startWidth * t;
            yield return null;
        }

        lineRenderer.enabled = false;
        lineRenderer.startWidth = startWidth;
        lineRenderer.endWidth = startWidth;
    }
}
