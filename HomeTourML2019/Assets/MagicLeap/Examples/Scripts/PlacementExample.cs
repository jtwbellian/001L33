// %BANNER_BEGIN%
// ---------------------------------------------------------------------
// %COPYRIGHT_BEGIN%
//
// Copyright (c) 2019 Magic Leap, Inc. All Rights Reserved.
// Use of this file is governed by the Creator Agreement, located
// here: https://id.magicleap.com/creator-terms
//
// %COPYRIGHT_END%
// ---------------------------------------------------------------------
// %BANNER_END%

using UnityEngine;
using UnityEngine.XR.MagicLeap;
using UnityEngine.UI;



namespace MagicLeap
{
    /// <summary>
    /// This class allows the user to cycle between various PlacementContent
    /// objects and see a visual representation of a valid or invalid location.
    /// Once a valid location has been determined the user may place the content.
    /// </summary>
    /// 

    [System.Serializable]
    public class Category
    {
        public Sprite pic;
        public GameObject[] prefabs;
        public Color color;
    }

    [RequireComponent(typeof(Placement))]
    public class PlacementExample : MonoBehaviour
    {

        const int ERASER = 6;
        #region Private Variables
        [SerializeField, Tooltip("The controller that is used in the scene to cycle and place objects.")]
        private ControllerConnectionHandler _controllerConnectionHandler = null;

        [SerializeField, Tooltip("The placement objects that are used in the scene.")]


        //public GameObject[] _placementPrefabs = null;

        public Category[] categories = null;
        public int currCat = 0;

        public Image categoryImage;
        public MeshRenderer cursorMesh;

        //public Transform prevObjLoc = null;
        //public Transform nextObjLoc = null;
        private MLInputController _controller;
        private Placement _placement = null;
        private PlacementObject _placementObject = null;
        private PlacementObject hitObject = null; 

        //private GameObject _previousObj = null;
        //private GameObject _nextObj = null;
        private int _placementIndex = 0;
        #endregion

        #region Unity Methods
        void Start()
        {

            if (_controllerConnectionHandler == null)
            {
                Debug.LogError("Error: PlacementExample._controllerConnectionHandler is not set, disabling script.");
                enabled = false;
                return;
            }

            _placement = GetComponent<Placement>();

            MLInput.OnControllerButtonDown += HandleOnButtonDown;
            MLInput.OnTriggerDown += HandleOnTriggerDown;

            _controller = MLInput.GetController(MLInput.Hand.Left);

            StartPlacement();
        }

        void Update()
        {
            // Update the preview location, inside of the validation area.
            if (_placementObject != null)
            {
                _placementObject.transform.position = _placement.AdjustedPosition - _placementObject.LocalBounds.center;
                _placementObject.transform.root.rotation = _placement.Rotation; //Quaternion.Euler(new Vector3(0f, _controller.Orientation.ToEuler().z, 0f)); // _placement.Rotation; 
                //_nextObj.transform.position = prevObjLoc.position;
            }
            else if (currCat == ERASER)
            {

                RaycastHit hit;
                int layerMask = 1 << 8; 

                // Does the ray intersect any objects excluding the player layer
                if (Physics.Raycast(_controller.Position, (_controller.Orientation * Vector3.forward), out hit, Mathf.Infinity, layerMask))
                {
                    Debug.DrawRay(_controller.Position,  (_controller.Orientation * Vector3.forward) * hit.distance, Color.yellow);
                    hitObject = hit.transform.GetComponentInChildren<PlacementObject>();
                    Debug.Log("Did Hit");
                }
                else
                    Debug.DrawRay(_controller.Position, (_controller.Orientation * Vector3.forward) * 100f, Color.red);
            }

            if(Input.GetKeyDown(KeyCode.N))
            {
                NextPlacementObject();
            }

            // Cycle with radial gesture 
            if (_controller.TouchpadGesture.Type == MLInputControllerTouchpadGestureType.RadialScroll &&
                _controller.TouchpadGestureState != MLInputControllerTouchpadGestureState.End)
            {
                if (_controller.TouchpadGesture.Direction == MLInputControllerTouchpadGestureDirection.Clockwise)
                {
                    NextPlacementObject();
                }
                else if (_controller.TouchpadGesture.Direction == MLInputControllerTouchpadGestureDirection.CounterClockwise)
                {
                    LastPlacementObject();
                }
            }

        }

        void OnDestroy()
        {
            MLInput.OnControllerButtonDown -= HandleOnButtonDown;
            MLInput.OnTriggerDown -= HandleOnTriggerDown;
        }
        #endregion

        #region Event Handlers
        /// <summary>
        /// Handles the event for button down.
        /// </summary>
        /// <param name="controller_id">The id of the controller.</param>
        /// <param name="button">The button that is being pressed.</param>
        private void HandleOnButtonDown(byte controllerId, MLInputControllerButton button)
        {
            /*if (_controllerConnectionHandler.IsControllerValid() && _controllerConnectionHandler.ConnectedController.Id == controllerId &&
                button == MLInputControllerButton.Bumper)
            {
                NextPlacementObject();
            }*/
            if (currCat < categories.Length - 1)
            {
                currCat++;
            }
            else
                currCat = 0;

            categoryImage.sprite = categories[currCat].pic;
            cursorMesh.material.SetColor("_Color", categories[currCat].color);
            _placementIndex = 0;

            StartPlacement();

        }


        private void HandleOnTriggerDown(byte controllerId, float pressure)
        {

            if (currCat == ERASER && hitObject != null)
            {
                Destroy(hitObject.gameObject);
            }
               
            _placement.Confirm();
        }

        private void HandlePlacementComplete(Vector3 position, Quaternion rotation)
        {
            
            if (categories[currCat].prefabs != null && categories[currCat].prefabs.Length > _placementIndex)
            {
                GameObject content = Instantiate(categories[currCat].prefabs[_placementIndex]);

                content.transform.position = position;
                content.transform.rotation = rotation;
                content.gameObject.SetActive(true);

                _placement.Resume();
            }
        }
        #endregion

        #region Private Methods
        private PlacementObject CreatePlacementObject(int index = 0)
        {
            // Destroy previous preview instance
            if (_placementObject != null)
            {
                Destroy(_placementObject.gameObject);
                //Destroy(_previousObj.gameObject);
                //Destroy(_nextObj.gameObject);
            }

            // Create the next preview instance.
            if (categories[currCat].prefabs != null && categories[currCat].prefabs.Length > index)
            {
                GameObject previewObject = Instantiate(categories[currCat].prefabs[index]);


                //_nextObj = (index + 1 < _placementPrefabs.Length) ? Instantiate(_placementPrefabs[index + 1]) : Instantiate(_placementPrefabs[0]);
                //_previousObj = (index - 1 > 0) ? Instantiate(_placementPrefabs[index - 1]) : Instantiate(_placementPrefabs[_placementPrefabs.Length - 1]);

                //_nextObj.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                //_previousObj.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

                // Detect all children in the preview and set children to ignore raycast.

                Collider[] colliders = previewObject.GetComponents<Collider>();

                if (colliders.Length == 0)
                {
                    colliders = previewObject.GetComponentsInChildren<Collider>();
                }

                for (int i = 0; i < colliders.Length; ++i)
                {
                    colliders[i].enabled = false;
                }

                // Find the placement object.
                PlacementObject placementObject = previewObject.GetComponent<PlacementObject>();

                if (placementObject == null)
                {
                    placementObject = previewObject.GetComponentInChildren<PlacementObject>();
                }

                if (placementObject == null)
                {
                    Destroy(previewObject);
                    //Destroy(previewNext);
                    //Destroy(previewPrevious);
                    Debug.LogError("Error: PlacementExample.placementObject is not set, disabling script.");

                    enabled = false;
                }

                return placementObject;
            }

            return null;
        }

        private void StartPlacement()
        {
            _placementObject = CreatePlacementObject(_placementIndex);

            if (_placementObject != null)
            {
                _placement.Cancel();
                _placement.Place(_controllerConnectionHandler.transform, _placementObject.Volume, _placementObject.AllowHorizontal, _placementObject.AllowVertical, HandlePlacementComplete);
            }
        }

        public void NextPlacementObject()
        {
            if (categories[currCat].prefabs != null)
            {
                _placementIndex++;
                if (_placementIndex >= categories[currCat].prefabs.Length)
                {
                    _placementIndex = 0;
                }
            }

            StartPlacement();
        }

        public void LastPlacementObject()
        {
            if (categories[currCat].prefabs != null)
            {
                _placementIndex --;

                if (_placementIndex <=0 )
                {
                    _placementIndex = categories[currCat].prefabs.Length - 1;
                }
            }

            StartPlacement();
        }
        #endregion
    }
}
