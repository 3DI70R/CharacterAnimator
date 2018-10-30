using Unity.Collections;
using UnityEngine;
using UnityEngine.Experimental.Animations;

namespace ThreeDISevenZeroR.CharacterAnimator
{
    public class IKAnimationJob : IAnimationJob
    {
        public NativeArray<Vector3> positions;
        public NativeArray<Vector3> hints;
        
        public void ProcessAnimation(AnimationStream stream)
        {
            if (stream.isHumanStream)
            {
                var human = stream.AsHuman();
   
                human.SolveIK();
            }
        }

        public void ProcessRootMotion(AnimationStream stream)
        {
            
        }
    }
}