import {AxiosError} from "axios";
import Toast from "react-native-toast-message";
import { ApiError } from "../types";

export const isApiError = (
    error: AxiosError
): error is AxiosError<ApiError> => {
    return error instanceof AxiosError;
};

export const handleValidationError = (err: AxiosError): void => {

    if (!isApiError(err) || !err.response || err.response?.status !== 400) {
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
