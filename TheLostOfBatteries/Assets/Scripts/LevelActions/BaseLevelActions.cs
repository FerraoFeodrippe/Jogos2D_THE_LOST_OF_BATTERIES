using UnityEngine;

public class BaseLevelActions:MonoBehaviour
{
    public static BaseLevelActions Instance { get; private set; }

    public void Awake()
    {
        Instance = this;
    }

    public virtual void OnTriggerSpeaked(){}
}

