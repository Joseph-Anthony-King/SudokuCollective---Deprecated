import * as axios from "axios";
import { requestHeader } from "../helpers/requestHeader";

const confirm = async function() {

    const kestralAPI = process.env.VUE_APP_API_KESTREL;
    const iisAPI = process.env.VUE_APP_API_IIS;
    const prodAPI = process.env.VUE_APP_API;

    const headers = requestHeader();

    const configRequest = {
        headers: headers
    };

    if (process.env.NODE_ENV !== "production") {

        try {
            
            const response = await axios.get(`${kestralAPI}/api/v1/helloworld`, configRequest );
            
            console.log(`The api is using kestral: ${kestralAPI}`);

            return { "url" : kestralAPI, "message" : response.data};
    
        } catch (error) {
            
            console.error(error);
            
            const response = await axios.get(`${iisAPI}/api/v1/helloworld`, configRequest);
            
            console.log(`The api is using IIS Express: ${iisAPI}`);
            
            return { "url" : iisAPI, "message" : response.data};
        }

    } else {
            
        const response = await axios.get(`${prodAPI}/api/v1/helloworld`, configRequest);
        
        return { "url" : prodAPI, "message" : response.data};
    }
};

export const apiURLConfirmationService = {
    confirm,
};