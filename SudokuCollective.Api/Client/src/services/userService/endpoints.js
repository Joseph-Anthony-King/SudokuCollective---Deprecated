import store from "../../store";

const baseURL = store.getters["settingsModule/getApiURL"];
export const getUserEnpoint = `${baseURL}/api/v1/users`;
export const getRequestPasswordResetEnpoint = `${getUserEnpoint}/requestPasswordReset`;
export const getResendRequestPasswordResetEndpoint = `${getUserEnpoint}/resendRequestPasswordReset`;
export const getCancelPasswordResetEndpoint = `${getUserEnpoint}/cancelPasswordReset`;
export const getCancelEmailConfirmationEndpoint = `${getUserEnpoint}/cancelEmailConfirmation`;
export const getCancelAllEmailRequestsEndpoint = `${getUserEnpoint}/cancelAllEmailRequests`;
