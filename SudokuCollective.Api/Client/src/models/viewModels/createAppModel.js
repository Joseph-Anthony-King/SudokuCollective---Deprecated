import store from "../../store";

class SignUpModel {
  constructor(name, devUrl, liveUrl) {
    this.name = name;
    this.ownerId = store.getters["settingsModule/getRequestorId"];
    this.devUrl = devUrl;
    this.liveUrl = liveUrl;
  }
}

export default SignUpModel;
