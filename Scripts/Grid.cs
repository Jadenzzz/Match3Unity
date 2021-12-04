using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public enum Type
    {
       BEAN,
       COUNT,
       BUBBLE,
       EMPTY,
    }
    [System.Serializable]
    public struct PiecePrefab
    {
        public Type type;
        public GameObject prefab;
    }

    public int xVec;
    public int yVec;

    public PiecePrefab[] piecePrefabs;
    public GameObject backgroundpf;
    private GamePiece[,] _pieces;
    public float fillTime = 0.1f;
    private bool _changeDir = false;
    private GamePiece _chosenPiece1;
    private GamePiece _chosenPiece2;
    public int score;

    public Dictionary<Type, GameObject> piecePrefabDict;
    // Start is called before the first frame update
    void Awake()
    {
        piecePrefabDict = new Dictionary<Type, GameObject>();
        for (int i = 0; i < piecePrefabs.Length; i++)
        {
            if(!piecePrefabDict.ContainsKey(piecePrefabs[i].type)){
            	piecePrefabDict.Add(piecePrefabs[i].type, piecePrefabs[i].prefab);
            }
        }
        for (int x = 0; x < xVec; x++)
        {
            for (int y = 0; y < yVec; y++)
            {
                GameObject background = Instantiate(backgroundpf, GetPosition( x, y), Quaternion.identity);
                background.transform.parent = transform;
            }
        }
        _pieces = new GamePiece[xVec, yVec];
        for (int x = 0; x < xVec; x++)
        {
            for (int y = 0; y < yVec; y++)
            {
                
                Spawn(x, y, Type.EMPTY);
                
            }
        }
        
        StartCoroutine(Fill());

        Destroy(_pieces[6, 4].gameObject);
        Spawn(6, 4, Type.BUBBLE);
        Destroy(_pieces[2, 4].gameObject);
        Spawn(2, 4, Type.BUBBLE);
        Destroy(_pieces[4, 4].gameObject);
        Spawn(4, 4, Type.BUBBLE);
    }

    // Update is called once per frame
    void Update()
    {
        
      

    }
    public Vector2 GetPosition(int x, int y)
    {
    	return new Vector2(transform.position.x - xVec/2 + x, transform.position.y + yVec/2 - y);
    }

    public GamePiece Spawn(int x, int y, Type type)
    {
    	GameObject newPiece =  (GameObject)Instantiate(piecePrefabDict[type], GetPosition(x,y), Quaternion.identity);
    	newPiece.transform.parent = transform;
    	_pieces[x,y] = newPiece.GetComponent<GamePiece>();
    	_pieces[x,y].Init(x,y,this,type);
    	

    	return _pieces[x,y];
    }
    public IEnumerator Fill()
    {
    	bool fill = true;

    	while(fill)
    	{
    		yield return new WaitForSeconds (fillTime);
    		while(IsFill()){
    			_changeDir = !_changeDir;
    			yield return new WaitForSeconds (fillTime);
    		}
            

            fill = ClearMatches();
    	}
        
    }
    public bool IsFill()
    {
    	bool movePiece = false;
        for (int x = 0; x < xVec; x++)
        {
            GamePiece pieceBelow = _pieces[x, 0];

            if (pieceBelow.Type == Type.EMPTY)
            {
                Destroy(pieceBelow.gameObject);
                GameObject newP = (GameObject)Instantiate(piecePrefabDict[Type.BEAN], GetPosition(x, -1), Quaternion.identity);
                newP.transform.parent = transform;
                _pieces[x, 0] = newP.GetComponent<GamePiece>();
                _pieces[x, 0].Init(x, -1, this, Type.BEAN);
                _pieces[x, 0].MoveableComponent.Moving(x, 0, fillTime);
                _pieces[x, 0].ColorComponent.SetColor((Colors.ColorType)Random.Range(0, _pieces[x, 0].ColorComponent.NumColors));
                movePiece = true;
            }
        }
        for (int y = yVec -2; y>=0; y--)
    	{
    		for(int dX = 0; dX < xVec; dX++)
    		{
    			int x = dX;
    			if(_changeDir)
    			{
    				x = xVec - 1 - dX;
    			}
    			GamePiece piece = _pieces[x,y];
    			if(piece.IsMoveable())
    			{
    				GamePiece pieceBelow = _pieces [x,y+1];

    				if(pieceBelow.Type == Type.EMPTY)
    				{
    					Destroy(pieceBelow.gameObject);
    					piece.MoveableComponent.Moving(x, y+1, fillTime);
    					_pieces [x,y+1] = piece;
    					Spawn(x,y,Type.EMPTY);
    					movePiece = true;
    				}
    				else
    				{
    					for(int di = -1; di <= 1; di ++)
    					{
    						if (di != 0)
    						{
    							int diX = x + di;
    							if(_changeDir)
    							{
    								diX = x - di;
    							}
    							if (diX >= 0 && diX < xVec)
    							{
    								GamePiece diagP = _pieces[diX, y+1];
    								if(diagP.Type == Type.EMPTY)
    								{
    									bool above = true;

    									for(int aboveY = y; aboveY >= 0; aboveY--)
    									{
    										GamePiece pieceAbove = _pieces [diX,aboveY];
    										if (pieceAbove.IsMoveable())
    										{
    											break;
    										}
    										else if(!pieceAbove.IsMoveable() && pieceAbove.Type != Type.EMPTY)
    										{
    											above = false;
    											break;
    										}
    									}
    									if(above == false)
    									{
    										Destroy (diagP.gameObject);
    										piece.MoveableComponent.Moving(diX, y+1, fillTime);
    										_pieces [diX, y+1] = piece;
    										Spawn(x,y, Type.EMPTY);
    										movePiece = true;
    										break;
    									}
    								}
    							}
    						}
    					}
    				}
    			}
    			

    		}
    	}
    	
        

        return movePiece;
    }
    public bool IsChosen (GamePiece p1, GamePiece p2)
    {
    	return (p1.X == p2.X && (int)Mathf.Abs(p1.Y - p2.Y) == 1) || (p1.Y == p2.Y && (int)Mathf.Abs(p1.X - p2.X) == 1);
    }

    public void Swap(GamePiece p1, GamePiece p2)
    {
        if (p1.IsMoveable() && p2.IsMoveable())
        {
            _pieces[p1.X, p1.Y] = p2;
            _pieces[p2.X, p2.Y] = p1;
            if (GetMatch(p1, p2.X, p2.Y) != null || GetMatch(p2, p1.X, p1.Y) != null)
            {
                int p1X = p1.X;
                int p1Y = p1.Y;

                p1.MoveableComponent.Moving(p2.X, p2.Y, fillTime);
                p2.MoveableComponent.Moving(p1X, p1Y, fillTime);
                StartCoroutine(Fill());
                ClearMatches();
                

            }

            else
            {
                _pieces[p1.X, p1.Y] = p1;
                _pieces[p2.X, p2.Y] = p2;
            }
            
        }

    }

    
    public void PressPiece (GamePiece p)
    {
    	_chosenPiece1 = p;
    }
    public void EnterPiece (GamePiece p)
    {
    	_chosenPiece2 = p;
    }
    public void ReleasePiece()
    {
    	if (IsChosen(_chosenPiece1, _chosenPiece2))
    	{
    		Swap(_chosenPiece1, _chosenPiece2);
        }
        

    }

    




    public List<GamePiece> GetMatch(GamePiece p, int nX, int nY)
    {
    	if (p.IsColored())
    	{
    		Colors.ColorType color = p.ColorComponent.Color;
    		List<GamePiece> horizontal = new List<GamePiece>();
    		List<GamePiece> vertical = new List<GamePiece>();
    		List<GamePiece> matching = new List<GamePiece>();

    		//horizontal
    		horizontal.Add(p);
    		for(int dir = 0; dir <= 1; dir++)
    		{
    			for(int xStep = 1; xStep <= xVec - 1; xStep++)
    			{
    				int x;
    				if(dir == 0)
    				{
    					x = nX - xStep;//Left
    				} else{
    					x = nX + xStep;//Right
    				}
    				
    				if(x < 0 || x >= xVec)
    				{
    					break;
    				}
    				if (_pieces[x, nY].IsColored() && _pieces[x,nY].ColorComponent.Color == color) 
    				{
    					horizontal.Add(_pieces[x,nY]);
    				} else{
    					break;
    				}
    				
    			}
    		}
    		if(horizontal.Count > 2)
    		{
    			for(int i = 0; i< horizontal.Count; i++)
    			{
    				matching.Add(horizontal[i]);
    			}
    		}

    		
    		//Traverse vertically
    		if(horizontal.Count > 2)
    		{
    			for(int i = 0; i < horizontal.Count; i++)
    			{
    				for(int dir = 0; dir <= 1; dir ++)
    				{
    					for(int yStep = 1; yStep <= yVec -1; yStep++)
    					{
    						int y;
    						if(dir == 0)
    						{
    							y = nY - yStep;//Up
    						}
    						else
    						{
    							y = nY + yStep;//Down
    						}
    						if(y < 0 || y > yVec-1)
    						{
    							break;
    						}
    						if (_pieces[horizontal[i].X, y].IsColored() && _pieces[horizontal[i].X, y].ColorComponent.Color == color)
                        	{
                            	vertical.Add(_pieces[horizontal[i].X, y]);
                        	}
                        	else
                        	{                                                                                                                                                        
                            	break;
                        	}
    					}
    				}
    				if(vertical.Count <= 1)
    				{
    					vertical.Clear();
    					
    				}
    				else
    				{
    					for(int j=0; j< vertical.Count ; j++)
    					{
    						matching.Add(vertical[j]);
    					}
    					break;
    				}
    			}
    		}
    		if(matching.Count > 2)
    		{
    			return matching;
    		}

    		
    		

    		//vertical
    		horizontal.Clear();
    		vertical.Clear();
    		vertical.Add(p);
    		for(int dir = 0; dir <= 1; dir++)
    		{
    			for(int yStep = 1; yStep <= yVec-1; yStep++)
    			{
    				int y;
    				if(dir == 0)
    				{
    					y = nY - yStep;
    				}
    				else
    				{
    					y = nY + yStep;
    				}
    				if(y < 0 || y >= yVec)
    				{
    					break;
    				}
    				if (_pieces[nX, y].IsColored() && _pieces[nX,y].ColorComponent.Color == color) 
    				{
    					vertical.Add(_pieces[nX, y]);
    				}
    				else
    				{
    					break;
    				}
    			}
    		}
    		if(vertical.Count > 2)
    		{
    			for(int i = 0; i< vertical.Count; i++)
    			{
    				matching.Add(vertical[i]);
    			}
    		}
    		//traverse
    		if(vertical.Count > 2)
    		{
    			for(int i = 0; i < vertical.Count; i++)
    			{
    				for(int dir = 0; dir <= 1; dir ++)
    				{
    					for(int xStep = 1; xStep <= yVec - 1; xStep++)
    					{
    						int x;
    						if(dir == 0)
    						{
    							x = nX - xStep;
    						}
    						else
    						{
    							x = nX + xStep;
    						}
    						if(x < 0 || x > xVec - 1)
    						{
    							break;
    						}
    						if (_pieces[x, vertical[i].Y].IsColored() && _pieces[x, vertical[i].Y].ColorComponent.Color == color)
                        	{
                            	horizontal.Add(_pieces[x, vertical[i].Y]);
                        	}
                        	else
                        	{                                                                                                                                                        
                            	break;
                        	}
    					}
    				}
    				if(horizontal.Count < 2)
    				{
    					horizontal.Clear();
    					
    				}
    				else
    				{
    					for(int j=0; j< horizontal.Count ; j++)
    					{
    						matching.Add(horizontal[j]);
    					}
    					break;
    				}
    			}
    		}
    		if(matching.Count > 2)
    		{
    			return matching;
    		}

    	}
    	return null;
    }
    public bool ClearMatches()
    {
    	bool fill = false;

    	for(int y=0 ; y<yVec; y++)
    	{
    		for(int x = 0; x < xVec; x++)
    		{
    			if(_pieces[x,y].IsClear())
    			{
    				List<GamePiece> matched = GetMatch(_pieces[x,y],x,y);
    				if(matched != null)
    				{
    					for(int i = 0; i < matched.Count; i++)
    					{
    						if(ClearPiece(matched[i].X, matched[i].Y))
    						{
    							fill = true;
    						}
                            
    					}
                        GameManger.GetInstance().Score += matched.Count * matched.Count;
                    }
    			}
    		}
    	}
    	return fill;
    }

    public bool ClearPiece(int x, int y)
    {
    	if(_pieces[x,y].IsClear() && !_pieces[x,y].ClearComponent.IsClear)
    	{
    		_pieces[x,y].ClearComponent.Clear();
    		Spawn(x,y,Type.EMPTY);
    		return true;
    	}
    	return false;
    }

  
}

