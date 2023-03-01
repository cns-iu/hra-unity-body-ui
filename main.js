function setSomeAttribute(key, value) {
  console.log(key, value);
  const bodyUi = document.querySelector('wc-body-ui');
  bodyUi.setAttribute(key, value);
  logToPage(`Changed ${key} to ${value}`);
}

function logToPage(str){
  const debugLog = document.querySelector('.DebugLog, .right-DebugLog')
  debugLog.innerHTML += `<p>${str}</p>`;
}

function changeCamType(){

}
