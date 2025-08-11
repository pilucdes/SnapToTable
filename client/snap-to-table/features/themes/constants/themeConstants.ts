import {ColorTheme, FontTheme, Theme} from '../types';

const fontTheme: FontTheme =
    {
        family: 'Poppins_400Regular'
    }

export const colorTheme: ColorTheme = {
    primary: '#264653',
    secondary: '#e76f51',
    accent: {
        opt1: '#f4a261',
        opt2: '#e9c46a',
        opt3: '#2a9d8f',
    }
};


export const lightTheme: Theme = {
    type: 'light',
    background: "#ececec",
    text: "#191919",
    error: "#e03d31",
    font: fontTheme,
    ...colorTheme,
};

export const darkTheme: Theme = {
    type: 'dark',
    background: "#091114",
    text: "#f2f4f5",
    error: "#f55347",
    font: fontTheme,
    ...colorTheme
};
