using UnityEngine;

public class ParalexController : MonoBehaviour {
    public Transform[] Backgrounds;
    public float ParalaxScale;
    public float ParalaxReductionScale;
    public float Smoothing;

    private Vector3 _lastPosition;

    public void Start()
    {
        _lastPosition = transform.position;

    }

    public void Update()
    {
        var paralax = (_lastPosition.x - transform.position.x) * ParalaxScale;

        for (var i=0; i< Backgrounds.Length; i++)
        {
            var backGroundTargetPosition = Backgrounds[i].position.x + paralax * (i * ParalaxReductionScale + 1);
            Backgrounds[i].position = Vector3.Lerp(
                Backgrounds[i].position,
                new Vector3(backGroundTargetPosition, Backgrounds[i].position.y, Backgrounds[i].position.z),
                Smoothing *  Time.deltaTime);
        }

        _lastPosition = transform.position;
    }



}
