import store from "../../store";

class SignUpModel {
  constructor(username, firstname, lastname, nickname, email, password) {
    this.userName = username;
    this.firstName = firstname;
    this.lastname = lastname;
    this.nickName = nickname;
    this.email = email;
    this.password = password;
    this.license = store.getters["settingsModule/getLicense"];
  }
}

export default SignUpModel;
