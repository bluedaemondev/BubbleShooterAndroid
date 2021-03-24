/*******************************************************
 * Copyright (C) 2016 Ngan Do - dttngan91@gmail
 *******************************************************/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GridManager
{

    private GridCell[,] generatedGridWithBalls;

    private int _gridSizeX;

    public int GetGridSizeX()
    {
        return _gridSizeX;
    }

    private int _gridSizeY;

    public int GetGridSizeY()
    {
        return _gridSizeY;
    }

    private int _cellSizeX;

    public int GetCellSizeX()
    {
        return _cellSizeX;
    }

    private int _cellSizeY;

    public int GetCellSizeY()
    {
        return _cellSizeY;
    }

    public GridManager(int gridSizeX, int gridSizeY, int cellSizeX, int cellSizeY)
    {
        _gridSizeX = gridSizeX;
        _gridSizeY = gridSizeY;
        _cellSizeX = cellSizeX;
        _cellSizeY = cellSizeY;
        generatedGridWithBalls = new GridCell[_gridSizeX, _gridSizeY];
        for (int i = 0; i < _gridSizeX; i++)
        {
            for (int j = 0; j < _gridSizeY; j++)
            {
                generatedGridWithBalls[i, j] = new GridCell(i, j, transformGridToVectorPosition(i, j));
            }
        }
    }

    #region Logic 
    public GridCell RegisterBall(int x, int y, Ball ball)
    {
        generatedGridWithBalls[x, y].Ball = ball;
        generatedGridWithBalls[x, y].Position = transformGridToVectorPosition(x, y);
        return generatedGridWithBalls[x, y];
    }

    public GridCell GetGridCell(int x, int y)
    {
        return generatedGridWithBalls[x, y];
    }

    Vector3 transformGridToVectorPosition(int x, int y)
    {
        float pivotTopLeftX = (y % 2 == 0) ? 0 : _cellSizeX / 2.0f;
        float positionX = pivotTopLeftX + x * _cellSizeX + _cellSizeX / 2.0f;
        float positionY = -y * _cellSizeY - _cellSizeY / 2.0f;
        return new Vector3(positionX, positionY, 0);
    }

    public bool IsValidGridPosition(int x, int y)
    {
        bool boundaryX = (x < _gridSizeX && x >= 0);
        bool boundaryY = (y < _gridSizeY && y >= 0);
        bool evenOrOdd = (y % 2) == 0 ? true : (x < _gridSizeX - 1);
        return boundaryX && boundaryY && evenOrOdd;
    }

    public bool IsOccupiedBall(int x, int y)
    {
        if (IsValidGridPosition(x, y))
        {
            return generatedGridWithBalls[x, y].Ball != null;
        }
        return false;
    }
    #endregion

    #region Neighbor
    public List<GridCell> GetNeighborSameColorBalls(Common.BallColors color, int x, int y)
    {
        List<GridCell> list = new List<GridCell>();
        List<GridCell> neightbors = GetNeighborCells(x, y);
        foreach (GridCell cell in neightbors)
        {
            if (IsOccupiedBall(cell.X, cell.Y) && cell.Ball.GetBallColor() == color)
            {
                list.Add(cell);
            }
        }
        return list;
    }

    public List<GridCell> GetNeighborCells(int x, int y)
    {
        List<GridCell> neighbors = new List<GridCell>();
        List<GridCell> pairs = new List<GridCell>();
        pairs.Add(new GridCell(x, y + 1));
        pairs.Add(new GridCell(x, y - 1));
        pairs.Add(new GridCell(x + 1, y));
        pairs.Add(new GridCell(x - 1, y));
        if (y % 2 == 0)
        {
            pairs.Add(new GridCell(x - 1, y + 1));
            pairs.Add(new GridCell(x - 1, y - 1));
        }
        else
        {
            pairs.Add(new GridCell(x + 1, y + 1));
            pairs.Add(new GridCell(x + 1, y - 1));
        }

        foreach (GridCell pair in pairs)
        {
            if (IsValidGridPosition(pair.X, pair.Y))
            {
                neighbors.Add(generatedGridWithBalls[pair.X, pair.Y]);
            }
        }

        return neighbors;
    }

    public GridCell FindNearestGridCell(Vector3 position)
    {
        List<GridCell> listNeighbors = new List<GridCell>();

        for (int i = 0; i < _gridSizeX; i++)
        {
            listNeighbors.Add(generatedGridWithBalls[i, 0]);
        }

        return findNearestGridCellInList(listNeighbors, position);
    }

    public GridCell FindNearestGridCell(GridCell cellClue, Vector3 position)
    {
        List<GridCell> listNeighbors = GetNeighborCells(cellClue.X, cellClue.Y);
        return findNearestGridCellInList(listNeighbors, position);
    }

    GridCell findNearestGridCellInList(List<GridCell> listNeighbors, Vector3 position)
    {
        float smallestDistance = 9999;
        GridCell nearestCell = null;
        foreach (GridCell gridCell in listNeighbors)
        {
            if (!IsOccupiedBall(gridCell.X, gridCell.Y))
            {
                float currentDistance = Vector3.Distance(gridCell.Position, position);

                if (currentDistance < smallestDistance)
                {
                    smallestDistance = currentDistance;
                    nearestCell = gridCell;
                }
            } 
        }
        return nearestCell;
    }

    public void RemoveBallFromGridCell(GridCell cell)
    {
        generatedGridWithBalls[cell.X, cell.Y].Ball = null;
    }


    public List<GridCell> GetListNeighborsSameColorRecursive(Ball bullet)
    {
        List<GridCell> sameColors = new List<GridCell>();
        List<GridCell> neighbors = GetNeighborSameColorBalls(bullet.GetBallColor(),
                                       bullet.GetGridPosition().X, bullet.GetGridPosition().Y);
        GridCell mainCell = bullet.GetGridPosition();
        do
        {
            List<GridCell> listTemp = new List<GridCell>();
            foreach (GridCell cell in neighbors)
            {
                List<GridCell> list = GetNeighborSameColorBalls(mainCell.Ball.GetBallColor(), cell.X, cell.Y);
                list = list.FindAll(c => !neighbors.Contains(c));
                list = list.FindAll(c => !listTemp.Contains(c));
                list = list.FindAll(c => !sameColors.Contains(c));
                if (list.Contains(mainCell))
                    list.Remove(mainCell);
                listTemp.AddRange(list);
            }
            sameColors.AddRange(neighbors);
            neighbors = listTemp;
        } while(neighbors.Count > 0);
        return sameColors;
    }
    #endregion 

    #region Holding Balls relate
    List<Ball> getListBallsFromListCells(List<GridCell> listCell)
    {
        List<Ball> listBalls = new List<Ball>();
        foreach (GridCell cell in listCell)
        {
            if(cell.Ball != null)
                listBalls.Add(cell.Ball);
        }
        return listBalls;
    }

    public List<Ball> GetListUnHoldBallsAndUnHoldFromGrid()
    {
        List<Ball> removedList = GetListUnHoldBallsAndUnHoldFromGridRecursive();
        return removedList;
    }

    List<Ball> GetListUnHoldBallsAndUnHoldFromGridRecursive()
    {
        
        List<Ball> removedList = new List<Ball>();
        List<GridCell> unDecidedList = new List<GridCell>();
        for (int j = 1; j < _gridSizeY; j++)
        {
            for (int i = 0; i < _gridSizeX; i++)
            {
                if (IsOccupiedBall(i, j))
                {
                    GridCell cell = generatedGridWithBalls[i, j];
                    List<GridCell> listParents = getParentBallsBasedCell(cell);
                    List<GridCell> listSameLevel = getSameLevelBallsBasedCell(cell);
                    if (listParents.Count == 0)
                    {
                        if (listSameLevel.Count == 0)
                        {
                            removedList.Add(cell.Ball);
                            RemoveBallFromGridCell(cell);
                        }
                        else
                        {
                            List<GridCell> listSameLevelNoParent = getSameLevelNoParentBallBasedCell(cell);
                            if (listSameLevelNoParent.Count > 0)
                            {
                                List<GridCell> list = listSameLevel.FindAll(c => unDecidedList.Contains(c));
                                if (list.Count > 0)
                                {
                                    removedList.Add(cell.Ball);
                                    RemoveBallFromGridCell(cell);

                                    removedList.AddRange(getListBallsFromListCells(list));
                                    list.ForEach(delegate(GridCell c)
                                        {
                                            RemoveBallFromGridCell(c);
                                            unDecidedList.Remove(c);
                                        });
                                }
                                else
                                {
                                    unDecidedList.Add(cell);
                                }
                            }
                        }
                    }

                }
            }
            unDecidedList.Clear();
        }  
        if (removedList.Count == 0)
            return removedList;
        
        removedList.AddRange(GetListUnHoldBallsAndUnHoldFromGridRecursive());
        return removedList;
    }

    List<GridCell> getSameLevelBallsBasedCell(GridCell cell)
    {
        List<GridCell> parentList = new List<GridCell>();
        List<GridCell> pairs = new List<GridCell>();
        pairs.Add(new GridCell(cell.X - 1, cell.Y));
        pairs.Add(new GridCell(cell.X + 1, cell.Y));
     
        foreach (GridCell pair in pairs)
        {
            if (IsValidGridPosition(pair.X, pair.Y) && IsOccupiedBall(pair.X, pair.Y))
            {
                parentList.Add(generatedGridWithBalls[pair.X, pair.Y]);
            }
        }
        return parentList;
    }

    List<GridCell> getParentBallsBasedCell(GridCell cell)
    {
        List<GridCell> parentList = new List<GridCell>();
        List<GridCell> pairs = new List<GridCell>();
        pairs.Add(new GridCell(cell.X, cell.Y - 1));
        if (cell.Y % 2 == 0)
        {
            pairs.Add(new GridCell(cell.X - 1, cell.Y - 1));
        }
        else
        {
            pairs.Add(new GridCell(cell.X + 1, cell.Y - 1));
        }

        foreach (GridCell pair in pairs)
        {
            if (IsValidGridPosition(pair.X, pair.Y) && IsOccupiedBall(pair.X, pair.Y))
            {
                parentList.Add(generatedGridWithBalls[pair.X, pair.Y]);
            }
        }
        return parentList;
    }

    List<GridCell> getChildBallsBasedCell(GridCell cell)
    {
        List<GridCell> childList = new List<GridCell>();
        List<GridCell> pairs = new List<GridCell>();
        pairs.Add(new GridCell(cell.X, cell.Y + 1));
        if (cell.Y > 0)
        {
            pairs.Add(new GridCell(cell.X - 1, cell.Y));
            pairs.Add(new GridCell(cell.X + 1, cell.Y));
        }

        if (cell.Y % 2 == 0)
        {
            pairs.Add(new GridCell(cell.X - 1, cell.Y + 1));
        }
        else
        {
            pairs.Add(new GridCell(cell.X + 1, cell.Y + 1));
        }

        foreach (GridCell pair in pairs)
        {
            if (IsValidGridPosition(pair.X, pair.Y) && IsOccupiedBall(pair.X, pair.Y))
            {
                childList.Add(generatedGridWithBalls[pair.X, pair.Y]);
            }
        }
        return childList;
    }

    List<GridCell> getSameLevelNoParentBallBasedCell(GridCell cell){
        List<GridCell> result = new List<GridCell>();
        List<GridCell> listChild = getSameLevelBallsBasedCell(cell);
        foreach (GridCell c in listChild)
        {
            List<GridCell> listParent = getParentBallsBasedCell(c);
            if (listParent.Count == 0)
            {
                result.Add(c);
            }
        }
        return result;
    }
   
    #endregion 

    #region Debug

    public void DrawGrids(Transform pivot, GameObject prefabGrid)
    {
        for (int i = 0; i < _gridSizeX; i++)
        {
            for (int j = 0; j < _gridSizeY; j++)
            {
                if (IsValidGridPosition(i, j))
                {
                    GameObject go = (GameObject)GameObject.Instantiate(prefabGrid);
                    go.transform.parent = pivot;
                    go.transform.localScale = Vector3.one;
                    go.transform.localPosition = transformGridToVectorPosition(i, j);
                }
            }
        }
    }

    #endregion

}
