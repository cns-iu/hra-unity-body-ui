function setSomeAttribute(key, value) {
  console.log(key, value);
  const bodyUi = document.getElementById('myInstance1');
  
  bodyUi.setAttribute(key, value);
  logToPage(`Changed ${key} to ${value}`);
}

function setupListeners() {

  console.log("Listeners getting set up");

  const bodyUi = document.getElementById('myInstance1');
  const bodyUi2 = document.getElementById('myInstance2');

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
  'default': 'https://ccf-api.hubmapconsortium.org/v1/scene?sex=both',
  'male': 'https://ccf-api.hubmapconsortium.org/v1/scene?sex=male',
  'female': 'https://ccf-api.hubmapconsortium.org/v1/scene?sex=female',
  'vhfleftkidney': 'https://ccf-api.hubmapconsortium.org/v1/reference-organ-scene?organ-iri=http%3A%2F%2Fpurl.obolibrary.org%2Fobo%2FUBERON_0004538&sex=female'
}

function getScene(url) {
  return fetch(url).then(r => r.json());
}

function logToPage(str){
  const debugLog = document.querySelector('.DebugLog, .right-DebugLog')
  debugLog.innerHTML = `${str}</br>` + debugLog.innerHTML;
}

document.addEventListener("DOMContentLoaded", setupListeners);
