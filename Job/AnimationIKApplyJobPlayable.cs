using Unity.Collections;
using UnityEngine;
using UnityEngine.Experimental.Animations;

namespace ThreeDISevenZeroR.CharacterAnimator
{
    public struct AnimationIKApplyJobPlayable : IAnimationJob
    {
        public IKLimbData.JobData leftHand;
        public IKLimbData.JobData rightHand;
        public IKLimbData.JobData leftFoot;
        public IKLimbData.JobData rightFoot;
        public IKLookData.JobData look;
 
        public void ProcessAnimation(AnimationStream output)
        {
            if (output.isHumanStream)
            {
                var humanOutput = output.AsHuman();
                
                ApplyLimbData(humanOutput, leftFoot, AvatarIKGoal.LeftFoot, AvatarIKHint.LeftKnee);
                ApplyLimbData(humanOutput, rightFoot, AvatarIKGoal.RightFoot, AvatarIKHint.RightKnee);
                ApplyLimbData(humanOutput, leftHand, AvatarIKGoal.LeftHand, AvatarIKHint.LeftElbow);
                ApplyLimbData(humanOutput, rightHand, AvatarIKGoal.RightHand, AvatarIKHint.RightElbow);
                ApplyLookData(humanOutput, look);
                    
                humanOutput.SolveIK();
            }
        }
        
        public void ProcessRootMotion(AnimationStream output) { }

        private void ApplyLimbData(AnimationHumanStream stream, IKLimbData.JobData data, AvatarIKGoal goal, AvatarIKHint hint)
        {
            if (data.positionWeight > 0)
            {
                if (data.positionWeight >= 1)
                {
                    stream.SetGoalPosition(goal, data.position);
                    stream.SetGoalWeightPosition(goal, 1f);
                }
                else
                {
                    var position = Vector3.Lerp(stream.GetGoalPosition(goal), data.position, data.positionWeight);
                    var positionWeight = Mathf.Lerp(stream.GetGoalWeightPosition(goal), 1f, data.positionWeight);
            
                    stream.SetGoalPosition(goal, position);
                    stream.SetGoalWeightPosition(goal, positionWeight);
                }
            }
            else
            {
                // For some reason, IK will be broken if same values are not reapplied before SolveIK is called
                stream.SetGoalPosition(goal, stream.GetGoalPosition(goal));
                stream.SetGoalWeightPosition(goal, stream.GetGoalWeightPosition(goal));
            }

            if (data.rotationWeight > 0)
            {
                if (data.rotationWeight >= 1)
                {
                    stream.SetGoalRotation(goal, data.rotation);
                    stream.SetGoalWeightRotation(goal, 1f);
                }
                else
                {
                    var rotation = Quaternion.Slerp(stream.GetGoalRotation(goal), data.rotation, data.rotationWeight);
                    var rotationWeight = Mathf.Lerp(stream.GetGoalWeightRotation(goal), 1f, data.rotationWeight);
            
                    stream.SetGoalRotation(goal, rotation);
                    stream.SetGoalWeightRotation(goal, rotationWeight);
                }
            }
            else
            {
                // Same as above
                stream.SetGoalRotation(goal, stream.GetGoalRotation(goal));
                stream.SetGoalWeightRotation(goal, stream.GetGoalWeightRotation(goal));
            }

            if (data.hintWeight > 0)
            {
                if (data.hintWeight >= 1)
                {
                    stream.SetHintPosition(hint, data.hintPosition);
                    stream.SetHintWeightPosition(hint, 1f);
                }
                else
                {
                    var hintPosition = Vector3.Lerp(stream.GetHintPosition(hint), data.hintPosition, data.hintWeight);
                    var hintWeight = Mathf.Lerp(stream.GetHintWeightPosition(hint), 1f, data.hintWeight);
            
                    stream.SetHintPosition(hint, hintPosition);
                    stream.SetHintWeightPosition(hint, hintWeight);
                }
            }
            else
            {
                // Same as above
                stream.SetHintPosition(hint, stream.GetHintPosition(hint));
                stream.SetHintWeightPosition(hint, stream.GetHintWeightPosition(hint));
            }
        }

        private void ApplyLookData(AnimationHumanStream stream, IKLookData.JobData data)
        {
            stream.SetLookAtPosition(data.position);
            stream.SetLookAtBodyWeight(data.bodyWeight);
            stream.SetLookAtHeadWeight(data.headWeight);
            stream.SetLookAtEyesWeight(data.eyesWeight);
            stream.SetLookAtClampWeight(data.weightClamp);
        }
    }
}