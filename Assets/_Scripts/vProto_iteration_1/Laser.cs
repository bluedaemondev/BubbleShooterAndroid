using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public int maxReflectionCount = 5;
    public int maxSplitCount = 5;
    public float maxStepDistance = 100;

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying)
        {
            return;
        }
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        var ghjkagsdkhm = (transform.position - mousePos).normalized * maxStepDistance;
        DrawPredictedReflection(this.transform.position, -ghjkagsdkhm, maxReflectionCount, maxSplitCount);
    }

    void DrawPredictedReflection(Vector2 position, Vector2 direction, int reflectionsRemaining, int splitsRemaining)
    {
        var gizmoHue = (reflectionsRemaining / (this.maxReflectionCount + 1f));
        Gizmos.color = Color.HSVToRGB(gizmoHue, 1, 1);
        RaycastHit2D hit2D = Physics2D.Raycast(position, direction, maxStepDistance);

        if (hit2D) //did we hit somthing?
        {
            Gizmos.DrawLine(position, hit2D.point);
            Gizmos.DrawWireSphere(hit2D.point, 0.25f);

            if (hit2D.transform.gameObject.tag == "Receiver")
            {
                Debug.Log("Receiver hit");
            }
            if (hit2D.transform.gameObject.tag == "bouncer") //mirror hit. set new pos where hit. reflect angle and make that new direction
            {
                //Debug.Log("Mirror Hit");
                direction = Vector2.Reflect(direction, hit2D.normal);
                position = hit2D.point + direction * 0.01f;

                if (reflectionsRemaining > 0)
                    DrawPredictedReflection(position, direction, --reflectionsRemaining, splitsRemaining);
            }
            if (hit2D.transform.gameObject.tag == "Splitter") //reflect and go ahead
            {

                Debug.Log("Splitter hit");
                if (splitsRemaining > 0)//go ahead
                {
                    Debug.Log("Splitting");
                    Vector2 splitPosition = new Vector2();
                    Vector2 findOppBegin = hit2D.point + direction * 1f;
                    RaycastHit2D[] findOppHit = Physics2D.RaycastAll(findOppBegin, -direction);
                    for (int i = 0; i <= findOppHit.Length; i++) //findOppHit[i].transform.gameObject != hit2D.transform.gameObject
                    {
                        if (findOppHit[i].transform.gameObject == hit2D.transform.gameObject)
                        {
                            splitPosition = findOppHit[i].point + direction * 0.01f;
                            break;
                        }
                    }

                    DrawPredictedReflection(splitPosition, direction, reflectionsRemaining, --splitsRemaining);
                }


                direction = Vector2.Reflect(direction, hit2D.normal);
                position = hit2D.point + direction * 0.01f;
                if (reflectionsRemaining > 0)//reflect too
                {
                    DrawPredictedReflection(position, direction, --reflectionsRemaining, splitsRemaining);
                }
            }
        }
    }
}
