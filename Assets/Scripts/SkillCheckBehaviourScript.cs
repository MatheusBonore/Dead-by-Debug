using UnityEngine;
using UnityEngine.UI;

public class SkillCheckBehaviourScript:MonoBehaviour
{
	public delegate void OnSkillCheck();
	public static event OnSkillCheck ScriptOnSkillCheck;

	public Image progressImage;

	private bool isChecking = false;

	private float angleInitialProgressZone;
	public enum directions
	{
		RIGHT, LEFT
	}
	public directions directon;

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
		pointerImage.fillAmount = Mathf.Clamp01(sizeIndicator);

		pointerRectTransform = pointerImage.GetComponent<RectTransform>();
		initialPointerRotation = (directon == directions.RIGHT) ? 0 : 11;

		//ResetSkillCheck();
		StartSkillCheck();
	}

	// Update is called once per frame
	void Update()
	{
		pointerImage.fillAmount = Mathf.Clamp01(sizeIndicator);

		initialPointerRotation = (directon == directions.RIGHT) ? 0 : 11;

		if (Input.GetKeyDown(KeyCode.Space) && !isChecking) {
			StartSkillCheck();
		} else {
			if (isChecking) {
				RotatePointer();
				CheckSkill();

				if (Mathf.Abs((int)pointerImage.transform.rotation.eulerAngles.z) == (int)((directon == directions.RIGHT) ? (sizeIndicator * 360f) : 359)) {
					FailSkillCheck();
				}
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
		pointerRectTransform.rotation = Quaternion.Euler(0f, 0f, initialPointerRotation);
	}

	void UpdateUI()
	{
		perfectZoneImage.fillAmount = Mathf.Clamp01(sizePerfectZone);
		progressZoneImage.fillAmount = Mathf.Clamp01(sizeProgressZone);

		angleInitialProgressZone = Random.Range(170f, 270f);


		if (directon == directions.RIGHT) {
			perfectZoneImage.transform.rotation = Quaternion.Euler(0f, 0f, angleInitialProgressZone);
			progressZoneImage.transform.rotation = Quaternion.Euler(0f, 0f, angleInitialProgressZone - ((sizePerfectZone * 360f)));
		} else {
			perfectZoneImage.transform.rotation = Quaternion.Euler(0f, 0f, angleInitialProgressZone - (sizeProgressZone * 360f));
			progressZoneImage.transform.rotation = Quaternion.Euler(0f, 0f, angleInitialProgressZone);
		}
	}

	void CheckSkill()
	{
		int currentRotation = (int)pointerImage.transform.rotation.eulerAngles.z;
		int successZoneStart = (int)angleInitialProgressZone;
		int successZoneEnd = (int)(Mathf.Abs((sizePerfectZone * 360f) - angleInitialProgressZone) - Mathf.Abs(sizeProgressZone * 360));

		if ((successZoneEnd >= currentRotation && directon == directions.RIGHT) || successZoneStart <= currentRotation && directon == directions.LEFT) {
			FailSkillCheck();
		} else {
			if (Input.GetKeyDown(KeyCode.Space)) {
				if (currentRotation <= successZoneStart && currentRotation >= successZoneEnd) {
					SuccessSkillCheck();
				} else {
					FailSkillCheck();
				}
			}
		}
	}

	void RotatePointer()
	{
		pointerImage.transform.Rotate(Vector3.forward * ((directon == directions.RIGHT) ? -skillCheckSpeed : skillCheckSpeed) * Time.deltaTime);
	}

	void SuccessSkillCheck()
	{
		isChecking = false;
		Debug.Log("Skill Check bem-sucedido!");

		ScriptOnSkillCheck();
		Destroy(gameObject);
	}

	void FailSkillCheck()
	{
		isChecking = false;
		Debug.Log("Skill Check falhou!");

		ScriptOnSkillCheck();
		Destroy(gameObject);
	}
}
