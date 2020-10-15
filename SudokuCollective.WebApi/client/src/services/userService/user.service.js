import * as axios from "axios";
import store from "../../store";
import User from "../../models/user";
import { requestHeader } from "../../helpers/requestHeader";
import { requestData } from "../../helpers/requestData";
import { requestDataUpdateUser } from "../../helpers/userRequestData/userRequestData";
import { getUserEnpoint } from "./service.endpoints";

const getUser = async function (id, pageListModel, fullRecord) {

    try {

        let params = "";

        if (fullRecord) {

            params = `/${id}?fullrecord=${fullRecord}`;

        } else {

            params = `/${id}`;
        }

        const config = {
            method: "post",
            url: `${getUserEnpoint}${params}`,
            headers: requestHeader(),
            data: requestData(pageListModel)
        };

        let user = new User();

        const response = await axios(config);

        user = assignAPIReponseToUser(response.data);

        return user;

    } catch (error) {
        
        console.error(error.name, error.message);
        return error.response;
    }
}

const getUsers = async function (pageListModel, fullRecord) {

    try {

        let params = "";

        if (fullRecord) {

            params = `?fullrecord=${fullRecord}`;

        }

        const config = {
            method: "post",
            url: `${getUserEnpoint}${params}`,
            headers: requestHeader(),
            data: requestData(pageListModel)
        };

        const response = await axios(config);

        return response;

    } catch (error) {
        
        console.error(error.name, error.message);
        return error.response;
    }
}

const updateUser = async function (
    id,
    pageListModel,
    userName,
    firstName,
    lastName,
    nickName,
    email) {

    try {

        const params = `/${id}`;

        const config = {
            method: "put",
            url: `${getUserEnpoint}${params}`,
            headers: requestHeader(),
            data: requestDataUpdateUser(
                pageListModel,
                userName,
                firstName,
                lastName,
                nickName,
                email)
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
    getUsers,
    updateUser,
    loginUser,
    logoutUser,
    assignAPIReponseToUser
};