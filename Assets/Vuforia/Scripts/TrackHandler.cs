using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class TrackHandler : DefaultTrackableEventHandler
{
    //DELEGAT
    public delegate void ObjectDetectedEventHandler(object sender, string detectedObjectName, GameObject gameObject);

    //DEFINICA DOGADJAJA
    public event ObjectDetectedEventHandler ObjectDetected;

    //PODIZANJE DOGADJAJA
    protected virtual void OnObjectDetected()
    {
        if (ObjectDetected != null)
            ObjectDetected(this, mTrackableBehaviour.TrackableName, mTrackableBehaviour.gameObject);
    }
    

    public override void OnTrackableStateChanged (
        TrackableBehaviour.Status previousStatus,
        TrackableBehaviour.Status newStatus)
    {
        m_PreviousStatus = previousStatus;
        m_NewStatus = newStatus;

        Debug.Log("Trackable " + mTrackableBehaviour.TrackableName +
                  " " + mTrackableBehaviour.CurrentStatus +
                  " -- " + mTrackableBehaviour.CurrentStatusInfo);

        if (newStatus == TrackableBehaviour.Status.DETECTED || newStatus == TrackableBehaviour.Status.TRACKED)
        {
            this.ObjectDetected += GroundPlane.OnObjectFound;
            this.ObjectDetected += Dimenzije.OnObjectFound;
            this.OnObjectDetected();
            this.ObjectDetected -= GroundPlane.OnObjectFound;
            this.ObjectDetected -= Dimenzije.OnObjectFound;
        }
        
        if (newStatus == TrackableBehaviour.Status.DETECTED ||
            newStatus == TrackableBehaviour.Status.TRACKED ||
            newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
        {
            OnTrackingFound();
            Debug.Log(mTrackableBehaviour.gameObject.transform.localPosition);
        }
        else if (previousStatus == TrackableBehaviour.Status.TRACKED &&
                 newStatus == TrackableBehaviour.Status.NO_POSE)
        {
            OnTrackingLost();
        }
        else
        {
            // For combo of previousStatus=UNKNOWN + newStatus=UNKNOWN|NOT_FOUND
            // Vuforia is starting, but tracking has not been lost or found yet
            // Call OnTrackingLost() to hide the augmentations
            OnTrackingLost();
        }
    }

    protected virtual void Start()
    {
        mTrackableBehaviour = GetComponent<TrackableBehaviour>();
        if (mTrackableBehaviour)
            mTrackableBehaviour.RegisterTrackableEventHandler(this);
    }

    protected virtual void OnDestroy()
    {
        if (mTrackableBehaviour)
            mTrackableBehaviour.UnregisterTrackableEventHandler(this);
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }
}
