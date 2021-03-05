﻿import store from "@/store";
import App from "@/models/app";

class User {
  constructor(data) {
    if (data == undefined) {
      this.id = 0;
      this.userName = "";
      this.firstName = "";
      this.lastName = "";
      this.nickName = "";
      this.fullName = "";
      this.email = "";
      this.emailConfirmed = false;
      this.receivedRequestToUpdateEmail = false;
      this.receivedRequestToUpdatePassword = false;
      this.isActive = false;
      this.isAdmin = false;
      this.isSuperUser = false;
      this.dateCreated = "";
      this.dateUpdated = "";
      this.isLoggedIn = false;
    } else {
      this.id = data.id;
      this.userName = data.userName;
      this.firstName = data.firstName;
      this.lastName = data.lastName;
      this.nickName = data.nickName;
      this.fullName = data.fullName;
      this.email = data.email;
      this.emailConfirmed = data.emailConfirmed;
      this.receivedRequestToUpdateEmail = data.receivedRequestToUpdateEmail;
      this.receivedRequestToUpdatePassword = data.receivedRequestToUpdatePassword;
      this.isActive = data.isActive;
      this.isAdmin = data.isAdmin;
      this.isSuperUser = data.isSuperUser;
      this.dateCreated = data.dateCreated;
      this.dateUpdated = data.dateUpdated;
      this.isLoggedIn = data.isLoggedIn !== undefined ? data.isLoggedIn : false;
    }
  }

  login(token) {
    this.isLoggedIn = true;

    store.dispatch("settingsModule/updateUser", new User());
    store.dispatch("settingsModule/updateAuthToken", token);
  }

  logout() {
    this.id = 0;
    this.userName = "";
    this.firstName = "";
    this.lastName = "";
    this.nickName = "";
    this.fullName = "";
    this.email = "";
    this.emailConfirmed = false;
    this.receivedRequestToUpdateEmail = false;
    this.receivedRequestToUpdatePassword = false;
    this.isActive = false;
    this.isAdmin = false;
    this.isSuperUser = "";
    this.dateCreated = "";
    this.dateUpdated = "";
    this.isLoggedIn = false;
  
    store.dispatch("settingsModule/updateUser", this);
    store.dispatch("settingsModule/updateAuthToken", "");
    store.dispatch("appModule/updateSelectedApp", new App());
    store.dispatch("appModule/removeApps");
  }
}

export default User;
