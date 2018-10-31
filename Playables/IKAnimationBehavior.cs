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

        protected IKAnimationBehavior GetIkFromInput(Playable p, int i)
        {
            var input = p.GetInput(i);

            if (typeof(IKAnimationBehavior).IsAssignableFrom(input.GetPlayableType()))
            {
                var inputIkPlayable = (ScriptPlayable<IKAnimationBehavior>) input;
                return inputIkPlayable.GetBehaviour();
            }
            
            // TODO: Passtrough node hides original input
            // Investigate for better solutions
            var inputChild = input.GetInput(1);
            if (typeof(IKAnimationBehavior).IsAssignableFrom(inputChild.GetPlayableType()))
            {
                var inputIkPlayable = (ScriptPlayable<IKAnimationBehavior>) inputChild;
                return inputIkPlayable.GetBehaviour();
            }

            return null;
        }
    }
}