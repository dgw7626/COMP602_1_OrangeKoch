using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Author: Corey Knight - 21130891
 */
public abstract class AbstractState
{
    public abstract AbstractState RunState(System.Object param);
}
