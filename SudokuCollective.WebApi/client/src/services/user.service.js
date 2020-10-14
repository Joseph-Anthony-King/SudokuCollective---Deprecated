import * as axios from "axios";
import store from "../store";
import User from "../models/user";
import { requestHeader } from "../helpers/requestHeader";

const getUser = async function (id) {

    try {

        const baseURL = store.getters["appSettingsModule/getApiURL"];
        const route = "/api/v1/users/";
        const headers = requestHeader();

        const config = {
            method: "post",
            url: `${baseURL}${route}${id}`,
            headers: headers,
            data: {
                License: process.env.VUE_APP_LICENSE,
                RequestorId: store.getters["appSettingsModule/getRequestorId"],
                AppId: parseInt(process.env.VUE_APP_ID),
                PageListModel: null
            }
        };

        let user = new User();

        const response = await axios(config);

        user = assignAPIReponseToUser(response.data);

        return user;

    } catch (error) {
        
        return error.response;
    }
}

const loginUser = function(user, token) {

    user.login();

    store.dispatch("userModule/updateUser", user);
    store.dispatch("appSettingsModule/updateAuthToken", token);
    store.dispatch("appSettingsModule/updateRequestorId", user.id);

    return user;
}

const logoutUser = function(user) {

    user.logout();

    store.dispatch("userModule/updateUser", user);
    store.dispatch("appSettingsModule/updateAuthToken", "");
    store.dispatch("appSettingsModule/updateRequestorId", 0);

    return user;
}

const assignAPIReponseToUser = function (data) {

    let user = new User();

    user.id = data.id;
    user.userName = data.userName;
    user.firstName = data.firstName;
    user.lastName = data.lastName;
    user.nickName = data.nickName;
    user.fullName = data.fullName;
    user.email = data.email;
    user.isActive = data.isActive;
    user.isAdmin = data.isAdmin;
    user.isSuperUser = data.isSuperUser;
    user.dateCreated = data.dateCreated;
    user.dateUpdated = data.dateUpdated;

    return user;
}

export const userService = {
    getUser,
    loginUser,
    logoutUser,
    assignAPIReponseToUser
};