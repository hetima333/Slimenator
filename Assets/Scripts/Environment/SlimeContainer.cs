using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeContainer : EnvironmentDestructible
{
    [SerializeField]
    public SOList
        _elements;

    [SerializeField]
    public SkillTier
        _startingTier;

    protected override void Update()
    {
        // for testing only
        if (Input.GetKeyDown(KeyCode.K))
        {
            Die();
        }
    }

    protected override void Die()
    {
        base.Die();
        int random = Random.Range(0, _elements.GetList().Count);
        GetSlimeFromPool(random, gameObject.transform.position + new Vector3(0,3,0));

    }

    public GameObject GetSlimeFromPool(int type, Vector3 position = new Vector3())
    {
        GameObject slime_obj = ObjectManager.Instance.InstantiateWithObjectPooling(_drop.GetPrefab(), position);
        Stats temp = EnumHolder.Instance.GetStats(_drop.GetPrefab().name);
        SlimeBase temp_component = slime_obj.GetComponent<SlimeBase>();

        if (temp_component != null)
            DestroyImmediate(temp_component);

        System.Type _MyScriptType = System.Type.GetType(((ElementType)_elements.GetList()[type]).GetSlimeScriptName());
        slime_obj.AddComponent(_MyScriptType);

        slime_obj.GetComponent<SlimeBase>().Init(temp, ((((ElementType)_elements.GetList()[type]).name.Equals("Lightning")) ? 2 : 1), ((ElementType)_elements.GetList()[type]), _startingTier);
        slime_obj.SetActive(true);

        return slime_obj;
    }
}
