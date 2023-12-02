using UnityEngine;

public class LifeTimer : MonoBehaviour
{
    public float LifeTime;

    private void Update()
    {
        LifeTime -= Time.deltaTime;
        if(LifeTime <= 0)
        {
            Destroy(gameObject);
        }
    }
}