using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetGameSession : MonoBehaviour
{
    public GameSession GetSession()
    {
        StartCoroutine(DelayedCall());
        return FindObjectOfType<GameSession>();
    }
    IEnumerator DelayedCall()
    {
        yield return new WaitForSeconds(1);
    }
}
