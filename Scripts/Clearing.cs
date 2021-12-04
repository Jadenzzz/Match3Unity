using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clearing : MonoBehaviour
{
	public AnimationClip clearAnimation;
	private bool _isClear = false;
	protected GamePiece piece;
	public bool IsClear
	{
		get{return _isClear;}
	}

	void Awake()
	{
		piece = GetComponent<GamePiece>();
	}
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
    }
    public void Clear()
    {
    	_isClear = true;
        StartCoroutine (ClearCoroutine());
    }
    private IEnumerator ClearCoroutine()
    {
    	Animator animator = GetComponent<Animator>();
    	if(animator){
    		animator.Play(clearAnimation.name);
    		yield return new WaitForSeconds (clearAnimation.length);
    		Destroy(gameObject);
    	}
    }
}
