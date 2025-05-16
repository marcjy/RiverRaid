using UnityEngine;

public class DestroyOnAnimationEvent : MonoBehaviour
{
    //Called from an Animation event
    public void Destroy()
    {
        Destroy(gameObject);
    }
}
