using UnityEngine;

public enum InterectionType
{
    Conversation,
    Nothing
}

// 인터렉션 개발
public interface IInterectable
{
    InterectionType Interect(); 
}
