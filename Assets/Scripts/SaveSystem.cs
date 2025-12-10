using UnityEngine;
using System.Collections.Generic;

public static class SaveSystem
{
    // ---------------- PLAYER ----------------

    public static void SavePlayer(Vector3 pos)
    {
        PlayerPrefs.SetFloat("p_x", pos.x);
        PlayerPrefs.SetFloat("p_y", pos.y);
        PlayerPrefs.SetFloat("p_z", pos.z);
    }

    public static Vector3 LoadPlayer()
    {
        return new Vector3(
            PlayerPrefs.GetFloat("p_x", 0),
            PlayerPrefs.GetFloat("p_y", 0),
            PlayerPrefs.GetFloat("p_z", 0)
        );
    }

    // ---------------- ENEMIES ----------------

    public static void SaveEnemies(List<EnemySaveData> list)
    {
        PlayerPrefs.SetInt("enemy_count", list.Count);

        for (int i = 0; i < list.Count; i++)
        {
            PlayerPrefs.SetFloat($"e{i}_x", list[i].position.x);
            PlayerPrefs.SetFloat($"e{i}_y", list[i].position.y);
            PlayerPrefs.SetFloat($"e{i}_z", list[i].position.z);

            PlayerPrefs.SetInt($"e{i}_dead", list[i].dead ? 1 : 0);
        }

        PlayerPrefs.Save();
    }

    public static List<EnemySaveData> LoadEnemies()
    {
        List<EnemySaveData> result = new List<EnemySaveData>();

        int count = PlayerPrefs.GetInt("enemy_count", 0);

        for (int i = 0; i < count; i++)
        {
            Vector3 pos = new Vector3(
                PlayerPrefs.GetFloat($"e{i}_x"),
                PlayerPrefs.GetFloat($"e{i}_y"),
                PlayerPrefs.GetFloat($"e{i}_z")
            );

            bool dead = PlayerPrefs.GetInt($"e{i}_dead") == 1;

            result.Add(new EnemySaveData(pos, dead));
        }

        return result;
    }

    // ---------------- RESET ----------------
    public static void ResetSave()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }
}

[System.Serializable]
public class EnemySaveData
{
    public Vector3 position;
    public bool dead;

    public EnemySaveData(Vector3 pos, bool deadState)
    {
        position = pos;
        dead = deadState;
    }
}
