import * as axios from "axios";
import { requestHeader } from "../../helpers/requestHeader";
import { getRegisterEndpoint } from "./service.endpoints";

const signUp = async function(signUpModel) {  

    const headers = requestHeader();

    const config = {
        method: "post",
        url: getRegisterEndpoint,
        headers: headers,
        data: signUpModel
    };

    console.log("config:", config);

    try {

        const response = await axios(config);

        return response;
        
    } catch (error) {
        
        return error.response;
    }
};

export const registerService = {
    signUp
}