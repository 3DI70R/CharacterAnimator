using UnityEngine.Playables;

namespace ThreeDISevenZeroR.CharacterAnimator
{
    public class IKAnimationBehaviorMixer : IKAnimationBehavior
    {
        public bool NormalizeInputs { get; set; }
        
        public override void PrepareFrame(Playable playable, FrameData info)
        {
            Reset();

            var inputCount = playable.GetInputCount();
            var weightSum = 0f;

            if (NormalizeInputs)
            {
                for (var i = 0; i < inputCount; i++)
                {
                    weightSum += playable.GetInputWeight(i);
                }
            }
            else
            {
                weightSum = 1;
            }

            if (weightSum > 0)
            {
                for (var i = 0; i < inputCount; i++)
                {
                    var weight = playable.GetInputWeight(i);

                    if (weight > 0)
                    {
                        var behavior = GetIkFromInput(playable, i);
                        
                        if (behavior != null)
                        {
                            Add(behavior, weight / weightSum);
                        }
                    }
                }
            }
        }
    }
}