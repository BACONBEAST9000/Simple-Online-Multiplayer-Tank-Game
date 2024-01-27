using Fusion;
using UnityEngine;

/// <summary>
/// Singleton for Photon Fusion's NetworkBehaviours.
/// </summary>
public class SingletonNetwork<T> : NetworkBehaviour where T : Component {
    public static T Instance { get; private set; }

    public virtual void Awake() {
        if (Instance == null) {
            Instance = this as T;
        }
        else {
            Destroy(gameObject);
        }
    }
}

/// <summary>
/// Singleton for Photon Fusion's NetworkBehaviours with DontDestroyOnLoad(gameObject) set.
/// </summary>
public class SingletonNetworkPersistent<T> : SingletonNetwork<T> where T : Component {

    public override void Awake() {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }
}

/// <summary>
/// Singleton for Photon Fusion's SimulationBehaviours.
/// </summary>
public class SingletonSimulationNetwork<T> : SimulationBehaviour where T : Component {
    public static T Instance { get; private set; }

    public virtual void Awake() {
        if (Instance == null) {
            Instance = this as T;
        }
        else {
            Destroy(gameObject);
        }
    }
}

/// <summary>
/// Singleton for Photon Fusion's SimulationBehaviours with DontDestroyOnLoad(gameObject) set.
/// </summary>
public class SingletonSimulationNetworkPersistent<T> : SingletonSimulationNetwork<T> where T : Component {

    public override void Awake() {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }
}