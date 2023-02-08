using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public static CameraFollow instance;

    public Transform target;
    public float smoothSpeed = 3f;
    private Vector3 offset;
    public bool followPlayer = true;

	private void Awake()
	{
        if (instance == null)
            instance = this;
	}

	// Start is called before the first frame update
	void Start()
    {
        followPlayer = true;
        offset = transform.position - target.position;    
    }

    // Update is called once per frame
    void Update()
    {
		if (followPlayer)
		{
            Vector3 desiredPosition = target.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;
        }
        if (PlayerController.instance.isLevelFinished)
        {
            transform.GetChild(0).GetComponent<ParticleSystem>().Play();
            
            PlayerController.instance.isLevelFinished = false;
        }
    }
}
