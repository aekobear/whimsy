using BeardedManStudios.Forge.Networking.Generated;
using System;
using UnityEngine;

namespace BeardedManStudios.Forge.Networking.Unity
{
	public partial class NetworkManager : MonoBehaviour
	{
		public delegate void InstantiateEvent(INetworkBehavior unityGameObject, NetworkObject obj);
		public event InstantiateEvent objectInitialized;

		public GameObject[] ChatManagerNetworkObject = null;
		public GameObject[] CubeForgeGameNetworkObject = null;
		public GameObject[] NessNetworkObject = null;
		public GameObject[] NetworkCameraNetworkObject = null;

		private void Start()
		{
			NetworkObject.objectCreated += (obj) =>
			{
				if (obj.CreateCode < 0)
					return;
				
				if (obj is ChatManagerNetworkObject && ChatManagerNetworkObject.Length > 0 && ChatManagerNetworkObject[obj.CreateCode] != null)
				{
					MainThreadManager.Run(() =>
					{
						var go = Instantiate(ChatManagerNetworkObject[obj.CreateCode]);
						var newObj = go.GetComponent<NetworkBehavior>();
						newObj.Initialize(obj);

						if (objectInitialized != null)
							objectInitialized(newObj, obj);
					});
				}
				else if (obj is CubeForgeGameNetworkObject && CubeForgeGameNetworkObject.Length > 0 && CubeForgeGameNetworkObject[obj.CreateCode] != null)
				{
					MainThreadManager.Run(() =>
					{
						var go = Instantiate(CubeForgeGameNetworkObject[obj.CreateCode]);
						var newObj = go.GetComponent<NetworkBehavior>();
						newObj.Initialize(obj);

						if (objectInitialized != null)
							objectInitialized(newObj, obj);
					});
				}
				else if (obj is NessNetworkObject && NessNetworkObject.Length > 0 && NessNetworkObject[obj.CreateCode] != null)
				{
					MainThreadManager.Run(() =>
					{
						var go = Instantiate(NessNetworkObject[obj.CreateCode]);
						var newObj = go.GetComponent<NetworkBehavior>();
						newObj.Initialize(obj);

						if (objectInitialized != null)
							objectInitialized(newObj, obj);
					});
				}
				else if (obj is NetworkCameraNetworkObject && NetworkCameraNetworkObject.Length > 0 && NetworkCameraNetworkObject[obj.CreateCode] != null)
				{
					MainThreadManager.Run(() =>
					{
						var go = Instantiate(NetworkCameraNetworkObject[obj.CreateCode]);
						var newObj = go.GetComponent<NetworkBehavior>();
						newObj.Initialize(obj);

						if (objectInitialized != null)
							objectInitialized(newObj, obj);
					});
				}
			};
		}

		private void InitializedObject(INetworkBehavior behavior, NetworkObject obj)
		{
			if (objectInitialized != null)
				objectInitialized(behavior, obj);

			obj.pendingInitialized -= InitializedObject;
		}

		public ChatManagerBehavior InstantiateChatManagerNetworkObject(int index = 0, Vector3? position = null, Quaternion? rotation = null)
		{
			var go = Instantiate(ChatManagerNetworkObject[index]);
			var netBehavior = go.GetComponent<NetworkBehavior>() as ChatManagerBehavior;
			var obj = new ChatManagerNetworkObject(Networker, netBehavior, index);
			go.GetComponent<ChatManagerBehavior>().networkObject = obj;

			FinializeInitialization(go, netBehavior, obj, position, rotation);
			
			return netBehavior;
		}

		public CubeForgeGameBehavior InstantiateCubeForgeGameNetworkObject(int index = 0, Vector3? position = null, Quaternion? rotation = null)
		{
			var go = Instantiate(CubeForgeGameNetworkObject[index]);
			var netBehavior = go.GetComponent<NetworkBehavior>() as CubeForgeGameBehavior;
			var obj = new CubeForgeGameNetworkObject(Networker, netBehavior, index);
			go.GetComponent<CubeForgeGameBehavior>().networkObject = obj;

			FinializeInitialization(go, netBehavior, obj, position, rotation);
			
			return netBehavior;
		}

		public NessBehavior InstantiateNessNetworkObject(int index = 0, Vector3? position = null, Quaternion? rotation = null)
		{
			var go = Instantiate(NessNetworkObject[index]);
			var netBehavior = go.GetComponent<NetworkBehavior>() as NessBehavior;
			var obj = new NessNetworkObject(Networker, netBehavior, index);
			go.GetComponent<NessBehavior>().networkObject = obj;

			FinializeInitialization(go, netBehavior, obj, position, rotation);
			
			return netBehavior;
		}

		public NetworkCameraBehavior InstantiateNetworkCameraNetworkObject(int index = 0, Vector3? position = null, Quaternion? rotation = null)
		{
			var go = Instantiate(NetworkCameraNetworkObject[index]);
			var netBehavior = go.GetComponent<NetworkBehavior>() as NetworkCameraBehavior;
			var obj = new NetworkCameraNetworkObject(Networker, netBehavior, index);
			go.GetComponent<NetworkCameraBehavior>().networkObject = obj;

			FinializeInitialization(go, netBehavior, obj, position, rotation);
			
			return netBehavior;
		}

	}
}