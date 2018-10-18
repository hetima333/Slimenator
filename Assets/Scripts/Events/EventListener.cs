using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventListener : MonoBehaviour
{
    [SerializeField]
    private List<GameEvent>
        GameEvent;
    [SerializeField]
    private List<UnityEvent>
        Response;
    private string
        UUID;
    
    private void OnEnable()
    {
        foreach (GameEvent ge in this.GameEvent)
        {
            ge.AddListener(this);
        }
    }

    private void OnDisable()
    {
        foreach (GameEvent ge in this.GameEvent)
        {
            ge.RemoveListener(this);
        }
    }

    public void OnEventRaised(string TypeOfEvent)
    {
        for (int i = 0; i <= GameEvent.Count - 1; ++i)
        {
            if (GameEvent[i].name.Equals(TypeOfEvent))
            {
                if (Response.Count > i)
                    Response[i].Invoke();

                break;
            }
        }
    }

    public void SetUUID(string UUIDInput)
    {
        UUID = UUIDInput;
    }

    public string GetUUID()
    {
        return UUID;
    }
}
