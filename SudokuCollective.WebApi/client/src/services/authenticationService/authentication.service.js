import * as axios from "axios";
import { requestHeader } from "../../helpers/requestHeader";
import { authenticateEndpoint } from "./service.endpoints";

const authenticateUser = async function (username, password) {

    const headers = requestHeader();

    const config = {
        method: "post",
        url: authenticateEndpoint,
        headers: headers,
        data: {
            UserName: `${username}`,
            Password: `${password}`
        }
    };

    try {

        const response = await axios(config);
    
        return response;

    } catch (error) {

        return error.response;
    }
}

export const authenticationService = {
    authenticateUser
};