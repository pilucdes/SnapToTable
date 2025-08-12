import {AxiosError} from "axios";
import {ApiException} from "../types/apiException";
import Toast from "react-native-toast-message";

export const isApiException = (
    error: AxiosError
): error is AxiosError<ApiException> => {
    return error instanceof AxiosError;
};

export const handleValidationException = (err: AxiosError): void => {

    if (!isApiException(err) || !err.response || err.response?.status !== 400) {
        return;
    }

    const validationError = err.response.data;

    const allErrors: string[] = [];
    Object.entries(validationError.errors).forEach(([,fieldErrors]) => {
        if (Array.isArray(fieldErrors)) {
            const prefixedErrors = fieldErrors.map(err => `${err}`);
            allErrors.push(...prefixedErrors);
        }
    });

    if (allErrors.length > 0) {
        Toast.show({
            type: 'error',
            text1: allErrors[0],
            visibilityTime: 4000,
        });
    }
}