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

    public enum UIEvent : byte
    {
        Click,
        Drag
    }

    public enum MouseEvent : byte
    {
        Press,
        Click
    }

    public enum CameraMode
    {

    }

    public enum eSound
    {
        Bgm,
        Effect,
        MaxCount
    }
}
