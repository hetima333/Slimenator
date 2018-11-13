using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeManager : SingletonMonoBehaviour<SlimeManager> {

    [SerializeField]
    private GameObject
        _prefab;

    [SerializeField]
    public SOList
        _elements;

    [SerializeField]
    public SkillTier
        _startingTier;

    private void Start()
    {
    }

    private void Update()
    {
        //Temporary
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //ObjectManager.Instance.InstantiateWithObjectPooling(_prefab, transform.position, transform.rotation).GetComponent<EnvironmentBase>();
            int random = Random.Range(0, _elements.GetList().Count);
            GetSlimeFromPool(random, Vector3.one);
        }
    }

    public GameObject GetSlimeFromPool(int type, Vector3 position = new Vector3())
    {
        GameObject slime_obj = ObjectManager.Instance.InstantiateWithObjectPooling(_prefab, position);
        //Stats temp = EnumHolder.Instance.GetStats(_prefab.name);
        //SlimeBase temp_component = slime_obj.GetComponent<SlimeBase>();

        //if (temp_component != null)
        //    DestroyImmediate(temp_component);

        //System.Type _MyScriptType = System.Type.GetType(((ElementType)_elements.GetList()[type]).GetSlimeScriptName());
        //slime_obj.AddComponent(_MyScriptType);

        //slime_obj.GetComponent<SlimeBase>().Init(temp, ((((ElementType)_elements.GetList()[type]).name.Equals("Lightning")) ? 2 : 1), ((ElementType)_elements.GetList()[type]), _startingTier);
        //slime_obj.SetActive(true);

        return slime_obj;
    }
}
