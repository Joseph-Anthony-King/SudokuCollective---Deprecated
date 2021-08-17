import store from "../../store";

const baseURL = store.getters["settingsModule/getApiURL"];
export const getDifficultiesEndpoint = `${baseURL}/api/v1/difficulties`;