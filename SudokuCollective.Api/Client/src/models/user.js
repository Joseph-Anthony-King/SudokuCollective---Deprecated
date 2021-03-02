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
      
      if (data.isLoggedIn !== null || data.isLoggedIn !== undefined) {
        this.isLoggedIn = data.isLoggedIn;
      } else {
        this.isLoggedIn = false;
      }
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
    this.emailConfirmed = false;
    this.receivedRequestToUpdateEmail = false;
    this.receivedRequestToUpdatePassword = false;
    this.isActive = false;
    this.isAdmin = false;
    this.isSuperUser = "";
    this.dateCreated = "";
    this.dateUpdated = "";
    this.isLoggedIn = false;
  }
}

export default User;
