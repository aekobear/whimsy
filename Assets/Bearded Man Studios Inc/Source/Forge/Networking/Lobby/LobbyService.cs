﻿using BeardedManStudios.Forge.Networking.Frame;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BeardedManStudios.Forge.Networking.Lobby
{
    /// <summary>
    /// The lobby service handles all functionality of handling generic player data
    /// </summary>
    public class LobbyService : INetworkBehavior
    {
        #region Private Data
        private LobbyServiceNetworkObject networkObject = null;
        private bool _initialized;
        #endregion

        #region Properties
        private static LobbyService _instance;
        /// <summary>
        /// The instance of the Lobby Service
        /// </summary>
        public static LobbyService Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new LobbyService();
                return _instance;
            }
        }

        /// <summary>
        /// This is the dummy service we will default to
        /// </summary>
        private DummyLobbyMaster _dummyService;

        private ILobbyMaster _masterLobby;
        /// <summary>
        /// The master lobby in which we will be doing callback to
        /// </summary>
        public ILobbyMaster MasterLobby
        {
            get
            {
                if (_masterLobby == null)
                    _masterLobby = _dummyService; //Always default to the dummy service incase we are null

                return _masterLobby;
            }
        }

        /// <summary>
        /// Whether we are the server or not
        /// </summary>
        public bool IsServer
        {
            get
            {
                if (networkObject != null)
                    return networkObject.IsServer;
                return false;
            }
        }

        private IClientMockPlayer _myself;
        /// <summary>
        /// Reference to my networking player
        /// </summary>
        public IClientMockPlayer MyMockPlayer
        {
            get
            {
                if (_myself == null)
                {
                    for (int i = 0; i < MasterLobby.LobbyPlayers.Count; ++i)
                    {
                        if (MasterLobby.LobbyPlayers[i].NetworkId == networkObject.MyPlayerId)
                        {
                            _myself = MasterLobby.LobbyPlayers[i];
                            break;
                        }
                    }

                    if (_myself == null)
                    {

                    }
                }

                return _myself;
            }
        }

        /// <summary>
        /// Reference to my networking player
        /// </summary>
        public NetworkingPlayer MyNetworkingPlayer
        {
            get
            {
                return networkObject.Networker.Me;
            }
        }
        #endregion

        #region Dummy Service
        /// <summary>
        /// A dummy player object
        /// </summary>
        private class DummyPlayer : IClientMockPlayer
        {
            private uint _networkID;
            public uint NetworkId
            {
                get
                {
                    return _networkID;
                }
                set
                {
                    _networkID = value;
                }
            }

            private string _name;
            public string Name
            {
                get
                {
                    return _name;
                }
                set
                {
                    _name = value;
                }
            }

            public DummyPlayer(uint id, string name)
            {
                NetworkId = id;
                Name = name;
            }
        }

        /// <summary>
        /// The purpose of this dummy lobby is to handle the player data when
        /// the lobby master doesn't exist
        /// </summary>
        private class DummyLobbyMaster : ILobbyMaster
        {
            private List<IClientMockPlayer> _lobbyPlayers = new List<IClientMockPlayer>();
            public List<IClientMockPlayer> LobbyPlayers
            {
                get
                {
                    return _lobbyPlayers;
                }
            }

            private Dictionary<uint, IClientMockPlayer> _lobbyPlayersMap = new Dictionary<uint, IClientMockPlayer>();
            public Dictionary<uint, IClientMockPlayer> LobbyPlayersMap
            {
                get
                {
                    return _lobbyPlayersMap;
                }
            }

            private Dictionary<int, List<IClientMockPlayer>> _lobbyTeams = new Dictionary<int, List<IClientMockPlayer>>();
            public Dictionary<int, List<IClientMockPlayer>> LobbyTeams
            {
                get
                {
                    return _lobbyTeams;
                }
            }

            public DummyLobbyMaster()
            {
                DummyPlayer player = new DummyPlayer(0, "Server");
                LobbyPlayers.Add(player);
                LobbyPlayersMap.Add(0, player);
                LobbyTeams.Add(0, new List<IClientMockPlayer>() { player });
            }

            public void OnFNPlayerConnected(IClientMockPlayer player)
            {
                if (!LobbyPlayers.Contains(player))
                {
                    LobbyPlayers.Add(player);
                    LobbyPlayersMap.Add(player.NetworkId, player);
                }
            }

            public void OnFNPlayerDisconnected(IClientMockPlayer player)
            {
                if (LobbyPlayers.Contains(player))
                {
                    LobbyPlayers.Remove(player);
                    LobbyPlayersMap.Remove(player.NetworkId);
                }
            }

            public void OnFNPlayerNameChanged(IClientMockPlayer player)
            {
                //We don't care about the name change since we are a dummy class
            }

            public void OnFNTeamChanged(IClientMockPlayer player, int newId)
            {
                if (!LobbyTeams.ContainsKey(newId))
                    LobbyTeams.Add(newId, new List<IClientMockPlayer>());

                //We do this to not make Foreach loops
                IEnumerator iter = LobbyTeams.GetEnumerator();
                iter.Reset();
                while (iter.MoveNext())
                {
                    if (iter.Current != null)
                    {
                        KeyValuePair<int, List<IClientMockPlayer>> kv = (KeyValuePair<int, List<IClientMockPlayer>>)iter.Current;
                        if (kv.Value.Contains(player))
                        {
                            kv.Value.Remove(player);
                            break;
                        }
                    }
                    else
                        break;
                }

                //We prevent the player being added twice to the same team
                if (!LobbyTeams[newId].Contains(player))
                    LobbyTeams[newId].Add(player);
            }

            public void OnFNAvatarIDChanged(IClientMockPlayer player, int newId)
            {
                //We don't care about the avatar id change because we are a dummy class
            }

            public void OnFNLobbyMasterKnowledgeTransfer(ILobbyMaster previousLobbyMaster)
            {
                LobbyPlayers.Clear();
                LobbyPlayersMap.Clear();
                LobbyTeams.Clear();
                for (int i = 0; i < previousLobbyMaster.LobbyPlayers.Count; ++i)
                {
                    IClientMockPlayer player = previousLobbyMaster.LobbyPlayers[i];
                    LobbyPlayers.Add(player);
                    LobbyPlayersMap.Add(player.NetworkId, player);
                }

                IEnumerator iter = previousLobbyMaster.LobbyTeams.GetEnumerator();
                iter.Reset();
                while (iter.MoveNext())
                {
                    if (iter.Current != null)
                    {
                        KeyValuePair<int, List<IClientMockPlayer>> kv = (KeyValuePair<int, List<IClientMockPlayer>>)iter.Current;
                        LobbyTeams.Add(kv.Key, kv.Value);
                    }
                    else
                        break;
                }
                foreach (KeyValuePair<int, List<IClientMockPlayer>> kv in previousLobbyMaster.LobbyTeams)
                {
                    LobbyTeams.Add(kv.Key, kv.Value);
                }
            }

            public void OnFNLobbyPlayerMessageReceived(IClientMockPlayer player, string message)
            {
                //We ignore all messages we have received from other players because we are a dummy class
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// We are a private constructor because we don't want
        /// to be created outside of the library
        /// </summary>
        private LobbyService()
        {
            _dummyService = new DummyLobbyMaster();
            SetLobbyMaster(_dummyService); //We set the dummy service by default
        }
        #endregion

        #region Nested Clases
        public class LobbyServiceNetworkObject : NetworkObject
        {
            public const int IDENTITY = -50;
            private byte[] _dirtyFields = new byte[0];

            public override int UniqueIdentity
            {
                get { return IDENTITY; }
            }

            protected override BMSByte WritePayload(BMSByte data)
            {
                return data;
            }

            protected override void ReadPayload(BMSByte payload, ulong timestep)
            {
            }

            protected override BMSByte SerializeDirtyFields()
            {
                dirtyFieldsData.Clear();
                dirtyFieldsData.Append(_dirtyFields);

                return dirtyFieldsData;
            }

            protected override void ReadDirtyFields(BMSByte data, ulong timestep)
            {
                Buffer.BlockCopy(data.byteArr, data.StartIndex(), readDirtyFlags, 0, readDirtyFlags.Length);
                data.MoveStartIndex(readDirtyFlags.Length);
            }

            public override void InterpolateUpdate()
            {
                if (IsOwner)
                    return;

            }

            private void Initialize()
            {
                readDirtyFlags = new byte[0];

            }

            public LobbyServiceNetworkObject() : base() { Initialize(); }
            public LobbyServiceNetworkObject(NetWorker networker, INetworkBehavior networkBehavior = null, int createCode = 0) : base(networker, networkBehavior, createCode) { Initialize(); }
            public LobbyServiceNetworkObject(NetWorker networker, uint serverId, FrameStream frame) : base(networker, serverId, frame) { Initialize(); }
        }
        #endregion

        #region Public API
        /// <summary>
        /// Set the lobby master
        /// </summary>
        /// <param name="lobbyMaster">The next lobby master</param>
        public void SetLobbyMaster(ILobbyMaster lobbyMaster)
        {
            //Ignore setting the same lobby twice
            if (lobbyMaster == _masterLobby)
                return;

            if (lobbyMaster != null)
            {
                if (_masterLobby != null)
                {
                    //We update the lobby with the previous one's information because it may be more up to date
                    lobbyMaster.OnFNLobbyMasterKnowledgeTransfer(_masterLobby);
                    _masterLobby = lobbyMaster;
                }
                else
                    _masterLobby = lobbyMaster;
            }
        }

        /// <summary>
        /// Assigns a new name for yourself
        /// </summary>
        /// <param name="newName">The next name you will be referred as</param>
        public void SetName(string newName)
        {
            networkObject.SendRpc("AssignName",
                true,   // Replace previous
                Receivers.AllBuffered,
                newName,
                networkObject.MyPlayerId);
        }

        /// <summary>
        /// Set your avatar to a new id
        /// </summary>
        /// <param name="avatarID">Next avatar id</param>
        public void SetAvatar(int avatarID)
        {
            networkObject.SendRpc("AssignAvatar", Receivers.All, networkObject.MyPlayerId, avatarID);
        }

        /// <summary>
        /// Sets your team id
        /// </summary>
        /// <param name="teamId">The new team id</param>
        public void SetTeamId(int teamId)
        {
            // TODO:  When someone joins they need to get the current players selections
            networkObject.SendRpc("AssignTeam", Receivers.All, networkObject.MyPlayerId, teamId);
        }

        /// <summary>
        /// Send a message to everyone connected
        /// </summary>
        /// <param name="message">The message you want to send</param>
        public void SendPlayerMessage(string message)
        {
            // TODO:  When someone joins they need to get the current players selections
            networkObject.SendRpc("MessageReceived", Receivers.All, networkObject.MyPlayerId, message);
        }

        public void KickPlayer(uint id)
        {
            if (networkObject.IsOwner && networkObject.IsServer)
            {
                //TODO: I am the server, so disconnect the id passed in here!
            }
        }

        /// <summary>
        /// Whether the id matches my player id
        /// </summary>
        /// <param name="id">Player id</param>
        /// <returns>True/False depending on the id passed in</returns>
        public bool MatchesMe(uint id)
        {
            return networkObject.MyPlayerId == id;
        }
        #endregion

        #region Network Behavior
        public void Initialize(NetworkObject obj)
        {
            // We have already initialized this object
            if (_initialized)
                return;

            networkObject = (LobbyServiceNetworkObject)obj;
            networkObject.AttachedBehavior = this;

            networkObject.RegisterRpc("AssignName", AssignName, typeof(string), typeof(uint));
            networkObject.RegisterRpc("PlayerJoined", PlayerJoined, typeof(uint));
            networkObject.RegisterRpc("PlayerLeft", PlayerLeft, typeof(uint));
            networkObject.RegisterRpc("AssignAvatar", AssignAvatar, typeof(uint), typeof(int));
            networkObject.RegisterRpc("AssignTeam", AssignTeam, typeof(uint), typeof(int));
            networkObject.RegisterRpc("MessageReceived", MessageReceived, typeof(uint), typeof(string));
            networkObject.RegistrationComplete();
            _initialized = true;

            //Logging.BMSLog.Log("SERVICE ID: " + networkObject.NetworkId);

            NetworkStart();
        }

        public void Initialize(NetWorker networker)
        {
            Initialize(new LobbyServiceNetworkObject(networker));
        }

        private void NetworkStart()
        {
            if (!networkObject.IsServer)
                return;

            networkObject.Networker.playerAccepted += PlayerConnected;
            networkObject.Networker.playerDisconnected += PlayerDisconnected;
        }

        /// <summary>
        /// Arguments:
        /// string playername
        /// uint playerid
        /// </summary>
        private void AssignName(RpcArgs args)
        {
            string playerName = args.GetNext<string>();
            uint playerId = args.GetNext<uint>();
            var player = GetClientMockPlayer(playerId);

            if (player == null)
                return;

            player.Name = playerName;

            if (networkObject.IsServer)
                args.Info.SendingPlayer.Name = playerName;

            MasterLobby.OnFNPlayerNameChanged(player);
        }

        public void PlayerConnected(IClientMockPlayer player)
        {
            //Logging.BMSLog.Log("SEVERRRR: " + player.NetworkId);
            player.Name = "Player " + player.NetworkId;
            networkObject.SendRpc("PlayerJoined", Receivers.AllBuffered, player.NetworkId);
        }
        /// <summary>
        /// Arguments:
        /// uint playerid
        /// </summary>
        private void PlayerJoined(RpcArgs args)
        {
            uint playerId = args.GetNext<uint>();
            var player = CreateClientMockPlayer(playerId, "Player " + playerId);
            //Logging.BMSLog.Log("PLAYER JOINED!! :" + playerId);

            MasterLobby.OnFNPlayerConnected(player);
        }
        /// <summary>
        /// Arguments:
        /// uint playerid
        /// </summary>
        private void PlayerLeft(RpcArgs args)
        {
            uint playerId = args.GetNext<uint>();
            var player = GetClientMockPlayer(playerId);

            if (player == null)
                return;

            MasterLobby.OnFNPlayerDisconnected(player);
        }
        /// <summary>
        /// Arguments:
        /// uint playerid
        /// int avatarid
        /// </summary>
        private void AssignAvatar(RpcArgs args)
        {
            uint playerId = args.GetNext<uint>();
            int avatarId = args.GetNext<int>();
            var player = GetClientMockPlayer(playerId);

            if (player == null)
                return;

            MasterLobby.OnFNAvatarIDChanged(player, avatarId);
        }
        /// <summary>
        /// Arguments:
        /// uint playerid
        /// int teamid
        /// </summary>
        private void AssignTeam(RpcArgs args)
        {
            uint playerId = args.GetNext<uint>();
            int teamId = args.GetNext<int>();
            var player = GetClientMockPlayer(playerId);

            if (player == null)
                return;

            MasterLobby.OnFNTeamChanged(player, teamId);
        }
        /// <summary>
        /// Arguments:
        /// uint playerid
        /// string message
        /// </summary>
        private void MessageReceived(RpcArgs args)
        {
            uint playerId = args.GetNext<uint>();
            string message = args.GetNext<string>();
            var player = GetClientMockPlayer(playerId);

            if (player == null)
                return;

            MasterLobby.OnFNLobbyPlayerMessageReceived(player, message);
        }

        private IClientMockPlayer CreateClientMockPlayer(uint playerId, string playerName)
        {
            var player = new DummyPlayer(playerId, playerName);
            return player;
        }

        private IClientMockPlayer GetClientMockPlayer(uint playerId)
        {
            var targetPlayer = MasterLobby.LobbyPlayers.First(c => c.NetworkId == playerId);
            return targetPlayer;
        }

        private void PlayerConnected(NetworkingPlayer player)
        {
            //Logging.BMSLog.Log("GG: " + player.Ip);
            player.Name = "Player " + player.NetworkId;
            networkObject.SendRpc("PlayerJoined", Receivers.AllBuffered, player.NetworkId);
        }

        private void PlayerDisconnected(NetworkingPlayer player)
        {
            // TODO:  This should be called
            //Logging.BMSLog.Log("OH NO: " + player.Ip);
            //BeardedManStudios.Forge.Logging.BMSLog.Log("Player disconnected");
            networkObject.SendRpc("PlayerLeft", Receivers.AllBuffered, player.NetworkId);
        }
        #endregion
    }
}
