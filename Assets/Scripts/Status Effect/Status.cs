using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status : MonoBehaviour
{
    private Dictionary<EnumHolder.EffectType, List<Effect>>
        _Status = new Dictionary<EnumHolder.EffectType, List<Effect>>();

    public void Init()
    {
        if (_Status.Count > 0)
        {
            foreach (var st in _Status)
            {
                for (int i = st.Value.Count - 1; i >= 0; --i)
                {
                    Destroy(st.Value[i]);
                    st.Value.RemoveAt(i);
                }
            }
        }

        _Status.Clear();
    }

    void Update ()
    {
        if (_Status.Count > 0)
        {
            foreach (var st in _Status)
            {
                for(int i = st.Value.Count - 1; i >= 0; --i)
                {
                    st.Value[i].UpdateEffect();

                    if (st.Value[i].IsEffectDone())
                    {
                        Destroy(st.Value[i]);
                        st.Value.RemoveAt(i);
                    }
                }
            }
        }
	}

    public float GetValue(EnumHolder.EffectType states)
    {
        float temp = 0;

        if (_Status.ContainsKey(states))
        {
            foreach (Effect e in _Status[states])
            {
                if(e.GetDelayOver())
                    temp += e.GetAmount();
            }
        }

        return temp;
    }

    public void AddStatus(StatusEffect se)
    {
        IElement temp = gameObject.GetComponent<IElement>();
        Effect go_so;
        if (temp != null)
        {
            if(temp.GetElementType().Equals(se.GetElement()))
            {
                return;
            }
        }

        foreach(Effect e in se.GetEffectList())
        {
            if(!_Status.ContainsKey(e.GetEffectType()))
                _Status.Add(e.GetEffectType(), new List<Effect>());

            go_so = Instantiate(e);
            _Status[e.GetEffectType()].Add(go_so);
            go_so.SetTimer(se.GetTimer());
        }

        GameObject go_temp = Instantiate(se.GetParticle(), gameObject.transform);
        Destroy(go_temp, se.GetTimer());
    }


    public void UpdateStatMultiplyer(ref Stats stats)
    {
        foreach (var st in _Status)
        {
            float temp = 0;

            foreach(Effect e in st.Value)
            {
                temp += e.GetAmount();
            }

            switch(st.Key)
            {
                case EnumHolder.EffectType.HEALTH:
                    {
                        stats.HealthMultiplyerProperties = Mathf.Clamp(1 + temp, 0, int.MaxValue);
                    }
                    break;

                case EnumHolder.EffectType.SPEED:
                    {
                        stats.SpeedMultiplyerProperties = Mathf.Clamp(1 + temp, 0, int.MaxValue);
                    }
                    break;

                case EnumHolder.EffectType.DAMAGE:
                    {
                        stats.DamageMultiplyerProperties = Mathf.Clamp(1 + temp, 0, int.MaxValue);
                    }
                    break;

                case EnumHolder.EffectType.SUCKING:
                    {
                        stats.SuckingPowerMultiplyerProperties = Mathf.Clamp(1 + temp, 0, int.MaxValue);
                    }
                    break;

                default:
                    //ハイ馬鹿しね～
                    //stats.HealthMultiplyerProperties = Mathf.Clamp(1 + temp, 0, int.MaxValue);
                    break;
            }
        }
    }
}
