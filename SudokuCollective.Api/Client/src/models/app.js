import User from "@/models/user";

class App {
  constructor(
    id,
    name,
    ownerId,
    devUrl,
    liveUrl,
    isActive,
    inDevelopment,
    permitSuperUserAccess,
    permitCollectiveLogins,
    disableCustomUrls,
    customEmailConfirmationDevUrl,
    customEmailConfirmationLiveUrl,
    customPasswordResetDevUrl,
    customPasswordResetLiveUrl,
    gameCount,
    userCount,
    dateCreated,
    dateUpdated,
    users
  ) {
    if (!arguments.length) {
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
      this.id = id;
      this.name = name;
      this.ownerId = ownerId;
      this.devUrl = devUrl;
      this.liveUrl = liveUrl;
      this.isActive = isActive;
      this.inDevelopment = inDevelopment;
      this.permitSuperUserAccess = permitSuperUserAccess;
      this.permitCollectiveLogins = permitCollectiveLogins;
      this.disableCustomUrls = disableCustomUrls;
      this.customEmailConfirmationDevUrl = customEmailConfirmationDevUrl;
      this.customEmailConfirmationLiveUrl = customEmailConfirmationLiveUrl;
      this.customPasswordResetDevUrl = customPasswordResetDevUrl;
      this.customPasswordResetLiveUrl = customPasswordResetLiveUrl;
      this.gameCount = gameCount;
      this.userCount = userCount;
      this.dateCreated = dateCreated;
      this.dateUpdated = dateUpdated;
      this.license = "";

      for (const user of users) {
        const u = new User();
        u.clone(user);
        this.users.push(u);
      }
    }
  }

  updateLicense(license) {
    this.license = license;
  }

  clone(data) {
    if (data !== undefined) {
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

      for (const user of data.users) {
        const u = new User();
        u.clone(user);
        this.users.push(u);
      }
    }
  }
}

export default App;