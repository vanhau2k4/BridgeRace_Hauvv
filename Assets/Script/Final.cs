using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.Rendering;

public class Final : MonoBehaviour
{
    public Transform[] targetPositions; // Mảng chứa 3 vị trí mục tiêu.
    public float detectionRadius = 200f; // Bán kính phát hiện.

    Player player;
    Enemy[] enemies;
    SpoinBrick spoinBrick;
    private void Start()
    {
        player = FindObjectOfType<Player>();
        enemies = FindObjectsOfType<Enemy>();
        spoinBrick = FindObjectOfType<SpoinBrick>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out CharacterBase character))
        {
            Invoke(nameof(DelayCanvasFinal), 2);
            gameObject.SetActive(false);
            player.Final();

            for (int i = 0; i < enemies.Length; i++)
            {
                enemies[i].StopAllActions();
            }

                // Tìm người chơi trong phạm vi và gán vị trí nếu đủ điều kiện
                List<CharacterBase> nearbyPlayers = FindNearbyPlayers(character, detectionRadius);

            if (nearbyPlayers.Count == 3 && targetPositions.Length == 3)
            {
                AssignPlayersToPositions(nearbyPlayers);
            }
        }
        
    }

    private void DelayCanvasFinal()
    {
        UIManager.Instance.CloseALL();
        UIManager.Instance.OpenUI<CanvasVitoty>();
    }

    private List<CharacterBase> FindNearbyPlayers(CharacterBase currentPlayer, float range)
    {
        List<CharacterBase> players = new List<CharacterBase>();

        // Tìm các đối tượng trong phạm vi detectionRadius
        Collider[] hitColliders = Physics.OverlapSphere(currentPlayer.transform.position, range);

        foreach (Collider hitCollider in hitColliders)
        {
            if (hitCollider.TryGetComponent(out CharacterBase player) && !players.Contains(player))
            {
                players.Add(player);
            }
        }

        // Đảm bảo luôn có currentPlayer trong danh sách
        if (!players.Contains(currentPlayer))
        {
            players.Add(currentPlayer);
        }

        // Chỉ giữ lại 3 người chơi
        return players.GetRange(0, Mathf.Min(players.Count, 3));
    }

    private void AssignPlayersToPositions(List<CharacterBase> players)
    {
        List<(CharacterBase player, int bestPositionIndex, float bestDistance)> playerDistances = new List<(CharacterBase, int, float)>();

        // Tính khoảng cách của mỗi người chơi đến từng vị trí và tìm vị trí gần nhất
        foreach (var player in players)
        {
            int bestPositionIndex = -1;
            float bestDistance = float.MaxValue;

            for (int i = 0; i < targetPositions.Length; i++)
            {
                float distance = Vector3.Distance(player.transform.position, targetPositions[i].position);
                if (distance < bestDistance)
                {
                    bestDistance = distance;
                    bestPositionIndex = i;
                }
            }

            playerDistances.Add((player, bestPositionIndex, bestDistance));
        }

        // Gán từng người chơi vào vị trí gần nhất mà chưa có ai
        bool[] positionsTaken = new bool[targetPositions.Length]; // Để đánh dấu vị trí đã được sử dụng

        foreach (var playerData in playerDistances)
        {
            int targetIndex = playerData.bestPositionIndex;

            // Tìm vị trí gần nhất chưa bị lấy
            while (positionsTaken[targetIndex])
            {
                targetIndex = (targetIndex + 1) % targetPositions.Length;
            }

            // Đánh dấu vị trí đã được sử dụng và gán vị trí cho người chơi
            positionsTaken[targetIndex] = true;
            playerData.player.transform.position = targetPositions[targetIndex].position;
        }
    }
}
