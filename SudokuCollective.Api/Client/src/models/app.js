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
      this.customEmailConfirmationDevUrl = "";
      this.customEmailConfirmationLiveUrl = "";
      this.customPasswordResetDevUrl = "";
      this.customPasswordResetLiveUrl = "";
      this.gameCount = 0;
      this.userCount = 0;
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
      this.customEmailConfirmationDevUrl = data.customEmailConfirmationDevUrl;
      this.customEmailConfirmationLiveUrl = data.customEmailConfirmationLiveUrl;
      this.customPasswordResetDevUrl = data.customPasswordResetDevUrl;
      this.customPasswordResetLiveUrl = data.customPasswordResetLiveUrl;
      this.gameCount = data.gameCount;
      this.userCount = data.userCount;
      this.dateCreated = data.dateCreated;
      this.dateUpdated = data.dateUpdated;
      this.users = [];

      if (data.users.length > 0) {
        for (const userApp of data.users) {
          const user = new User(userApp.user);
          this.users.push(user);
        }
      }
      
      this.license = "";
    }
  }

  updateLicense(license) {
    this.license = license;
  }
}

export default App;