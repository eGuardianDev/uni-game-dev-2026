using UnityEngine;
using System.Collections;

public class LazerAll : Ability
{
    [Header("Laser Settings")]
    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private int directionCount = 6;
    [SerializeField] private LayerMask hitLayers;

    [Header("References")]
    [SerializeField] private LineRenderer lineRendererPrefab;

    private LineRenderer[] lineRenderers;
    private Coroutine disableCoroutine;

    void Start()
    {
        // Spawn a LineRenderer for each direction
        lineRenderers = new LineRenderer[directionCount];
        for (int i = 0; i < directionCount; i++)
        {
            lineRenderers[i] = Instantiate(lineRendererPrefab, transform);
            lineRenderers[i].positionCount = 2;
            lineRenderers[i].enabled = false;
        }
    }


    protected override bool isCastable() => true;

    protected override bool OnCast()
    {
        if (player_script_ == null) return false;

        Vector2 origin = player_script_.transform.position;

        for (int i = 0; i < directionCount; i++)
        {
            float angle = i * (360f / directionCount);
            Vector2 direction = new Vector2(
                Mathf.Cos(angle * Mathf.Deg2Rad),
                Mathf.Sin(angle * Mathf.Deg2Rad)
            );

            FireSingleLaser(lineRenderers[i], origin, direction);
        }

        if (disableCoroutine != null) StopCoroutine(disableCoroutine);
        disableCoroutine = StartCoroutine(FadeAndDisable());

        return true;
    }
    private void FireSingleLaser(LineRenderer lr, Vector2 origin, Vector2 direction)
    {
        lr.enabled = true;
        lr.SetPosition(0, origin);

        float radius = 0.5f;
        RaycastHit2D[] hits = Physics2D.CircleCastAll(origin, radius, direction, Range, hitLayers);
        System.Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.TryGetComponent<Enemy>(out var enemy))
                enemy.GetDamage(Damage);
        }

        lr.SetPosition(1, hits.Length > 0
            ? hits[hits.Length - 1].point
            : origin + direction * Range);
    }
    private IEnumerator FadeAndDisable()
    {
        yield return new WaitForSeconds(1f);

        float elapsed = 0f;
        float startWidth = lineRenderers[0].startWidth;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float t = 1f - (elapsed / fadeDuration);
            foreach (var lr in lineRenderers)
            {
                lr.startWidth = startWidth * t;
                lr.endWidth = startWidth * t;
            }
            yield return null;
        }

        foreach (var lr in lineRenderers)
        {
            lr.enabled = false;
            lr.startWidth = startWidth;
            lr.endWidth = startWidth;
        }
    }
}