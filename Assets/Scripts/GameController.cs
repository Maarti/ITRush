using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// necessaire pour gerer les element de l'UI

public class GameController : MonoBehaviour
{
	/*[System.Serializable]
    public struct Spawnable
    {
        public GameObject gameObject;
        public float chance;
    }*/
	public GameObject malusPrefab, bonusPrefab, coffeePrefab;
	public Vector3 spawnValue;
	public int hazardCount, coffeeScore, coffeeConsumption;
	public float spawnWait, startWait, waveWait, timeMax, targetBonusChance;
	public Slider scoreSlider, coffeeSlider;
	public Button pauseButton, menuButton;
	public Image uiDestroyBoltArea, uiMagnetBoltArea;
	public GameObject[] decors;
	public CanvasGroup uiLevelComplete, uiGameOver, uiPause, uiOverdose;
	public Text timerText;
	public bool isOverdosed;

	private int score, nbDecors, countBonus, countMalus, countCoffee, coffeeHash = Animator.StringToHash ("Coffee");
	private bool isGameOver, isGamePaused, isLevelComplete;
	private float timeLeft, overdoseDuration = 6f;
	private float[] bonusChances;
	private Animator cameraAnimator;
	// [0] target ; [1] real ; [2] current

	// Use this for initialization
	void Start ()
	{
		cameraAnimator = Camera.main.GetComponent<Animator> ();
		// Init vars
		AudioListener.pause = false;
		isGameOver = false;
		isGamePaused = false;
		isLevelComplete = false;
		countBonus = 0;
		countMalus = 0;
		countCoffee = 0;
		timeMax = ApplicationController.ac.currentLevel.bronzeTime;
		timeLeft = timeMax;
		nbDecors = decors.Length;
		uiLevelComplete.alpha = 0f;
		uiLevelComplete.blocksRaycasts = false;
		uiGameOver.alpha = 0f;
		uiGameOver.blocksRaycasts = false;
		uiPause.alpha = 0f;
		uiPause.blocksRaycasts = false;
		uiOverdose.alpha = 0f;
		Time.timeScale = 1;
		score = 200;
		coffeeScore = 500;
		UpdateScore ();
		UpdateCoffee ();
		targetBonusChance = ApplicationController.ac.currentLevel.bonusRate;
		bonusChances = new float[3] { targetBonusChance, 0f, targetBonusChance };
		bonusPrefab.GetComponent<DestroyByContact> ().scoreValue = ApplicationController.ac.currentLevel.bonusValue;
		malusPrefab.GetComponent<DestroyByContact> ().scoreValue = ApplicationController.ac.currentLevel.malusValue;
		Debug.Log ("Start level " + ApplicationController.ac.currentLevel.id + " - " + timeLeft + "sec");
		// Start coroutines
		StartCoroutine (SpawnWaves ());
		StartCoroutine (SpawnCoffee ());
		StartCoroutine (LooseCoffeeCoroutine ());
		StartCoroutine (SpawnDecor ());
		StartCoroutine (TimerCoroutine ());
	}

	void Update ()
	{
		if (!isGameOver && !isLevelComplete) {
			timeLeft = Mathf.Clamp (timeLeft - Time.deltaTime, 0f, timeMax);
			if (timeLeft <= 0)
				GameOver ();
		}
	}

	IEnumerator SpawnWaves ()
	{
		yield return new WaitForSeconds (startWait);
		while (!isLevelComplete && !isGameOver) {
			for (int i = 0; i < hazardCount; i++) {
				float randSpawn = Random.value;
				InstanciateCollectible (randSpawn);
				yield return new WaitForSeconds (spawnWait);
			}
			UpdateChances ();
			yield return new WaitForSeconds (waveWait);
		}
	}

	void InstanciateCollectible (float rand)
	{
		Vector3 spawnPosition = new Vector3 (Random.Range (-spawnValue.x, spawnValue.x), spawnValue.y, spawnValue.z);
		Quaternion spawnRotation = Quaternion.identity;

		if (rand <= bonusChances [2]) {
			//Instantiate (bonusPrefab, spawnPosition, spawnRotation);
			Instantiate (bonusPrefab, spawnPosition, bonusPrefab.transform.rotation);
			countBonus++;
		} else {
			Instantiate (malusPrefab, spawnPosition, spawnRotation);
			countMalus++;
		}
	}

	public void AddScore (int newScoreValue)
	{
		if (isGameOver || isLevelComplete)
			return;
		score = Mathf.Clamp (score + newScoreValue, 0, 1000);
		// score += newScoreValue;
		UpdateScore ();
		if (score >= 1000)
			LevelFinished ();
	}

	void UpdateScore ()
	{
		scoreSlider.value = score;
		Color lerpedColor = Color.Lerp (Color.red, Color.green, Mathf.PingPong (score / 1000f, 1));
		scoreSlider.transform.GetChild (1).GetComponentInChildren<Image> ().color = lerpedColor;
	}

	public void AddCoffee (int coffeeAdded)
	{
		if (isGameOver || isLevelComplete || isOverdosed)
			return;
		coffeeScore = Mathf.Clamp (coffeeScore + coffeeAdded, 0, 1000);
		UpdateCoffee ();
		if (coffeeScore >= 1000)
			StartCoroutine (Overdose ());
	}

	void UpdateCoffee ()
	{
		coffeeSlider.value = coffeeScore;
		cameraAnimator.SetInteger (coffeeHash, coffeeScore);
	}

	public void pauseGame ()
	{
		isGamePaused = !isGamePaused;
		if (isGamePaused) {
			uiPause.alpha = 1f;
			uiPause.blocksRaycasts = true;
			Time.timeScale = 0;
			pauseButton.GetComponent<CanvasGroup> ().alpha = 1;
			uiDestroyBoltArea.enabled = false;
			uiMagnetBoltArea.enabled = false;
			AudioListener.pause = true;
		} else {
			uiPause.alpha = 0f;
			uiPause.blocksRaycasts = false;
			Time.timeScale = 1;
			pauseButton.GetComponent<CanvasGroup> ().alpha = .3f;
			uiDestroyBoltArea.enabled = true;
			uiMagnetBoltArea.enabled = true;
			AudioListener.pause = false;
		}
	}

	IEnumerator SpawnDecor ()
	{
		while (true) {
			int decorToSpawn = Random.Range (0, nbDecors);   	// Selection decor aleatoire
			Instantiate (decors [decorToSpawn]);  				// (le decor est ensuite placé aléatoirement à droite ou à gauche grâce au scripte RandomDecorPosition qui lui est attaché)
			float wait = Random.Range (1f, 5f); 				// Temps d'attente entre 2 objets
			yield return new WaitForSeconds (wait);
		}
	}

	void LevelFinished ()
	{
		StopGame ();
		isLevelComplete = true;
		uiLevelComplete.alpha = 1;
		uiLevelComplete.blocksRaycasts = true;
		float totalTime = timeMax - timeLeft;
		ApplicationController.ac.LevelFinished (totalTime);
		//ApplicationController.ac.earnMedal (0, 2);

	}

	void GameOver ()
	{
		StopGame ();
		isGameOver = true;
		uiGameOver.alpha = 1;
		uiGameOver.blocksRaycasts = true;
	}

	void StopGame ()
	{
		//GameObject.FindGameObjectWithTag("Player").GetComponent<CapsuleCollider>().enabled = false;
		uiMagnetBoltArea.enabled = false;
		uiDestroyBoltArea.enabled = false;
		pauseButton.enabled = false;
		Debug.Log ("Game Stopped : nbTotalBonus=" + countBonus + "   nbTotalMalus=" + countMalus + "   nbTotalCoffee=" + countCoffee);
	}

	private IEnumerator TimerCoroutine ()
	{
		bool redEffect = false;
		while (!isLevelComplete) {
			//timerText.text = string.Format("{0:0}:{1:00}", minutes, seconds);
			timerText.text = "" + (int)timeLeft;
			if (timeLeft < 15 && !redEffect) {
				redEffect = true;
				//timerText.GetComponent<Outline>().effectColor = Color.red;
				//timerText.GetComponent<Outline>().effectDistance.Set(1f, -1f);
				timerText.GetComponent<Outline> ().enabled = true;
				timerText.fontSize = 46;

				//timerText.fontStyle = FontStyle.Bold;
			}
			yield return new WaitForSeconds (0.2f);
		}
	}


	void UpdateChances ()
	{
		// On met à jour les chances réelles
		bonusChances [1] = (float)countBonus / (countBonus + countMalus);

		// On met à jour les chances appliquées :
		//      Plus les chances réelles s'éloignent des chances cibles,
		//      plus on compensera dans l'autre sens pour qu'elles s'en rapprochent
		bonusChances [2] = Mathf.PingPong (bonusChances [0] + (bonusChances [0] - bonusChances [1]), 1f);

		Debug.Log ("bonus : target=" + bonusChances [0] + "   real=" + bonusChances [1] + "    current=" + bonusChances [2]);
	}

	IEnumerator SpawnCoffee ()
	{
		yield return new WaitForSeconds (startWait);
		while (!isLevelComplete && !isGameOver) {
			// float randSpawn = Random.value;
			Vector3 spawnPosition = new Vector3 (Random.Range (-spawnValue.x, spawnValue.x), coffeePrefab.transform.position.y, spawnValue.z);
			//Quaternion spawnRotation = Quaternion.identity;s
			Instantiate (coffeePrefab, spawnPosition, coffeePrefab.transform.rotation);
			countCoffee++;
			float waitForCoffee = Random.Range (waveWait - 2, waveWait + 2);
			yield return new WaitForSeconds (waitForCoffee);
		}
	}

	IEnumerator LooseCoffeeCoroutine ()
	{
		while (!isLevelComplete && !isGameOver) {
			AddCoffee (-coffeeConsumption);
			yield return new WaitForSeconds (.5f);
		}
	}

	IEnumerator Overdose ()
	{       
		isOverdosed = true;
		uiOverdose.alpha = 1f;
		Debug.Log ("Overdose=" + isOverdosed);
		yield return new WaitForSeconds (overdoseDuration);
		isOverdosed = false;
		uiOverdose.alpha = 0f;
		AddCoffee (-750);
		Debug.Log ("Overdose=" + isOverdosed);
	}
}
