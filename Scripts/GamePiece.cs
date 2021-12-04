using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePiece : MonoBehaviour
{
	private int _x;
	private int _y;
	private Grid.Type _type;
	private Grid _grid;
	private Move _moveableComponent;
	private Colors _colorComponent;
	private Clearing _clearComponent;




	public int X
	{
		get{return _x;}
		set
		{
			if (IsMoveable())
			{
				_x = value;
			}
		}
	}

	public int Y
	{
		get{return _y;}
		set
		{
			if (IsMoveable())
			{
				_y = value;
			}
		}
	}

	public Grid.Type Type
	{
		get{return _type;}
	}

	public Grid grid
	{
		get{return _grid;}
	}

	public Move MoveableComponent
	{
		get{return _moveableComponent;}
	}
	public Colors ColorComponent
	{
		get{return _colorComponent;}
	}
	public Clearing ClearComponent
	{
		get{return _clearComponent;}
	}

	void Awake()
	{
		_moveableComponent = GetComponent<Move>();
		_colorComponent = GetComponent<Colors>();
		_clearComponent = GetComponent<Clearing>();
	}



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Init(int x, int y, Grid grid, Grid.Type type)
    {
    	_x = x;
    	_y = y;
    	_grid = grid;
    	_type = type;
    }

    public bool IsMoveable()
    {
    	return _moveableComponent != null;
    }

    public bool IsColored()
    {
    	return _colorComponent != null;
    }
    public bool IsClear()
    {
    	return _clearComponent != null;
    }
    void OnMouseDown()
    {
    	_grid.PressPiece(this);
    }
    void OnMouseEnter()
    {
    	_grid.EnterPiece(this);
    }
    
    void OnMouseUp()
    {
    	_grid.ReleasePiece();
    }
    
}
