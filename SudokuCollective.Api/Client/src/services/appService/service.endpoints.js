import store from "../../store";

const baseURL = store.getters["appSettingsModule/getApiURL"];
export const getAppEnpoint = `${baseURL}/api/v1/apps`;
export const getObtainAdminPrivilegesEnpoint = `${getAppEnpoint}/obtainAdminPrivileges`;