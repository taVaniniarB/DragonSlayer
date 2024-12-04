using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    Exploration,    // 일반 전투 상태
    PreparingBoss,  // 보스전 조건을 충족했지만 보스 구역에 진입하지 않은 상태
    BossBattle,     // 보스전 상태
    Victory         // 승리
}
