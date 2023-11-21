using UnityEngine;

public class Bullet : MonoBehaviour, IDamageable {

    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private float _secondsOfLifeTime = 6f;

    public void Start() {
        Destroy(gameObject, _secondsOfLifeTime);
    }

    public void Initalize(Vector3 directionOfFire, float force) {
        _rigidbody.AddForce(directionOfFire * force, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.collider.TryGetComponent(out IDamageable damageable)) {
            damageable.OnDamage();
            Destroy(gameObject);
        }
    }

    public void OnDamage() {
        Destroy(gameObject);
    }
}