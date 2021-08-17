import store from "../../store";

const baseURL = store.getters["settingsModule/getApiURL"];
export const getSolutionEndpoint = `${baseURL}/api/v1/solutions`;
export const postSolveEndpoint = `${getSolutionEndpoint}/solve`;
