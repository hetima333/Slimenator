using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Position {

    public int _x { get; set; }
    public int _z { get; set; }

    public Position(int x,int z)
    {
        _x = x;
        _z = z;
    }

    public Position() : this(0, 0) { }

}
