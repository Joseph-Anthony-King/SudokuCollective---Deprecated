import store from "../../store";

const baseURL = store.getters["appSettingsModule/getApiURL"];
export const getUserEnpoint = baseURL + "/api/v1/users/";