using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "String Holder")]
public class StringList : GenericList<string>
{
    public bool IsTagNotHitable(string tag)
    {
        foreach(string s in GetList())
        {
            if (s.Equals(tag))
                return true;
        }

        return false;
    }
}
