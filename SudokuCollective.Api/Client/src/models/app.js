class App {
  constructor(data) {
    if (data === undefined) {
      this.id = 0;
      this.name = "";
      this.ownerId = 0;
      this.devUrl = "";
      this.liveUrl = "";
      this.isActive = false;
      this.active = "";
      this.inDevelopment = false;
      this.status = "";
      this.permitSuperUserAccess = false;
      this.permitCollectiveLogins = false;
      this.disableCustomUrls = false;
      this.customActions = "No";
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
      this.active = data.isActive ? "Active" : "Deactivated";
      this.inDevelopment = data.inDevelopment;
      this.status = data.inDevelopment ? "In Development" : "In Production";
      this.permitSuperUserAccess = data.permitSuperUserAccess;
      this.permitCollectiveLogins = data.permitCollectiveLogins;
      this.disableCustomUrls = data.disableCustomUrls;
      this.customActions =
        !data.disableCustomUrls &&
        data.customEmailConfirmationAction !== "" &&
        data.customPasswordResetAction !== ""
          ? "Yes"
          : "No";
      this.customEmailConfirmationAction = data.customEmailConfirmationAction;
      this.customPasswordResetAction = data.customPasswordResetAction;
      this.userCount = data.userCount;
      this.timeFrame = data.timeFrame;
      this.accessDuration = data.accessDuration;
      this.dateCreated =
        data.dateCreated !== null
          ? new Date(data.dateCreated).toLocaleString()
          : null;
      this.dateUpdated =
        data.dateUpdated !== null
          ? new Date(data.dateUpdated).toLocaleString()
          : null;
      this.users = [];

      if (data.users !== null) {
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
