using UnityEngine;

public static class Define
{
    public enum EnemyState
    {
        Idle,
        Move,
        Attack,
        Die
    }

    public static int waveCount { get; set; } = 0;
    public static int Gold { get; set; } = 0;    

}
