import store from "../../store";

const baseURL = store.getters["settingsModule/getApiURL"];
export const getGamesEndpoint = `${baseURL}/api/v1/games`;