using UnityEngine.Events;

public class Events
{
    [System.Serializable] public class EventBowState : UnityEvent <PlayerController.BowState, PlayerController.BowState> { }
   // [System.Serializable] public class EventGameState : UnityEvent<GameManager.GameState, GameManager.GameState> { }

}
