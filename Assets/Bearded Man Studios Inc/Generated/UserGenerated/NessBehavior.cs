using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Unity;
using UnityEngine;

namespace BeardedManStudios.Forge.Networking.Generated
{
	[GeneratedRPC("{\"types\":[[\"Vector2\"][\"string\"][\"Vector2\"][\"Vector2\"][\"Vector2\"]]")]
	[GeneratedRPCVariableNames("{\"types\":[[\"destination\"][\"text\"][\"target\"][\"target\"][\"target\"]]")]
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
			networkObject.RegisterRpc("Take", Take, typeof(Vector2));
			networkObject.RegisterRpc("Place", Place, typeof(Vector2));
			networkObject.RegisterRpc("Interact", Interact, typeof(Vector2));
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
		/// <summary>
		/// Arguments:
		/// Vector2 target
		/// </summary>
		public abstract void Take(RpcArgs args);
		/// <summary>
		/// Arguments:
		/// Vector2 target
		/// </summary>
		public abstract void Place(RpcArgs args);
		/// <summary>
		/// Arguments:
		/// Vector2 target
		/// </summary>
		public abstract void Interact(RpcArgs args);

		// DO NOT TOUCH, THIS GETS GENERATED PLEASE EXTEND THIS CLASS IF YOU WISH TO HAVE CUSTOM CODE ADDITIONS
	}
}