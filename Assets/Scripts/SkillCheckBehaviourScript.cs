using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class SkillCheckBehaviourScript:MonoBehaviour
{
	public delegate void OnSkillCheck();
	public static event OnSkillCheck ScriptOnSkillCheck;

	public delegate void OnGreatSkillCheck();
	public static event OnGreatSkillCheck ScriptOnGreatSkillCheck;

	public delegate void OnFailSkillCheck();
	public static event OnFailSkillCheck ScriptOnFailSkillCheck;

	public Image progressImage;

	private bool isChecking = false;

	private float angleInitialProgressZone;
	public enum directions
	{
		RIGHT, LEFT
	}
	public directions direction = directions.RIGHT;

	public Image perfectZoneImage;
	[Range(0, 1)]
	public float sizePerfectZone = 0.06f;

	public Image progressZoneImage;
	[Range(0, 1)]
	public float sizeProgressZone = 0.15f;

	public Image pointerImage;
	[Range(0, 1)]
	public float sizeIndicator = 0.03f;
	public float skillCheckSpeed = 100f;

	private RectTransform pointerRectTransform;
	private float initialPointerRotation;

	// Start is called before the first frame update
	void Start()
	{
		pointerRectTransform = pointerImage.GetComponent<RectTransform>();

		StartSkillCheck();
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.R)) {
			StartSkillCheck();
		}

		if (Input.GetKeyDown(KeyCode.Space) && !isChecking) {
			StartSkillCheck();
		} else {
			if (isChecking) {
				RotatePointer();
				CheckSkill();
			}
		}
	}

	void StartSkillCheck()
	{
		ResetSkillCheck();
		isChecking = true;

		UpdateUI();
	}

	void ResetSkillCheck()
	{
		isChecking = false;

		pointerImage.fillAmount = Mathf.Clamp01(sizeIndicator);
		pointerImage.fillClockwise = (direction == directions.RIGHT) ? true : false;
		initialPointerRotation = (direction == directions.RIGHT) ? sizeIndicator * 360f : 360f - (sizeIndicator * 360f);

		pointerRectTransform.rotation = Quaternion.Euler(0f, 0f, initialPointerRotation);
	}

	void UpdateUI()
	{
		perfectZoneImage.fillAmount = Mathf.Clamp01(sizePerfectZone);
		progressZoneImage.fillAmount = Mathf.Clamp01(sizeProgressZone);

		angleInitialProgressZone = Random.Range(
			(direction == directions.RIGHT)
				? 90f + (sizePerfectZone * 360f)
				: 270f,
			(direction == directions.RIGHT)
				? 360f - (sizeProgressZone * 360f)
				: (sizePerfectZone * 360f) + (sizeProgressZone * 360f)
		);

		perfectZoneImage.transform.rotation = Quaternion.Euler(0f, 0f, angleInitialProgressZone);
		progressZoneImage.transform.rotation = Quaternion.Euler(0f, 0f, (direction == directions.RIGHT)
			? angleInitialProgressZone + (sizeProgressZone * 360f)
			: angleInitialProgressZone - (sizePerfectZone * 360f)
		);
	}

	void CheckSkill()
	{
		float currentRotation = pointerImage.transform.eulerAngles.z;

		float successGreatZoneStart = (direction == directions.RIGHT)
			? (perfectZoneImage.transform.eulerAngles.z - (sizePerfectZone * 360f))
			: perfectZoneImage.transform.eulerAngles.z;
		float successGreatZoneEnd = (direction == directions.RIGHT)
			? (successGreatZoneStart + (sizePerfectZone * 360f))
			: (successGreatZoneStart - (sizePerfectZone * 360f));

		float successZoneStart = successGreatZoneEnd;
		float successZoneEnd = (direction == directions.RIGHT)
			? (successZoneStart + (sizeProgressZone * 360))
			: (successZoneStart - (sizeProgressZone * 360));

		if (Input.GetKeyDown(KeyCode.Space)) {
			if ((successGreatZoneStart <= currentRotation && successGreatZoneEnd >= currentRotation && direction == directions.RIGHT) || (successGreatZoneStart >= currentRotation && successGreatZoneEnd <= currentRotation && direction == directions.LEFT)) {
				GreatSkillCheck();
			} else if ((successZoneStart <= currentRotation && successZoneEnd >= currentRotation && direction == directions.RIGHT) || (successZoneStart >= currentRotation && successZoneEnd <= currentRotation && direction == directions.LEFT)) {
				SuccessSkillCheck();
			} else {
				FailSkillCheck();
			}
		}

		if ((successZoneEnd <= currentRotation && direction == directions.RIGHT) || (successZoneEnd >= currentRotation && direction == directions.LEFT)) {
			FailSkillCheck();
		}
	}

	void RotatePointer()
	{
		pointerImage.transform.Rotate(Vector3.forward * ((direction == directions.RIGHT) ? skillCheckSpeed : -skillCheckSpeed) * Time.deltaTime);
	}

	void SuccessSkillCheck()
	{
		isChecking = false;
		//ClearLog();
		//Debug.Log("Skill Check bem-sucedido!");

		ScriptOnSkillCheck();
		Destroy(gameObject);
	}

	void GreatSkillCheck()
	{
		isChecking = false;
		//ClearLog();
		//Debug.Log("Ótima Skill Check!");

		ScriptOnGreatSkillCheck();
		Destroy(gameObject);
	}

	void FailSkillCheck()
	{
		isChecking = false;
		//ClearLog();
		//Debug.Log("Skill Check falhou!");

		ScriptOnFailSkillCheck();
		Destroy(gameObject);
	}

	void ClearLog()
	{
		var assembly = Assembly.GetAssembly(typeof(UnityEditor.Editor));
		var type = assembly.GetType("UnityEditor.LogEntries");
		var method = type.GetMethod("Clear");
		method.Invoke(new object(), null);
	}
}
