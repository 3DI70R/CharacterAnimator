using System.Collections.Generic;
using UnityEngine.Playables;

namespace ThreeDISevenZeroR.CharacterAnimator
{
    public class IKAnimationBehaviorLayerMixer : IKAnimationBehavior
    {
        private readonly HashSet<int> additiveLayers = new HashSet<int>();

        public void SetLayerAdditive(int layer, bool isAdditive)
        {
            if (isAdditive)
            {
                additiveLayers.Add(layer);
            }
            else
            {
                additiveLayers.Remove(layer);
            }
        }

        public bool GetLayerAdditive(int layer)
        {
            return additiveLayers.Contains(layer);
        }
        
        public override void PrepareFrame(Playable playable, FrameData info)
        {
            Reset();

            var inputCount = playable.GetInputCount();

            for (var i = 0; i < inputCount; i++)
            {
                var weight = playable.GetInputWeight(i);

                if (weight > 0)
                {
                    var behavior = GetIkFromInput(playable, i);
                    
                    if (behavior != null)
                    {
                        if (GetLayerAdditive(i))
                        {
                            Add(behavior, weight);
                        }
                        else
                        {
                            Mix(behavior, weight);
                        }
                    }
                }
            }
        }
    }
}