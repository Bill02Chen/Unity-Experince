using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceBrick : MonoBehaviour
{
    public Brick[] BrickLib;
    public Material[] MatLib;
    protected Brick PrefabBrick;
    public Material TransparentMat;
    protected Material BrickMat;

    protected Controller Controller;
    protected Brick CurrentBrick;
    protected bool PositionOk;

    void Awake()
    {
        Controller = GetComponent<Controller>();
    }

    // Start is called before the first frame update
    void Start()
    {
        PrefabBrick = BrickLib[0];
        BrickMat = MatLib[0];
    }

    // Update is called once per frame
    void Update()
    {
        if (Controller.Mode != Controller.ControllerMode.Build)
        {
            if (CurrentBrick != null)
                Destroy(CurrentBrick.gameObject);
            return;
        }
        else
        {
            if (CurrentBrick == null)
                SetNextBrick();
        }

        //////////////////modified in between ///////////////////

        if (CurrentBrick != null)
        {
            Physics.Raycast(Camera.main.transform.position + Vector3.up * 0.1f * Controller.CameraDistance, Camera.main.transform.forward, out var PrehitInfo, float.MaxValue, LegoLogic.LayerMaskBrick);
            //Here 0.2f can change; you can try maybe 0.3f
            if (PrehitInfo.point.y <= 0.2f)
            {
                if (Physics.Raycast(Camera.main.transform.position + Vector3.up * 0.1f * Controller.CameraDistance, Camera.main.transform.forward, out var hitInfo, float.MaxValue, LegoLogic.LayerMaskLego))
                {

                    //snap to grid
                    Debug.Log(hitInfo.point);

                    var position = LegoLogic.SnapToGrid(hitInfo.point);
                    //Debug.Log(position);
                    //try to find a collision free position
                    var placePosition = position;
                    PositionOk = false;
                    for (int i = 0; i < 10; i++)
                    {
                        //Debug.Log(placePosition + CurrentBrick.transform.rotation * CurrentBrick.Collider.center);
                        var collider = Physics.OverlapBox(placePosition + CurrentBrick.transform.rotation * CurrentBrick.Collider.center, CurrentBrick.Collider.size / 2, CurrentBrick.transform.rotation, LegoLogic.LayerMaskBrick);
                        PositionOk = (collider.Length == 0);
                        if (PositionOk)
                            break;
                        else
                            placePosition.y += LegoLogic.Grid.y;
                    }

                    if (PositionOk)
                        CurrentBrick.transform.position = placePosition;
                    else
                        CurrentBrick.transform.position = position;
                }
            }
            else
            {
                if (Physics.Raycast(Camera.main.transform.position + Vector3.up * 0.1f * Controller.CameraDistance, Camera.main.transform.forward, out var hitInfo, float.MaxValue, LegoLogic.LayerMaskBrick))
                {

                    //snap to grid
                    Debug.Log(hitInfo.point);

                    var position = LegoLogic.SnapToGrid(hitInfo.point);
                    //Debug.Log(position);
                    //try to find a collision free position
                    var placePosition = position;
                    PositionOk = false;
                    for (int i = 0; i < 10; i++)
                    {
                        //Debug.Log(placePosition + CurrentBrick.transform.rotation * CurrentBrick.Collider.center);

                        var collider = Physics.OverlapBox(placePosition + CurrentBrick.transform.rotation * CurrentBrick.Collider.center, CurrentBrick.Collider.size / 2, CurrentBrick.transform.rotation, LegoLogic.LayerMaskBrick);
                        PositionOk = (collider.Length == 0);

                        if (PositionOk)
                            break;
                        else
                            placePosition.y += LegoLogic.Grid.y;
                    }

                    if (PositionOk)
                        CurrentBrick.transform.position = placePosition;
                    else
                        CurrentBrick.transform.position = position;
                }
            }
        }
        //////////////////modified in between ///////////////////

        //Place the brick
        if (Input.GetMouseButtonDown(0) && CurrentBrick != null && PositionOk)
        {
            CurrentBrick.Collider.enabled = true;
            CurrentBrick.SetMaterial(BrickMat);
            var rot = CurrentBrick.transform.rotation;
            CurrentBrick = null;
            SetNextBrick();
            CurrentBrick.transform.rotation = rot;
        }

        //Rotate Brick
        if (Input.GetKeyDown(KeyCode.E))
            CurrentBrick.transform.Rotate(Vector3.up, 90);



        //Delete Brick
        if (Input.GetMouseButtonDown(1))
        {
            //////////////////modified in between ///////////////////
            if (Physics.Raycast(Camera.main.transform.position + Vector3.up * 0.1f * Controller.CameraDistance, Camera.main.transform.forward, out var hitInfo, float.MaxValue, LegoLogic.LayerMaskBrick))
            //////////////////modified in between ///////////////////
            {
                var brick = hitInfo.collider.GetComponent<Brick>();
                if (brick != null)
                {
                    //brick.SetMaterial(TransparentMat);
                    GameObject.DestroyImmediate(brick.gameObject);
                }

            }
        }

    }

    public void SetNextBrick()
    {
        CurrentBrick = Instantiate(PrefabBrick);
        CurrentBrick.Collider.enabled = false;
        CurrentBrick.SetMaterial(TransparentMat);
    }

    public void SetPrefab(int brick, int mat)
    {
        PrefabBrick = BrickLib[brick];
        BrickMat = MatLib[mat];
    }
}