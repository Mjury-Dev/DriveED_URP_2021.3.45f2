using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LeaderboardTable : MonoBehaviour
{
    private Transform entryContainer;
    private Transform entryTemplate;

    private List<LeaderboardEntry> leaderboardEntryList;
    private List<Transform> leaderboardEntryTransformList;

    private const int MAX_ENTRIES = 10;

    private void Awake()
    {
        entryContainer = transform.Find("leaderboardEntryContainer");
        entryTemplate = entryContainer.Find("leaderboardEntryTemplate");
        entryTemplate.gameObject.SetActive(false);

        LoadLeaderboard();
        SortAndTrimLeaderboard();
        CreateLeaderboardUI();
        SaveLeaderboard();
    }

    // ================= PUBLIC API =================

    public void AddEntry(string name, int score)
    {
        leaderboardEntryList.Add(new LeaderboardEntry
        {
            name = name,
            score = score
        });

        SortAndTrimLeaderboard();
        RefreshLeaderboardUI();
        SaveLeaderboard();
    }

    // ================= CORE LOGIC =================

    private void LoadLeaderboard()
    {
        if (PlayerPrefs.HasKey("leaderboardTable"))
        {
            string json = PlayerPrefs.GetString("leaderboardTable");
            Leaderboard leaderboard = JsonUtility.FromJson<Leaderboard>(json);
            leaderboardEntryList = leaderboard.leaderboardEntryList;
        }
        else
        {
            leaderboardEntryList = new List<LeaderboardEntry>();
        }
    }

    private void SaveLeaderboard()
    {
        Leaderboard leaderboard = new Leaderboard
        {
            leaderboardEntryList = leaderboardEntryList
        };

        PlayerPrefs.SetString("leaderboardTable", JsonUtility.ToJson(leaderboard));
        PlayerPrefs.Save();
    }

    private void SortAndTrimLeaderboard()
    {
        leaderboardEntryList.Sort((a, b) => b.score.CompareTo(a.score));

        if (leaderboardEntryList.Count > MAX_ENTRIES)
        {
            leaderboardEntryList.RemoveRange(
                MAX_ENTRIES,
                leaderboardEntryList.Count - MAX_ENTRIES
            );
        }
    }

    // ================= UI =================

    private void CreateLeaderboardUI()
    {
        leaderboardEntryTransformList = new List<Transform>();

        foreach (LeaderboardEntry entry in leaderboardEntryList)
        {
            CreateLeaderboardEntryTransform(entry);
        }
    }

    private void RefreshLeaderboardUI()
    {
        foreach (Transform t in leaderboardEntryTransformList)
        {
            Destroy(t.gameObject);
        }

        CreateLeaderboardUI();
    }

    private void CreateLeaderboardEntryTransform(LeaderboardEntry entry)
    {
        float templateHeight = 30f;

        Transform entryTransform = Instantiate(entryTemplate, entryContainer);
        RectTransform rect = entryTransform.GetComponent<RectTransform>();

        rect.anchoredPosition =
            new Vector2(0, -templateHeight * leaderboardEntryTransformList.Count);

        entryTransform.gameObject.SetActive(true);

        int rank = leaderboardEntryTransformList.Count + 1;

        entryTransform.Find("posText")
            .GetComponent<TextMeshProUGUI>().text = GetRankString(rank);

        entryTransform.Find("scoreText")
            .GetComponent<TextMeshProUGUI>().text = entry.score.ToString();

        entryTransform.Find("nameText")
            .GetComponent<TextMeshProUGUI>().text = entry.name;

        leaderboardEntryTransformList.Add(entryTransform);
    }

    private string GetRankString(int rank)
    {
        switch (rank)
        {
            case 1: return "1ST";
            case 2: return "2ND";
            case 3: return "3RD";
            default: return rank + "TH";
        }
    }

    // ================= DATA =================

    [System.Serializable]
    private class Leaderboard
    {
        public List<LeaderboardEntry> leaderboardEntryList;
    }

    [System.Serializable]
    private class LeaderboardEntry
    {
        public int score;
        public string name;
    }
}

/* To add entries from other scripts example code: FindObjectOfType<LeaderboardTable>().AddEntry("YOU", 123);
 * 
 * leaderboard currently filled with place holders
*/