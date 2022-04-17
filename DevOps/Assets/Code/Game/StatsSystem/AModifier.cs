
using System;
using UnityEngine;

namespace OK.StatsSystem
{
    [Serializable]
    public class AModifier
    {
        [Serializable]
        public enum ModifierType
        {
            FLAT = 100,
            PERCENTAGE_ADDITIVE = 200,
            PERCENtAGE_MULTITIVE = 300
        }

        public  float Value;
        public  int Order;
        public  ModifierType Type;
        public  object Source;

        public AModifier(float value, ModifierType type, int order, object source)
        {
            Value = value;
            Type = type;
            Order = order;
            Source = source;
        }

        public AModifier(float value, ModifierType type, object source) : this(value, type, (int) type, source)
        {
        }

        public AModifier(float value, ModifierType type, int order) : this(value, type, order, null)
        {
        }

        public AModifier(float value, ModifierType type) : this(value, type, (int) type, null)
        {
        }
    }
}