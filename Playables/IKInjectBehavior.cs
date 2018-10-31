using UnityEngine;
using UnityEngine.Experimental.Animations;
using UnityEngine.Playables;

namespace ThreeDISevenZeroR.CharacterAnimator
{
    public class IKInjectBehavior : IKAnimationBehaviorMixer
    {
        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {

        }

        private void ApplyLimbIK(AnimationHumanStream humanStream, AvatarIKGoal goal, AvatarIKHint hint, IKLimbData data)
        {
            humanStream.SetGoalPosition(goal, data.position);
            humanStream.SetGoalWeightPosition(goal, data.positionWeight);
			
            humanStream.SetGoalRotation(goal, data.rotation);
            humanStream.SetGoalWeightRotation(goal, data.rotationWeight);
			
            humanStream.SetHintPosition(hint, data.hintPosition);
            humanStream.SetHintWeightPosition(hint, data.hintWeight);
        }
    }
}