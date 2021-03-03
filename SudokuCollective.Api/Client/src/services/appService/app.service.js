import * as axios from "axios";
import store from "@/store";
import App from "@/models/app";
import { requestHeader } from "@/helpers/requestHeader";
import { requestData } from "@/helpers/requestData";
import { requestDataUpdateApp } from "@/helpers/appRequestData/appRequestData";
import { 
  getAppEnpoint,
  getByLicenseEnpoint,
  getLicenseEndpoint,
  getMyAppsEndpoint,
  getObtainAdminPrivilegesEnpoint
} from "./service.endpoints";

const getApp = async function(id) {
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

const getByLicense = async function(license) {
  try {
    const data = {
      license: license,
      requestorId: store.getters["settingsModule/getRequestorId"],
      appId: store.getters["settingsModule/getAppId"],
      pageListModel: null
    };

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

const postLicense = async function(createAppModel) {

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

const getLicense = async function(id) {
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

const getMyApps = async function() {
  try {
    const config = {
      method: "put",
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

const postObtainAdminPrivileges = async function () {
  try {
    const config = {
      method: "post",
      url: `${getObtainAdminPrivilegesEnpoint}`,
      headers: requestHeader(),
      data: requestData(),
    };

    const response = await axios(config);

    return response;
  } catch (error) {
    console.error(error.name, error.message);
    return error.response;
  }
};

const updateApp = async function (
  id,
  name,
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
      customEmailConfirmationDevUrl: customEmailConfirmationDevUrl,
      customEmailConfirmationLiveUrl: customEmailConfirmationLiveUrl,
      customPasswordResetDevUrl: customPasswordResetDevUrl,
      customPasswordResetLiveUrl: customPasswordResetLiveUrl,
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

const deleteApp = async function(app) {
  try {
    let params = `/${app.id}`;

    const data = {
      license: app.license,
      requestorId: app.ownerId,
      appId: app.id,
      pageListModel: null
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

export const appService = {
  getApp,
  getByLicense,
  postLicense,
  getLicense,
  getMyApps,
  postObtainAdminPrivileges,
  updateApp,
  deleteApp,
};
