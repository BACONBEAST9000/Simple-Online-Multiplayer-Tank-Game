using Fusion;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : NetworkBehaviour {

    private const float INVINCIBLITY_TIME = 0.25f;

    public static event Action<Bullet> OnLocalCollisionHitWall;

    [Header("Collision")]
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private SphereCollider _sphereCollider;
    [SerializeField] private LayerMask _collisionLayers;
    [SerializeField] private LayerMask _wallLayers;

    [field: Header("Other")]
    [field: SerializeField] public BulletVisual BulletVisual { get; private set; }
    [SerializeField] private float _additionalRadius = 0;    
    [SerializeField] private float _secondsOfLifeTime = 6f;
    
    public int Damage { get; private set; } = 1;

    [Networked] private TickTimer LifeTime { get; set; }

    public Player Owner { get; private set; }

    private List<LagCompensatedHit> _bulletCollisions = new();

    private TickTimer _playerSelfHitInvincibilityTimer;
    private bool _canHurtPlayerWhoShotThis;

    public void Initialize(Vector3 directionOfFire, float force, Player owner) {
        LifeTime = TickTimer.CreateFromSeconds(Runner, _secondsOfLifeTime);
        _playerSelfHitInvincibilityTimer = TickTimer.CreateFromSeconds(Runner, INVINCIBLITY_TIME);
        
        _rigidbody.AddForce(directionOfFire * force, ForceMode.Impulse);
        Owner = owner;
        
        _canHurtPlayerWhoShotThis = false;
        Physics.IgnoreCollision(_sphereCollider, Owner.Collider);
    }

    public override void FixedUpdateNetwork() {
        if (!Object.HasStateAuthority || LifeTime.Expired(Runner)) {
            Despawn();
            return;
        }

        if (!_canHurtPlayerWhoShotThis && _playerSelfHitInvincibilityTimer.Expired(Runner)) {
            Physics.IgnoreCollision(_sphereCollider, Owner.Collider, false);
            _canHurtPlayerWhoShotThis = true;
        }

        CheckForCollisions();
    }

    private void CheckForCollisions() {
        float radius = _sphereCollider.radius + _additionalRadius;
        Runner.LagCompensation.OverlapSphere(transform.position, radius, Object.InputAuthority, _bulletCollisions, _collisionLayers, HitOptions.IncludePhysX);

        if (_bulletCollisions.Count == 0) {
            return;
        }

        foreach (LagCompensatedHit hit in _bulletCollisions) {
            GameObject hitGameObject = hit.GameObject;

            bool hitObjectIsNullOrThisObject = hitGameObject == null || hitGameObject == gameObject;
            if (hitObjectIsNullOrThisObject) {
                continue;
            }

            bool hitPlayerWhenJustSpawned = !_canHurtPlayerWhoShotThis && hitGameObject == Owner.gameObject;
            if (hitPlayerWhenJustSpawned) {
                return;
            }

            if (hitGameObject.TryGetComponent(out Bullet bullet)) {
                bullet.Despawn();
            }

            else if (hitGameObject.TryGetComponent(out IDamageable damageable)) {
                damageable.OnDamage(this);
            }

            print($"[Hit Info]: GameObject Name: {hitGameObject.name}");
            Despawn();
            break;
        }
    }

    private void Despawn() => Runner.Despawn(Object);

    // Only works for Host player, so RPC needed to invoke event.
    private void OnCollisionEnter(Collision collision) {
        if (Utils.IsInLayer(collision.collider.gameObject, _wallLayers)) {
            RPC_HitWall();
        }
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_HitWall() {
        OnLocalCollisionHitWall?.Invoke(this);
    }
}