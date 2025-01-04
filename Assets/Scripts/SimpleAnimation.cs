
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;

public class SimpleAnimation : MonoBehaviour
{
    public bool doingAnimation = false;
    public float hopTime = .2f;
    public float hopDistance = .5f;
    public float hopSpeed = .001f;


    public void Awake()
    {
        Time.timeScale = 1;
    }

    public void DoLittleHop()
    {
        StartCoroutine(LittleHop(GetComponentInParent<Transform>()));
    }

    public void DoBigHop()
    {
        StartCoroutine(BigHop(GetComponentInParent<Transform>()));
    }

    public void DoRightSlide()
    {
        StartCoroutine(RightSlide(GetComponentInParent<Transform>()));
    }

    public void DoLeftSlide()
    {
        StartCoroutine(LeftSlide(GetComponentInParent<Transform>()));
    }


    IEnumerator LittleHop(Transform gO)
    {
        if (doingAnimation)
        {
            yield return new WaitUntil(() => !doingAnimation);
        }
        if (gO==null)
        {
            Debug.LogWarning("object destroyed mid animation");
            yield break;
        }

        doingAnimation = true;
        float timer = 0;
        float originalHeight = gO.transform.position.y;
        float endHeight = gO.transform.position.y + hopDistance;
        float halfwayTime = hopTime/2;
        Vector2 originalPosition = new Vector2(gO.position.x, gO.position.y);
        Vector2 peakPosition = new Vector2(gO.position.x, endHeight);

        // if (doingAnimation)
        // {
        // Vector2 currentPosition = new Vector2(gO.position.x, gO.position.y);
        // Vector2 newPosition = new Vector2(gO.transform.position.x, gO.transform.position.y + hopDistance);
        // gO.transform.position = Vector2.Lerp(currentPosition, newPosition, Time.deltaTime*hopSpeed);    
        // }
        

        while (timer < halfwayTime)
        {
            if (gO!=null)
            {
                float percent = timer/halfwayTime;
                // float midChangeHeight = hopDistance * percent;
                // midChangeHeight += originalHeight;
                // Debug.Log(midChangeHeight);
                gO.transform.position = Vector2.Lerp(originalPosition, peakPosition, percent);
                timer += Time.deltaTime;
                yield return null;
            }
            else
            {
                Debug.LogWarning("object destroyed mid animation");
                doingAnimation = false;
                yield break;
            }
        }
        // Debug.Log("Finished Going Up");
        timer = 0;
        while(timer < halfwayTime)
        {
            if (gO != null)
            {
                float percent = timer/halfwayTime;
                // float midChangeHeight = hopDistance * percent;
                // midChangeHeight = endHeight - midChangeHeight;
                // Debug.Log(midChangeHeight);
                gO.transform.position =  Vector2.Lerp(peakPosition, originalPosition,  percent);
                timer += Time.deltaTime;
                yield return null;
            }
            else
            {
                Debug.LogWarning("object destroyed mid animation");
                doingAnimation = false;
                yield break;
            }
        }


        // if (doingAnimation)
        // {
        // Vector2 currentPosition = new Vector2(gO.position.x, gO.position.y);
        // Vector2 newPosition = new Vector2(gO.transform.position.x, gO.transform.position.y - hopDistance);
        // gO.transform.position = Vector2.Lerp(currentPosition, newPosition, Time.deltaTime*hopSpeed);   
        // }
        

        gO.transform.position = originalPosition;
        // Debug.Log("Finished Going Down");
        doingAnimation = false;
        yield return null;
    }

    IEnumerator BigHop(Transform gO)
    {
        if (doingAnimation)
        {
            yield return new WaitUntil(() => !doingAnimation);
        }
        if (gO==null)
        {
            Debug.LogWarning("object destroyed mid animation");
            yield break;
        }

        doingAnimation = true;
        float timer = 0;
        float originalHeight = gO.transform.position.y;
        float endHeight = gO.transform.position.y + hopDistance*2; //edited hop distance to go two times as high
        float halfwayTime = hopTime;
        Vector2 originalPosition = new Vector2(gO.position.x, gO.position.y);
        Vector2 peakPosition = new Vector2(gO.position.x, endHeight);

        // if (doingAnimation)
        // {
        // Vector2 currentPosition = new Vector2(gO.position.x, gO.position.y);
        // Vector2 newPosition = new Vector2(gO.transform.position.x, gO.transform.position.y + hopDistance);
        // gO.transform.position = Vector2.Lerp(currentPosition, newPosition, Time.deltaTime*hopSpeed);    
        // }
        

        while (timer < halfwayTime)
        {
            if (gO!=null)
            {
                float percent = timer/halfwayTime;
                // float midChangeHeight = hopDistance * percent;
                // midChangeHeight += originalHeight;
                // Debug.Log(midChangeHeight);
                gO.transform.position = Vector2.Lerp(originalPosition, peakPosition, percent);
                timer += Time.deltaTime;
                yield return null;
            }
            else
            {
                Debug.LogWarning("object destroyed mid animation");
                doingAnimation = false;
                yield break;
            }
        }
        // Debug.Log("Finished Going Up");
        timer = 0;
        while(timer < halfwayTime)
        {
            if (gO != null)
            {
                float percent = timer/halfwayTime;
                // float midChangeHeight = hopDistance * percent;
                // midChangeHeight = endHeight - midChangeHeight;
                // Debug.Log(midChangeHeight);
                gO.transform.position =  Vector2.Lerp(peakPosition, originalPosition,  percent);
                timer += Time.deltaTime;
                yield return null;
            }
            else
            {
                Debug.LogWarning("object destroyed mid animation");
                doingAnimation = false;
                yield break;
            }
        }


        // if (doingAnimation)
        // {
        // Vector2 currentPosition = new Vector2(gO.position.x, gO.position.y);
        // Vector2 newPosition = new Vector2(gO.transform.position.x, gO.transform.position.y - hopDistance);
        // gO.transform.position = Vector2.Lerp(currentPosition, newPosition, Time.deltaTime*hopSpeed);   
        // }
        

        gO.transform.position = originalPosition;
        // Debug.Log("Finished Going Down");
        doingAnimation = false;
        yield return null;
    }

    IEnumerator RightSlide(Transform gO)
    {
        if (doingAnimation)
        {
            yield return new WaitUntil(() => !doingAnimation);
        }
        if (gO==null)
        {
            Debug.LogWarning("object destroyed mid animation");
            yield break;
        }

        doingAnimation = true;
        float timer = 0;
        float originalXPo = gO.transform.position.x;
        float endX = gO.transform.position.x + hopDistance;
        float halfwayTime = hopTime/2;
        Vector2 originalPosition = new Vector2(gO.position.x, gO.position.y);
        Vector2 peakPosition = new Vector2(endX, gO.position.y);
        

        while (timer < halfwayTime)
        {
            if (gO!=null)
            {
                float percent = timer/halfwayTime;
                gO.transform.position = Vector2.Lerp(originalPosition, peakPosition, percent);
                timer += Time.deltaTime;
                yield return null;
            }
            else
            {
                Debug.LogWarning("object destroyed mid animation");
                doingAnimation = false;
                yield break;
            }
        }
        // Debug.Log("Finished Moving Forward");
        timer = 0;
        while(timer < halfwayTime)
        {
            if (gO != null)
            {
                float percent = timer/halfwayTime;
                gO.transform.position =  Vector2.Lerp(peakPosition, originalPosition,  percent);
                timer += Time.deltaTime;
                yield return null;
            }
            else
            {
                Debug.LogWarning("object destroyed mid animation");
                doingAnimation = false;
                yield break;
            }
        }

        gO.transform.position = originalPosition;
        // Debug.Log("Finished Moving Back");
        doingAnimation = false;
        yield return null;
    }

    IEnumerator LeftSlide(Transform gO)
    {
        if (doingAnimation)
        {
            yield return new WaitUntil(() => !doingAnimation);
        }
        if (gO==null)
        {
            Debug.LogWarning("object destroyed mid animation");
            yield break;
        }

        doingAnimation = true;
        float timer = 0;
        float originalXPo = gO.transform.position.x;
        float endX = gO.transform.position.x - hopDistance;
        float halfwayTime = hopTime/2;
        Vector2 originalPosition = new Vector2(gO.position.x, gO.position.y);
        Vector2 peakPosition = new Vector2(endX, gO.position.y);
        

        while (timer < halfwayTime)
        {
            if (gO!=null)
            {
                float percent = timer/halfwayTime;
                gO.transform.position = Vector2.Lerp(originalPosition, peakPosition, percent);
                timer += Time.deltaTime;
                yield return null;
            }
            else
            {
                Debug.LogWarning("object destroyed mid animation");
                doingAnimation = false;
                yield break;
            }
        }
        // Debug.Log("Finished Moving Forward");
        timer = 0;
        while(timer < halfwayTime)
        {
            if (gO != null)
            {
                float percent = timer/halfwayTime;
                gO.transform.position =  Vector2.Lerp(peakPosition, originalPosition,  percent);
                timer += Time.deltaTime;
                yield return null;
            }
            else
            {
                Debug.LogWarning("object destroyed mid animation");
                doingAnimation = false;
                yield break;
            }
        }

        gO.transform.position = originalPosition;
        // Debug.Log("Finished Moving Back");
        doingAnimation = false;
        yield return null;
    }



}
