class App {
  constructor(data) {
    if (data === undefined) {
      this.id = 0;
      this.name = "";
      this.ownerId = 0;
      this.devUrl = "";
      this.liveUrl = "";
      this.isActive = false;
      this.inDevelopment = false;
      this.permitSuperUserAccess = false;
      this.permitCollectiveLogins = false;
      this.disableCustomUrls = false;
      this.customEmailConfirmationAction = "";
      this.customPasswordResetAction = "";
      this.userCount = 0;
      this.timeFrame = 0;
      this.accessDuration = 0;
      this.dateCreated = "";
      this.dateUpdated = "";
      this.users = [];
      this.license = "";
    } else {
      this.id = data.id;
      this.name = data.name;
      this.ownerId = data.ownerId;
      this.devUrl = data.devUrl;
      this.liveUrl = data.liveUrl;
      this.isActive = data.isActive;
      this.inDevelopment = data.inDevelopment;
      this.permitSuperUserAccess = data.permitSuperUserAccess;
      this.permitCollectiveLogins = data.permitCollectiveLogins;
      this.disableCustomUrls = data.disableCustomUrls;
      this.customEmailConfirmationAction = data.customEmailConfirmationAction;
      this.customPasswordResetAction = data.customPasswordResetAction;
      this.userCount = data.userCount;
      this.timeFrame = data.timeFrame;
      this.accessDuration = data.accessDuration;
      this.dateCreated = data.dateCreated;
      this.dateUpdated = data.dateUpdated;
      this.users = [];

      if (data.users != null) {
        data.users.forEach((user) => {
          if (user.id > 0 && user.userName !== undefined) {
            this.users.push(user);
          }
        });
      }

      if (data.license !== undefined) {
        this.license = data.license;
      } else {
        this.license = "";
      }
    }
  }

  updateLicense(license) {
    this.license = license;
  }
}

export default App;
