﻿using Runtime.Data.ValueObjects;
using UnityEngine;

namespace Runtime.Data.UnityObjects
{
    [CreateAssetMenu(fileName = "CD_Input", menuName = "3DGame/CD_Input", order = 0)]
    public class CD_Input : ScriptableObject
    {
        public InputData inputData;
    }
}