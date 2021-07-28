import { appService } from "@/services/appService/appService";
import App from "@/models/app";
import User from "@/models/user"

const getApp = async function (id) {
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
      status: response.status,
      success: response.data.success,
      message: response.data.message.substring(17),
      app: app
    }
  } else {
    return {
      success: response.data.success,
      message: response.data.message.substring(17)
    }
  }
}

const getByLicense = async function () {

  const data = {
    license: process.env.VUE_APP_LICENSE,
    requestorId: 1,
    appId: 1,
    paginator: null,
  };

  const response = await appService.getByLicense(data);

  if (response.status === 200) {
    return new App(response.data.app);
  } else {
    return new App();
  }
}

const postLicense = async function (data) {

  const response = await appService.postLicense(data);

  if (response.status === 201) {
    const appsResponse = await getMyApps();

    return {
      status: response.status,
      success: response.data.success,
      message: response.data.message.substring(17),
      app: response.data.app,
      apps: appsResponse.apps
    }
  } else if (response.status === 404) {
    return {
      status: response.status,
      success: response.data.success,
      message: response.data.message.substring(17)
    }
  } else {
    return {
      status: response.status,
      success: response.data.success,
      message: response.data.message
    }
  }
}

const updateApp = async function (data) {

  const response = await appService.putUpdateApp(data);

  if (response.status === 200) {

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
      status: response.status,
      success: response.data.success,
      message: response.data.message.substring(17),
      app: app
    }
  } else {
    return {
      success: response.data.success,
      message: response.data.message.substring(17)
    }
  }
}

const getMyApps = async function () {
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
      status: response.status,
      success: response.data.success,
      message: response.data.message.substring(17),
      apps: tempArray
    }
  } else {
    return {
      success: response.data.success,
      message: response.data.message.substring(17)
    }
  }
}

const getRegisteredApps = async function (userid) {
  const response = await appService.getRegisteredApps(userid);

  if (response.data.success) {
    let tempArray = [];

    for (const app of response.data.apps) {
      const registeredApp = new App(app);
      tempArray.push(registeredApp);
    }

    return {
      status: response.status,
      success: response.data.success,
      message: response.data.message.substring(17),
      apps: tempArray
    }
  } else {

    return {
      status: response.status,
      success: response.data.success,
      message: response.data.message.substring(17)
    }
  }
}

const getNonAppUsers = async function (id) {  

  const response = await appService.getNonAppUsers(id);

  return {
    status: response.status,
    success: response.data.success,
    message: response.data.message.substring(17),
    users: response.data.users
  }
}

const deleteApp = async function (app) {
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
        status: response.status,
        success: response.data.success,
        message: response.data.message.substring(17),
        app: new App(),
        apps: tempArray
      }
    }
  } else {
    return {
      status: response.status,
      success: response.data.success,
      message: response.data.message.substring(17)
    }
  }
}

const resetApp = async function (app) {
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
        status: response.status,
        success: response.data.success,
        message: response.data.message.substring(17),
        app: response.data.app,
        apps: tempArray
      }
    }
  } else {
    return {
      status: response.status,
      success: response.data.success,
      message: response.data.message.substring(17)
    }
  }
}

const addUser = async function (appid, userid) {
  const response = await appService.putAddUser(
    appid,
    userid
  );
  return {
    status: response.status,
    success: response.data.success,
    message: response.data.message.substring(17)
  }
}

const removeUser = async function (appid, userid) {
  const response = await appService.deleteRemoveUser(
    appid,
    userid
  );
  return {
    status: response.status,
    success: response.data.success,
    message: response.data.message.substring(17)
  }
}

const activateAdminPrivileges = async function (appid, userid) {
  const response = await appService.putActivateAdminPrivileges(
    appid,
    userid
  );
  return {
    status: response.status,
    success: response.data.success,
    message: response.data.message.substring(17)
  }
}

const deactivateAdminPrivileges = async function (appid, userid) {
  const response = await appService.putDeactivateAdminPrivileges(
    appid,
    userid
  );
  return {
    status: response.status,
    success: response.data.success,
    message: response.data.message.substring(17)
  }
}

const getTimeFrames = async function () {
  return await appService.getTimeFrames();
}

export const appProvider = {
  getApp,
  getByLicense,
  postLicense,
  updateApp,
  getMyApps,
  getRegisteredApps,
  getNonAppUsers,
  deleteApp,
  resetApp,
  addUser,
  removeUser,
  activateAdminPrivileges,
  deactivateAdminPrivileges,
  getTimeFrames
}
