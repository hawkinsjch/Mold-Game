using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Config/Player")]
public class PlayerConfig : ScriptableObject
{
    [Min(0)]
    public float walkSpeed = 3;
}
