import { appService } from "@/services/appService/appService";
import App from "@/models/app";
import User from "@/models/user"

const getApp = async function(id) {
  var response = await appService.getApp(id);

  if (response.data.success) {
    const app = new App(response.data.app);
    const licenseResponse = await appService.getLicense(app.id);
    if (licenseResponse.data.success) {
      app.updateLicense(licenseResponse.data.license);
    }
    const appUsersResponse = await appService.getAppUsers(app.id);
    appUsersResponse.data.users.forEach((user) => {
      const tempUser = new User(user);
      app.users.push(tempUser);
    });
    return {
      code: response.status,
      success: response.data.success,
      message: response.data.message,
      app: app
    }
  } else {
    return {
      success: response.data.success,
      message: response.data.message
    }
  }
}

const getMyApps = async function() {
  const response = await appService.getMyApps();

  if (response.data.success) {
    let tempArray = [];

    for (const app of response.data.apps) {
      const newApp = new App(app);
      const licenseResponse = await appService.getLicense(
        newApp.id
      );
      if (licenseResponse.data.success) {
        newApp.updateLicense(licenseResponse.data.license);
      }
      const appUsersResponse = await appService.getAppUsers(newApp.id);
      appUsersResponse.data.users.forEach((user) => {
        const tempUser = new User(user);
        newApp.users.push(tempUser);
      });
      tempArray.push(newApp);
    }
    return {
      code: response.status,
      success: response.data.success,
      message: response.data.message,
      apps: tempArray
    }
  } else {
    return {
      success: response.data.success,
      message: response.data.message
    }
  }
}

const getRegisteredApps = async function(userid) {
  const response = await appService.getRegisteredApps(userid);

  if (response.data.success) {
    let tempArray = [];

    for (const app of response.data.apps) {
      const registeredApp = new App(app);
      tempArray.push(registeredApp);
    }

    return {
      code: response.status,
      success: response.data.success,
      message: response.data.message,
      apps: tempArray
    }
  } else {

    return {
      code: response.status,
      success: response.data.success,
      message: response.data.message
    }
  }
}

const deleteApp = async function(app) {
  const response = await appService.deleteApp(app);

  if (response.status === 200) {
    const appsResponse = await appService.getMyApps();

    if (appsResponse.data.success) {
      let tempArray = [];

      for (const app of appsResponse.data.apps) {
        const newApp = new App(app);
        const licenseResponse = await appService.getLicense(
          newApp.id
        );
        if (licenseResponse.data.success) {
          newApp.updateLicense(licenseResponse.data.license);
        }
        const appUsersResponse = await appService.getAppUsers(newApp.id);
        appUsersResponse.data.users.forEach((user) => {
          const tempUser = new User(user);
          newApp.users.push(tempUser);
        });
        tempArray.push(newApp);
      }
      tempArray.forEach((app) => {
        if (response.data.app.id === app.id) {
          response.data.app.users = app.users;
        }
      })
      return {
        code: response.status,
        success: response.data.success,
        message: response.data.message,
        app: new App(),
        apps: tempArray
      }
    }
  } else {
    return {
      code: response.status,
      success: response.data.success,
      message: response.data.message
    }
  }
}

const resetApp = async function(app) {
  const response = await appService.resetApp(app);

  if (response.status === 200) {
    const appsResponse = await appService.getMyApps();

    if (appsResponse.data.success) {
      let tempArray = [];

      for (const app of appsResponse.data.apps) {
        const newApp = new App(app);
        const licenseResponse = await appService.getLicense(
          newApp.id
        );
        if (licenseResponse.data.success) {
          newApp.updateLicense(licenseResponse.data.license);
        }
        const appUsersResponse = await appService.getAppUsers(newApp.id);
        appUsersResponse.data.users.forEach((user) => {
          const tempUser = new User(user);
          newApp.users.push(tempUser);
        });
        tempArray.push(newApp);
      }
      tempArray.forEach((app) => {
        if (response.data.app.id === app.id) {
          response.data.app.users = app.users;
        }
      })
      return {
        code: response.status,
        success: response.data.success,
        message: response.data.message,
        app: response.data.app,
        apps: tempArray
      }
    }
  } else {
    return {
      code: response.status,
      success: response.data.success,
      message: response.data.message
    }
  }
}

export const appProvider = {
  getApp,
  getMyApps,
  getRegisteredApps,
  deleteApp,
  resetApp
}
