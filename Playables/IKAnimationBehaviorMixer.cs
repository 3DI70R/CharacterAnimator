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
                    if (GetIkFromInput(playable, i).IsValid())
                    {
                        weightSum += playable.GetInputWeight(i);
                    }
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
                        var ikPlayable = GetIkFromInput(playable, i);

                        if (ikPlayable.IsValid())
                        {
                            var behavior = ikPlayable.GetBehaviour();
                            // TODO: Need to investigate, how to prepare frames in input before evaluating mixer
                            behavior.PrepareFrame(ikPlayable, info);
                            Add(behavior, weight / weightSum);
                        }
                    }
                }
            }
        }
    }
}