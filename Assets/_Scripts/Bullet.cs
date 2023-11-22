using Fusion;
using UnityEngine;

public class Bullet : NetworkBehaviour {

    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private float _secondsOfLifeTime = 6f;
    
    [Networked] private TickTimer Life { get; set; }

    public void Initialize(Vector3 directionOfFire, float force) {
        Life = TickTimer.CreateFromSeconds(Runner, _secondsOfLifeTime);
        _rigidbody.AddForce(directionOfFire * force, ForceMode.Impulse);
    }

    public override void FixedUpdateNetwork() {
        if (Life.Expired(Runner)) {
            Despawn();
        }
    }


    private void OnCollisionEnter(Collision collision) {
        if (collision.collider.TryGetComponent(out Bullet bullet)) {
            Despawn();
        }
        
        if (collision.collider.TryGetComponent(out IDamageable damageable)) {
            damageable.OnDamage();
            Despawn();
        }
    }

    private void Despawn() => Runner.Despawn(Object);
}