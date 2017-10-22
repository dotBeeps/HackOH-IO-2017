using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AudioAnalyzer.FrequencyBeatDetection;

public class EqManager : MonoBehaviour 
{
	public GameObject peerPrefab;
	public FreqBeatDetection m_BeatDetector;
	public float m_Offset;
	public float m_SpawnPoint;
	public int m_HeightPeers; 

	[Header("Color Setting")]
	public Color[] m_Colors;
	[Range(0,1)]
	public float m_LerpSpeed = 0.2f;

	[Header("Particle Setting")]
	[Range(0,5)]
	public int m_EmmitByBeat;
	[Range(1.1f, 10f)]
	public float m_Sensitivity;
	public float m_ParticleScale;
	public float m_GravityMod = -0.5f;
	public float m_StartSpeed = 5;


	Color currentColor = Color.white;
	int setColor;

	Transform[] instanceTr;
	ParticleSystem[] instancePS;
	MeshRenderer[] instanceRenderer;
	int arraySize;



	#region engine methods
	void Awake()
	{
		arraySize = m_BeatDetector.FREQUENCY_SIZE;
		instanceTr = new Transform[arraySize];
		instancePS = new ParticleSystem[arraySize];
		instanceRenderer = new MeshRenderer[arraySize];
		setColor = Random.Range (0, 3);
		SpawnEQ ();
	}

	void Update()
	{
		ChangeScale ();
		ChangeColor ();
		BeatEmmiter ();
	}
	#endregion

	#region spawn
	void SpawnEQ()
	{
		Transform peers = new GameObject ("Peers").transform;
		Transform previous = null;
		for (int i = 0; i < arraySize; i++) {
			if (!previous) {
				Transform instance = Instantiate (peerPrefab).transform;
				instance.position = new Vector3 (m_SpawnPoint, 0, 0);

				instanceTr [i] = instance;
				instancePS [i] = instance.GetComponent<ParticleSystem> ();
				instanceRenderer [i] = instance.GetComponent<MeshRenderer> ();

				instance.parent = peers;
				previous = instance;
			} else {
				Transform instance = Instantiate (peerPrefab).transform;
				instance.position = new Vector3 (previous.position.x + m_Offset, 0, 0);

				instanceTr [i] = instance;
				instancePS [i] = instance.GetComponent<ParticleSystem> ();
				instanceRenderer [i] = instance.GetComponent<MeshRenderer> ();

				instance.parent = peers;
				previous = instance;
			}
		}
	}
	#endregion

	#region eq serialize
	void BeatEmmiter()
	{
		for (int i = 0; i < arraySize; i++) {
			ParticleSystem current = instancePS [i];
			ParticleSystem.MainModule main = current.main;
			if (instanceTr[i].localScale.y > m_Sensitivity) {
				main.startColor = currentColor;
				main.startSize = m_ParticleScale;
				main.startSpeed = m_StartSpeed;
				main.gravityModifier = m_GravityMod;

				current.Emit (m_EmmitByBeat);
			} else {
				main.startColor = Color.white;

				current.Emit (0);
			}
		}
	}

	void ChangeColor()
	{
		if (currentColor != m_Colors [setColor]) {
			currentColor = Color.Lerp (currentColor, m_Colors [setColor], m_LerpSpeed);
		} else {
			currentColor = instanceRenderer [0].materials [0].color;
			setColor = Random.Range (0, 3);
		}

		for (int i = 0; i < arraySize; i++) {
			instanceRenderer [i].materials [0].color = currentColor;
			instanceRenderer [i].materials [0].SetColor ("_EmissionColor", currentColor);
		}
	}

	void ChangeScale()
	{
		for (int i = 0; i < arraySize; i++) {
			instanceTr [i].localScale = new Vector3 (1, (m_BeatDetector.m_FrequencyEq [i] * m_HeightPeers) + 0.2f, 1);
		}
	}
	#endregion
}
