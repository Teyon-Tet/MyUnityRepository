﻿using System;
using UnityEngine;

namespace Runtime.Data.ValueObjects
{
    [Serializable]
    public struct InputData
    {
        public float horizontalInputSpeed;
        public Vector2 clampValues;
        public float clampSpeed;
    }
}