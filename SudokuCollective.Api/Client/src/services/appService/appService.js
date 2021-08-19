import * as axios from "axios";
import store from "@/store";
import { processError } from "@/helpers/commonFunctions/commonFunctions";
import Paginator from "@/models/viewModels/paginator";
import { requestHeader } from "@/helpers/requestHeader";
import { requestData } from "@/helpers/requestData";
import { requestDataUpdateApp } from "@/helpers/appRequestData/appRequestData";
import {
  getAppEndpoint,
  getByLicenseEndpoint,
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
      url: `${getAppEndpoint}${params}`,
      headers: requestHeader(),
      data: requestData(),
    };

    return await axios(config);
  } catch (error) {
    return processError(error);
  }
};

const getByLicense = async function (data) {
  try {
    const config = {
      method: "post",
      url: `${getByLicenseEndpoint}`,
      headers: requestHeader(),
      data: requestData(data),
    };

    return await axios(config);
  } catch (error) {
    return processError(error);
  }
};

const postLicense = async function (data) {
  try {
    const config = {
      method: "post",
      url: getLicenseEndpoint,
      headers: requestHeader(),
      data: data,
    };

    return await axios(config);
  } catch (error) {
    return processError(error);
  }
};

const getLicense = async function (id) {
  try {
    let params = `/${id}`;

    const config = {
      method: "get",
      url: `${getLicenseEndpoint}${params}`,
      headers: requestHeader(),
      data: null,
    };

    return await axios(config);
  } catch (error) {
    return processError(error);
  }
};

const getMyApps = async function () {
  try {
    const config = {
      method: "post",
      url: `${getMyAppsEndpoint}`,
      headers: requestHeader(),
      data: requestData(),
    };

    return await axios(config);
  } catch (error) {
    return processError(error);
  }
};

const getApps = async function () {
  try {
    const config = {
      method: "post",
      url: `${getAppEndpoint}`,
      headers: requestHeader(),
      data: requestData(),
    };

    return await axios(config);
  } catch (error) {
    return processError(error);
  }
};

const getRegisteredApps = async function (userid) {
  try {
    let params = `/${userid}`;

    const config = {
      method: "post",
      url: `${getRegisteredAppsEndpoint}${params}`,
      headers: requestHeader(),
      data: requestData(),
    };

    return await axios(config);
  } catch (error) {
    return processError(error);
  }
};

const getAppUsers = async function (id) {
  try {
    let params = `/${id}/GetAppUsers`;

    const config = {
      method: "post",
      url: `${getAppEndpoint}${params}`,
      headers: requestHeader(),
      data: requestData(),
    };

    return await axios(config);
  } catch (error) {
    return processError(error);
  }
};

const getNonAppUsers = async function (id) {
  try {
    let params = `/${id}/getNonAppUsers`;

    const config = {
      method: "post",
      url: `${getAppEndpoint}${params}`,
      headers: requestHeader(),
      data: requestData(),
    };

    return await axios(config);
  } catch (error) {
    return processError(error);
  }
};

const putActivateAdminPrivileges = async function (appId, userId) {
  try {
    const params = `/${appId}/activateAdminPrivileges/${userId}`;

    const payload = {
      license: store.getters["settingsModule/getLicense"],
      requestorId: store.getters["settingsModule/getRequestorId"],
      appId: store.getters["settingsModule/getAppId"],
      paginator: new Paginator(),
    };

    const data = requestData(payload);

    const config = {
      method: "put",
      url: `${getAppEndpoint}${params}`,
      headers: requestHeader(),
      data: data,
    };

    return await axios(config);
  } catch (error) {
    return processError(error);
  }
};

const putDeactivateAdminPrivileges = async function (appId, userId) {
  try {
    const params = `/${appId}/deactivateAdminPrivileges/${userId}`;

    const payload = {
      license: store.getters["settingsModule/getLicense"],
      requestorId: store.getters["settingsModule/getRequestorId"],
      appId: store.getters["settingsModule/getAppId"],
      paginator: new Paginator(),
    };

    const data = requestData(payload);

    const config = {
      method: "put",
      url: `${getAppEndpoint}${params}`,
      headers: requestHeader(),
      data: data,
    };

    return await axios(config);
  } catch (error) {
    return processError(error);
  }
};

const updateApp = async function (data) {
  try {
    const params = `/${data.id}`;

    const payload = {
      name: data.name,
      devUrl: data.devUrl,
      liveUrl: data.liveUrl,
      isActive: data.isActive,
      inDevelopment: data.inDevelopment,
      permitSuperUserAccess: data.permitSuperUserAccess,
      permitCollectiveLogins: data.permitCollectiveLogins,
      disableCustomUrls: data.disableCustomUrls,
      customEmailConfirmationAction: data.customEmailConfirmationAction,
      customPasswordResetAction: data.customPasswordResetAction,
      timeFrame: data.timeFrame,
      accessDuration: data.accessDuration,
      paginator: data.paginator,
    };

    const config = {
      method: "put",
      url: `${getAppEndpoint}${params}`,
      headers: requestHeader(),
      data: requestDataUpdateApp(payload),
    };

    return await axios(config);
  } catch (error) {
    return processError(error);
  }
};

const deleteApp = async function (app) {
  try {
    let params = `/${app.id}`;

    const data = {
      license: app.license,
      requestorId: app.ownerId,
      appId: app.id,
      paginator: null,
    };

    const config = {
      method: "delete",
      url: `${getAppEndpoint}${params}`,
      headers: requestHeader(),
      data: requestData(data),
    };

    return await axios(config);
  } catch (error) {
    return processError(error);
  }
};

const resetApp = async function (app) {
  try {
    let params = `/${app.id}/reset`;

    const data = {
      license: app.license,
      requestorId: app.ownerId,
      appId: app.id,
      paginator: null,
    };

    const config = {
      method: "put",
      url: `${getAppEndpoint}${params}`,
      headers: requestHeader(),
      data: requestData(data),
    };

    return await axios(config);
  } catch (error) {
    return processError(error);
  }
};

const putAddUser = async function (appId, userId) {
  try {
    const params = `/${appId}/adduser/${userId}`;

    const payload = {
      license: store.getters["settingsModule/getLicense"],
      requestorId: store.getters["settingsModule/getRequestorId"],
      appId: store.getters["settingsModule/getAppId"],
      paginator: new Paginator(),
    };

    const data = requestData(payload);

    const config = {
      method: "put",
      url: `${getAppEndpoint}${params}`,
      headers: requestHeader(),
      data: data,
    };

    return await axios(config);
  } catch (error) {
    return processError(error);
  }
};

const deleteRemoveUser = async function (appId, userId) {
  try {
    const params = `/${appId}/removeuser/${userId}`;

    const payload = {
      license: store.getters["settingsModule/getLicense"],
      requestorId: store.getters["settingsModule/getRequestorId"],
      appId: store.getters["settingsModule/getAppId"],
      paginator: new Paginator(),
    };

    const data = requestData(payload);

    const config = {
      method: "delete",
      url: `${getAppEndpoint}${params}`,
      headers: requestHeader(),
      data: data,
    };

    return await axios(config);
  } catch (error) {
    return processError(error);
  }
};

const putActivateApp = async function (id) {
  try {
    const params = `/${id}/activate`;

    const config = {
      method: "put",
      url: `${getAppEndpoint}${params}`,
      headers: requestHeader(),
      data: requestData(),
    };

    return await axios(config);
  } catch (error) {
    return processError(error);
  }
};

const putDeactivateApp = async function (id) {
  try {
    const params = `/${id}/deactivate`;

    const config = {
      method: "put",
      url: `${getAppEndpoint}${params}`,
      headers: requestHeader(),
      data: requestData(),
    };

    return await axios(config);
  } catch (error) {
    return processError(error);
  }
};

const getTimeFrames = async function () {
  try {
    const config = {
      method: "get",
      url: `${getTimeFramesEndpoint}`,
      headers: requestHeader(),
    };

    const response = await axios(config);
    console.log("get time frames response:", response);
    return response.data;
  } catch (error) {
    return processError(error);
  }
};

export const appService = {
  getApp,
  getByLicense,
  postLicense,
  getLicense,
  getMyApps,
  getApps,
  getRegisteredApps,
  getAppUsers,
  getNonAppUsers,
  putActivateAdminPrivileges,
  putDeactivateAdminPrivileges,
  updateApp,
  deleteApp,
  resetApp,
  putAddUser,
  deleteRemoveUser,
  putActivateApp,
  putDeactivateApp,
  getTimeFrames,
};
