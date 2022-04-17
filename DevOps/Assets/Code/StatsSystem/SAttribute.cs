using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace OK.StatsSystem
{
    [Serializable]
    public class SAttribute
    {
        public virtual float Value
        {
            get
            {
                if (_isDirty || BaseValue != _lastBaseValue)
                {
                    _lastBaseValue = BaseValue;
                    _value = CalculateFinalValue();
                    _isDirty = false;
                }

                return _value;
            }
        }

        public float BaseValue;

        public readonly ReadOnlyCollection<AModifier> StatModifiers;
        protected readonly List<AModifier> _statsModifiers;
        protected bool _isDirty = true;
        protected float _value;
        protected float _lastBaseValue = float.MinValue;

        public Attribute()
        {
            _statsModifiers = new List<AModifier>();
            StatModifiers = _statsModifiers.AsReadOnly();
        }

        public Attribute(float baseValue) : this()
        {
            BaseValue = baseValue;
        }

        public virtual void AddModifier(AModifier mod)
        {
            _isDirty = true;
            _statsModifiers.Add(mod);
            _statsModifiers.Sort((x, y) =>
            {
                if (x.Order < y.Order)
                    return -1;
                else if (x.Order > y.Order)
                    return 1;
                return 0;
            });
        }

        public virtual bool RemoveModifier(AModifier mod)
        {
            if (_statsModifiers.Remove(mod))
            {
                _isDirty = true;
                return true;
            }

            return false;
        }

        public virtual bool RemoveAllModifiersFromSource(object Source)
        {
            bool didRemove = false;
            for (int i = _statsModifiers.Count; i >= 0; i--)
            {
                if (_statsModifiers[i].Source == Source)
                {
                    _isDirty = true;
                    didRemove = true;
                    _statsModifiers.RemoveAt(i);
                }
            }

            return didRemove;
        }

        protected virtual float CalculateFinalValue()
        {
            float finalValue = BaseValue;
            float sumOfPrecentage = 0;

            for (int i = 0; i < _statsModifiers.Count; i++)
            {
                var modifier = _statsModifiers[i];
                if (modifier.Type == AModifier.ModifierType.FLAT)
                    finalValue += modifier.Value;
                else if (modifier.Type == AModifier.ModifierType.PERCENTAGE_ADDITIVE)
                {
                    sumOfPrecentage += modifier.Value;
                    if (i + 1 >= _statsModifiers.Count ||
                        _statsModifiers[i + 1].Type != AModifier.ModifierType.PERCENTAGE_ADDITIVE)
                    {
                        finalValue *= 1 + sumOfPrecentage;
                        sumOfPrecentage = 0;
                    }
                }
                else if (modifier.Type == AModifier.ModifierType.PERCENtAGE_MULTITIVE)
                    finalValue *= 1 + modifier.Value;
            }

            //X.YYYYf
            return (float) Math.Round(finalValue, 4);
        }
    }
}