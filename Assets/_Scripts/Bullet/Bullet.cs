using Fusion;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : NetworkBehaviour {

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

    private List<LagCompensatedHit> _hits = new();

    public void Initialize(Vector3 directionOfFire, float force, Player owner) {
        LifeTime = TickTimer.CreateFromSeconds(Runner, _secondsOfLifeTime);
        _rigidbody.AddForce(directionOfFire * force, ForceMode.Impulse);
        Owner = owner;
    }

    public override void Spawned() {
        //_soundEmitter.Play(_soundToPlay);
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

        foreach (LagCompensatedHit hit in _hits) {
            GameObject hitGameObject = hit.GameObject;

            if (hitGameObject == null || hitGameObject == gameObject) {
                continue;
            }

            if (hitGameObject.TryGetComponent(out Bullet bullet)) {
                print("Bullet hit another bullet");
                bullet.Despawn();
            }

            else if (hitGameObject.TryGetComponent(out IDamageable damageable)) {
                print("Bullet hit damagable!");
                damageable.OnDamage(this);
            }

            print($"[Hit Info]: GameObject Name: {hitGameObject.name}");
            Despawn();
            break;
        }
    }

    private void Despawn() => Runner.Despawn(Object);

    // Only works for Host player.
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