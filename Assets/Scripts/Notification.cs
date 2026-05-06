using UnityEngine;

public class Notification : MonoBehaviour
{
    [SerializeField] private float lifetime;
    [SerializeField] private int priority;
    public int Priority { get { return priority; } }

    void Update()
    {
        lifetime -= Time.deltaTime;

        if(lifetime < 0)
        {
            Finish();
        }
    }

    private void Finish()
    {
        Destroy(this.gameObject);
    }
}
