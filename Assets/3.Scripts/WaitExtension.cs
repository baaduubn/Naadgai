using System.Collections;
using UnityEngine;
using UnityEngine.Events;


public static class WaitExtension
{/// <summary>
 /// this.Wait(0.5f, () =>{      });
 /// </summary>
 /// <param name="mono"></param>
 /// <param name="delay"></param>
 /// <param name="action"></param>
    public static void Wait(this MonoBehaviour mono, float delay, UnityAction action)
    {
   mono.StartCoroutine(ExecuteAction(delay, action, mono));
    }
    private static IEnumerator ExecuteAction(float delay, UnityAction action, MonoBehaviour mono)
    {
        float time = 0;
        while (time < delay)
        {
            if (mono == null)
                yield break;
            time += Time.deltaTime;
            yield return null;
        }
        action.Invoke();
        yield break;
    }
}
