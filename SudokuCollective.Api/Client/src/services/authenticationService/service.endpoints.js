﻿import store from "../../store";

const baseURL = store.getters["appSettingsModule/getApiURL"];
export const authenticateEndpoint =  `${baseURL}/api/v1/authenticate`;
export const confirmUserNameEndpoint = `${baseURL}/api/v1/authenticate/confirmusername`;