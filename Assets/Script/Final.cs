using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Final : MonoBehaviour
{
    public Transform[] targetPositions;
    public float detectionRadius = 200f;
    private int playerRank = 0;

    private Player player;
    private Enemy[] enemies;

    private void Start()
    {
        player = FindObjectOfType<Player>();
        enemies = FindObjectsOfType<Enemy>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out CharacterBase character)) return;

        player.Final();
        foreach (var enemy in enemies)
        {
            enemy.StopAllActions();
        }

        List<CharacterBase> nearbyPlayers = FindNearbyPlayers(character);
        if (nearbyPlayers.Count == 4 && targetPositions.Length == 3)
        {
            playerRank = AssignPlayersToPositions(nearbyPlayers, player);
            Invoke(nameof(DelayCanvasFinal), 2);
            gameObject.SetActive(false);
        }
    }

    private void DelayCanvasFinal()
    {
        UIManager.Instance.CloseALL();
        CanvasVitoty victoryCanvas = UIManager.Instance.OpenUI<CanvasVitoty>();
        victoryCanvas.SetRanking(playerRank);
        victoryCanvas.SetScore(player.listBrickHiden.Count);
    }

    private List<CharacterBase> FindNearbyPlayers(CharacterBase currentPlayer)
    {
        List<CharacterBase> players = new List<CharacterBase>();
        Collider[] hitColliders = Physics.OverlapSphere(currentPlayer.transform.position, detectionRadius);

        foreach (Collider hit in hitColliders)
        {
            CharacterBase player = hit.GetComponent<CharacterBase>();
            if (player != null && !players.Contains(player))
            {
                players.Add(player);
            }
        }

        if (!players.Contains(currentPlayer))
        {
            players.Add(currentPlayer);
        }

        players.Sort((p1, p2) =>
            Vector3.Distance(p1.transform.position, transform.position)
            .CompareTo(Vector3.Distance(p2.transform.position, transform.position))
        );

        return players.Take(4).ToList();
    }

    private int AssignPlayersToPositions(List<CharacterBase> players, Player player)
    {
        players.Sort((p1, p2) =>
            Vector3.Distance(p1.transform.position, transform.position)
            .CompareTo(Vector3.Distance(p2.transform.position, transform.position))
        );

        CharacterBase farthestPlayer = players[players.Count - 1];
        players.RemoveAt(players.Count - 1);

        HashSet<int> takenPositions = new HashSet<int>();
        int playerRank = 4;

        for (int i = 0; i < players.Count; i++)
        {
            players[i].transform.position = targetPositions[i].position;
            takenPositions.Add(i);

            if (players[i] == player)
            {
                playerRank = i + 1;
            }
        }

        return playerRank;
    }
}
