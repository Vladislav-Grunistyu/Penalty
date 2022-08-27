using System;
using System.Collections.Generic;
using UnityEngine;

public class Goalkepper : MonoBehaviour
{
    public static Action onGoalkepperCaught;

    [SerializeField] Transform[] _patchElements;

    private float _speed = 0;
    private int _moveingTo = 0;
    private readonly int _movementDirection = 1;
    private readonly float _maxDistance = .1f;

    private IEnumerator<Transform> _pointInPatch;
    private void Start()
    {
        _pointInPatch = GetNextPatchPoint();

        _pointInPatch.MoveNext();

        if (_pointInPatch.Current == null)
            throw new InvalidOperationException();

        transform.position = _pointInPatch.Current.position;
    }

    private void FixedUpdate()
    {
        if (_pointInPatch == null || _pointInPatch.Current == null)
        {
            return;
        }
        transform.position = Vector3.MoveTowards(transform.position, _pointInPatch.Current.position, Time.deltaTime * _speed);

        var distanceSqure = (transform.position - _pointInPatch.Current.position).sqrMagnitude;
        if (distanceSqure < _maxDistance * _maxDistance)
        {
            _pointInPatch.MoveNext();
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Puck>())
        {
            Destroy(collision.gameObject);
            onGoalkepperCaught?.Invoke();
        }
    }

    private IEnumerator<Transform> GetNextPatchPoint()
    {
        if (_patchElements == null || _patchElements.Length < 1)
        {
            yield break;
        }
        while (true)
        {
            yield return _patchElements[_moveingTo];

            if (_patchElements.Length == 1)
            {
                continue;
            }
            _moveingTo += _movementDirection;
            if (_moveingTo >= _patchElements.Length)
            {
                _moveingTo = 0;
            }
            if (_moveingTo < 0)
            {
                _moveingTo = _patchElements.Length - 1;
            }
        }
    }

    public void AddSpeed(float speed)
    {
        if (speed <= 0)
            throw new InvalidOperationException();
        _speed += speed;
    }
}
