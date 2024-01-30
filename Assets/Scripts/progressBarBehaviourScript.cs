using UnityEngine;
using UnityEngine.UI;

public class progressBarBehaviourScript:MonoBehaviour
{
	public Image progressBar;

	private bool isComplete = false;

	public GameObject canvasObject;
	public GameObject skillCheckObject;
	private GameObject skillCheck = null;

	[Range(0, 1)]
	public float initialProgress = 0f;
	public float duration = 50f;
	private float progress = 0f;

	public float probabilitySkillTest = 0.001f;
	private bool isSkillCheck = false;

	// Start is called before the first frame update
	void Start()
	{
		progressBar.fillAmount = Mathf.Clamp01(initialProgress);
		progress = (initialProgress * 100) / 2;
	}

	// Update is called once per frame
	void Update()
	{
		if (!isComplete) {
			if (progress < duration) {
				SkillCheck();

				progress += Time.deltaTime;
				UpdateProgressBar(progress / duration);
			} else {
				if (skillCheck != null) {
					Destroy(skillCheck);
				}

				isComplete = true;
				Destroy(gameObject);
			}
		}
	}

	void UpdateProgressBar(float proportion)
	{
		progressBar.fillAmount = Mathf.Clamp01(proportion);
	}

	void SkillCheck()
	{
		if (!isSkillCheck) {
			if (Random.value < probabilitySkillTest) {
				isSkillCheck = true;

				skillCheck = Instantiate(skillCheckObject, new Vector3(0f, 0f, 0f), Quaternion.identity);
				skillCheck.transform.SetParent(canvasObject.transform, false);

				SkillCheckBehaviourScript.ScriptOnSkillCheck += OnSkillCheck;
				SkillCheckBehaviourScript.ScriptOnGreatSkillCheck += OnGreatSkillCheck;
				SkillCheckBehaviourScript.ScriptOnFailSkillCheck += OnFailSkillCheck;
			}
		}
	}

	void DestroySkillCheck()
	{
		isSkillCheck = false;
		skillCheck = null;

		SkillCheckBehaviourScript.ScriptOnSkillCheck -= OnSkillCheck;
		SkillCheckBehaviourScript.ScriptOnGreatSkillCheck -= OnGreatSkillCheck;
		SkillCheckBehaviourScript.ScriptOnFailSkillCheck -= OnFailSkillCheck;
	}

	void OnSkillCheck()
	{
		Debug.Log("A");
		DestroySkillCheck();
	}

	void OnGreatSkillCheck()
	{
		Debug.Log("B");
		DestroySkillCheck();
	}

	void OnFailSkillCheck()
	{
		Debug.Log("C");
		DestroySkillCheck();
	}
}
