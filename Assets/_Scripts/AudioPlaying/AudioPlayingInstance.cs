using Fusion;
using UnityEngine;

public abstract class AudioPlayingInstance : NetworkBehaviour {
    [Header("Sound Emitter")]
    [SerializeField] protected SoundEmitter soundEmitter;
}
