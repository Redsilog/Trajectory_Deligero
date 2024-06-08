using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ball : MonoBehaviour
{
    [SerializeField] float _Angle;
    [SerializeField] LineRenderer _Line;
    [SerializeField] float _Step;
    [SerializeField] Transform _FirePoint;
    [SerializeField] float _IncrementRate = 10.0f;
    [SerializeField] public float delay;
    [SerializeField] animation Animation;

    float _InitialVelocity;
    private void Update()
    {
        float angle = _Angle * Mathf.Deg2Rad;
        DrawPath(_InitialVelocity, angle, _Step);

        if (Input.GetMouseButton(0))
        {
            _InitialVelocity += _IncrementRate * Time.deltaTime;
            transform.position = _FirePoint.position;
            Animation.animator.SetBool("dance", false);
        }

        if (Input.GetMouseButtonUp(0))
        {
            StopAllCoroutines();
            StartCoroutine(Coroutine_Movement(_InitialVelocity, angle));
            _InitialVelocity = 0;
        }
    }

    private void DrawPath(float v0, float angle, float step)
    {
        step = Mathf.Max(0.01f, step);
        float totalTime = 10;
        _Line.positionCount = (int)(totalTime / step) + 2;
        int count = 0;
        for (float i = 0; i < totalTime; i += step)
        {
            float x = v0 * i * Mathf.Cos(angle);
            float y = v0 * i * Mathf.Sin(angle) - 0.5f * -Physics.gravity.y * Mathf.Pow(i, 2);
            _Line.SetPosition(count, _FirePoint.position + new Vector3(x, y, 0));
            count++;
        }
        float xfinal = v0 * totalTime * Mathf.Cos(angle);
        float yfinal = v0 * totalTime * Mathf.Sin(angle) - 0.5f * -Physics.gravity.y * Mathf.Pow(totalTime, 2);
        _Line.SetPosition(count, _FirePoint.position + new Vector3(xfinal, yfinal, 0));
    }

    private void CalculatePath(Vector3 targetPos, float angle, out float v0, out float time)
    {
        float xt = targetPos.x;
        float yt = targetPos.y;
        float g = -Physics.gravity.y;

        float v1 = Mathf.Pow(xt, 2) * g;
        float v2 = 2 * xt * Mathf.Sin(angle) * Mathf.Cos(angle);
        float v3 = 2 * yt * Mathf.Pow(Mathf.Cos(angle), 2);
        v0 = Mathf.Sqrt(v1 / (v2 - v3));

        time = xt / (v0 * Mathf.Cos(angle));
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Goal"))
        {
            Animation.animator.SetBool("dance", true);
        }
    }

    IEnumerator Coroutine_Movement(float v0, float angle)
    {
        yield return new WaitForSeconds(delay);
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.useGravity = true;

            float x = v0 * Mathf.Cos(angle);
            float y = v0 * Mathf.Sin(angle);
            rb.velocity = new Vector3(x, y, 0);
    }
}