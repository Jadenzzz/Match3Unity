using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
	private GamePiece _piece;
	private IEnumerator moveCoroutine;
	void Awake()
	{
		_piece = GetComponent<GamePiece>();
	}
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Moving(int newX, int newY, float time)
    {
    	if(moveCoroutine != null)
    	{
    		StopCoroutine(moveCoroutine);
    	}

    	moveCoroutine = MoveCoroutine(newX, newY, time);
    	StartCoroutine (moveCoroutine);

    	
    }
    private IEnumerator MoveCoroutine(int newX, int newY, float time)
    {
    	_piece.X = newX;
    	_piece.Y = newY;

    	Vector3 start = transform.position;
    	Vector3 end = _piece.grid.GetPosition(newX, newY);

    	for(float t =0; t<= 1*time; t+= Time.deltaTime)
    	{
    		_piece.transform.position = Vector3.Lerp(start, end,t/time);
    		yield return 0;
    	}
    	_piece.transform.position = end;
    }
}
