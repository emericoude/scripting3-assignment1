using UnityEngine;

public class Move : MonoBehaviour {

    [Header("General")]
    [SerializeField] private Transform _goal;
    [SerializeField] private float _speed = 3f;
    
    enum MoveType{MoveTowards, Translate, Lerp}
    [SerializeField] private MoveType _moveType = MoveType.MoveTowards;

    [Header("Translate")]
    [SerializeField] private float _maxDistance = 0.1f;

    void Update() {
        transform.LookAt(_goal);
        Vector3 direction = _goal.position - transform.position;

        switch (_moveType)
        {
            case MoveType.MoveTowards:
                transform.position = Vector3.MoveTowards(transform.position, _goal.position, _speed * Time.deltaTime);
                break;
            
            case MoveType.Translate:
                if (Vector3.Distance(transform.position, _goal.position) > _maxDistance)
                    transform.Translate(direction.normalized * _speed * Time.deltaTime);
                break;

            case MoveType.Lerp:
                if (Vector3.Distance(transform.position, _goal.position) > _maxDistance)
                    transform.position = Vector3.Lerp(transform.position, _goal.position, _speed * Time.deltaTime);
                break;
        }
    }
}
