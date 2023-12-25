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
        Wait,
        Start,
        Play,
        Clear,
        GameOver,
        End
    }

    public enum eInputMode : byte
    {
        Default,
        Flying,
    }
}
