using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractState
{
    public abstract AbstractState RunState(System.Object param);
}
