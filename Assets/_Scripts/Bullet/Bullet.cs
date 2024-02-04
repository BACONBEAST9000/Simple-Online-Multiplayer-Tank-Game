using Fusion;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : NetworkBehaviour {

    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private SphereCollider _sphereCollider;
    [SerializeField] private LayerMask _collisionLayers;
    [field: SerializeField] public BulletVisual BulletVisual { get; private set; }
    [SerializeField] private float _additionalRadius = 0;

    [Space]
    
    [SerializeField] private float _secondsOfLifeTime = 6f;
    
    public int Damage { get; private set; } = 1;

    [Networked] private TickTimer LifeTime { get; set; }

    public Player Owner { get; private set; }

    private List<LagCompensatedHit> _hits = new List<LagCompensatedHit>();

    public void Initialize(Vector3 directionOfFire, float force, Player owner) {
        LifeTime = TickTimer.CreateFromSeconds(Runner, _secondsOfLifeTime);
        _rigidbody.AddForce(directionOfFire * force, ForceMode.Impulse);
        Owner = owner;
    }

    public override void FixedUpdateNetwork() {
        if (!Object.HasStateAuthority || LifeTime.Expired(Runner)) {
            Despawn();
            return;
        }

        CheckForCollisions();
    }

    // TODO: Refactor
    private void CheckForCollisions() {
        float radius = _sphereCollider.radius + _additionalRadius;
        int bulletHitCount = Runner.LagCompensation.OverlapSphere(transform.position, radius, Object.InputAuthority, _hits, _collisionLayers, HitOptions.IncludePhysX);

        if (bulletHitCount == 0) {
            return;
        }

        for (int i = 0; i < bulletHitCount; i++) {
            GameObject hitGameObject = _hits[i].GameObject;

            if (hitGameObject == null || hitGameObject == gameObject) {
                continue;
            }

            if (hitGameObject.TryGetComponent(out Bullet bullet)) {
                bullet.Despawn();
                Despawn();
                break;
            }

            if (hitGameObject.TryGetComponent(out IDamageable damageable)) {
                damageable.OnDamage(this);
                Despawn();
                break;
            }

            print($"[Hit Info]: GameObject Name: {hitGameObject.name}");
        }
    }

    private void Despawn() => Runner.Despawn(Object);
}