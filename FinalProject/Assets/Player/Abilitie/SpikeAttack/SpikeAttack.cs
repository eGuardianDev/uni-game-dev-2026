using UnityEngine;

public class SpikeAttack : Ability
{

    [SerializeField] private GameObject spikes;
    [SerializeField] private int count;
    [SerializeField] private float radius;
    protected override bool isCastable()
    {
        return true; 
    }

    protected override bool OnCast()
    {
        for (int i = 0; i < count; i++)
        {
            float angle = i * (360f / count);
            float rad = angle * Mathf.Deg2Rad;

            Vector3 spawnPos = player_script_.transform.position + new Vector3(
                Mathf.Cos(rad), 
                Mathf.Sin(rad), 
                0) * radius;
            Quaternion rotation = Quaternion.Euler(0, 0, angle + 180f);

            GameObject spik =Instantiate(spikes, spawnPos, rotation);
            spik.GetComponent<Spike>().damage = this.Damage;
        }
        return true;
    }
}
