using UnityEngine;
using UnityEngine.Animations;

namespace ThreeDISevenZeroR.CharacterAnimator
{
    public struct ControllerValue<T>
    {
        private static readonly ValueAccessor<bool> boolAccessor = new BoolAccessor();
        private static readonly ValueAccessor<int> intAccessor = new IntAccessor();
        private static readonly ValueAccessor<float> floatAccessor = new FloatAccessor();
        
        private abstract class ValueAccessor<V>
        {
            public abstract V Get(AnimatorControllerPlayable p, int paramId);
            public abstract void Set(AnimatorControllerPlayable p, int paramId, V value);
            public abstract void Reset(AnimatorControllerParameter param, AnimatorControllerPlayable p, int paramId);
        }
        private class BoolAccessor : ValueAccessor<bool>
        {
            public override bool Get(AnimatorControllerPlayable p, int paramId)
            {
                return p.GetBool(paramId);
            }

            public override void Set(AnimatorControllerPlayable p, int paramId, bool value)
            {
                p.SetBool(paramId, value);
            }

            public override void Reset(AnimatorControllerParameter param, AnimatorControllerPlayable p, int paramId)
            {
                p.SetBool(paramId, param.defaultBool);
            }
        }
        private class IntAccessor : ValueAccessor<int>
        {
            public override int Get(AnimatorControllerPlayable p, int paramId)
            {
                return p.GetInteger(paramId);
            }

            public override void Set(AnimatorControllerPlayable p, int paramId, int value)
            {
                p.SetInteger(paramId, value);
            }

            public override void Reset(AnimatorControllerParameter param, AnimatorControllerPlayable p, int paramId)
            {
                p.SetInteger(paramId, param.defaultInt);
            }
        }
        private class FloatAccessor : ValueAccessor<float>
        {
            public override float Get(AnimatorControllerPlayable p, int paramId)
            {
                return p.GetFloat(paramId);
            }

            public override void Set(AnimatorControllerPlayable p, int paramId, float value)
            {
                p.SetFloat(paramId, value);
            }

            public override void Reset(AnimatorControllerParameter param, AnimatorControllerPlayable p, int paramId)
            {
                p.SetFloat(paramId, param.defaultFloat);
            }
        }
        
        private ValueAccessor<T> accessor;
        private AnimatorControllerPlayable playable;
        private AnimatorControllerParameter paramCache;
        private int paramId;

        public string Name
        {
            get { return Parameter.name; }
        }
        
        public T Value
        {
            get { return accessor.Get(playable, paramId); }
            set { accessor.Set(playable, paramId, value); }
        }
        
        public bool IsControlledByCurve
        {
            get { return playable.IsParameterControlledByCurve(paramId); }
        }

        public AnimatorControllerParameter Parameter
        {
            get
            {
                if (paramCache == null)
                {
                    var count = playable.GetParameterCount();
                    for (var i = 0; i < count; i++)
                    {
                        var param = playable.GetParameter(i);
                        
                        if (param.nameHash == paramId)
                        {
                            paramCache = param;
                        }
                    }
                }

                return paramCache;
            }
        }

        public void Reset()
        {
            accessor.Reset(Parameter, playable, paramId);
        }

        private ControllerValue(AnimatorControllerPlayable playable, int paramId, ValueAccessor<T> accessor)
        {
            this.playable = playable;
            this.paramId = paramId;
            this.accessor = accessor;
            paramCache = null;
        }
        
        public static ControllerValue<bool> CreateBool(AnimatorControllerPlayable p, int id)
        {
            return new ControllerValue<bool>(p, id, ControllerValue<bool>.boolAccessor);
        }
        
        public static ControllerValue<int> CreateInt(AnimatorControllerPlayable p, int id)
        {
            return new ControllerValue<int>(p, id, ControllerValue<int>.intAccessor);
        }
        
        public static ControllerValue<float> CreateFloat(AnimatorControllerPlayable p, int id)
        {
            return new ControllerValue<float>(p, id, ControllerValue<float>.floatAccessor);
        }
    }
}