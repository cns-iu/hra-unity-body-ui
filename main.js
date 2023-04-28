function setSomeAttribute(key, value) {
  //get the first screen
  const bodyUi = document.getElementById('myInstance1');
  
  //set the attribute directly
  bodyUi.setAttribute(key, value);

  //log data to both console
  console.log(key, value);
  logToPage(`Changed ${key} to ${value}`);
}

function setupListeners() {

  console.log("Listeners getting set up");

  //make a reference for both screens
  const bodyUi = document.getElementById('myInstance1');
  const bodyUi2 = document.getElementById('myInstance2');

  //add a listener for initialized to both screens and then pass the scene (as a url commented code is node array)
  bodyUi.addEventListener('initialized', (event) => {
    logToPage('bodyui1', event);
    bodyUi.setAttribute('scene', SCENE.default);
    // getScene(SCENE.default).then((scene) => {
    //   console.log(scene)
    //   bodyUi.setAttribute('scene', scene);
    // });
  });
  bodyUi2.addEventListener('initialized', (event) => {
    logToPage('bodyui2', event);
    
    bodyUi2.setAttribute('scene', SCENE.default);
    // getScene(SCENE.default).then((scene) => {
    //   console.log(scene)
    //   bodyUi2.setAttribute('scene', SCENE.default);
    // });
  });

  //Add listeners to first screen that set data for second screen
  for (const attribute of UnityBodyUI.observedAttributes) {
    // skip interactive?
    bodyUi.addEventListener(attribute + 'Change', (event) => {
      if(attribute != 'scene'){
        console.log("adding listeners for: ", attribute)
        const value = event.detail; 
        bodyUi2.setAttribute(attribute, value);
      }
    });
  }
  bodyUi.addEventListener('rotationChange', (event) => {
      const [x, y] = event.detail;
      bodyUi2.setAttribute('rotationx', x);
      bodyUi2.setAttribute('rotationy', y);
  });
}

const SCENE = {
  //reference to test scenes
  'default': 'https://ccf-api.hubmapconsortium.org/v1/scene?sex=both',
  'male': 'https://ccf-api.hubmapconsortium.org/v1/scene?sex=male',
  'female': 'https://ccf-api.hubmapconsortium.org/v1/scene?sex=female',
  'vhfleftkidney': 'https://ccf-api.hubmapconsortium.org/v1/reference-organ-scene?organ-iri=http%3A%2F%2Fpurl.obolibrary.org%2Fobo%2FUBERON_0004538&sex=female'
}

function getScene(url) {
  //Return the json got from the url
  return fetch(url).then(r => r.json());
}

function logToPage(str){
  //log a string to the webpage console
  const debugLog = document.querySelector('.DebugLog, .right-DebugLog')
  debugLog.innerHTML = `${str}</br>` + debugLog.innerHTML;
}

document.addEventListener("DOMContentLoaded", setupListeners);
