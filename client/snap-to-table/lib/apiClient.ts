import axios from 'axios';

const apiClient = axios.create({
    baseURL: process.env.EXPO_PUBLIC_API_BASE_URL,
    headers: {
        'Content-Type': 'application/json',
    },
});
apiClient.interceptors.response.use(
    (response) => response,
    (error) => {

        console.group('🚨 API Error Details');
        console.log('URL:', error.config?.url);
        console.log('Base URL:', error.config.baseURL);
        console.log('Request:', error.request);
        console.log('Method:', error.config?.method);
        console.log('Status:', error.response?.status);
        console.log('Status Text:', error.response?.statusText);
        console.log('Response Data:', error.response?.data);
        console.log('Request Headers:', error.config?.headers);
        console.log('Response Headers:', error.response?.headers);
        console.groupEnd();

        return Promise.reject(error);
    }
);

export default apiClient;