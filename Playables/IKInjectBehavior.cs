using UnityEngine.Experimental.Animations;
using UnityEngine.Playables;

namespace ThreeDISevenZeroR.CharacterAnimator
{
    public class IKInjectBehavior : IKAnimationBehaviorMixer
    {
        public AnimationScriptPlayable ikInjectPlayable;

        public override void PrepareFrame(Playable playable, FrameData info)
        {
            base.PrepareFrame(playable, info);
            
            var job = ikInjectPlayable.GetJobData<IKInjectJobPlayable>();
            job.leftHand = leftHand.jobData;
            job.rightHand = rightHand.jobData;
            job.leftFoot = leftFoot.jobData;
            job.rightFoot = rightFoot.jobData;
            job.look = look.jobData;
            ikInjectPlayable.SetJobData(job);
        }
    }
}