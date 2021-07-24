import store from "../../store";

const baseURL = store.getters["settingsModule/getApiURL"];
export const getRegisterEndpoint = `${baseURL}/api/v1/register`;
export const getResendEmailConfirmationEndpoint = `${getRegisterEndpoint}/resendEmailConfirmation`;
