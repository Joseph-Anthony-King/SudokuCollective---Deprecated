class User {
  constructor(
    id,
    username,
    firstname,
    lastname,
    nickname,
    fullname,
    email,
    emailConfirmed,
    receivedRequestToUpdateEmail,
    receivedRequestToUpdatePassword,
    isactive,
    isadmin,
    issuperuser,
    datecreated,
    dateupdated
  ) {
    if (!arguments.length) {
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
      this.dateCreated = new Date("0001-01-01T00:00:00Z");
      this.dateUpdated = new Date("0001-01-01T00:00:00Z");
      this.isLoggedIn = false;
    } else {
      this.id = id;
      this.userName = username;
      this.firstName = firstname;
      this.lastName = lastname;
      this.nickName = nickname;
      this.fullName = fullname;
      this.email = email;
      this.emailConfirmed = emailConfirmed;
      this.receivedRequestToUpdateEmail = receivedRequestToUpdateEmail;
      this.receivedRequestToUpdatePassword = receivedRequestToUpdatePassword;
      this.isActive = isactive;
      this.isActive = isadmin;
      this.isSuperUser = issuperuser;
      this.dateCreated = new Date(datecreated);
      this.dateUpdated = new Date(dateupdated);
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
    this.dateCreated = new Date("0001-01-01T00:00:00Z");
    this.dateUpdated = new Date("0001-01-01T00:00:00Z");
    this.isLoggedIn = false;
  }

  clone(data) {
    if (data !== undefined) {
      this.id = data.id;
      this.userName = data.userName;
      this.firstName = data.firstName;
      this.lastName = data.lastName;
      this.nickName = data.nickName;
      this.fullName = data.fullName;
      this.email = data.email;
      this.emailConfirmed = data.emailConfirmed;
      this.receivedRequestToUpdateEmail = data.receivedRequestToUpdateEmail;
      this.receivedRequestToUpdatePassword =
        data.receivedRequestToUpdatePassword;
      this.isActive = data.isActive;
      this.isAdmin = data.isAdmin;
      this.isSuperUser = data.isSuperUser;
      this.dateCreated = data.dateCreated;
      this.dateUpdated = data.dateUpdated;

      if (data.isLoggedIn) {
        this.isLoggedIn = data.isLoggedIn;
      }
    }
  }
}

export default User;
