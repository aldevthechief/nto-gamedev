using UnityEngine;

public class LifeTimer : MonoBehaviour
{
    public float LifeTime;

    private void Update()
    {
        LifeTime -= Time.unscaledDeltaTime;
        if(LifeTime <= 0)
        {
            Destroy(gameObject);
        }
    }
}