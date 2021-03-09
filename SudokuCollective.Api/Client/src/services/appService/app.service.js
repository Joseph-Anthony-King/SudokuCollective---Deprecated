import * as axios from "axios";
import store from "@/store";
import App from "@/models/app";
import PageListModel from "@/models/viewModels/pageListModel"
import { requestHeader } from "@/helpers/requestHeader";
import { requestData } from "@/helpers/requestData";
import { requestDataUpdateApp } from "@/helpers/appRequestData/appRequestData";
import { appUserRequestData } from "@/helpers/appRequestData/appUserRequestData";
import {
  getAppEnpoint,
  getByLicenseEnpoint,
  getLicenseEndpoint,
  getMyAppsEndpoint,
  getObtainAdminPrivilegesEnpoint,
  getDeactivateAdminPrivilegesEnpoint,
  getAddAppUserEndpoint,
  getTimeFramesEnpoint,
} from "./service.endpoints";

const getApp = async function (id, fullRecord) {
  try {
    let params;

    if (fullRecord === undefined) {
      params = `/${id}?fullRecord=false`;
    } else {
      params = `/${id}?fullRecord=${fullRecord}`;
    }

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

const getByLicense = async function (data, fullRecord) {
  try {
    let params;

    if (fullRecord === undefined) {
      params = "?fullRecord=false";
    } else {
      params = `?fullRecord=${fullRecord}`;
    }

    const config = {
      method: "post",
      url: `${getByLicenseEnpoint}${params}`,
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

const getMyApps = async function (fullRecord) {
  try {
    let params;

    if (fullRecord === undefined) {
      params = "?fullRecord=false";
    } else {
      params = `?fullRecord=${fullRecord}`;
    }

    const config = {
      method: "put",
      url: `${getMyAppsEndpoint}${params}`,
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

const getAppUsers = async function (id, fullRecord) {
  try {
    let params;

    if (fullRecord === undefined) {
      params = `/${id}/getAppUsers?fullRecord=false`;
    } else {
      params = `/${id}/getAppUsers?fullRecord=${fullRecord}`;
    }

    const config = {
      method: "put",
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

const getNonAppUsers = async function (id, fullRecord) {
  try {
    let params;

    if (fullRecord === undefined) {
      params = `/${id}/getNonAppUsers?fullRecord=false`;
    } else {
      params = `/${id}/getNonAppUsers?fullRecord=${fullRecord}`;
    }

    const config = {
      method: "put",
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

const postObtainAdminPrivileges = async function (
  userId,
  license,
  pagination) {
  try {
    const params = `/${userId}`;
    let pageListModel;

    if (pagination === undefined) {
      pageListModel = new PageListModel();
    } else {
      pageListModel = new PageListModel(
        pagination.page,
        pagination.itemsPerPage,
        pagination.sortBy,
        pagination.orderByDescending,
        pagination.includeCompletedGames
      );
    }

    const payload = {
      targetLicense: license,
      pageListModel: pageListModel
    }

    const data = appUserRequestData(payload);

    const config = {
      method: "post",
      url: `${getObtainAdminPrivilegesEnpoint}${params}`,
      headers: requestHeader(),
      data: data
    };

    console.log("postObtainAdminPrivileges config:", config);

    const response = await axios(config);

    return response;
  } catch (error) {
    console.error(error.name, error.message);
    return error.response;
  }
};

const putDeactivateAdminPrivileges = async function (
  userId,
  license,
  pagination) {
  try {
    const params = `/${userId}`;
    let pageListModel;

    if (pagination === undefined) {
      pageListModel = new PageListModel();
    } else {
      pageListModel = new PageListModel(
        pagination.page,
        pagination.itemsPerPage,
        pagination.sortBy,
        pagination.orderByDescending,
        pagination.includeCompletedGames
      );
    }

    const payload = {
      targetLicense: license,
      pageListModel: pageListModel
    }

    const data = appUserRequestData(payload);

    const config = {
      method: "put",
      url: `${getDeactivateAdminPrivilegesEnpoint}${params}`,
      headers: requestHeader(),
      data: data
    };

    console.log("postObtainAdminPrivileges config:", config);

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

const resetApp = async function (app) {
  try {
    let params = `/${app.id}/resetapp`;

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

const putAddUser = async function (
  userId,
  license,
  pagination) {
  try {
    const params = `/${userId}`;
    let pageListModel;

    if (pagination === undefined) {
      pageListModel = new PageListModel();
    } else {
      pageListModel = new PageListModel(
        pagination.page,
        pagination.itemsPerPage,
        pagination.sortBy,
        pagination.orderByDescending,
        pagination.includeCompletedGames
      );
    }

    const payload = {
      targetLicense: license,
      pageListModel: pageListModel
    }

    const data = appUserRequestData(payload);

    const config = {
      method: "put",
      url: `${getAddAppUserEndpoint}${params}`,
      headers: requestHeader(),
      data: data,
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
      method: "put",
      url: `${getTimeFramesEnpoint}`,
      headers: requestHeader(),
      data: requestData(),
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
  getAppUsers,
  getNonAppUsers,
  postObtainAdminPrivileges,
  putDeactivateAdminPrivileges,
  updateApp,
  deleteApp,
  resetApp,
  putAddUser,
  getTimeFrames,
};
