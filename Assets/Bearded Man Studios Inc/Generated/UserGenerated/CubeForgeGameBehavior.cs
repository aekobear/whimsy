using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Unity;
using UnityEngine;

namespace BeardedManStudios.Forge.Networking.Generated
{
	[GeneratedRPC("{\"types\":[[\"Vector3\", \"Vector3\", \"byte[]\"][\"byte\", \"Vector3\"][\"Vector3\"][\"string\"]]")]
	[GeneratedRPCVariableNames("{\"types\":[[\"minimum\", \"maximum\", \"data\"][\"typebyte\", \"position\"][\"position\"][\"message\"]]")]
	public abstract partial class CubeForgeGameBehavior : NetworkBehavior
	{
		public CubeForgeGameNetworkObject networkObject = null;

		public override void Initialize(NetworkObject obj)
		{
			// We have already initialized this object
			if (networkObject != null && networkObject.AttachedBehavior != null)
				return;
			
			networkObject = (CubeForgeGameNetworkObject)obj;
			networkObject.AttachedBehavior = this;

			base.SetupHelperRpcs(networkObject);
			networkObject.RegisterRpc("InitializeMap", InitializeMap, typeof(Vector3), typeof(Vector3), typeof(byte[]));
			networkObject.RegisterRpc("CreatePrimitive", CreatePrimitive, typeof(byte), typeof(Vector3));
			networkObject.RegisterRpc("DestroyPrimitive", DestroyPrimitive, typeof(Vector3));
			networkObject.RegisterRpc("TestMe", TestMe, typeof(string));
			networkObject.RegistrationComplete();

			MainThreadManager.Run(NetworkStart);

			networkObject.onDestroy += DestroyGameObject;
		}

		public override void Initialize(NetWorker networker)
		{
			Initialize(new CubeForgeGameNetworkObject(networker, createCode: TempAttachCode));
		}

		private void DestroyGameObject()
		{
			MainThreadManager.Run(() => { Destroy(gameObject); });
			networkObject.onDestroy -= DestroyGameObject;
		}

		/// <summary>
		/// Arguments:
		/// Vector3 minimum
		/// Vector3 maximum
		/// byte[] data
		/// </summary>
		public abstract void InitializeMap(RpcArgs args);
		/// <summary>
		/// Arguments:
		/// byte typebyte
		/// Vector3 position
		/// </summary>
		public abstract void CreatePrimitive(RpcArgs args);
		/// <summary>
		/// Arguments:
		/// Vector3 position
		/// </summary>
		public abstract void DestroyPrimitive(RpcArgs args);
		/// <summary>
		/// Arguments:
		/// string message
		/// </summary>
		public abstract void TestMe(RpcArgs args);

		// DO NOT TOUCH, THIS GETS GENERATED PLEASE EXTEND THIS CLASS IF YOU WISH TO HAVE CUSTOM CODE ADDITIONS
	}
}