using UnityEngine;

public static class CharacterSelectData
{
    // 1Pが選んだキャラクター
    public static GameObject Player1Character;

    // 2Pが選んだキャラクター
    public static GameObject Player2Character;

    // 選択データをリセット
    public static void Clear()
    {
        Player1Character = null;
        Player2Character = null;
    }
}