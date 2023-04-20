# hra-unity-body-ui

Application requirements and Libraries:

    Unity ver 2021.3.4.1f

    Visual Studio Code



How to Run:

    Download the repository and open the unity project (the folder called unity-body-ui)

    Navigate to the build settings menu and if not done already convert the project to a WebGL build

    Then build the project with the folder destination set to the build folder in this project

    From there the wc-body-ui.js file will recognize the build and automatically use that as the unity project for the web component

    Lastly open up the index.html file on your browser of choice and the hra body ui should be there



Web Component Scripts:

    index.html - The index page for the project. Contains two references to the web component, an on screen console log, and buttons/text fields to pass data to the unity build

    wc-body-ui.js - The web component script. Build using the auto generated unity WebGL code and then modified by adding in observed variables and functions for data passing

    main.js - The main js file for the index page. Contains functions that get called from the index page which facilitates the data passing towards the web component

    style.css - The css file for styling the index page. Contains code that formats the web page and the buttons into a nice layout


Web Component Functions:

    wc-body-ui.js
        * SetUnityInstance() - Called during the initialization of the unity web component. Passes the name of the id to unity
        * attributeChangedCallback() - a builtin function that waits for any changes in observed attributes at passes the new data to the unity build
    
    main.js
        * SetSomeAttribute() - This function gets called from the buttons on the index page and sets a variable for the first web component to be equal to the user input
        * SetupListeners() - An initialization function that gets called when the dom loads. It connects listeners to the first web component instance which wait for changes in the observed variables and passes them to the second web component instance
        * GetScene() - GetScene is a testing function that passes a url containing the json for the HRA models
        * LogToPage() - A helper function that logs changes in data to the web page for easy debugging and confirmation of variable changes
            


Unity Scripts:

    JSBridge - recieves signals from the WebComponent and applies the changes to unity


    WebGLPluginJS - 

    WebGLPluginJSLib - 

    SceneSetter - Takes the signals from JSBridge and updates the scene accordingly

