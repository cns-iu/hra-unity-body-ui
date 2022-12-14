sendMessage(eventName, payload) {
    if(this.#myGameInstance){
      this.#myGameInstance.SendMessage("JSBridge", eventName, payload);
    }
    else{
      console.log("Game Instance Not Set", eventName, payload);
    }
}

listenRotationX(el){
    el.addEventListener('click', function (){
      // Get the first json element
      const txtMessage = document.getElementById("jsonMessage1");
      const message = txtMessage.value;
      txtMessage.value = "";

      // Clear the input field
      const intMessage = document.getElementById("jsonMessage2");
      const value = intMessage.value;
      intMessage.value = "";

      var JsonObj = {
        "name":message,
        "value":parseInt(value)
      }

      var jsonString = JSON.stringify(JsonObj)

      // Send message to the Unity scene
    })
}

document.querySelector('wc-body-ui').listenRotationX(document.getElementById('jsonRotationX'))
document.querySelector('wc-body-ui').listenRotationY(document.getElementById('jsonRotationY'))