using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/GameEvent")]
public class GameEvent : ScriptableObject
{
    [SerializeField]
    private List<EventListener> 
        Event_Listeners;

    private GameEvent()
    {
        Event_Listeners = new List<EventListener>();
    }
 
    public void InvokeAllListeners()
    {
        for (int i = Event_Listeners.Count - 1; i >= 0; i--)
        {
            Event_Listeners[i].OnEventRaised(this.name);
        }
    }

    public void InvokeSpecificListner(int UUIDInput)
    {
        for (int i = Event_Listeners.Count - 1; i >= 0; i--)
        {
            if (Event_Listeners[i].gameObject.GetInstanceID().Equals(UUIDInput))
            {
                Event_Listeners[i].OnEventRaised(this.name);
                break;
            }
        }
    }

    public void AddListener(EventListener listener)
    {
        Event_Listeners.Add(listener);
    }

    public void RemoveListener(EventListener listener)
    {
        Event_Listeners.Remove(listener);
    }

    public void RemoveAllListeners()
    {
        for (int i = Event_Listeners.Count - 1; i >= 0; i--)
        {
            Event_Listeners.Remove(Event_Listeners[i]);
        }
    }
}
