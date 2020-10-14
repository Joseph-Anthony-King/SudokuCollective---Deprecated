import * as axios from "axios";
import store from "../store";
import { requestHeader } from "../helpers/requestHeader";

const authenticateUser = async function (username, password) {

    const baseURL = store.getters["appConfigModule/getBaseURL"];
    const route = "/api/v1/authenticate";
    const headers = requestHeader();

    const config = {
        method: "post",
        url: `${baseURL}${route}`,
        headers: headers,
        data: {
            UserName: `${username}`,
            Password: `${password}`
        }
    };

    const response = await axios(config);

    return response;
}

export const authenticationService = {
    authenticateUser
};