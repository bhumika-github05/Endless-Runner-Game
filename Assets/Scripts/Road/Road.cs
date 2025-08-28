using UnityEngine;

public class Road : MonoBehaviour
{
    [SerializeField] public float roadLength;

    public Road DestroyRoad()
    {
        Destroy(gameObject);
        return this;
    }
}
