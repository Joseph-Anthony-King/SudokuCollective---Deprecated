import store from "../../store";

const baseURL = store.getters["settingsModule/getApiURL"];
export const getAppEnpoint = `${baseURL}/api/v1/apps`;
export const getByLicenseEnpoint = `${getAppEnpoint}/getByLicense`;
export const getMyAppsEndpoint = `${getAppEnpoint}/getMyApps`;
export const getLicenseEndpoint = `${baseURL}/api/v1/licenses`;
export const getTimeFramesEndpoint = `${getAppEnpoint}/getTimeFrames`;
export const getRegisteredAppsEndpoint = `${getAppEnpoint}/registered`;