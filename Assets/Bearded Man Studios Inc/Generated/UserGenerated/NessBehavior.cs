using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Unity;
using UnityEngine;

namespace BeardedManStudios.Forge.Networking.Generated
{
	[GeneratedRPC("{\"types\":[[\"Vector2\"][\"string\"]]")]
	[GeneratedRPCVariableNames("{\"types\":[[\"destination\"][\"text\"]]")]
	public abstract partial class NessBehavior : NetworkBehavior
	{
		public NessNetworkObject networkObject = null;

		public override void Initialize(NetworkObject obj)
		{
			// We have already initialized this object
			if (networkObject != null && networkObject.AttachedBehavior != null)
				return;
			
			networkObject = (NessNetworkObject)obj;
			networkObject.AttachedBehavior = this;

			base.SetupHelperRpcs(networkObject);
			networkObject.RegisterRpc("MoveTo", MoveTo, typeof(Vector2));
			networkObject.RegisterRpc("SpawnSpeechBubble", SpawnSpeechBubble, typeof(string));
			networkObject.RegistrationComplete();

			MainThreadManager.Run(NetworkStart);

			networkObject.onDestroy += DestroyGameObject;
		}

		public override void Initialize(NetWorker networker)
		{
			Initialize(new NessNetworkObject(networker, createCode: TempAttachCode));
		}

		private void DestroyGameObject()
		{
			MainThreadManager.Run(() => { Destroy(gameObject); });
			networkObject.onDestroy -= DestroyGameObject;
		}

		/// <summary>
		/// Arguments:
		/// Vector2 destination
		/// </summary>
		public abstract void MoveTo(RpcArgs args);
		/// <summary>
		/// Arguments:
		/// string text
		/// </summary>
		public abstract void SpawnSpeechBubble(RpcArgs args);

		// DO NOT TOUCH, THIS GETS GENERATED PLEASE EXTEND THIS CLASS IF YOU WISH TO HAVE CUSTOM CODE ADDITIONS
	}
}