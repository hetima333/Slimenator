using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeManager : MonoBehaviour {
    private static SlimeManager _instance;
    public static SlimeManager Instance { get { return _instance; } }

    [SerializeField]
    private GameObject[] _slimeList;
    private List<GameObject> _slimePool = new List<GameObject>();

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    private void Start()
    {
        for (int slime_list_count = 0; slime_list_count < _slimeList.Length; ++slime_list_count)
        {
            ExpandPool(slime_list_count);
        }
    }

    private void Update()
    {
        //Temporary
        if (Input.GetKeyDown(KeyCode.Space))
        {
            int random = Random.Range(0, 5);
            GameObject slime = GetSlimeFromPool((SlimeStats.Slime_Type)random);
            slime.transform.position = new Vector3(1, 1, 1);
        }
    }

    public GameObject GetSlimeFromPool(SlimeStats.Slime_Type type)
    {
        foreach (GameObject slime_obj in _slimePool)
        {
            if (!slime_obj.activeSelf && slime_obj.name.Equals(_slimeList[(int)type].name + "(Clone)"))
            {
                slime_obj.GetComponent<SlimeBase>().Init(100,2,type);
                slime_obj.SetActive(true);
                return slime_obj;
            }
        }

        ExpandPool((int)type);

        return GetSlimeFromPool(type);
    }

    public int GetSizeOfSlime()
    {
        return _slimeList.Length;
    }

    private void ExpandPool(int type)
    {
        for (int i = 0; i < 5; ++i)
        {
            GameObject obj = null;
            obj = Instantiate(_slimeList[type]);
            obj.SetActive(false);
            _slimePool.Add(obj);

            //obj.transform.parent = gameObject.transform;
        }
    }
}
