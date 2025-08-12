import React from 'react';
import {
    BaseToast,
    ErrorToast,
    ToastConfig,
} from 'react-native-toast-message';
import tw from '@/lib/tailwind';

const baseStyle = tw.style(`bg-gray-800 border-l-0 h-auto`);
const containerStyle = tw.style(`px-4 py-2`);
const text1Style = tw.style(`text-base font-semibold text-white`);
const text2Style = tw.style(`text-sm text-gray-300 max-w-xs flex-wrap`);

export const toastConfig: ToastConfig = {
    
    success: (props) => (
        <BaseToast
            {...props}
            style={[baseStyle]}
            contentContainerStyle={containerStyle}
            text1Style={text1Style}
            text2Style={text2Style}
            text2NumberOfLines={2}
        />
    ),
    
    error: (props) => (
        <ErrorToast
            {...props}
            style={[baseStyle]}
            contentContainerStyle={containerStyle}
            text1Style={text1Style}
            text2Style={text2Style}
            text2NumberOfLines={2}
        />
    )
};