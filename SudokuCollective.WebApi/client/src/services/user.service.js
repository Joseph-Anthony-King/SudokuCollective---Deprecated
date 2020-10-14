import * as axios from "axios";
import store from "../store";
import User from "../models/user";
import { requestHeader } from "../helpers/requestHeader";

const getUser = async function (id) {

    try {

        const baseURL = store.getters["appConfigModule/getBaseURL"];
        const route = "/api/v1/users/";
        const headers = requestHeader();

        const config = {
            method: "post",
            url: `${baseURL}${route}${id}`,
            headers: headers,
            data: {
                License: process.env.VUE_APP_LICENSE,
                RequestorId: store.getters["appConfigModule/getRequestorId"],
                AppId: parseInt(process.env.VUE_APP_ID),
                PageListModel: null
            }
        };

        console.log("request config:", config);

        let user = new User();

        const response = await axios(config);

        user.id = response.data.id;
        user.userName = response.data.userName;
        user.firstName = response.data.firstName;
        user.lastName = response.data.lastName;
        user.nickName = response.data.nickName;
        user.fullName = response.data.fullName;
        user.email = response.data.email;
        user.isActive = response.data.isActive;
        user.isAdmin = response.data.isAdmin;
        user.isSuperUser = response.data.isSuperUser;
        user.dateCreated = response.data.dateCreated;
        user.dateUpdated = response.data.dateUpdated;

        console.log("userService getUser result:", user);

        return user;

    } catch (error) {

        console.error(error);
    }
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
    assignAPIReponseToUser
};