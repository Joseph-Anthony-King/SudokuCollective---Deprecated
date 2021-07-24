import * as axios from "axios";
import store from "@/store";
import App from "@/models/app";
import Paginator from "@/models/viewModels/paginator"
import { requestHeader } from "@/helpers/requestHeader";
import { requestData } from "@/helpers/requestData";
import { requestDataUpdateApp } from "@/helpers/appRequestData/appRequestData";
import {
  getAppEnpoint,
  getByLicenseEnpoint,
  getLicenseEndpoint,
  getMyAppsEndpoint,
  getTimeFramesEndpoint,
  getRegisteredAppsEndpoint,
} from "./endpoints";

const getApp = async function (id) {
  try {
    let params = `/${id}`;

    const config = {
      method: "post",
      url: `${getAppEnpoint}${params}`,
      headers: requestHeader(),
      data: requestData(),
    };

    const response = await axios(config);

    return response;
  } catch (error) {
    console.error(error.name, error.message);
    return error.response;
  }
}

const getByLicense = async function (data) {
  try {
    const config = {
      method: "post",
      url: `${getByLicenseEnpoint}`,
      headers: requestHeader(),
      data: requestData(data),
    };

    const response = await axios(config);

    return response;
  } catch (error) {
    console.error(error.name, error.message);
    return error.response;
  }
}

const postLicense = async function (createAppModel) {

  try {
    const config = {
      method: "post",
      url: getLicenseEndpoint,
      headers: requestHeader(),
      data: createAppModel,
    };

    const response = await axios(config);

    return response;
  } catch (error) {
    return error.response;
  }
}

const getLicense = async function (id) {
  try {
    let params = `/${id}`;

    const config = {
      method: "get",
      url: `${getLicenseEndpoint}${params}`,
      headers: requestHeader(),
      data: null,
    };

    const response = await axios(config);

    return response;
  } catch (error) {
    console.error(error.name, error.message);
    return error.response;
  }
}

const getMyApps = async function () {
  try {
    const config = {
      method: "post",
      url: `${getMyAppsEndpoint}`,
      headers: requestHeader(),
      data: requestData(),
    };

    const response = await axios(config);

    return response;
  } catch (error) {
    console.error(error.name, error.message);
    return error.response;
  }
}

const getRegisteredApps = async function (userid) {
  try {
    let params = `/${userid}`;

    const config = {
      method: "post",
      url: `${getRegisteredAppsEndpoint}${params}`,
      headers: requestHeader(),
      data: requestData(),
    };

    const response = await axios(config);

    return response;
  } catch (error) {
    console.error(error.name, error.message);
    return error.response;
  }
}

const getAppUsers = async function (id) {
  try {
    let params = `/${id}/GetAppUsers`;

    const config = {
      method: "post",
      url: `${getAppEnpoint}${params}`,
      headers: requestHeader(),
      data: requestData(),
    };

    const response = await axios(config);

    return response;
  } catch (error) {
    console.error(error.name, error.message);
    return error.response;
  }
}

const getNonAppUsers = async function (id) {
  try {
    let params = `/${id}/getNonAppUsers`;

    const config = {
      method: "post",
      url: `${getAppEnpoint}${params}`,
      headers: requestHeader(),
      data: requestData(),
    };

    const response = await axios(config);

    return response;
  } catch (error) {
    console.error(error.name, error.message);
    return error.response;
  }
}

const putActivateAdminPrivileges = async function (
  appId,
  userId,
  pagination) {
  try {
    const params = `/${appId}/activateAdminPrivileges/${userId}`;
    let paginator;

    if (pagination === undefined) {
      paginator = new Paginator();
    } else {
      paginator = new Paginator(
        pagination.page,
        pagination.itemsPerPage,
        pagination.sortBy,
        pagination.orderByDescending,
        pagination.includeCompletedGames
      );
    }

    const payload = {
      license: store.getters["settingsModule/getLicense"],
      requestorId: store.getters["settingsModule/getRequestorId"],
      appId: store.getters["settingsModule/getAppId"],
      paginator: paginator
    }

    const data = requestData(payload);

    const config = {
      method: "put",
      url: `${getAppEnpoint}${params}`,
      headers: requestHeader(),
      data: data
    };

    const response = await axios(config);

    return response;
  } catch (error) {
    console.error(error.name, error.message);
    return error.response;
  }
};

const putDeactivateAdminPrivileges = async function (
  appId,
  userId,
  pagination) {
  try {
    const params = `/${appId}/deactivateAdminPrivileges/${userId}`;
    let paginator;

    if (pagination === undefined) {
      paginator = new Paginator();
    } else {
      paginator = new Paginator(
        pagination.page,
        pagination.itemsPerPage,
        pagination.sortBy,
        pagination.orderByDescending,
        pagination.includeCompletedGames
      );
    }

    const payload = {
      license: store.getters["settingsModule/getLicense"],
      requestorId: store.getters["settingsModule/getRequestorId"],
      appId: store.getters["settingsModule/getAppId"],
      paginator: paginator
    }

    const data = requestData(payload);

    const config = {
      method: "put",
      url: `${getAppEnpoint}${params}`,
      headers: requestHeader(),
      data: data
    };

    const response = await axios(config);

    return response;
  } catch (error) {
    console.error(error.name, error.message);
    return error.response;
  }
};

const putUpdateApp = async function (
  id,
  name,
  devUrl,
  liveUrl,
  isActive,
  inDevelopment,
  permitSuperUserAccess,
  permitCollectiveLogins,
  disableCustomUrls,
  customEmailConfirmationAction,
  customPasswordResetAction,
  timeFrame,
  accessDuration,
) {
  try {
    const params = `/${id}`;

    const payload = {
      name: name,
      devUrl: devUrl,
      liveUrl: liveUrl,
      isActive: isActive,
      inDevelopment: inDevelopment,
      permitSuperUserAccess: permitSuperUserAccess,
      permitCollectiveLogins: permitCollectiveLogins,
      disableCustomUrls: disableCustomUrls,
      customEmailConfirmationAction: customEmailConfirmationAction,
      customPasswordResetAction: customPasswordResetAction,
      timeFrame: timeFrame,
      accessDuration: accessDuration
    }

    const config = {
      method: "put",
      url: `${getAppEnpoint}${params}`,
      headers: requestHeader(),
      data: requestDataUpdateApp(payload),
    };

    let app = new App();

    const response = await axios(config);

    app = new App(response.data.app);
    store.dispatch("appModule/updateSelectedApp", app);

    return response;
  } catch (error) {
    return error.response;
  }
};

const deleteApp = async function (app) {
  try {
    let params = `/${app.id}`;

    const data = {
      license: app.license,
      requestorId: app.ownerId,
      appId: app.id,
      paginator: null
    };

    const config = {
      method: "delete",
      url: `${getAppEnpoint}${params}`,
      headers: requestHeader(),
      data: requestData(data),
    };

    const response = await axios(config);

    return response;
  } catch (error) {
    console.error(error.name, error.message);
    return error.response;
  }
}

const resetApp = async function (app) {
  try {
    let params = `/${app.id}/resetapp`;

    const data = {
      license: app.license,
      requestorId: app.ownerId,
      appId: app.id,
      paginator: null
    };

    const config = {
      method: "delete",
      url: `${getAppEnpoint}${params}`,
      headers: requestHeader(),
      data: requestData(data),
    };

    const response = await axios(config);

    return response;
  } catch (error) {
    console.error(error.name, error.message);
    return error.response;
  }
}

const putAddUser = async function (
  appId,
  userId,
  pagination) {
  try {
    const params = `/${appId}/adduser/${userId}`;
    let paginator;

    if (pagination === undefined) {
      paginator = new Paginator();
    } else {
      paginator = new Paginator(
        pagination.page,
        pagination.itemsPerPage,
        pagination.sortBy,
        pagination.orderByDescending,
        pagination.includeCompletedGames
      );
    }

    const payload = {
      license: store.getters["settingsModule/getLicense"],
      requestorId: store.getters["settingsModule/getRequestorId"],
      appId: store.getters["settingsModule/getAppId"],
      paginator: paginator
    }

    const data = requestData(payload);

    const config = {
      method: "put",
      url: `${getAppEnpoint}${params}`,
      headers: requestHeader(),
      data: data
    };

    const response = await axios(config);

    return response;
  } catch (error) {
    console.error(error.name, error.message);
    return error.response;
  }
}

const deleteRemoveUser = async function (
  appId,
  userId,
  pagination) {
  try {
    const params = `/${appId}/removeuser/${userId}`;
    let paginator;

    if (pagination === undefined) {
      paginator = new Paginator();
    } else {
      paginator = new Paginator(
        pagination.page,
        pagination.itemsPerPage,
        pagination.sortBy,
        pagination.orderByDescending,
        pagination.includeCompletedGames
      );
    }

    const payload = {
      license: store.getters["settingsModule/getLicense"],
      requestorId: store.getters["settingsModule/getRequestorId"],
      appId: store.getters["settingsModule/getAppId"],
      paginator: paginator
    }

    const data = requestData(payload);

    const config = {
      method: "delete",
      url: `${getAppEnpoint}${params}`,
      headers: requestHeader(),
      data: data
    };

    const response = await axios(config);

    return response;
  } catch (error) {
    console.error(error.name, error.message);
    return error.response;
  }
}

const getTimeFrames = async function () {
  try {
    const config = {
      method: "get",
      url: `${getTimeFramesEndpoint}`,
      headers: requestHeader()
    };
    const response = await axios(config);
    return response.data;
  } catch (error) {
    console.error(error.name, error.message);
    return error.response;
  }
};

export const appService = {
  getApp,
  getByLicense,
  postLicense,
  getLicense,
  getMyApps,
  getRegisteredApps,
  getAppUsers,
  getNonAppUsers,
  putActivateAdminPrivileges,
  putDeactivateAdminPrivileges,
  putUpdateApp,
  deleteApp,
  resetApp,
  putAddUser,
  deleteRemoveUser,
  getTimeFrames,
};
