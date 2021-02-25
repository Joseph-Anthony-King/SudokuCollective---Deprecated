import * as axios from "axios";
import { requestHeader } from "../../helpers/requestHeader";
import { requestData } from "../../helpers/requestData";
import { 
  getAppEnpoint,
  getAppByLicenseEnpoint,
  getLicenseEndpoint,
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
  getLicense,
  postObtainAdminPrivileges,
};
