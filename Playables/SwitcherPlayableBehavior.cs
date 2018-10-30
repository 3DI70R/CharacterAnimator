using UnityEngine.Animations;
using UnityEngine.Playables;

namespace ThreeDISevenZeroR.CharacterAnimator
{
    public class SwitcherPlayableBehavior : PlayableBehaviour
    {
        private AnimationMixerPlayable animationPlayable;

        public SwitcherPlayableBehavior(PlayableGraph graph)
        {
            
        }

        public override void PrepareFrame(Playable playable, FrameData info)
        {
            base.PrepareFrame(playable, info);
        }
    }
}