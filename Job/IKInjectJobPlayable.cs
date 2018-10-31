using Unity.Collections;
using UnityEngine;
using UnityEngine.Experimental.Animations;

namespace ThreeDISevenZeroR.CharacterAnimator
{
    public struct IKInjectJobPlayable : IAnimationJob
    {
        private static MuscleHandle[] allMuscleHandles;
        private readonly NativeArray<TransformStreamHandle> transforms;

        public IKLimbData.JobData leftHand;
        public IKLimbData.JobData rightHand;
        public IKLimbData.JobData leftFoot;
        public IKLimbData.JobData rightFoot;
        
        public IKInjectJobPlayable(NativeArray<TransformStreamHandle> transforms)
        {
            this.transforms = transforms;

            if (allMuscleHandles == null)
            {
                allMuscleHandles = GetAllMuscleHandles();
            }
            
            leftHand = new IKLimbData.JobData();
            rightHand = new IKLimbData.JobData();
            leftFoot = new IKLimbData.JobData();
            rightFoot = new IKLimbData.JobData();
        }
        
        public void ProcessAnimation(AnimationStream output)
        {
            var input = output.GetInputStream(1);

            if (input.isValid)
            {
                for (var i = 0; i < transforms.Length; i++)
                {
                    var handle = transforms[i];

                    if (handle.IsValid(input))
                    {
                        var position = handle.GetLocalPosition(input);
                        var rotation = handle.GetLocalRotation(input);
                        var scale = handle.GetLocalScale(input);
                        handle.SetLocalPosition(output, position);
                        handle.SetLocalRotation(output, rotation);
                        handle.SetLocalScale(output, scale);
                    }
                }

                if (output.isHumanStream && input.isHumanStream)
                {
                    var humanOutput = output.AsHuman();
                    var humanInput = input.AsHuman();

                    foreach (var handle in allMuscleHandles)
                    {
                        var muscle = humanInput.GetMuscle(handle);
                        humanOutput.SetMuscle(handle, muscle);
                    }

                    ApplyLimbData(humanOutput, leftFoot, AvatarIKGoal.LeftFoot, AvatarIKHint.LeftKnee);
                    ApplyLimbData(humanOutput, rightFoot, AvatarIKGoal.RightFoot, AvatarIKHint.RightKnee);
                    ApplyLimbData(humanOutput, leftHand, AvatarIKGoal.LeftHand, AvatarIKHint.LeftElbow);
                    ApplyLimbData(humanOutput, rightHand, AvatarIKGoal.RightHand, AvatarIKHint.RightElbow);
 
                    humanOutput.SolveIK();
                }
            }
        }

        private void ApplyLimbData(AnimationHumanStream stream, IKLimbData.JobData data, AvatarIKGoal goal, AvatarIKHint hint)
        {
            stream.SetGoalPosition(goal, data.position);
            stream.SetGoalWeightPosition(goal, data.positionWeight);
            
            stream.SetGoalRotation(goal, data.rotation);
            stream.SetGoalWeightRotation(goal, data.rotationWeight);
            
            stream.SetHintPosition(hint, data.hintPosition);
            stream.SetHintWeightPosition(hint, data.hintWeight);
        }

        public void ProcessRootMotion(AnimationStream output)
        {
            var input = output.GetInputStream(1);
            output.velocity = input.velocity;
            output.angularVelocity = input.angularVelocity;
        }

        private static MuscleHandle[] GetAllMuscleHandles()
        {
            var handles = new MuscleHandle[MuscleHandle.muscleHandleCount];
            MuscleHandle.GetMuscleHandles(handles);
            return handles;
        }
    }
}