using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Town : MonoBehaviour
{
    [SerializeField] float admitRadius = 2f;
    HashSet<Actor> actors;
    Dictionary<Actor, IEnumerator> actorSleepCoroutines;

    void Start()
    {
        actors = new HashSet<Actor>();
        actorSleepCoroutines = new Dictionary<Actor, IEnumerator>();
    }

    IEnumerator SleepActorForDurationCoroutine(Actor actor, float duration)
    {
        GameObject go = actor.gameObject;
        go.SetActive(false);
        yield return new WaitForSeconds(duration);

        // Remove the coroutine list
        if (actorSleepCoroutines.ContainsKey(actor))
            actorSleepCoroutines.Remove(actor);

        WakeAndEject(actor);
    }

    void SleepActorForDuration(Actor actor, float duration)
    {
        IEnumerator coroutine = SleepActorForDurationCoroutine(actor, duration);
        StartCoroutine(coroutine);

        actorSleepCoroutines.Add(actor, coroutine);
    }

    void WakeAndEject(Actor actor)
    {
        if (!actor.gameObject.activeInHierarchy)
        {
            // Wake
            actor.gameObject.SetActive(true);

            // Eject at random loc
            actor.GetComponentInParent<Transform>().position = this.transform.position + new Vector3(Random.Range(-3f, 3f), 0, Random.Range(-3f, 3f));
        }
    }

    public float AdmitRadius { get { return admitRadius; } }

    public bool IsAdmitable(Actor actor)
    {
        float dist = Vector3.Distance(actor.transform.position, this.transform.position);
        return dist <= admitRadius;
    }

    public void Admit(Actor actor, float duration)
    {
        // Add
        actors.Add(actor);

        // Make it sleep
        SleepActorForDuration(actor, duration);
    }

}
