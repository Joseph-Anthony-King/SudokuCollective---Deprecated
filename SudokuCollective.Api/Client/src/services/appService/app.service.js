import * as axios from "axios";
import { requestHeader } from "../../helpers/requestHeader";
import { requestData } from "../../helpers/requestData";
import { 
  getAppEnpoint,
  getAppByLicenseEnpoint,
  getLicenseEndpoint,
  getMyAppsEndpoint,
  getObtainAdminPrivilegesEnpoint
} from "./service.endpoints";

const getApp = async function(id, pageListModel) {
  try {
    let params = `/${id}`;

    const config = {
      method: "post",
      url: `${getAppEnpoint}${params}`,
      headers: requestHeader(),
      data: requestData(pageListModel),
    };

    const response = await axios(config);

    return response;
  } catch (error) {
    console.error(error.name, error.message);
    return error.response;
  }
}

const getAppByLicense = async function(pageListModel) {
  try {
    const config = {
      method: "post",
      url: `${getAppByLicenseEnpoint}`,
      headers: requestHeader(),
      data: requestData(pageListModel),
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

const getMyApps = async function(pageListModel) {
  try {
    const config = {
      method: "put",
      url: `${getMyAppsEndpoint}`,
      headers: requestHeader(),
      data: requestData(pageListModel),
    };

    const response = await axios(config);

    return response;
  } catch (error) {
    console.error(error.name, error.message);
    return error.response;
  }
}

const postObtainAdminPrivileges = async function (pageListModel) {
  try {
    const config = {
      method: "post",
      url: `${getObtainAdminPrivilegesEnpoint}`,
      headers: requestHeader(),
      data: requestData(pageListModel),
    };

    const response = await axios(config);

    return response;
  } catch (error) {
    console.error(error.name, error.message);
    return error.response;
  }
};

export const appService = {
  getApp,
  getAppByLicense,
  postLicense,
  getLicense,
  getMyApps,
  postObtainAdminPrivileges,
};
