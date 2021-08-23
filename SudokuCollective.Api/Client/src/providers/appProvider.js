import { processFailure } from "@/helpers/commonFunctions/commonFunctions";
import { appService } from "@/services/appService/appService";
import App from "@/models/app";
import User from "@/models/user";

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
      app: app,
    };
  } else {
    return processFailure(response);
  }
};

const getByLicense = async function () {
  const data = {
    license: process.env.VUE_APP_LICENSE,
    requestorId: 1,
    appId: 1,
    paginator: null,
  };

  const response = await appService.getByLicense(data);

  if (response.data.success) {
    return new App(response.data.app);
  } else {
    return processFailure(response);
  }
};

const postLicense = async function (data) {
  const response = await appService.postLicense(data);

  if (response.data.success) {
    const appsResponse = await getMyApps();

    return {
      status: response.status,
      success: response.data.success,
      message: response.data.message.substring(17),
      app: response.data.app,
      apps: appsResponse.apps,
    };
  } else {
    return processFailure(response);
  }
};

const updateApp = async function (data) {
  const response = await appService.updateApp(data);

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
      app: app,
    };
  } else {
    return processFailure(response);
  }
};

const getMyApps = async function () {
  const response = await appService.getMyApps();

  if (response.data.success) {
    response.data.apps = response.data.apps.sort(function (a, b) {
      if (a.id < b.id) {
        return -1;
      }
      if (a.id > b.id) {
        return 1;
      }
      return 0;
    });

    let tempArray = [];

    for (const app of response.data.apps) {
      const newApp = new App(app);
      const licenseResponse = await appService.getLicense(newApp.id);
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
      apps: tempArray,
    };
  } else {
    return processFailure(response);
  }
};

const getApps = async function () {
  const response = await appService.getApps();

  if (response.data.success) {
    response.data.apps = response.data.apps.sort(function (a, b) {
      if (a.id < b.id) {
        return -1;
      }
      if (a.id > b.id) {
        return 1;
      }
      return 0;
    });

    let tempArray = [];

    for (const app of response.data.apps) {
      const newApp = new App(app);
      const licenseResponse = await appService.getLicense(newApp.id);
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
      apps: tempArray,
    };
  } else {
    return processFailure(response);
  }
};

const getRegisteredApps = async function (userid) {
  const response = await appService.getRegisteredApps(userid);

  if (response.data.success) {
    response.data.apps = response.data.apps.sort(function (a, b) {
      if (a.id < b.id) {
        return -1;
      }
      if (a.id > b.id) {
        return 1;
      }
      return 0;
    });

    let apps = [];

    for (const app of response.data.apps) {
      const registeredApp = new App(app);
      apps.push(registeredApp);
    }

    return {
      status: response.status,
      success: response.data.success,
      message: response.data.message.substring(17),
      apps: apps,
    };
  } else {
    return processFailure(response);
  }
};

const getNonAppUsers = async function (id) {
  const response = await appService.getNonAppUsers(id);

  if (response.data.success) {
    return {
      status: response.status,
      success: response.data.success,
      message: response.data.message.substring(17),
      users: response.data.users,
    };
  } else {
    return processFailure(response);
  }
};

const deleteApp = async function (app) {
  const response = await appService.deleteApp(app);

  if (response.data.success) {
    const appsResponse = await appService.getMyApps();

    if (appsResponse.data.success) {
      let tempArray = [];

      for (const app of appsResponse.data.apps) {
        const newApp = new App(app);
        const licenseResponse = await appService.getLicense(newApp.id);
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
      });
      return {
        status: response.status,
        success: response.data.success,
        message: response.data.message.substring(17),
        app: new App(),
        apps: tempArray,
      };
    }
  } else {
    return processFailure(response);
  }
};

const resetApp = async function (app) {
  const response = await appService.resetApp(app);

  if (response.data.success) {
    const appsResponse = await appService.getMyApps();

    if (appsResponse.data.success) {
      let tempArray = [];

      for (const app of appsResponse.data.apps) {
        const newApp = new App(app);
        const licenseResponse = await appService.getLicense(newApp.id);
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
      });
      return {
        status: response.status,
        success: response.data.success,
        message: response.data.message.substring(17),
        app: response.data.app,
        apps: tempArray,
      };
    }
  } else {
    return processFailure(response);
  }
};

const addUser = async function (appid, userid) {
  const response = await appService.putAddUser(appid, userid);

  if (response.data.success) {
    return {
      status: response.status,
      success: response.data.success,
      message: response.data.message.substring(17),
    };
  } else {
    return processFailure(response);
  }
};

const removeUser = async function (appid, userid) {
  const response = await appService.deleteRemoveUser(appid, userid);

  if (response.data.success) {
    return {
      status: response.status,
      success: response.data.success,
      message: response.data.message.substring(17),
    };
  } else {
    return processFailure(response);
  }
};

const activateAdminPrivileges = async function (appid, userid) {
  const response = await appService.putActivateAdminPrivileges(appid, userid);

  if (response.data.success) {
    return {
      status: response.status,
      success: response.data.success,
      message: response.data.message.substring(17),
    };
  } else {
    return processFailure(response);
  }
};

const deactivateAdminPrivileges = async function (appid, userid) {
  const response = await appService.putDeactivateAdminPrivileges(appid, userid);

  if (response.data.success) {
    return {
      status: response.status,
      success: response.data.success,
      message: response.data.message.substring(17),
    };
  } else {
    return processFailure(response);
  }
};

const activateApp = async function (id) {
  var response = await appService.putActivateApp(id);

  if (response.data.success) {
    return {
      status: response.status,
      success: response.data.success,
      message: response.data.message.substring(17),
    };
  } else {
    return processFailure(response);
  }
};

const deactivateApp = async function (id) {
  var response = await appService.putDeactivateApp(id);

  if (response.data.success) {
    return {
      status: response.status,
      success: response.data.success,
      message: response.data.message.substring(17),
    };
  } else {
    return processFailure(response);
  }
};

const getTimeFrames = async function () {
  return await appService.getTimeFrames();
};

export const appProvider = {
  getApp,
  getByLicense,
  postLicense,
  updateApp,
  getMyApps,
  getApps,
  getRegisteredApps,
  getNonAppUsers,
  deleteApp,
  resetApp,
  addUser,
  removeUser,
  activateAdminPrivileges,
  deactivateAdminPrivileges,
  activateApp,
  deactivateApp,
  getTimeFrames,
};
