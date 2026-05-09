using UnityEngine;

public class Ability : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private string name_;
    [SerializeField] private string description_;
    [SerializeField] private int damage_;
    [SerializeField] private int manaCost_;
    [SerializeField] private int coolDown_;
    [SerializeField] private int range_;

    public string Name => name_;
    public string Description => description_;
    public int Damage => damage_;
    public int ManaCost => manaCost_;
    public int CoolDown => coolDown_;
    public int Range => range_;

    [Header("UI")]
    public Sprite Icon;

    public enum KeyCasted { Q, W, E, R }
    public KeyCasted AssignedTo = KeyCasted.Q;

    public PlayerScript player_script_;

    private float lastCastTime_ = -Mathf.Infinity;
    public bool IsOnCooldown => Time.time - lastCastTime_ < coolDown_;
    public float CooldownRemaining => Mathf.Max(0, coolDown_ - (Time.time - lastCastTime_));

    public GameObject copy_offline;

    protected virtual bool isCastable()
    {
        return false;
    }
    public bool Cast()
    {
        if (IsOnCooldown && !isCastable()) return false;
        if (!OnCast())
        {
            return false;
        }
        lastCastTime_ = Time.time;
        return true;
    }

    protected virtual bool OnCast()
    {
        return false;
        // override in subclass
    }
}