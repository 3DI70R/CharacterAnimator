using UnityEngine;
using UnityEngine.Playables;

namespace ThreeDISevenZeroR.CharacterAnimator
{
    public class IKAnimationBehavior : PlayableBehaviour
    {
        public readonly IKLimbData leftFoot = new IKLimbData();
        public readonly IKLimbData rightFoot = new IKLimbData();
        public readonly IKLimbData leftHand = new IKLimbData();
        public readonly IKLimbData rightHand = new IKLimbData();
        public readonly IKLookData look = new IKLookData();

        public void Reset()
        {
            look.Reset();
            leftHand.Reset();
            rightHand.Reset();
            leftFoot.Reset();
            rightFoot.Reset();
        }
        
        public void Add(IKAnimationBehavior playable, float weight)
        {
            look.Add(playable.look, weight);
            leftHand.Add(playable.leftHand, weight);
            rightHand.Add(playable.rightHand, weight);
            leftFoot.Add(playable.leftFoot, weight);
            rightFoot.Add(playable.rightFoot, weight);
        }

        public void Mix(IKAnimationBehavior playable, float weight)
        {
            look.Mix(playable.look, weight);
            leftHand.Mix(playable.leftHand, weight);
            rightHand.Mix(playable.rightHand, weight);
            leftFoot.Mix(playable.leftFoot, weight);
            rightFoot.Mix(playable.rightFoot, weight);
        }

        protected ScriptPlayable<IKAnimationBehavior> GetIkFromInput(Playable p, int i)
        {
            var input = p.GetInput(i);

            if (typeof(IKAnimationBehavior).IsAssignableFrom(input.GetPlayableType()))
            {
                return (ScriptPlayable<IKAnimationBehavior>) input;
            }

            // TODO: Passtrough node hides original input
            // Animation playable seems to have no problems with finding inputs
            var inputChild = input.GetInput(i);
            if (typeof(IKAnimationBehavior).IsAssignableFrom(inputChild.GetPlayableType()))
            {
                return (ScriptPlayable<IKAnimationBehavior>) inputChild;
            }

            return ScriptPlayable<IKAnimationBehavior>.Null;
        }
    }
}