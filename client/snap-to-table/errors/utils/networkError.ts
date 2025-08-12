import {AxiosError} from "axios";
import Toast from "react-native-toast-message";

const networkErrorCodes = [
    'ECONNABORTED',
    'ERR_NETWORK',
    'ERR_BAD_OPTION',
    'ENOTFOUND',
    'ECONNREFUSED',
    'EHOSTUNREACH'
];

const isNetworkError = (error: AxiosError) => {

    if (!error.code) {
        return false;
    }

    return networkErrorCodes.includes(error.code)
}
export const handleNetworkError = (err: AxiosError): void => {

    if (!isNetworkError(err)) {
        return;
    }
   
    Toast.show({
        type: 'error',
        text1: "Network Error",
        text2: "Unable to connect to the server. Please check your internet connection and try again.",
        visibilityTime: 5000,
    });
}
