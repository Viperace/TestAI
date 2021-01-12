using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Actor : MonoBehaviour
{
	IAgentAction currentAction;

    void Start()
    {
    }

	public void SetCurrentAction(IAgentAction act)
    {
		this.currentAction = act;
    }

	public void WanderAround(Vector3 pos)
    {
		IAgentAction wander = new Wander(this, pos, 5, 30);
		SetCurrentAction(wander);
		wander.Run();
	}


	public void GotoTownAndEnter(Town town, float stayDuration)
	{
		IAgentAction goTown = new GotoTarget(this.gameObject, town.gameObject, town.AdmitRadius*0.5f);
		IAgentAction enterTown = new EnterTown(this, town, stayDuration);

		ActionSequence sequence = new ActionSequence(this, goTown, enterTown);
		sequence.Run();
	}

	public void GotoTarget(GameObject target, System.Action onCompleteAction = null)
	{
		System.Action callback = null;
		callback += () => Debug.Log("Arrived " + target);
		callback += () => currentAction = null;
		callback += onCompleteAction;
		currentAction = new GotoTarget(this.gameObject, target, 1.5f, callback);
		currentAction.Run();
	}

	

	void Update()
	{
		if(currentAction != null)
			currentAction.Update();
	}
}
