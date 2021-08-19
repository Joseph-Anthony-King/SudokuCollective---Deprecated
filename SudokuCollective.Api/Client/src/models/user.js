class User {
  constructor(data) {
    if (data === undefined) {
      this.id = 0;
      this.userName = "";
      this.firstName = "";
      this.lastName = "";
      this.nickName = "";
      this.fullName = "";
      this.email = "";
      this.isEmailConfirmed = false;
      this.emailConfirmed = "No",
      this.receivedRequestToUpdateEmail = false;
      this.receivedRequestToUpdatePassword = false;
      this.isActive = false;
      this.acive = "No";
      this.isAdmin = false;
      this.admin = "No";
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
      this.isEmailConfirmed = data.isEmailConfirmed;
      this.emailConfirmed = data.isEmailConfirmed ? "Yes" : "No";
      this.receivedRequestToUpdateEmail = data.receivedRequestToUpdateEmail;
      this.receivedRequestToUpdatePassword =
        data.receivedRequestToUpdatePassword;
      this.isActive = data.isActive;
      this.active = data.isActive ? "Yes" : "No";
      this.isAdmin = data.isAdmin;
      this.admin = data.isAdmin ? "Yes" : "No";
      this.isSuperUser = data.isSuperUser;
      this.dateCreated =
        data.dateCreated !== null
          ? new Date(data.dateCreated).toLocaleString()
          : null;
      this.dateUpdated =
        data.dateUpdated !== null
          ? new Date(data.dateUpdated).toLocaleString()
          : null;
      this.isLoggedIn = data.isLoggedIn !== undefined ? data.isLoggedIn : false;
    }
  }

  login() {
    this.isLoggedIn = true;
  }

  logout() {
    this.id = 0;
    this.userName = "";
    this.firstName = "";
    this.lastName = "";
    this.nickName = "";
    this.fullName = "";
    this.email = "";
    this.isEmailConfirmed = false;
    this.emailConfirmed = "No";
    this.receivedRequestToUpdateEmail = false;
    this.receivedRequestToUpdatePassword = false;
    this.isActive = false;
    this.acive = "No";
    this.isAdmin = false;
    this.admin = "No";
    this.isSuperUser = "";
    this.dateCreated = "";
    this.dateUpdated = "";
    this.isLoggedIn = false;
  }
}

export default User;
