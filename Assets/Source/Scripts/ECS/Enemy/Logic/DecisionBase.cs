using System.Collections;
using System.Collections.Generic;
using Ingame.Enemy;
using UnityEngine;

namespace Ingame.Enemy.Logic
{
    public abstract class DecisionBase : ScriptableObject
    {
        public abstract bool Decide();
    }
}
