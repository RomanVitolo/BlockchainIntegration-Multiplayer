using System;
using UnityEngine;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using TMPro;
using UnityEngine.U2D.Animation;

public class Agent : NetworkBehaviour
{
    public static string LocalUserName;
    public static int LocalCharacterIndex;
    
    [SerializeField] private TextMeshProUGUI _userNameText;
    [SerializeField] private SpriteLibraryAsset[] _characterSpriteLibrary;

    [SyncVar(OnChange = nameof(OnUsernameChanged))]
    public string UserName;
    private void OnUsernameChanged(string oldValue, string newValue, bool isServer)
    {
        _userNameText.text = newValue;
    }

    [SyncVar(OnChange = nameof(OnCharacterIndexChanged))]
    public int CharacterIndex;
    private void OnCharacterIndexChanged(int oldValue, int newValue, bool isServer)
    {
        GetComponent<SpriteLibrary>().spriteLibraryAsset = _characterSpriteLibrary[newValue];
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        if (!IsOwner) return;
        
        ServerSetName(LocalUserName);
        ServerSetCharacterIndex(LocalCharacterIndex);
    }

    [ServerRpc]
    private void ServerSetName(string userName)
    {
        UserName = userName;
    }
    [ServerRpc]
    private void ServerSetCharacterIndex(int index)
    {
        CharacterIndex = index;
    }
}
