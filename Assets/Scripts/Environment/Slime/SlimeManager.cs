using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeManager : SingletonMonoBehaviour<SlimeManager> {

    [SerializeField]
    private GameObject
        _prefab;

    private void Start()
    {
    }

    private void Update()
    {
        //Temporary
        if (Input.GetKeyDown(KeyCode.Space))
        {
            int random = Random.Range(0, EnumHolder.Instance._elements.Count);
            GetSlimeFromPool(random);
        }
    }

    public GameObject GetSlimeFromPool(int type)
    {
        GameObject slime_obj = ObjectManager.Instance.InstantiateWithObjectPooling(_prefab, new Vector3 (0,0.5f,0));
        Stats temp = EnumHolder.instance.GetStats(_prefab.name);
        SlimeBase temp_component = slime_obj.GetComponent<SlimeBase>();

        if (temp_component != null)
            DestroyImmediate(temp_component);

        System.Type _MyScriptType = System.Type.GetType(EnumHolder.Instance._elements[type].GetSlimeScriptName());
        slime_obj.AddComponent(_MyScriptType);

        slime_obj.GetComponent<SlimeBase>().Init(temp, ((EnumHolder.Instance._elements[type].name.Equals("Lightning")) ? 2 : 1), EnumHolder.Instance._elements[type]);
        slime_obj.SetActive(true);

        return slime_obj;
    }
}
