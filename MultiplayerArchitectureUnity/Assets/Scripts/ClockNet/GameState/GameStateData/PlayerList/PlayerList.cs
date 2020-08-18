using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ClockNet.GameState.GameStateData.PlayerList
{
    /// <summary>
    /// Type of this Player.
    /// </summary>
    public enum PlayerType
    {
        AI,
        HUMAN
    }

    /// <summary>
    /// State of GameState being loaded and up-to-date.
    /// </summary>
    public enum PlayerLoadState
    {
        UNLOADED, //GameState have to be sent
        READY //GameState is in sync and ready to be used by player
    }

    /// <summary>
    /// Contains all fields that should be remembered about a single player (or slot) such as ID, ClientHandleID, type or load state.
    /// </summary>
    [System.Serializable]
    public class PlayerData
    {
        public int ID;
        public int networkID;

        public bool slotTaken;
        public string nickname;

        public PlayerType type;
        public PlayerLoadState loadState;

        public PlayerData(int id)
        {
            ID = id;
            networkID = -1;
            nickname = "Player";
            slotTaken = false;
            type = PlayerType.AI;
            loadState = PlayerLoadState.UNLOADED;
        }
    }

    /// <summary>
    /// Array of PlayerDatas.
    /// </summary>
    [System.Serializable]
    public class PlayerList 
    {
        public PlayerData[] players;
        [System.NonSerialized]
        private PlayerData clientData;


        /// <summary>
        /// Get this client data.
        /// </summary>
        /// <returns>Data of this particular client</returns>
        public PlayerData GetClientData()
        {
            if (clientData != null)
                return clientData;
            else
                return new PlayerData(0);
        }
    
        public PlayerList(int maxPlayers)
        {
            clientData = null;
            players = new PlayerData[maxPlayers];
            for(int i = 0; i<maxPlayers;i++)
            {
                players[i] = new PlayerData(i+1);
            }
        }

        public bool RemovePlayer(int id)
        {
            if (id >= 0 && id < players.Length)
            {
                players[id] = new PlayerData(id);
                return true;
            }

            return false;
        }

        public bool RemovePlayerByNetID(int id)
        {
            for (int i = 0; i < players.Length; i++)
            {
                if (players[i].networkID == id)
                {
                    RemovePlayer(i);
                    return true;
                }
            }
            return false;
        }

        public int JoinPlayer(bool local=true, PlayerType type=PlayerType.AI, string nickname="Player")
        {
            for (int i = 0; i < players.Length; i++)
            {
                if (players[i].slotTaken == false)
                {
                    Debug.Log("Adding player: " + i);
                
                    players[i].type = type;
                    players[i].nickname = nickname;

                    players[i].loadState = PlayerLoadState.READY;

                    players[i].slotTaken = true;

                    players[i].networkID = -1;

                    if(local==false)
                    {
                        players[i].networkID = FindNextEmptySocketID();
                    }

                    if (clientData == null)
                        clientData = players[i];

                    return i;
                }
            }

            return -1;
        }

        public void SwapPlaces(int ida, int idb)
        {
            if (ida == idb) return;
            players[ida - 1].ID = idb;
            players[idb - 1].ID = ida;
            PlayerData temp = players[idb - 1];
            players[idb - 1] = players[ida - 1];
            players[ida - 1] = temp;

        
        }

        public int FindNextEmptySocketID()
        {
            bool[] appeared = new bool[players.Length];

            for (int i = 0; i < players.Length; i++)
            {
                if (players[i].networkID != -1) 
                    appeared[players[i].networkID] = true;
            }

            for (int i = 0; i < players.Length; i++)
            {
                if (appeared[i] == false)
                {
                    return i;
                }
            }

            return -1;
        }
    }
}