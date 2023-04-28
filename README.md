# hra-unity-body-ui

## Application requirements and Libraries

- Unity ver 2021.3.4.1f
- Visual Studio Code (for JavaScript web component development)

## How to Run

1. Download the repository and open the unity project (the folder called unity-body-ui)
1. Navigate to the build settings menu and if not done already convert the project to a WebGL build
1. Then build the project with the folder destination set to the build folder in this project
1. From there the wc-body-ui.js file will recognize the build and automatically use that as the unity project for the web component
1. Lastly open up the index.html file on your browser of choice and the hra body ui should be there

## Web Component Scripts

- index.html - The index page for the project. Contains two references to the web component, an on screen console log, and buttons/text fields to pass data to the unity build
- wc-body-ui.js - The web component script. Build using the auto generated unity WebGL code and then modified by adding in observed variables and functions for data passing
- main.js - The main js file for the index page. Contains functions that get called from the index page which facilitates the data passing towards the web component
- style.css - The css file for styling the index page. Contains code that formats the web page and the buttons into a nice layout

## Web Component Functions

- wc-body-ui.js
    - SetUnityInstance() - Called during the initialization of the unity web component. Passes the name of the id to unity
    - attributeChangedCallback() - a builtin function that waits for any changes in observed attributes at passes the new data to the unity build
- main.js
    - SetSomeAttribute() - This function gets called from the buttons on the index page and sets a variable for the first web component to be equal to the user input
    - SetupListeners() - An initialization function that gets called when the dom loads. It connects listeners to the first web component instance which wait for changes in the observed variables and passes them to the second web component instance
    - GetScene() - GetScene is a testing function that passes a url containing the json for the HRA models
    - LogToPage() - A helper function that logs changes in data to the web page for easy debugging and confirmation of variable changes

## Unity Scripts

### Top Level Scripts
- JSBridge.cs - recieves signals from the WebComponent and applies the changes to unity
- SceneSetter.cs - Takes the signals from JSBridge and updates the scene accordingly
- ColldierTest.cs - A first implementation for organ colliders. Generates a box collider for each leaf of the organ reference and then attaches the organ control script to enable clicking interactions.
- ModelLoader.cs - Loads the models for the organs using GLTFast
- OrganControllScript.cs - A script that is connected to the children of the organ and refers to the top level organ to enable rotation and translation.
- SpatialSceneManager.cs - A function that loads all the model and manages them. Passes the models through the main logic loops to set their rotation, opacity, and connect them to the tissue blocks.

### JS Scripts
- WebGLPluginJS.cs - A C# file that takes advantage of [DllImport("__Internal")] to connect to the JSLib file and output code to java script. This file holds all the function names and is the one that gets called in from Unity.
- WebGLPluginJS.jsLib - This file is paired with the C# file to contain functions for data pasing back to java script. Incidentially this file is not a C# file so you may need to "reveal all files" in the text editor you are using as under normal circumstances it will remain hidden

### Utility Scripts
- DisableWedGLInputCapture - A special file that enables the user to press keyboard buttons for the webv page. Without this file the WebGL would disable keybaord inputs and the user would not be able to type inputs.
- LeaveFinder.cs - A utility script that finds all the leaves for a given unity game object. Used to great effect for finding all the children during organ loading.
- MaterialExtensions.cs - A utility file that allows the user to modify the material for the organs and adjust opacity and other parameters.
- MatrixExtensions.cs - A utility script that contains one function to build the transoform matrixes for the organs based off their data


 ### Data Scripts
- DataFetcher.cs - Has a function that convers a URL to a NodeArray which will be depreciated soon. More importantly it holds the JSON objects for the organs and the output objects.
- OrganData.cs - Data for each individual organ to store and reference
- TissueBlockData.cs - A script for the tissue blocks so that they can hold their data and refer to them when needed


## Unity Functions:

### Top Level Scripts
- JSBridge.cs:
    - contains all the event functions for recievin and sending data (need to finish)

- SceneSetter.cs:
    - SetCameraInteractivity(bool interacive) - set whether the camera cna be interacted with or not
    - SetOrganRotationX(float xRotation) - set the x rotation for the root of the organ
    - SetOrganRotationY(float yRotation) - sets the y rotation for the root of the organ
    - SetCameraZoom(float zoom) - Sets the zoom for the camera
    - SetCameraType(string cameraType) - Sets the camera type used (Orthographic or perspective)
    - LoadScene(NodeArray nodeArray) - Loads the scene based off the given node array. Passes the data off to the Spatial scene manager either as a call to set scene if nothing has been set or as load scene if it has already been initialized

- ColldierTest.cs:
    - AddColliderAroundChildren(GameObject wrapper) - Adds an imperfect box collider around each organ child and then attaches the organ control script 

- ModelLoader.cs:
    - GetModel(string url) - Takes in a url, cleans it, and passes it off to load model
    - LoadModel(string path) - Old loading model function. Will be depreciated when node arrays get passed into unity instead of urls
    - ResetWrapper() - resets the wrapper parent for the models

- OrganControllScript.cs:

- SpatialSceneManager.cs:
    - SetScene(NodeArray _nodeArray) - Starts the inital logic chain for loading the models. Gets called first and then every time after a node array is passed in from the JS side change scene is used instead
    - ChangeScene(NodeArray _nodeArray) - Sets the scene based off the node array passsed it. turns off not used organs and places the visibile organs with proper rotations
    - Get(string url) - Get the url for the web request and pass the ndoe array data
    - GetOrgans() - A task that gets all the organs from the node array and loads in the organs, sets their rotatin, and their opacity
    - CreateAndPlaceTissueBlocks() - Instantiates the tissue blocks and attaches the script
    - PlaceOrgan(GameObject organ, SpatialSceneNode node) - Places the organ based off the transform matrix data in the spatial scene node
    - SetOrganData(GameObject obj, SpatialSceneNode node) - Sets the organ data script attached to each of the leaves
    - SetOrganOpacity(GameObject organWrapper, float alpha) - Function that sets the organ opacity for each material of the organ gameobjects
    - ParentTissueBlocksToOrgans(List<GameObject> tissueBlocks, List<GameObject> organs) - Uses both lists to join the tissue blocks with their proper orgna parents
    - GetEntityIdsBySex(string url) - gets the id for the sex of each organ
    - GetOrganSex() - Get a specific organ's sex
    - ReflectZ() - Matrix utility function that reflexts across the z axis
    - SetTissueBlockData(GameObject obj, SpatialSceneNode node) - Sets the tissue block data for the tissue blocks
    - SetCellTypeData(GameObject obj) - Sets the cell type data for the organ

### JS Scripts
- WebGLPluginJS.cs & WebGLPluginJS.jsLib: 
    - SendConsoleLog(string str) - Debug function to log a string to the console
    - SendEvent(string _id, string eventName, string jsonP) - Function that sends an output after a variable gets changed by a JS data pass
    - SendOutput(string _id, string eventName, string jsonP) - Function that sends an output after one of the observed variables gets changed in unity


### Utility Scripts
- LeaveFinder.cs:
    - FindLeaves(Transform parent, List<Transform> leafArray) - Finds all the leaves of a parent object given an array of expected children and then returns a list of the transforms for each leaf

- MaterialExtensions.cs:
    - ToOpaqueMode(this Material material) - A function that takes in a material and makes it opaque
    - ToFadeMode(this Material material) - Function that takes in a material and fades it
- MatrixExtensions.cs:
    - BuildMatrix(float[] transformMatrix) - Takes in a list of floats coresponding to a transform matrix and returns said matrix as a Unity Matrix4x4

    ### Unity Scene Hierarchy
    ![image](https://user-images.githubusercontent.com/16376952/235123056-90b8fad5-0dcc-48c1-b899-3b4fbc2b6d12.png)
