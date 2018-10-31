﻿using Unity.Collections;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Experimental.Animations;
using UnityEngine.Playables;

namespace ThreeDISevenZeroR.CharacterAnimator
{
	public partial class CharacterAnimator : MonoBehaviour
	{
		private const string graphName = "Character";
    
		private enum StreamType
		{
			Unknown = -1,
			Animation = 0,
			IK = 1
		}

		[SerializeField]
		private Animator playableAnimator;
    
		private PlayableGraph graph;
		private AnimationPlayableOutput animationOutput;
		private AnimationScriptPlayable ikInjectPlayable;
		private ScriptPlayable<IKInjectBehavior> ikOutput;

		private ScriptPlayableOutput ikScriptOutput;
		private LayerMixerNode rootLayerMixer;

		public ILayerMixerNode Root => rootLayerMixer;

		private NativeArray<TransformStreamHandle> streamTransforms;

		private void Awake()
		{
			graph = PlayableGraph.Create(graphName);
			graph.SetTimeUpdateMode(DirectorUpdateMode.GameTime);

			streamTransforms = GetStreamHandles();

			var job = new IKInjectJobPlayable(streamTransforms);
			ikInjectPlayable = AnimationScriptPlayable.Create(graph, job, 1);
			ikInjectPlayable.SetProcessInputs(false);
			
			animationOutput = AnimationPlayableOutput.Create(graph, graphName + ".Animation", playableAnimator);
			animationOutput.SetSourcePlayable(ikInjectPlayable);

			ikScriptOutput = ScriptPlayableOutput.Create(graph, graphName + ".IK");
			ikOutput = ScriptPlayable<IKInjectBehavior>.Create(graph, new IKInjectBehavior
			{
				ikInjectPlayable = ikInjectPlayable
			});
			ikScriptOutput.SetSourcePlayable(ikOutput);

			rootLayerMixer = new LayerMixerNode(graph, gameObject);
			AttachOutputs(rootLayerMixer);
   
			graph.Play();
		}

		private NativeArray<TransformStreamHandle> GetStreamHandles()
		{
			var transforms = GetComponentsInChildren<Transform>();
			var transformCount = transforms.Length - 1;
			var handles = new NativeArray<TransformStreamHandle>(transformCount, 
				Allocator.Persistent, 
				NativeArrayOptions.UninitializedMemory);
			
			for (var i = 0; i < transformCount; ++i)
			{
				handles[i] = playableAnimator.BindStreamTransform(transforms[i + 1]);
			}

			return handles;
		}

		private void AttachOutputs(NodeBase node)
		{
			var types = node.OutputTypes;
			var playable = node.Playable;
        
			for(var i = 0; i < types.Length; i++)
			{
				var t = types[i];

				switch (t)
				{
					case StreamType.Animation:
						ikInjectPlayable.AddInput(playable, i, 1f);
						break;
					
					case StreamType.IK:
						ikOutput.AddInput(playable, i, 1f);
						break;
				}
			}
		}
		
		private void OnDestroy()
		{
			graph.Destroy();
			streamTransforms.Dispose();
		}
	}
}