using System.Collections;
using UnityEngine;

public class BaseLevelActions:MonoBehaviour
{
    public static BaseLevelActions Instance { get; private set; }

    public void Awake()
    {
        Instance = this;
    }

    public virtual void DoAction(string nome) { }
    public virtual void OnTriggerSpeaked(){}
    public virtual void OnTriggerInteracted() {}
    public virtual void PlayerOnCollider(Collider2D other) { }
    public virtual void NPCOnCollider(Collider2D other) { }
    public virtual void SetInputPlayer(bool pode) { }

}

