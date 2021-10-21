using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class EntityCombat : MonoBehaviour {

    [SerializeField, Tooltip("The damage the entity does to other entities")]       int _baseDamage;
    [SerializeField, Tooltip("The range of the entity attack as a float")]          float _attackRange;
    [SerializeField, Tooltip("How often the entity can attack")]                    float _attackCooldown;

    [SerializeField, Tooltip("The attack point of the player")]                     Transform attackPoint;
    [SerializeField, Tooltip("The layer(s) which should be counted as enemies")]    LayerMask enemyLayers;

    UnityEvent _eventOnAttack = new UnityEvent();

    /// <summary>
    /// The damage the entity does to other entities
    /// </summary>
    public int baseDamage {
        get => _baseDamage;
        set => _baseDamage = value;
    }

    /// <summary>
    /// The range of the player attack
    /// </summary>
    public float attackRange {
        get => _attackRange;
        set => _attackRange = value;
    }

    /// <summary>
    /// Controls how often the player can attack
    /// </summary>
    public float attackCooldown {
        get => _attackCooldown;
        set => _attackCooldown = value;
    }

    public float timeLeftToAllowAttack { get; protected set; }

    /// <summary>
    /// Unity event for when the player is attacking
    /// </summary>
    public UnityEvent eventOnAttack => _eventOnAttack;

    public bool canAttack => timeLeftToAllowAttack <= 0f;

    /// <summary>
    /// Perform an attack.
    /// Will attack all enemies within range
    /// </summary>
    public virtual void Attack(int damage) {
        // Invokes the listener of eventOnAttack 
        eventOnAttack.Invoke();
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, _attackRange, enemyLayers);

        // We want unique entities because some entities might have multiple colliders
        IEnumerable<Entity> uniqueEntities = hitEnemies.Select(c => c.GetComponentInParent<Entity>()).Distinct();

        foreach(Entity entity in uniqueEntities) {
            entity.InflictDamage(damage);
        }

        timeLeftToAllowAttack = attackCooldown;
    }

    /// <summary>
    /// Will be called every Update
    /// </summary>
    public virtual void UpdateCombat () {
        if (timeLeftToAllowAttack > 0) {
            timeLeftToAllowAttack -= Time.deltaTime; 
        }
    }

    void Update () {
        UpdateCombat();
    }

    void OnDrawGizmosSelected() {
        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, _attackRange);
    }

}
