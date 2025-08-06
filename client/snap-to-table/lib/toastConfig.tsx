import React from 'react';
import {
    BaseToast,
    ErrorToast,
    ToastConfig,
} from 'react-native-toast-message';
import tw from '@/lib/tailwind';

const baseStyle = tw.style(`bg-gray-800 border-l-2`);
const containerStyle = tw.style(`px-4 py-2`);
const text1Style = tw.style(`text-base font-semibold text-white`);
export const toastConfig: ToastConfig = {
    
    success: (props) => (
        <BaseToast
            {...props}
            style={[baseStyle,tw`border-green-500`]}
            contentContainerStyle={containerStyle}
            text1Style={text1Style}
        />
    ),
    
    error: (props) => (
        <ErrorToast
            {...props}
            style={[baseStyle,tw`border-red-500`]}
            contentContainerStyle={containerStyle}
            text1Style={text1Style}
        />
    )
};