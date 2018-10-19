using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnvironmentBase : MonoBehaviour
{
    GameObject 
        _gameobject;

    private float
        _lifetime,
        _lifeElapse;

    private Vector3
        _size;

    private bool
        _isStatic,
        _isDestructible,
        _willExpire;

    #region Getter/Setter
    public float Lifetime
    {
        get
        {
            return _lifetime;
        }

        set
        {
            _lifetime = value;
        }
    }

    public float LifeElapse
    {
        get
        {
            return _lifeElapse;
        }

        set
        {
            _lifeElapse = value;
        }
    }

    public Vector3 Size
    {
        get
        {
            return _size;
        }

        set
        {
            _size = value;
        }
    }

    public bool IsStatic
    {
        get
        {
            return _isStatic;
        }

        set
        {
            _isStatic = value;
        }
    }

    public bool WillExpire
    {
        get
        {
            return _willExpire;
        }

        set
        {
            _willExpire = value;
        }
    }

    public bool IsDestructible
    {
        get
        {
            return _isDestructible;
        }

        set
        {
            _isDestructible = value;
        }
    }

    #endregion

    public virtual void InitObjectWithLife(float lifetime, Vector3 pos, Vector3 size, bool isStatic = true)
    {
        _gameobject = gameObject;
        _gameobject.transform.position = pos;
        _gameobject.transform.localScale = size;
        _size = size;
        _lifetime = lifetime;
        _isStatic = isStatic;
        _willExpire = true;
        _lifeElapse = 0;
    }

    public virtual void InitObject(Vector3 pos, Vector3 size, bool isDestructible, bool isStatic = true)
    {
        _gameobject = gameObject;
        _gameobject.transform.position = pos;
        _gameobject.transform.localScale = size;
        _size = size;
        _isDestructible = isDestructible;
        _isStatic = isStatic;
        _willExpire = false;
    }

    protected virtual void Awake()
    {
    }

    // Use this for initialization
    protected virtual void Start () {
		
	}

    // Update is called once per frame
    protected virtual void Update () {
        if (_willExpire)
        {
            _lifeElapse += Time.deltaTime;

            if (_lifeElapse > _lifetime)
            {
                _gameobject.SetActive(false);
            }
        }
    }
}
