import User from "@/models/user";

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
      this.gameCount = 0;
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
      this.gameCount = data.gameCount;
      this.userCount = data.userCount;
      this.timeFrame = data.timeFrame;
      this.accessDuration = data.accessDuration;
      this.dateCreated = data.dateCreated;
      this.dateUpdated = data.dateUpdated;
      this.users = [];

      if (data.users.length > 0) {
        for (const userApp of data.users) {
          if (userApp.user !== undefined) {
            const user = new User(userApp.user);
            this.users.push(user);
          } else {
            const user = new User(userApp);
            this.users.push(user);
          }
        }
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