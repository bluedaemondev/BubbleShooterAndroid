using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PointsManager : MonoBehaviour
{
    public static PointsManager instance { get; private set; }

    int total = 0;

    [Header("Multiplicador de combo actual")]
    public float comboMultiplier = 1;
    [Header("Maximo del multiplicador de combo")]
    public float comboMultiplierMax = 4;
    [Space]
    [Header("Tiempo hasta bajar un nivel de multiplicador de combo")]
    public float timeToResetMultiplier = 4.5f;


    //public float currentTimeInCombo = 0; // timer actual

    public UnityEvent<int> onPointsUpdate;
    public UnityEvent<float> onMultiplierUpdate;
    public UnityEvent onSetMultiplier;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
            instance = this;

        onPointsUpdate = new UnityEvent<int>();
        onMultiplierUpdate = new UnityEvent<float>();

    }

    /// <summary>
    /// Llamar en un inicio de escena si requiere iniciar el sistema de vuelta
    /// o por primera vez
    /// </summary>
    public void StartAccountance()
    {
        total = 0;
        comboMultiplier = 1;

        StopAllCoroutines();
        onPointsUpdate.Invoke(GetTotalPoints());
        onMultiplierUpdate.Invoke(GetComboMultiplier());
    }

    /// <summary>
    /// Para iniciar un multiplicador de puntos
    /// </summary>
    /// <param name="targetNumber">Numero maximo deseado para el multiplicador</param>
    public void SetMultiplier(float targetNumber)
    {
        for (int i = 0; i < targetNumber; i++)
        {
            SumToComboMultiplier();
            if (comboMultiplier == comboMultiplierMax)
                break;
        }
        StartCoroutine(DecreaseMultiplierOverTime());
    }

    /// <summary>
    /// Coroutine para ir bajando el multiplicador a medida que pasa el tiempo
    /// </summary>
    /// <returns></returns>
    private IEnumerator DecreaseMultiplierOverTime()
    {
        while (comboMultiplier > 1)
        {
            // espero los segundos necesarios hasta bajar un nivel de multiplicador
            yield return new WaitForSeconds(timeToResetMultiplier);
            // bajo un nivel y actualizo ui necesaria
            LowerComboMultiplier();
            // repito hasta bajarlo a 1
        }
    }

    public int GetTotalPoints()
    {
        return this.total;
    }

    public void AddToTotal(int pts)
    {
        this.total += (int)(pts * comboMultiplier);

        onPointsUpdate.Invoke(GetTotalPoints());

        Debug.Log("total actual = " + GetTotalPoints());
    }

    public float SumToComboMultiplier()
    {
        if (this.comboMultiplier < comboMultiplierMax)
            this.comboMultiplier++;
        else
            this.comboMultiplier = comboMultiplierMax;

        onMultiplierUpdate.Invoke(GetComboMultiplier());

        // definir una formula para sumar puntos de forma equilibrada
        // con alguna curva de internet

        return GetComboMultiplier();
    }
    public float LowerComboMultiplier()
    {
        if (this.comboMultiplier > 1)
            this.comboMultiplier--;
        else
            ResetComboMultiplier();

        // definir una formula para sumar puntos de forma equilibrada
        // con alguna curva de internet
        onMultiplierUpdate.Invoke(GetComboMultiplier());

        return GetComboMultiplier();
    }

    public float GetComboMultiplier()
    {
        return this.comboMultiplier;
    }

    public void ResetComboMultiplier()
    {
        this.comboMultiplier = 1;
    }
}
