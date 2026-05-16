using System;
using UnityEngine;
using UnityEngine.UI;

public class FaceCamManagerSc : MonoBehaviour
{
    public RawImage faceCamImage;
    public Transform[] faceCams;
    public Transform faceCam;
    public Transform faceCam1;
    public Transform faceCam2;
    public Transform faceCam3;
    public Transform faceCam4;
    public Transform faceCam5;
    public Transform faceCam6;
    public Transform faceCam7;
    public Transform faceCam8;
    public Transform faceCam9;
    public Transform faceCam10;

    private int currentLookAreaIndex = -1;

    void Start()
    {
        faceCams = new Transform[11] { faceCam,faceCam1,faceCam2,faceCam3,faceCam4,faceCam5,
        faceCam6,faceCam7,faceCam8,faceCam9,faceCam10};

        offImage();
    }

    // Update is called once per frame
    void Update()
    {
        //2コース以外では非表示にする
        updateCourseIndex();
        //LookAreaIndexに応じてFaceCamの位置を変更する
        updateFaceCamPos();
        // Anchor と同じ位置・向きに揃える
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }

    private void FixedUpdate()
    {

    }

    public void updateCourseIndex()
    {
        
    }

    public void updateFaceCamPos()
    {
        int index = Mathf.Clamp(currentLookAreaIndex, 0, faceCams.Length - 1);
        transform.SetParent(faceCams[index]);//
    }

    public void offImage()
    {
        faceCamImage.enabled = false;
    }

    public void onImage()
    {
        faceCamImage.enabled = true;
    }

    public void updateLookAreaIndex(int index)
    {
        currentLookAreaIndex = index;
    }
}
