/*******************************************************
 * Copyright (C) 2016 Ngan Do - dttngan91@gmail
 *******************************************************/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BallManager : MonoBehaviour
{

    public GameObject BallPrefab;
    public Transform PivotGrid;

    GridManager _gridManager;
    GridManager _secondaryGridManager;

    int _numberOfInitRow;
    int _numberOfDiffColor;
    Vector3 _originalPosition;

    Common.SimpleEvent _clearBallEvent;
    Common.SimpleEventIntegerParams _scoreEvent;
    Score _score;

    // Use this for initialization
    void Start()
    {
        _originalPosition = PivotGrid.localPosition;
        _score = new Score();
    }

    // Update is called once per frame
    //void Update()
    //{
    //}

    #region Logic ball

    public void InitBalls(LevelProfile level)
    {
        if (_gridManager == null)
        {
            _gridManager = new GridManager(8, 14, 120, 100);

            if (level is ProceduralLevelProfile)
            {
                _secondaryGridManager = new GridManager(8, 14, 120, 100, new Vector2(0, -_gridManager.GetCellSizeY() / 2 * 27 - _gridManager.GetCellSizeY() / 2.0f));
            }

            _numberOfInitRow = level.GetInitRow();
            _numberOfDiffColor = level.GetNumColor();
        }

        for (int i = 0; i < _gridManager.GetGridSizeX(); i++)
        {
            for (int j = 0; j < _numberOfInitRow; j++)
            {
                if (_gridManager.IsValidGridPosition(i, j))
                {
                    Ball ball = instantiateNewBall(randomBallColor(_numberOfDiffColor + 1));
                    assignBallToGrid(ball, i, j);
                    ball.FixPosition();
                }
            }
        }

    }

    public Ball GenerateBallAsBullet()
    {
        Common.BallColors randomColor = (Common.BallColors)Random.Range(1, _numberOfDiffColor + 1);
        Ball ball = instantiateNewBall(randomColor);
        ball.tag = Common.LAYER_BULLET;
        ball.SetNewLayer(Common.LAYER_BULLET);
        return ball;
    }

    Ball instantiateNewBall(Common.BallColors color)
    {
        GameObject go = (GameObject)GameObject.Instantiate(BallPrefab);
        //GameObject go = ObjectPooler.instance.SpawnFromPool("ball");
        go.transform.parent = PivotGrid;
        go.transform.localScale = Vector3.one;
        go.transform.localPosition = new Vector3(-1000, 0, 0);

        Ball ball = go.GetComponent<Ball>();
        ball.Init(this);
        ball.SetBallColor(color);

        return ball;
    }

    public void SwitchGridsPosition()
    {
        var gridtemp = this._secondaryGridManager;
        Debug.Log("Finished grid - switching to next");
        _secondaryGridManager = _gridManager;
        _gridManager = gridtemp;

    }

    void assignBallToGrid(Ball ball, int x, int y, bool useMain = true)
    {
        GridCell grid;
        if (useMain)
            grid = _gridManager.RegisterBall(x, y, ball);
        else
        {
            grid = _secondaryGridManager.RegisterBall(x, y, ball);
        }

        ball.SetGridPosition(grid);

        ball.transform.localPosition = ball.GetGridPosition().Position;
        ball.name = "Ball_" + x.ToString() + y.ToString();
    }

    Common.BallColors randomBallColor(int num)
    {
        return (Common.BallColors)Random.Range(1, num);
    }

    Common.BallColors getBallColorsFixGeneration(int x, int y)
    {
        Common.BallColors color = Common.BallColors.None;
        if (y < 2)
        {
            if (x < 2)
            {
                color = Common.BallColors.Blue;
            }
            else if (x < 4)
            {
                color = Common.BallColors.Green;
            }
            else if (x < 6)
            {
                color = Common.BallColors.Yellow;
            }
            else if (x < 8)
            {
                color = Common.BallColors.Red;
            }
        }
        else
        {
            if (x < 2)
            {
                color = Common.BallColors.Red;
            }
            else if (x < 4)
            {
                color = Common.BallColors.Yellow;
            }
            else if (x < 6)
            {
                color = Common.BallColors.Blue;
            }
            else if (x < 8)
            {
                color = Common.BallColors.Green;
            }
        }
        return color;
    }

    public void AssignBulletToGrid(Ball bullet, Vector3 position)
    {
        bullet.transform.parent = PivotGrid;
        bullet.transform.localScale = Vector3.one;

        GridCell nearestCell = _gridManager.FindNearestGridCell(bullet.transform.localPosition);
        assignBallToGrid(bullet, nearestCell.X, nearestCell.Y);
    }

    public void AssignBulletToGrid(Ball bullet, GridCell gridCellClue)
    {

        bullet.transform.parent = PivotGrid;
        bullet.transform.localScale = Vector3.one;

        GridCell nearestCell = _gridManager.FindNearestGridCell(gridCellClue, bullet.transform.localPosition);
        assignBallToGrid(bullet, nearestCell.X, nearestCell.Y);
    }

    void removeBallFromGrid(GridCell cell)
    {
        _gridManager.RemoveBallFromGridCell(cell);
    }
    void AddBallToSecondaryGridCell(GridCell cell)
    {
        var positionInSecondaryGrid = CalculateComplementaryGridCell(cell);
        if (_secondaryGridManager.IsValidGridPosition(positionInSecondaryGrid.x, positionInSecondaryGrid.y))
        {
            var ball = instantiateNewBall(randomBallColor(_numberOfDiffColor + 1));
            assignBallToGrid(ball, positionInSecondaryGrid.x, positionInSecondaryGrid.y, false);
            ball.FixPosition();

            //Debug.Log("ball => " + ball.name + " " + ball.GetGridPosition());

        }
    }

    private Vector2Int CalculateComplementaryGridCell(GridCell cell)
    {
        Vector2Int result = new Vector2Int();

        result.x = cell.X;
        result.y = cell.Y;


        return result;
    }

    void removeBallFromGame(Ball ball)
    {
        if (ball != null)
            ball.RemoveBall();
    }

    int removeAllUnHoldBall()
    {
        List<Ball> listUnHoldBalls = _gridManager.GetListUnHoldBallsAndUnHoldFromGrid();

        foreach (Ball b in listUnHoldBalls)
        {
            b.EffectFallingBall();
        }
        return listUnHoldBalls.Count;
    }

    #endregion

    #region Actions and Events
    public void ExplodeSameColorBall(Ball ball)
    {
        if (checkAndExplodeSameColorBall(ball))
        {
            AudioManager.Instance.PlaySound(AudioManager.Instance.success);
            checkClearBalls();
        }
    }

    public bool checkAndExplodeSameColorBall(Ball bullet)
    {
        //Debug.Log("Checking...");
        // get list same colors
        List<GridCell> listSameColors;
        if (!bullet.isLineSpecial)
        {
            listSameColors = _gridManager.GetListNeighborsSameColorRecursive(bullet);
            if(_secondaryGridManager != null)
            {
                listSameColors.AddRange(_secondaryGridManager.GetListNeighborsSameColorRecursive(bullet));
            }
        }
        else
        {
            // es especial de linea
            listSameColors = _gridManager.GetListNeighborsInLine(bullet);
        }

        bool isExploded = listSameColors.Count >= 2;

        // check explode 
        if (isExploded)
        {
            // remove all same colors
            listSameColors.Add(bullet.GetGridPosition());
            int noBallsSameColor = listSameColors.Count;
            foreach (GridCell cell in listSameColors)
            {
                if (cell.Ball)
                    cell.Ball.EffectExplodeBall(); // To test
                else
                {
                    PointsManager.instance.InstantiatePointAddEffect(cell.Position);
                }

                removeBallFromGrid(cell);
                //AddToSecondaryGrid(cell);
            }

            // remove unrelated/ unhold balls
            int noBallFallingDown = removeAllUnHoldBall();

            if (_scoreEvent != null)
            {
                int calScore = _score.CalculateScore(noBallsSameColor, noBallFallingDown);
                _score.SetScore(_score.GetScore() + calScore);
                _scoreEvent(_score.GetScore());
            }
        }

        return isExploded;
    }

    public float PushDown()
    {

        for (int rowEmpty = _secondaryGridManager.GetGridSizeY() - 1; rowEmpty >= 0; rowEmpty--)
        {
            // recorro desde abajo hacia arriba buscando la primera fila que no tenga nada
            //para agregar una linea
            if (!_secondaryGridManager.IsOccupiedBall(0, rowEmpty))
            {
                AddLineToSecondaryGrid(rowEmpty);
                break;
            }
        }

        float heightDown = _gridManager.GetCellSizeY();
        PivotGrid.localPosition -= new Vector3(0, heightDown, 0);

        return heightDown;
    }

    private void AddLineToSecondaryGrid(int rowEmpty)
    {
        for (int x = 0; x < _secondaryGridManager.GetGridSizeX(); x++)
        {
            var targetCell = _gridManager.GetGridCell(x, rowEmpty);
            this.AddBallToSecondaryGridCell(targetCell);
            //if (_secondaryGridManager.IsValidGridPosition(x, rowEmpty))
            //{
            //var newball = instantiateNewBall(randomBallColor(_numberOfDiffColor + 1));
            //assignBallToGrid(newball, x, rowEmpty, false);
            //newball.FixPosition();
            //}
        }
    }

    public void ClearBalls()
    {
        for (int i = 0; i < _gridManager.GetGridSizeX(); i++)
        {
            for (int j = 0; j < _gridManager.GetGridSizeY(); j++)
            {
                removeBallFromGame(_gridManager.GetGridCell(i, j).Ball);
                removeBallFromGrid(_gridManager.GetGridCell(i, j));

                if(_secondaryGridManager != null)
                {
                    removeBallFromGame(_secondaryGridManager.GetGridCell(i, j).Ball);
                    removeBallFromGrid(_secondaryGridManager.GetGridCell(i, j));
                }
            }
        }
    }

    int countRemainingBalls()
    {
        int count = 0;
        for (int i = 0; i < _gridManager.GetGridSizeX(); i++)
        {
            for (int j = 0; j < _gridManager.GetGridSizeY(); j++)
            {
                if (_gridManager.IsOccupiedBall(i, j))
                    count++;
                //if (_secondaryGridManager.IsOccupiedBall(i, j))
                //    count++;

            }
        }
        return count;
    }

    void checkClearBalls()
    {
        if (countRemainingBalls() == 0)
        {
            if (_clearBallEvent != null)
            {
                _clearBallEvent();
            }
        }
    }

    public void RegisterEventClearBall(Common.SimpleEvent e)
    {
        _clearBallEvent = e;
    }

    public void Reset()
    {
        PivotGrid.localPosition = _originalPosition;
        _score.SetScore(0);
    }

    public void RegisterEventCalculateScore(Common.SimpleEventIntegerParams e)
    {
        _scoreEvent = e;
    }

    #endregion
}
