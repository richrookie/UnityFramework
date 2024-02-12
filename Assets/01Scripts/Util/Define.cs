public class Define
{
    public static float BOUND = 0.5f;


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

    public enum CameraMode : byte
    {

    }

    public enum eSound : byte
    {
        Bgm,
        Effect,
        MaxCount
    }

    public enum ObjectType : byte
    {
        NotAssigned,
        GameObject,
        TextMesh,
        Image,
        Button,
        Text,
    }
}
