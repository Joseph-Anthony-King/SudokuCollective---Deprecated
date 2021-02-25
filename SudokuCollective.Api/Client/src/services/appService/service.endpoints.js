import store from "../../store";

const baseURL = store.getters["appSettingsModule/getApiURL"];
export const getAppEnpoint = `${baseURL}/api/v1/apps`;
export const getAppByLicenseEnpoint = `${getAppEnpoint}/getByLicense`;
export const getLicenseEndpoint = `${baseURL}/api/v1/licenses`;
export const getObtainAdminPrivilegesEnpoint = `${getAppEnpoint}/obtainAdminPrivileges`;