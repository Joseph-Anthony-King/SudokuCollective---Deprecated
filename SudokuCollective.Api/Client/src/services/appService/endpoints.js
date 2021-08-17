import store from "../../store";

const baseURL = store.getters["settingsModule/getApiURL"];
export const getAppEndpoint = `${baseURL}/api/v1/apps`;
export const getByLicenseEndpoint = `${getAppEndpoint}/getByLicense`;
export const getMyAppsEndpoint = `${getAppEndpoint}/getMyApps`;
export const getRegisteredAppsEndpoint = `${getAppEndpoint}/getMyRegistered`;
export const getLicenseEndpoint = `${baseURL}/api/v1/licenses`;
export const getTimeFramesEndpoint = `${getAppEndpoint}/timeFrames`;
