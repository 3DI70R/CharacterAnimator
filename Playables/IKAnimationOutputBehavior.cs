using UnityEngine.Experimental.Animations;
using UnityEngine.Playables;

namespace ThreeDISevenZeroR.CharacterAnimator
{
    public class IKAnimationOutputBehavior : IKAnimationBehaviorMixer
    {
        public AnimationScriptPlayable postprocess;

        public override void PrepareFrame(Playable playable, FrameData info)
        {
            base.PrepareFrame(playable, info);
            
            var job = postprocess.GetJobData<AnimationIKApplyJobPlayable>();
            job.leftHand = leftHand.jobData;
            job.rightHand = rightHand.jobData;
            job.leftFoot = leftFoot.jobData;
            job.rightFoot = rightFoot.jobData;
            job.look = look.jobData;
            postprocess.SetJobData(job);
        }
    }
}