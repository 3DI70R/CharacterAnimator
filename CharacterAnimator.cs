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
			Animation = 0
		}

		[SerializeField]
		private Animator playableAnimator;
    
		private PlayableGraph graph;
		private AnimationPlayableOutput animationOutput;
		private LayerMixerNodeNode rootLayerMixer;

		public ILayerMixerNode Root
		{
			get { return rootLayerMixer; }
		}
		
		private void Awake()
		{
			graph = PlayableGraph.Create(graphName);
			animationOutput = AnimationPlayableOutput.Create(graph, graphName + ".Animation", playableAnimator);

			rootLayerMixer = new LayerMixerNodeNode(graph, gameObject);
			AttachOutputs(rootLayerMixer);
   
			graph.Play();
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
						animationOutput.SetSourcePlayable(playable, i);
						break;
				}
			}
		}

		private void OnDestroy()
		{
			graph.Destroy();
		}
	}
}