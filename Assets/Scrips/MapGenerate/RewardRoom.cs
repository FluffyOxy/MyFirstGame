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

        float dice = Random.Range(0, 100);
        float rate = 0;
        if(dice < (rate += slot.witcherRate))
        {
            Instantiate(_manager.witcherPrefab, rewardTransform.position, Quaternion.identity);
        }
        else if(dice < (rate += slot.traderRate))
        {
            Instantiate(_manager.traderPrefab, rewardTransform.position, Quaternion.identity);
        }
        else
        {
            GenerateRewardBox(_manager, slot);
        }
        
    }

    private void GenerateRewardBox(MapGenerateManager _manager, RewardSlot slot)
    {
        List<Drop> drops = new List<Drop>();
        if (Random.Range(0f, 100f) < slot.mimicRate)
        {
            GenerateMimic(_manager, slot, drops);
        }
        else
        {
            drops.AddRange(_manager.GetPrimaryRewards(slot.rewardAmount));
            if (Random.Range(0f, 100f) < slot.advancedRewardRate)
            {
                drops.AddRange(_manager.GetAdvancedRewards(slot.advancedAmount));
                Chest newChest =
                    Instantiate(
                        _manager.advancedRewardChestPrefab, rewardTransform.position, Quaternion.identity
                    ).GetComponent<Chest>();
                newChest.SetDrops(drops);
            }
            else
            {
                Chest newChest =
                    Instantiate(
                        _manager.primaryRewardChestPrefab, rewardTransform.position, Quaternion.identity
                    ).GetComponent<Chest>();
                newChest.SetDrops(drops);
            }
        }
    }
    private void GenerateMimic(MapGenerateManager _manager, RewardSlot slot, List<Drop> drops)
    {
        drops.AddRange(_manager.GetPrimaryRewards(slot.rewardAmount));
        if (Random.Range(0f, 100f) < slot.mimicAdvancedRewardRate)
        {
            drops.AddRange(_manager.GetAdvancedRewards(slot.advancedAmount));
        }

        Enemy_Mimic newMimic =
            Instantiate(
                _manager.mimicChestPrefab, rewardTransform.position, Quaternion.identity
            ).GetComponent<Enemy_Mimic>();
        newMimic.SetDrops(drops);
    }
}
