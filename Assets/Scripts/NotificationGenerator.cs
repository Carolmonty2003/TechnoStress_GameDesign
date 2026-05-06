using UnityEngine;
using System.Collections.Generic;

public class NotificationGenerator : MonoBehaviour
{
    private PriorityQueue<Notification, int> notifications;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddNotification(Notification notification)
    {
        notifications.Enqueue(notification, notification.Priority);
    }

    public Notification GetNotification()
    {
        return notifications.Dequeue();
    }
}
