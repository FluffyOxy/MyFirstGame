using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardRoom : Room
{
    [SerializeField] private Transform rewardTransform;

    protected override void PreGenerateRoom(MapGenerateManager _manager, Line _currentLine, int _index)
    {
        type = RoomType.Reward;

        base.PreGenerateRoom(_manager, _currentLine, _index);
    }

    protected override void GenerateCurrentRoom(MapGenerateManager _manager, Line _currentLine, int _index)
    {
        base.GenerateCurrentRoom(_manager, _currentLine, _index);

        RewardSlot slot = _currentLine.lineEndReward;
        if (!_currentLine.isEndRoom)
        {
            slot = _currentLine.rewards[_currentLine.rewardIndex];
        }

        List<Drop> drops = null;
        if(Random.Range(0f, 100f) < slot.mimicRate)
        {
            if(Random.Range(0f, 100f) < slot.mimicAdvancedRewardRate)
            {
                drops = _manager.GetAdvancedRewards(slot.advancedAmount);
            }
            else
            {
                drops = _manager.GetPrimaryRewards(slot.rewardAmount);
            }
            Enemy_Mimic newMimic = 
                Instantiate(
                    _manager.mimicChestPrefab, rewardTransform.position, Quaternion.identity
                ).GetComponent<Enemy_Mimic>();
            newMimic.SetDrops(drops);
        }
        else
        {
            if(Random.Range(0f, 100f) < slot.advancedRewardRate)
            {
                drops = _manager.GetAdvancedRewards(slot.advancedAmount);
                Chest newChest =
                    Instantiate(
                        _manager.advancedRewardChestPrefab, rewardTransform.position, Quaternion.identity
                    ).GetComponent<Chest>();
                newChest.SetDrops(drops);
            }
            else
            {
                drops = _manager.GetPrimaryRewards(slot.rewardAmount);
                Chest newChest =
                    Instantiate(
                        _manager.primaryRewardChestPrefab, rewardTransform.position, Quaternion.identity
                    ).GetComponent<Chest>();
                newChest.SetDrops(drops);
            }
        }
    }
}
