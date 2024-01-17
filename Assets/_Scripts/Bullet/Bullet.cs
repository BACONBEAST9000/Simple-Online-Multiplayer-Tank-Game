using Fusion;
using UnityEngine;

public class Bullet : NetworkBehaviour {

    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private float _secondsOfLifeTime = 6f;
    [field: SerializeField] public BulletVisual BulletVisual { get; private set; }
   
    public int Damage { get; private set; } = 1;

    [Networked] private TickTimer LifeTime { get; set; }

    public Player Owner { get; private set; }

    public void Initialize(Vector3 directionOfFire, float force, Player owner) {
        LifeTime = TickTimer.CreateFromSeconds(Runner, _secondsOfLifeTime);
        _rigidbody.AddForce(directionOfFire * force, ForceMode.Impulse);
        Owner = owner;
    }

    public override void FixedUpdateNetwork() {
        if (LifeTime.Expired(Runner)) {
            Despawn();
        }
    }

    // TODO: Consider lag compensation. Will need to check for Tank network hitbox.
    private void OnCollisionEnter(Collision collision) {
        if (collision.collider.TryGetComponent(out Bullet bullet)) {
            Despawn();
        }
        
        if (collision.collider.TryGetComponent(out IDamageable damageable)) {
            damageable.OnDamage(this);
            Despawn();
        }
    }

    private void Despawn() => Runner.Despawn(Object);
}