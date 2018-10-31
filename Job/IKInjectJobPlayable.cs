using Unity.Collections;
using UnityEngine;
using UnityEngine.Experimental.Animations;

namespace ThreeDISevenZeroR.CharacterAnimator
{
    public struct IKInjectJobPlayable : IAnimationJob
    {
        public NativeArray<TransformStreamHandle> transforms;
        
        public void ProcessAnimation(AnimationStream stream)
        {
            var input = stream.GetInputStream(1);

            if (input.isValid)
            {
                for (var i = 0; i < transforms.Length; i++)
                {
                    var t = transforms[i];
                    t.SetLocalPosition(stream, t.GetLocalPosition(input));
                    t.SetLocalRotation(stream, t.GetLocalRotation(input));
                    t.SetLocalScale(stream, t.GetLocalScale(input));
                }
            }
            
            var human = stream.AsHuman();
            human.SetGoalWeightPosition(AvatarIKGoal.LeftFoot, 1f);
            human.SolveIK();
        }

        public void ProcessRootMotion(AnimationStream stream)
        {
            
        }
    }
}