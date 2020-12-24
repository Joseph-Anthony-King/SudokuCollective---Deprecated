import store from "../../store";

const baseURL = store.getters["appSettingsModule/getApiURL"];
export const getUserEnpoint = baseURL + "/api/v1/users";
export const getRequestPasswordResetEnpoint = baseURL + "/api/v1/users/requestPasswordReset";