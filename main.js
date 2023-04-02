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

  for (const attribute of UnityBodyUI.observedAttributes) {
    // skip interactive?
    bodyUi.addEventListener(attribute + 'Change', (event) => {
      const value = event.detail; // check
      console.log(attribute, event);
      bodyUi2.setAttribute(attribute, value);
    });
    console.log(attribute);
  }
  bodyUi.addEventListener('rotationChange', (event) => {
      const [x, y] = event.detail;
      bodyUi2.setAttribute('rotationx', x);
      bodyUi2.setAttribute('rotationy', y);
      console.log('rotationChange', [x, y]);
  });
}

function logToPage(str){
  const debugLog = document.querySelector('.DebugLog, .right-DebugLog')
  debugLog.innerHTML += `<p>${str}</p>`;
}

function changeCamType(){

}

document.addEventListener("DOMContentLoaded", setupListeners);
