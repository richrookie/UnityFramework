public class Define
{
    public enum eScene : byte
    {
        UnKnown,
        Login,
        Lobby,
        Game
    }

    public enum eGameState : byte
    {
        Ready,
        Start,
        Play,
        Clear,
        GameOver,
        Wait,
        End
    }
}
