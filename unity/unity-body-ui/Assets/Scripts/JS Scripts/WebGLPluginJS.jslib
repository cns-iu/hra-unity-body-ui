// Read more about creating JS plugins: https://www.patrykgalach.com/2020/04/27/unity-js-plugin/
// Creating functions for the Unity
mergeInto(LibraryManager.library, {

   SendConsoleLog: function (str){
      const output = UTF8ToString(str);
      console.log(output)
   },

   SendEvent: function (id, eventName, payload){
      console.log("Data has been updated");

      const _id=UTF8ToString(id);
      const registry = window.UNITY_BODY_UI_REGISTRY || {};
      const instance = registry[_id];
      const json_obj = JSON.parse(UTF8ToString(payload));

      if(instance){
        const _name = UTF8ToString(eventName);

        //for numbers
        if(_name=="rotation" || _name=="rotationX" || _name=="zoom") {
            const event = new CustomEvent( name, { detail: json_obj.num });
            instance.dispatchEvent(event);
        }
        else if(_name=="camera") {
            const event = new CustomEvent( _name, { detail: json_obj.str });
            instance.dispatchEvent(event);
        }
        else if(_name=="interactivity") {
            const event = new CustomEvent( _name, { detail: json_obj.boolean });
            instance.dispatchEvent(event);
        }
        else {
            
        }
      }
   },

   SendOutput: function (id, eventName, payload){
      const _id=UTF8ToString(id);
      const registry = window.UNITY_BODY_UI_REGISTRY || {};
      const instance = registry[_id];
      

      if(instance){
        const _name = UTF8ToString(eventName);
        console.log(_name);

        if(_name=="rotationChange") {
            const json_obj = JSON.parse(UTF8ToString(payload));
            console.log(json_obj);
            const event = new CustomEvent( _name, { detail: [json_obj.rotationX, json_obj.rotationY] });
            instance.dispatchEvent(event);
        }
        else if(_name=="initialized"){
            console.log("Initialized?");
            const event = new CustomEvent( _name, { detail: true });
            instance.dispatchEvent(event);
        }
        else{
            const json_obj = JSON.parse(UTF8ToString(payload));
            console.log(json_obj);
            const event = new CustomEvent( _name, { detail: json_obj });
            instance.dispatchEvent(event);
        }
      }
   }
});